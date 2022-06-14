using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.Framework.Exceptions;
using Luck.Framework.Extensions;
using Luck.Framework.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Luck.EventBus.RabbitMQ;

public  class IntegrationEventBusRabbitMq : IIntegrationEventBus, IDisposable
{
    private readonly IRabbitMQPersistentConnection _persistentConnection;
    private readonly ILogger<IntegrationEventBusRabbitMq> _logger;
    private readonly int _retryCount;
    private readonly IIntegrationEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private string _handleName = nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync);
    private IModel? _consumerChannel;
    private bool _isDisposed = false;

    public IntegrationEventBusRabbitMq(IRabbitMQPersistentConnection persistentConnection,
        ILogger<IntegrationEventBusRabbitMq> logger, int retryCount,
        IIntegrationEventBusSubscriptionsManager subsManager, IServiceProvider serviceProvider)
    {
        _persistentConnection = persistentConnection;
        _logger = logger;
        _retryCount = retryCount;
        _subsManager = subsManager ?? new RabbitMqEventBusSubscriptionsManager();
        _serviceProvider = serviceProvider;
        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    private void SubsManager_OnEventRemoved(object? sender, EventRemovedEventArgs args)
    {
        var eventName = args.EventType.Name;

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }


        using var channel = _persistentConnection.CreateModel();
        var type = args.EventType.GetCustomAttribute<RabbitMQAttribute>();
        if (type is null)
            throw new ArgumentNullException($"事件未配置[RabbitMQAttribute] 特性");

        channel.QueueUnbind(type.Queue ?? eventName,
            type.Exchange,
            type.RoutingKey);

        if (_subsManager.IsEmpty) _consumerChannel?.Close();
    }

    public void Publish<TEvent>(IIntegrationEvent @event) where TEvent : IIntegrationEvent
    {
        if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();


        var type = @event.GetType();

        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    _logger.LogWarning(ex, "发布集成事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.EventId,
                        $"{time.TotalSeconds:n1}", ex.Message);
                });

        _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.EventId, type.Name);


        var rabbitMqAttribute = type.GetCustomAttribute<RabbitMQAttribute>();

        if (rabbitMqAttribute is null)
            throw new ArgumentNullException($"{nameof(@event)}未设置<RabbitMQAttribute>特性,无法发布事件");

        if (string.IsNullOrEmpty(rabbitMqAttribute.Queue))
            rabbitMqAttribute.Queue = type.Name;

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });

        using var channel = _persistentConnection.CreateModel();
        var properties = channel.CreateBasicProperties();

        //创建交换机
        channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type);
        //创建队列
        //channel.QueueDeclare(queue: rabbitMQAttribute.Queue, durable: false);
        if (!string.IsNullOrEmpty(rabbitMqAttribute.RoutingKey) && !string.IsNullOrEmpty(rabbitMqAttribute.Queue))
            //通过RoutingKey将队列绑定交换机
            channel.QueueBind(rabbitMqAttribute.Queue, rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey);
        policy.Execute(() =>
        {
            properties.DeliveryMode = 2;
            _logger.LogTrace("向RabbitMQ发布事件: {EventId}", @event.EventId);
            channel.BasicPublish(rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey, true, properties, body);
        });
    }

    /// <summary>
    /// 对外暴露的订阅者方法
    /// </summary>
    /// <typeparam name="T">传入参数</typeparam>
    /// <typeparam name="TH">处理器</typeparam>
    public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        Subscribe(typeof(T), typeof(TH));
    }


    /// <summary>
    /// 检查订阅事件是否存在
    /// </summary>
    /// <param name="eventType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void CheckEventType(Type eventType)
    {
        Check.NotNull(eventType, nameof(eventType));
        if (eventType.IsDeriveClassFrom<IIntegrationEvent>() == false)
            throw new ArgumentNullException($"{eventType}没有继承IIntegrationEvent", nameof(eventType));
    }

    /// <summary>
    /// 检查订阅者是否存在
    /// </summary>
    /// <param name="handlerType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void CheckHandlerType(Type handlerType)
    {
        Check.NotNull(handlerType, nameof(handlerType));
        if (handlerType.IsBaseOn(typeof(IIntegrationEventHandler<>)) == false)
            throw new ArgumentNullException($"{nameof(handlerType)}IIntegrationEventHandler<>", nameof(handlerType));
    }

    /// <summary>
    /// 集成事件订阅者处理
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="handlerType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Subscribe(Type eventType, Type handlerType)
    {
        CheckEventType(eventType);
        CheckHandlerType(handlerType);
        var rabbitMqAttribute = eventType.GetCustomAttribute<RabbitMQAttribute>();
        
        if (rabbitMqAttribute == null)
            throw new ArgumentNullException($"{nameof(eventType)}未设置<RabbitMQAttribute>特性,无法发布事件");

        _consumerChannel = CreateConsumerChannel(rabbitMqAttribute);
        var eventName = _subsManager.GetEventKey(eventType);
        DoInternalSubscription(eventName, rabbitMqAttribute);
        _subsManager.AddSubscription(eventType, handlerType);
        StartBasicConsume(eventType, rabbitMqAttribute);
    }


    /// <summary>
    /// 创建通道
    /// </summary>
    /// <param name="rabbitMqAttribute"></param>
    /// <returns></returns>
    private IModel CreateConsumerChannel(RabbitMQAttribute rabbitMqAttribute)
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        _logger.LogTrace("创建RabbitMQ消费者通道");
        var channel = _persistentConnection.CreateModel();
        //创建交换机
        channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type);
        //创建队列
        channel.QueueDeclare(rabbitMqAttribute.Queue, true, false, false, null);

        channel.CallbackException += (sender, ea) =>
        {
            _logger.LogWarning(ea.Exception, "重新创建RabbitMQ消费者通道");
            _consumerChannel?.Dispose();
            _consumerChannel = CreateConsumerChannel(rabbitMqAttribute);
        };
        return channel;
    }

    private void StartBasicConsume(Type eventType, RabbitMQAttribute rabbitMqAttribute)
        //where T : IntegrationEvent
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
                        throw new InvalidOperationException($"假异常请求: \"{message}\"");

                    await ProcessEvent(eventType, message);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "----- 错误处理消息 \"{Message}\"", message);
                }

                // Even on exception we take the message off the queue.
                // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
                // For more information see: https://www.rabbitmq.com/dlx.html
                _consumerChannel.BasicAck(ea.DeliveryTag, false);
            };

            _consumerChannel.BasicConsume(
                rabbitMqAttribute.Queue,
                false,
                consumer);
        }
        else
        {
            _logger.LogError("StartBasicConsume不能调用_consumerChannel == null");
        }
    }


    private async Task ProcessEvent(Type eventType, string message)
        //where T : IntegrationEvent
    {
        var eventName = _subsManager.GetEventKey(eventType);
        _logger.LogTrace("处理RabbitMQ事件: {EventName}", eventName);

        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            using var scope = _serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
            var subscriptionTypes = _subsManager.GetHandlersForEvent(eventName);
            foreach (var subscriptionType in subscriptionTypes)
            {
                //var eventType = _subsManager.GetEventTypeByName(eventName);
                var integrationEvent = JsonSerializer.Deserialize(message, eventType,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                if (integrationEvent is null) throw new LuckException("集成事件不能为空。。。");

                var method = concreteType.GetMethod(_handleName);
                if (method == null)
                {
                    _logger.LogError("无法找到IIntegrationEventHandler事件处理器,下处理者方法");
                    throw new LuckException("无法找到IIntegrationEventHandler事件处理器,下处理者方法");
                }

                var handler = scope?.ServiceProvider.GetService(subscriptionType);
                if (handler == null) continue;


                await Task.Yield();
                var obj = method.Invoke(handler, new object[] { integrationEvent });
                if (obj == null) continue;
                await (Task)obj;
            }
        }
        else
            _logger.LogWarning("没有订阅RabbitMQ事件: {EventName}", eventName);
    }

    private void DoInternalSubscription(string eventName, RabbitMQAttribute rabbitMqAttribute)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
        if (!containsKey)
        {
            if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();

            _consumerChannel.QueueBind(rabbitMqAttribute.Queue,
                rabbitMqAttribute.Exchange,
                rabbitMqAttribute.RoutingKey);
        }
    }

    public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();

        _logger.LogInformation("移除事件 {EventName}", eventName);

        _subsManager.RemoveSubscription<T, TH>();
    }


    /// <summary>
    /// 释放对象和链接
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                if (_consumerChannel != null) _consumerChannel.Dispose();
                _subsManager.Clear();
            }

            _isDisposed = true;
        }
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

        //告诉GC，不要调用析构函数
        GC.SuppressFinalize(this);
    }
}