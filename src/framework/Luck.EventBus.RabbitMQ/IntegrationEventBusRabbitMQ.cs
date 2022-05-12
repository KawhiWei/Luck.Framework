using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.Framework.Exceptions;
using Luck.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Luck.EventBus.RabbitMQ
{
    public class IntegrationEventBusRabbitMQ : IIntegrationEventBus, IDisposable
    {

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<IntegrationEventBusRabbitMQ> _logger;
        private readonly int _retryCount;
        private readonly IIntegrationEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private string _handleName = nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync);
        private IModel? _consumerChannel;
        private bool _isDisposed = false;
        public IntegrationEventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, ILogger<IntegrationEventBusRabbitMQ> logger, int retryCount, IIntegrationEventBusSubscriptionsManager subsManager, IServiceProvider serviceProvider)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _retryCount = retryCount;
            _subsManager = subsManager ?? new RabbitMQEventBusSubscriptionsManager();
            _serviceProvider = serviceProvider;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }
        ~IntegrationEventBusRabbitMQ()
        {
            Dispose(false);
        }
        private void SubsManager_OnEventRemoved(object? sender, EventRemovedEventArgs args)
        {
            var eventName = args.EventType.Name;

            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }


            using (var channel = _persistentConnection.CreateModel())
            {
                var type = args.EventType.GetCustomAttribute<RabbitMQAttribute>();
                if (type is null)
                    throw new ArgumentNullException("事件未配置[RabbitMQAttribute] 特性");

                channel.QueueUnbind(queue: type.Queue ?? eventName,
                    exchange: type.Exchange,
                    routingKey: type.RoutingKey);

                if (_subsManager.IsEmpty)
                {

                    _consumerChannel?.Close();
                }
            }
        }

        public void Publish<TEvent>(IIntegrationEvent @event) where TEvent : IIntegrationEvent
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }


            var type = @event.GetType();

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(ex, "发布集成事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
            });

            _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.Id, type.Name);


            var rabbitMQAttribute = type.GetCustomAttribute<RabbitMQAttribute>();

            if (rabbitMQAttribute is null)
                throw new ArgumentNullException($"{nameof(@event)}未设置<RabbitMQAttribute>特性,无法发布事件");

            if (string.IsNullOrEmpty(rabbitMQAttribute.Queue))
                rabbitMQAttribute.Queue = type.Name;

            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

            using (var channel = _persistentConnection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();

                //创建交换机
                channel.ExchangeDeclare(exchange: rabbitMQAttribute.Exchange, type: rabbitMQAttribute.Type);
                //创建队列
                //channel.QueueDeclare(queue: rabbitMQAttribute.Queue, durable: false);
                if (!string.IsNullOrEmpty(rabbitMQAttribute.RoutingKey) && !string.IsNullOrEmpty(rabbitMQAttribute.Queue))
                {
                    //通过RoutingKey将队列绑定交换机
                    channel.QueueBind(rabbitMQAttribute.Queue, rabbitMQAttribute.Exchange, rabbitMQAttribute.RoutingKey);
                }
                policy.Execute(() =>
                {
                    properties.DeliveryMode = 2;
                    _logger.LogTrace("向RabbitMQ发布事件: {EventId}", @event.Id);
                    channel.BasicPublish(exchange: rabbitMQAttribute.Exchange, routingKey: rabbitMQAttribute.RoutingKey, mandatory: true, basicProperties: properties, body: body);
                });
            }
        }

        public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
        {

            var rabbitMQAttribute = typeof(T).GetCustomAttribute<RabbitMQAttribute>();

            if (rabbitMQAttribute == null)
            {
                throw new ArgumentNullException($"{nameof(T)}未设置<RabbitMQAttribute>特性,无法发布事件");
            }
            _consumerChannel = CreateConsumerChannel(rabbitMQAttribute);
            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName, rabbitMQAttribute);
            _subsManager.AddSubscription<T, TH>();
            StartBasicConsume<T>(rabbitMQAttribute);
        }




        private IModel CreateConsumerChannel(RabbitMQAttribute rabbitMQAttribute)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("创建RabbitMQ消费者通道");
            var channel = _persistentConnection.CreateModel();

            //创建交换机
            channel.ExchangeDeclare(exchange: rabbitMQAttribute.Exchange, type: rabbitMQAttribute.Type);
            //创建队列
            channel.QueueDeclare(queue: rabbitMQAttribute.Queue, durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "重新创建RabbitMQ消费者通道");
                _consumerChannel?.Dispose();
                _consumerChannel = CreateConsumerChannel(rabbitMQAttribute);
            };

            //channel.ExchangeDeclare(exchange: rabbitMQAttribute.Exchange,
            //                        type: rabbitMQAttribute.Type);
            //channel.QueueDeclare(queue: rabbitMQAttribute.Queue,
            //            durable: true,
            //            exclusive: false,
            //            autoDelete: false,
            //            arguments: null);
            return channel;
        }

        private void StartBasicConsume<T>(RabbitMQAttribute rabbitMQAttribute)
                    where T : IntegrationEvent
        {
            _logger.LogTrace("启动RabbitMQ基本消耗");
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                //consumer.Received += Consumer_Received;
                consumer.Received += async (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.Span);
                    try
                    {
                        if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                        {
                            throw new InvalidOperationException($"假异常请求: \"{message}\"");
                        }

                        await ProcessEvent<T>(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "----- 错误处理消息 \"{Message}\"", message);
                    }

                    // Even on exception we take the message off the queue.
                    // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
                    // For more information see: https://www.rabbitmq.com/dlx.html
                    _consumerChannel.BasicAck(ea.DeliveryTag, multiple: false);

                };

                _consumerChannel.BasicConsume(
                    queue: rabbitMQAttribute.Queue,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume不能调用_consumerChannel == null");
            }
        }


        private async Task ProcessEvent<T>(string message)
                       where T : IntegrationEvent
        {
            var eventName = typeof(T).Name;
            _logger.LogTrace("处理RabbitMQ事件: {EventName}", typeof(T).Name);

            if (_subsManager.HasSubscriptionsForEvent<T>())
            {
                using (var scope = _serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope())
                {
                    var subscriptionTypes = _subsManager.GetHandlersForEvent<T>();
                    foreach (var subscriptionType in subscriptionTypes)
                    {

                        var eventType = typeof(T);
                        //var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        if (integrationEvent is null)
                        {
                            throw new LuckException("集成事件不能为空。。。");

                        }

                        var method = concreteType.GetMethod(_handleName);
                        if (method == null)
                        {
                            _logger.LogError("无法找到IIntegrationEventHandler事件处理器,下处理者方法");
                            throw new LuckException("无法找到IIntegrationEventHandler事件处理器,下处理者方法");

                        }
                        var handler = scope?.ServiceProvider.GetService(subscriptionType);
                        if (handler == null)
                        {

                            continue;
                        }


                        await Task.Yield();
                        var obj = method.Invoke(handler, new object[] { integrationEvent });
                        if (obj == null)
                        {
                            continue;
                        }
                        await (Task)obj;
                    }
                }
            }
            else
            {
                _logger.LogWarning("没有订阅RabbitMQ事件: {EventName}", eventName);
            }
        }
        private void DoInternalSubscription(string eventName, RabbitMQAttribute rabbitMQAttribute)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                _consumerChannel.QueueBind(queue: rabbitMQAttribute.Queue,
                                    exchange: rabbitMQAttribute.Exchange,
                                    routingKey: rabbitMQAttribute.RoutingKey);
            }

        }

        public void Unsubscribe<T, TH>()
    where T : IntegrationEvent
    where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("移除事件 {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }


   
        protected virtual void Dispose(bool disposing)
        {

            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_consumerChannel != null)
                    {
                        _consumerChannel.Dispose();
                    }
                    _subsManager.Clear();


                }
                _isDisposed = true;
            }



        }

        public void Dispose()
        {
            Dispose(true);

            //告诉GC，不要调用析构函数
            GC.SuppressFinalize(this);

        }

    }
}