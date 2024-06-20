using System.Diagnostics;
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
using Luck.EventBus.RabbitMQ.Abstraction;
using Luck.EventBus.RabbitMQ.Diagnostics;
using Luck.EventBus.RabbitMQ.Enums;
using Luck.Framework.Infrastructure;


namespace Luck.EventBus.RabbitMQ;

public class IntegrationEventBusRabbitMq : IIntegrationEventBus, IDisposable
{
    private readonly IRabbitMqPersistentConnection _persistentConnection;
    private readonly ILogger<IntegrationEventBusRabbitMq> _logger;
    private readonly int _retryCount;
    private readonly IIntegrationEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _handleName = nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync);
    private bool _isDisposed = false;

    private readonly DiagnosticListener _listener =
        new DiagnosticListener(RabbitMqDiagnosticListenerNames.DiagnosticListenerName);

    public IntegrationEventBusRabbitMq(int retryCount, IServiceProvider serviceProvider)
    {
        _persistentConnection = serviceProvider.GetService<IRabbitMqPersistentConnection>() ??
                                throw new ArgumentNullException(nameof(_persistentConnection)); // persistentConnection;
        _logger = serviceProvider.GetService<ILogger<IntegrationEventBusRabbitMq>>() ??
                  throw new ArgumentNullException(nameof(_logger)); //(logger);
        _retryCount = retryCount;
        _subsManager = serviceProvider.GetService<IIntegrationEventBusSubscriptionsManager>() ??
                       throw new ArgumentNullException(
                           nameof(_subsManager)); //subsManager ?? new RabbitMqEventBusSubscriptionsManager();
        _serviceProvider = serviceProvider;
        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    /// <summary>
    /// 发布消息
    /// </summary>
    /// <param name="event"></param>
    /// <param name="priority"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    public void Publish<TEvent>(TEvent @event, byte? priority = 1) where TEvent : IIntegrationEvent
    {
        if (_listener.IsEnabled(RabbitMqDiagnosticListenerNames.CreateRabbitMqConnectionBefore))
        {
            _listener.Write(RabbitMqDiagnosticListenerNames.CreateRabbitMqConnectionBefore, "准备创建RabbitMQ链接");
        }

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        if (_listener.IsEnabled(RabbitMqDiagnosticListenerNames.CreateRabbitMqConnectionAfter))
        {
            _listener.Write(RabbitMqDiagnosticListenerNames.CreateRabbitMqConnectionAfter, "创建RabbitMQ链接成功");
        }

        var type = @event.GetType();


        if (_listener.IsEnabled(RabbitMqDiagnosticListenerNames.PublishIntegrationEventBusForRabbitMqBefore))
        {
            _listener.Write(RabbitMqDiagnosticListenerNames.PublishIntegrationEventBusForRabbitMqBefore,
                $"发布集成事件前诊断器：EventId：{@event.EventId}；EventName：{type.Name}；Data：{@event.Serialize()}");
        }

        _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.EventId, type.Name);

        var rabbitMqAttribute = type.GetCustomAttribute<RabbitMqAttribute>();

        if (rabbitMqAttribute is null)
            throw new ArgumentNullException($"{nameof(@event)}未设置<RabbitMQAttribute>特性,无法发布事件");

        if (string.IsNullOrEmpty(rabbitMqAttribute.Queue))
            rabbitMqAttribute.Queue = type.Name;

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });


        if (_listener.IsEnabled(RabbitMqDiagnosticListenerNames.RabbitMqCreateModelBefore))
        {
            _listener.Write(RabbitMqDiagnosticListenerNames.RabbitMqCreateModelBefore, $"准备创建Rabbit虚拟通道");
        }

        var channel = _persistentConnection.GetChannel();

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.DeliveryMode = 2;
        properties.Priority = priority.GetValueOrDefault();

        if (_listener.IsEnabled(RabbitMqDiagnosticListenerNames.RabbitMqCreateModelAfter))
        {
            _listener.Write(RabbitMqDiagnosticListenerNames.RabbitMqCreateModelAfter, $"创建RabbitMQ虚拟通道成功");
        }

        if (rabbitMqAttribute is not { WorkModel: EWorkModel.None })
        {
            //创建交换机
            channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true);
        }

        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    if (_listener.IsEnabled(RabbitMqDiagnosticListenerNames.PublishIntegrationEventBusForRabbitMqError))
                    {
                        _listener.Write(RabbitMqDiagnosticListenerNames.PublishIntegrationEventBusForRabbitMqError,
                            $"发布集成事件异常：EventId：{@event.EventId}；EventName：{type.Name}，异常：{ex.Message}");
                    }

                    _logger.LogError(ex, "无法发布事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.EventId,
                        $"{time.TotalSeconds:n1}", ex.Message);
                });

        policy.Execute(() =>
        {
            channel.BasicPublish(rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey, true, properties, body);
            if (_listener.IsEnabled(RabbitMqDiagnosticListenerNames.PublishIntegrationEventBusForRabbitMqAfter))
            {
                _listener.Write(RabbitMqDiagnosticListenerNames.PublishIntegrationEventBusForRabbitMqAfter,
                    $"发布集成事件成功：EventId：{@event.EventId}；EventName：{type.Name}");
            }

            _logger.LogTrace("向RabbitMQ发布事件成功: {EventId}", @event.EventId);
        });
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
            throw new ArgumentNullException(nameof(eventType), $"{eventType}没有继承IIntegrationEvent");
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
            throw new ArgumentNullException(nameof(handlerType), $"{nameof(handlerType)}IIntegrationEventHandler<>");
    }

    /// <summary>
    /// 集成事件订阅者处理
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void Subscribe()
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        var handlerTypes = AssemblyHelper.FindTypes(o =>
            o is { IsClass: true, IsAbstract: false } && o.IsBaseOn(typeof(IIntegrationEventHandler<>)));
        foreach (var handlerType in handlerTypes)
        {
            var implementedType = handlerType.GetTypeInfo().ImplementedInterfaces
                .FirstOrDefault(o => o.IsBaseOn(typeof(IIntegrationEventHandler<>)));
            var eventType = implementedType?.GetTypeInfo().GenericTypeArguments.FirstOrDefault();
            if (eventType == null)
            {
                continue;
            }

            CheckEventType(eventType);
            CheckHandlerType(handlerType);
            var rabbitMqAttribute = eventType.GetCustomAttribute<RabbitMqAttribute>();

            if (rabbitMqAttribute == null)
                throw new ArgumentNullException($"{nameof(eventType)}未设置<RabbitMQAttribute>特性,无法发布事件");

            Task.Factory.StartNew(() =>
            {
                using var consumerChannel = CreateConsumerChannel(rabbitMqAttribute);
                var eventName = _subsManager.GetEventKey(eventType);
                DoInternalSubscription(eventName, rabbitMqAttribute, consumerChannel);
                using var scope = _serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
                // 检查消费者是否已经注册,若是未注册则不启动消费.
                var handler = scope?.ServiceProvider.GetService(handlerType);
                if (handler is null) return;
                _subsManager.AddSubscription(eventType, handlerType);
                StartBasicConsume(eventType, rabbitMqAttribute, consumerChannel);
            });
        }
    }

    /// <summary>
    /// 创建消费者通道
    /// </summary>
    /// <param name="rabbitMqAttribute"></param>
    /// <returns></returns>
    private IModel CreateConsumerChannel(RabbitMqAttribute rabbitMqAttribute)
    {
        // if (!_persistentConnection.IsConnected)
        // {
        //     _persistentConnection.TryConnect();
        // }
        _logger.LogTrace("创建RabbitMQ消费者通道");
        var channel = _persistentConnection.CreateModel();
        //创建交换机
        channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true);
        //创建队列
        channel.QueueDeclare(rabbitMqAttribute.Queue, true, false, false, null);

        channel.CallbackException += (_, ea) =>
        {
            _logger.LogWarning(ea.Exception, "重新创建RabbitMQ消费者通道");
            _subsManager.Clear();
//            _consumerChannel = CreateConsumerChannel(rabbitMqAttribute);
            Subscribe();
        };
        return channel;
    }

    private void StartBasicConsume(Type eventType, RabbitMqAttribute rabbitMqAttribute, IModel? consumerChannel)
        //where T : IntegrationEvent
    {
        _logger.LogTrace("启动RabbitMQ基本消耗");
        if (consumerChannel is not null)
        {
            var consumer = new AsyncEventingBasicConsumer(consumerChannel);
            //consumer.Received += Consumer_Received;
            consumer.Received += async (_, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                try
                {
                    if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                        throw new InvalidOperationException($"假异常请求: \"{message}\"");
                    await ProcessEvent(eventType, message, () => consumerChannel.BasicAck(ea.DeliveryTag, false));
                }
                catch (Exception? ex)
                {
                    _logger.LogWarning(ex, "----- 错误处理消息 \"{Message}\"", message);
                }
            };
            _ = consumerChannel.BasicConsume(rabbitMqAttribute.Queue, false, consumer);
            while (true)
            {
                Thread.Sleep(100000);
            }
        }
        else
        {
            _logger.LogError("StartBasicConsume不能调用_consumerChannel == null");
        }
    }


    private async Task ProcessEvent(Type eventType, string message, Action ack)
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
                if (method is null)
                {
                    _logger.LogError("无法找到IIntegrationEventHandler事件处理器,下处理者方法");
                    throw new LuckException("无法找到IIntegrationEventHandler事件处理器,下处理者方法");
                }

                var handler = scope?.ServiceProvider.GetService(subscriptionType);
                if (handler is null)
                {
                    continue;
                }

                await Task.Yield();
                var obj = method.Invoke(handler, new[] { integrationEvent });
                if (obj is null)
                {
                    continue;
                }

                await (Task)obj;
                ack.Invoke();
            }
        }
        else
            _logger.LogWarning("没有订阅RabbitMQ事件: {EventName}", eventName);
    }

    private void DoInternalSubscription(string eventName, RabbitMqAttribute rabbitMqAttribute, IModel consumerChannel)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
        if (containsKey) return;
        if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();

        consumerChannel.QueueBind(rabbitMqAttribute.Queue,
            rabbitMqAttribute.Exchange,
            rabbitMqAttribute.RoutingKey);
    }

    private void SubsManager_OnEventRemoved(object? sender, EventRemovedEventArgs args)
    {
        var eventName = args.EventType.Name;

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }


        using var channel = _persistentConnection.CreateModel();
        var type = args.EventType.GetCustomAttribute<RabbitMqAttribute>();
        if (type is null)
            throw new ArgumentNullException(nameof(args), $"事件未配置[RabbitMQAttribute] 特性");

        channel.QueueUnbind(type.Queue ?? eventName,
            type.Exchange,
            type.RoutingKey);
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