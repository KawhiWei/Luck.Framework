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

internal sealed class RabbitMqEventBus : IIntegrationEventBus, IAsyncDisposable
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

    public async Task PublishAsync<TEvent>(TEvent @event, byte? priority = 1, CancellationToken cancellationToken = default) 
        where TEvent : IIntegrationEvent
    {
        var type = @event.GetType();
        string? eventContent = null;

        if (!_persistentConnection.IsConnected)
        {
            await _persistentConnection.TryConnectAsync(cancellationToken);
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

        eventContent = Encoding.UTF8.GetString(body);

        // 从发布通道池获取通道
        var channel = await _persistentConnection.GetPublishChannelAsync(cancellationToken);

        try
        {
            var properties = new BasicProperties
            {
                Persistent = true,
                DeliveryMode = DeliveryModes.Persistent,
                Priority = priority.GetValueOrDefault()
            };

            if (rabbitMqAttribute is not { WorkModel: EWorkModel.None })
            {
                await channel.ExchangeDeclareAsync(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true, cancellationToken: cancellationToken);
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        var publishFailedData = new PublishEventData(
                            new LuckEventDefinition(EventIds.PublishFailed, LuckLogLevel.Error),
                            EventBusType.RabbitMQ,
                            type.FullName ?? type.Name,
                            type.Name,
                            eventContent,
                            rabbitMqAttribute.Exchange,
                            rabbitMqAttribute.RoutingKey,
                            rabbitMqAttribute.Queue,
                            exception: ex);
                        publishFailedData.RawContent = eventContent;
                        WriteEvent(EventIds.PublishFailed.Name!, publishFailedData);

                        _logger.LogError(ex, "无法发布事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.EventId,
                            $"{time.TotalSeconds:n1}", ex.Message);
                    });

            await policy.ExecuteAsync(async ct =>
            {
                await channel.BasicPublishAsync(rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey, true, properties, body, ct);
                _logger.LogTrace("向RabbitMQ发布事件成功: {EventId}", @event.EventId);
            }, cancellationToken);

            var publishedData = new PublishEventData(
                    new LuckEventDefinition(EventIds.Published, LuckLogLevel.Information),
                    EventBusType.RabbitMQ,
                    type.FullName ?? type.Name,
                    type.Name,
                    eventContent,
                    rabbitMqAttribute.Exchange,
                    rabbitMqAttribute.RoutingKey,
                    rabbitMqAttribute.Queue,
                    @event.EventId.ToString(),
                    body.Length);
            publishedData.RawContent = eventContent;
            WriteEvent(EventIds.Published.Name!, publishedData);
        }
        finally
        {
            _persistentConnection.ReturnPublishChannel(channel);
        }
    }

    private void CheckEventType(Type eventType)
    {
        Check.NotNull(eventType, nameof(eventType));
        if (eventType.IsDeriveClassFrom<IIntegrationEvent>() == false)
            throw new ArgumentNullException(nameof(eventType), $"{eventType}没有继承IIntegrationEvent");
    }

    private void CheckHandlerType(Type handlerType)
    {
        Check.NotNull(handlerType, nameof(handlerType));
        if (handlerType.IsBaseOn(typeof(IIntegrationEventHandler<>)) == false)
            throw new ArgumentNullException(nameof(handlerType), $"{nameof(handlerType)}IIntegrationEventHandler<>");
    }

    public async Task SubscribeAsync(CancellationToken cancellationToken = default)
    {
        if (!_persistentConnection.IsConnected)
        {
            await _persistentConnection.TryConnectAsync(cancellationToken);
        }

        var handlerTypes = AssemblyHelper.FindTypes(o =>
            o is { IsClass: true, IsAbstract: false } && o.IsBaseOn(typeof(IIntegrationEventHandler<>)));

        var tasks = handlerTypes.Select(async handlerType =>
        {
            var implementedType = handlerType.GetTypeInfo().ImplementedInterfaces
                .FirstOrDefault(o => o.IsBaseOn(typeof(IIntegrationEventHandler<>)));
            var eventType = implementedType?.GetTypeInfo().GenericTypeArguments.FirstOrDefault();
            if (eventType == null)
            {
                return;
            }

            CheckEventType(eventType);
            CheckHandlerType(handlerType);
            var rabbitMqAttribute = eventType.GetCustomAttribute<RabbitMqAttribute>();

            if (rabbitMqAttribute == null)
                throw new ArgumentNullException($"{nameof(eventType)}未设置<RabbitMQAttribute>特性,无法发布事件");

            // 使用独立的消费通道
            var consumerChannel = await CreateConsumerChannelAsync(rabbitMqAttribute, cancellationToken);
            var eventName = _subsManager.GetEventKey(eventType);
            await DoInternalSubscriptionAsync(eventName, rabbitMqAttribute, consumerChannel, cancellationToken);
            
            using var scope = _serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
            var handler = scope?.ServiceProvider.GetService(handlerType);
            if (handler is null) return;
            
            _subsManager.AddSubscription(eventType, handlerType);
            await StartBasicConsumeAsync(eventType, handlerType, rabbitMqAttribute, consumerChannel, cancellationToken);
        });

        await Task.WhenAll(tasks);
    }

    private async Task<IChannel> CreateConsumerChannelAsync(RabbitMqAttribute rabbitMqAttribute, CancellationToken cancellationToken)
    {
        _logger.LogTrace("创建RabbitMQ消费者通道");
        
        // 从消费通道池获取独立通道
        var channel = await _persistentConnection.GetConsumerChannelAsync(cancellationToken);
        
        await channel.ExchangeDeclareAsync(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(rabbitMqAttribute.Queue, true, false, false, null, cancellationToken: cancellationToken);

        channel.ChannelShutdownAsync += async (_, ea) =>
        {
            if (ea.Exception is not null)
            {
                _logger.LogWarning(ea.Exception, "RabbitMQ消费者通道关闭，重新创建...");
                _subsManager.Clear();
                await SubscribeAsync();
            }
            await Task.CompletedTask;
        };

        return channel;
    }

    private async Task StartBasicConsumeAsync(Type eventType, Type handlerType, RabbitMqAttribute rabbitMqAttribute, IChannel consumerChannel, CancellationToken cancellationToken)
    {
        _logger.LogTrace("启动RabbitMQ基本消耗");
        if (consumerChannel is not null)
        {
            var consumer = new AsyncEventingBasicConsumer(consumerChannel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                
                var receivedData = new ConsumeEventData(
                    new LuckEventDefinition(EventIds.Received, LuckLogLevel.Information),
                    EventBusType.RabbitMQ,
                    eventType.FullName ?? eventType.Name,
                    eventType.Name,
                    rabbitMqAttribute.Exchange,
                    rabbitMqAttribute.RoutingKey,
                    rabbitMqAttribute.Queue,
                    ea.BasicProperties.MessageId ?? ea.DeliveryTag.ToString(),
                    ea.Body.Length,
                    ea.ConsumerTag);
                receivedData.RawContent = message;
                WriteEvent(EventIds.Received.Name!, receivedData);

                try
                {
                    if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                        throw new InvalidOperationException($"假异常请求: \"{message}\"");
                    await ProcessEventAsync(eventType, handlerType, message, async () => 
                        await consumerChannel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken));
                }
                catch (Exception? ex)
                {
                    _logger.LogWarning(ex, "----- 错误处理消息 \"{Message}\"", message);
                }
            };

            await consumerChannel.BasicConsumeAsync(rabbitMqAttribute.Queue, false, consumer, cancellationToken);
            
            // 保持消费循环
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
        else
        {
            _logger.LogError("StartBasicConsume不能调用_consumerChannel == null");
        }
    }

    private async Task ProcessEventAsync(Type eventType, Type handlerType, string message, Func<Task> ack)
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
                    await ack();

                    var duration = DateTime.UtcNow - startTime;
                    var processedData = new ProcessEventData(
                        new LuckEventDefinition(EventIds.Processed, LuckLogLevel.Information),
                        EventBusType.RabbitMQ,
                        eventType.FullName ?? eventType.Name,
                        eventType.Name,
                        handlerType.FullName ?? handlerType.Name,
                        eventContent,
                        duration);
                    processedData.RawContent = message;
                    WriteEvent(EventIds.Processed.Name!, processedData);
                }
                catch (Exception ex)
                {
                    var duration = DateTime.UtcNow - startTime;
                    var processFailedData = new ProcessEventData(
                        new LuckEventDefinition(EventIds.ProcessFailed, LuckLogLevel.Error),
                        EventBusType.RabbitMQ,
                        eventType.FullName ?? eventType.Name,
                        eventType.Name,
                        handlerType.FullName ?? handlerType.Name,
                        eventContent,
                        duration,
                        ex);
                    processFailedData.RawContent = message;
                    WriteEvent(EventIds.ProcessFailed.Name!, processFailedData);
                    throw;
                }
            }
        }
        else
            _logger.LogWarning("没有订阅RabbitMQ事件: {EventName}", eventName);
    }

    private async Task DoInternalSubscriptionAsync(string eventName, RabbitMqAttribute rabbitMqAttribute, IChannel consumerChannel, CancellationToken cancellationToken)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
        if (containsKey)
        {
            return;
        }

        if (!_persistentConnection.IsConnected)
        {
            await _persistentConnection.TryConnectAsync(cancellationToken);
        }

        await consumerChannel.QueueBindAsync(rabbitMqAttribute.Queue, rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey, cancellationToken: cancellationToken);
    }

    private async void SubsManager_OnEventRemoved(object? sender, EventRemovedEventArgs args)
    {
        var eventName = args.EventType.Name;

        if (!_persistentConnection.IsConnected)
        {
            await _persistentConnection.TryConnectAsync();
        }

        var channel = await _persistentConnection.CreateChannelAsync();
        try
        {
            var type = args.EventType.GetCustomAttribute<RabbitMqAttribute>();
            if (type is null)
                throw new ArgumentNullException(nameof(args), $"事件未配置[RabbitMQAttribute] 特性");

            await channel.QueueUnbindAsync(type.Queue ?? eventName, type.Exchange, type.RoutingKey);
        }
        finally
        {
            channel.Dispose();
        }
    }

    private void WriteEvent(string name, LuckEventData eventData)
    {
        _diagnosticListener.Write(name, eventData);
    }

    public async ValueTask DisposeAsync()
    {
        if (isDisposed)
        {
            return;
        }

        _subsManager.Clear();
        
        await _persistentConnection.DisposeAsync();
        
        isDisposed = true;
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    private delegate Task HandleAsyncDelegate<in TEvent>(TEvent @event) where TEvent : IIntegrationEvent;
}
