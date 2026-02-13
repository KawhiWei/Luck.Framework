using System.Collections.Concurrent;
using System.Diagnostics;
using Luck.EventBus.RabbitMQ.Attributes;
using Luck.EventBus.RabbitMQ.Abstraction;
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
using Luck.EventBus.RabbitMQ.Enums;
using Luck.Framework.Infrastructure;


namespace Luck.EventBus.RabbitMQ;

internal sealed class RabbitMqEventBus : IIntegrationEventBus, IDisposable
{
    private readonly IRabbitMqPersistentConnection _persistentConnection;
    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly int _retryCount;
    private readonly IIntegrationEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<(Type HandlerType, Type EventType), Delegate> _handleAsyncDelegateCache = [];
    private const string HandlerName = nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync);
    private bool isDisposed;

    private readonly DiagnosticListener _diagnosticListener;

    public RabbitMqEventBus(int retryCount, IServiceProvider serviceProvider)
    {
        _persistentConnection = serviceProvider.GetService<IRabbitMqPersistentConnection>() ??
                                throw new ArgumentNullException(nameof(_persistentConnection));
        _logger = serviceProvider.GetService<ILogger<RabbitMqEventBus>>() ??
                  throw new ArgumentNullException(nameof(_logger));
        _retryCount = retryCount;
        _subsManager = serviceProvider.GetService<IIntegrationEventBusSubscriptionsManager>() ??
                       throw new ArgumentNullException(nameof(_subsManager));
        _serviceProvider = serviceProvider;
        
        _diagnosticListener = new DiagnosticListener(DiagnosticConstants.DiagnosticListenerName);
        
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
        var type = @event.GetType();
        string? eventContent = null;

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.EventId, type.Name);

        var rabbitMqAttribute = type.GetCustomAttribute<RabbitMqAttribute>();

        if (rabbitMqAttribute is null)
        {
            throw new ArgumentNullException($"{nameof(@event)}未设置<RabbitMQAttribute>特性,无法发布事件");
        }

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });

        // 记录事件内容用于诊断
        eventContent = System.Text.Encoding.UTF8.GetString(body);

        var channel = _persistentConnection.GetChannel();

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.DeliveryMode = 2;
        properties.Priority = priority.GetValueOrDefault();

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
                    // 记录发布失败诊断事件
                    WriteEvent(EventIds.PublishFailed.Name!,
                        new PublishEventData(
                            new LuckEventDefinition(EventIds.PublishFailed, LuckLogLevel.Error),
                            EventBusType.RabbitMQ,
                            type.FullName ?? type.Name,
                            type.Name,
                            eventContent,
                            rabbitMqAttribute.Exchange,
                            rabbitMqAttribute.RoutingKey,
                            rabbitMqAttribute.Queue,
                            exception: ex));

                    _logger.LogError(ex, "无法发布事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.EventId,
                        $"{time.TotalSeconds:n1}", ex.Message);
                });

        policy.Execute(() =>
        {
            channel.BasicPublish(rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey, true, properties, body);
            _logger.LogTrace("向RabbitMQ发布事件成功: {EventId}", @event.EventId);
        });

        // 记录发布成功诊断事件
        WriteEvent(EventIds.Published.Name!,
            new PublishEventData(
                    new LuckEventDefinition(EventIds.Published, LuckLogLevel.Information),
                    EventBusType.RabbitMQ,
                    type.FullName ?? type.Name,
                    type.Name,
                    eventContent,
                    rabbitMqAttribute.Exchange,
                    rabbitMqAttribute.RoutingKey,
                    rabbitMqAttribute.Queue,
                    @event.EventId.ToString(),
                    body.Length));
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
                StartBasicConsume(eventType, handlerType, rabbitMqAttribute, consumerChannel);
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
        _logger.LogTrace("创建RabbitMQ消费者通道");
        var channel = _persistentConnection.GetChannel();
        //创建交换机
        channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true);
        //创建队列
        channel.QueueDeclare(rabbitMqAttribute.Queue, true, false, false, null);

        channel.CallbackException += (_, ea) =>
        {
            _logger.LogWarning(ea.Exception, "重新创建RabbitMQ消费者通道");
            _subsManager.Clear();
            Subscribe();
        };
        return channel;
    }

    private void StartBasicConsume(Type eventType, Type handlerType, RabbitMqAttribute rabbitMqAttribute, IModel? consumerChannel)
    {
        _logger.LogTrace("启动RabbitMQ基本消耗");
        if (consumerChannel is not null)
        {
            var consumer = new AsyncEventingBasicConsumer(consumerChannel);
            consumer.Received += async (_, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                
                // 记录接收消息诊断事件
                WriteEvent(EventIds.Received.Name!,
                    new ConsumeEventData(
                        new LuckEventDefinition(EventIds.Received, LuckLogLevel.Information),
                        EventBusType.RabbitMQ,
                        eventType.FullName ?? eventType.Name,
                        eventType.Name,
                        rabbitMqAttribute.Exchange,
                        rabbitMqAttribute.RoutingKey,
                        rabbitMqAttribute.Queue,
                        ea.BasicProperties.MessageId ?? ea.DeliveryTag.ToString(),
                        ea.Body.Length,
                        ea.ConsumerTag));

                try
                {
                    if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                        throw new InvalidOperationException($"假异常请求: \"{message}\"");
                    await ProcessEvent(eventType, handlerType, message, () => consumerChannel.BasicAck(ea.DeliveryTag, false));
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


    private async Task ProcessEvent(Type eventType, Type handlerType, string message, Action ack)
    {
        var eventName = _subsManager.GetEventKey(eventType);
        _logger.LogTrace("处理RabbitMQ事件: {EventName}", eventName);

        var startTime = DateTime.UtcNow;
        string? eventContent = null;

        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            using var scope = _serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
            var subscriptionTypes = _subsManager.GetHandlersForEvent(eventName);
            foreach (var subscriptionType in subscriptionTypes)
            {
                var integrationEvent = JsonSerializer.Deserialize(message, eventType,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                if (integrationEvent is null)
                {
                    throw new LuckException("集成事件不能为空。。。");
                }

                // 记录事件内容用于诊断
                eventContent = System.Text.Json.JsonSerializer.Serialize(integrationEvent, eventType);

                var key = (subscriptionType, eventType);

                if (!_handleAsyncDelegateCache.TryGetValue(key, out var cachedDelegate))
                {
                    var method = concreteType.GetMethod(HandlerName);
                    if (method is null)
                    {
                        _logger.LogError($"无法找到{nameof(integrationEvent)}事件处理器");
                        return;
                    }

                    var delegateType = typeof(HandleAsyncDelegate<>).MakeGenericType(eventType);

                    var handler = scope?.ServiceProvider.GetService(subscriptionType);
                    if (handler is null)
                    {
                        _logger.LogError($"在DI中无法找到{nameof(eventType)}事件处理器");
                        return;
                    }

                    var handleAsyncDelegate = Delegate.CreateDelegate(delegateType, handler, method);
                    _handleAsyncDelegateCache[key] = handleAsyncDelegate;
                    cachedDelegate = handleAsyncDelegate;
                }

                if (cachedDelegate.DynamicInvoke(integrationEvent) is not Task handlerTask)
                {
                    continue;
                }

                try
                {
                    await handlerTask;
                    ack.Invoke();

                    // 记录处理成功诊断事件
                    var duration = DateTime.UtcNow - startTime;
                    WriteEvent(EventIds.Processed.Name!,
                        new ProcessEventData(
                            new LuckEventDefinition(EventIds.Processed, LuckLogLevel.Information),
                            EventBusType.RabbitMQ,
                            eventType.FullName ?? eventType.Name,
                            eventType.Name,
                            handlerType.FullName ?? handlerType.Name,
                            eventContent,
                            duration));
                }
                catch (Exception ex)
                {
                    // 记录处理失败诊断事件
                    var duration = DateTime.UtcNow - startTime;
                    WriteEvent(EventIds.ProcessFailed.Name!,
                        new ProcessEventData(
                            new LuckEventDefinition(EventIds.ProcessFailed, LuckLogLevel.Error),
                            EventBusType.RabbitMQ,
                            eventType.FullName ?? eventType.Name,
                            eventType.Name,
                            handlerType.FullName ?? handlerType.Name,
                            eventContent,
                            duration,
                            ex));
                    throw;
                }
            }
        }
        else
            _logger.LogWarning("没有订阅RabbitMQ事件: {EventName}", eventName);
    }

    private void DoInternalSubscription(string eventName, RabbitMqAttribute rabbitMqAttribute, IModel consumerChannel)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
        if (containsKey)
        {
            return;
        }

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

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
    /// 写入诊断事件
    /// </summary>
    private void WriteEvent(string name, LuckEventData eventData)
    {
        if (_diagnosticListener.IsEnabled(name))
        {
            _diagnosticListener.Write(name, eventData);
        }
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        _subsManager.Clear();
        isDisposed = true;
    }

    private delegate Task HandleAsyncDelegate<in TEvent>(TEvent @event) where TEvent : IIntegrationEvent;
}