using System.Diagnostics;
using Luck.Framework.Event;

namespace OpenTelemetry.Luck.EventBus;

/// <summary>
/// EventBus OpenTelemetry 仪表化
/// </summary>
public class LuckEventBusInstrumentation : IDisposable
{
    private readonly ActivitySource _activitySource;
    private IDisposable? _allListenersSubscription;
    private readonly List<IDisposable> _subscriptions = new();
    private readonly object _lock = new();

    public const string SourceName = DiagnosticConstants.DiagnosticListenerName;

    public LuckEventBusInstrumentation(string? activitySourceName = null)
    {
        _activitySource = new ActivitySource(activitySourceName ?? SourceName);
        
        // 订阅所有 DiagnosticListener，当发现 EventBus 的监听器时进行订阅
        _allListenersSubscription = DiagnosticListener.AllListeners.Subscribe(new AllDiagnosticListenersObserver(this));
    }

    internal void SubscribeToListener(DiagnosticListener listener)
    {
        if (listener.Name != DiagnosticConstants.DiagnosticListenerName)
            return;

        lock (_lock)
        {
            // 避免重复订阅同一个监听器
            if (_subscriptions.Any(s => s is SingleDiagnosticListenerSubscription sub && sub.Listener == listener))
                return;

            var subscription = listener.Subscribe(new EventBusDiagnosticObserver(this));
            _subscriptions.Add(new SingleDiagnosticListenerSubscription(listener, subscription));
        }
    }

    internal void OnEvent(string eventName, object? eventData)
    {
        switch (eventName)
        {
            case nameof(EventIds.Published):
            case nameof(EventIds.PublishFailed):
                HandlePublished(eventData as PublishEventData);
                break;
            case nameof(EventIds.Received):
                HandleReceived(eventData as ConsumeEventData);
                break;
            case nameof(EventIds.Processed):
            case nameof(EventIds.ProcessFailed):
                HandleProcessed(eventData as ProcessEventData);
                break;
        }
    }

    private void HandlePublished(PublishEventData? data)
    {
        if (data == null) return;

        var activityName = $"{data.EventBusType}.Publish";
        using var activity = _activitySource.StartActivity(activityName, ActivityKind.Producer);
        if (activity == null) return;

        activity.SetTag("eventbus.event_type", data.EventType);
        activity.SetTag("eventbus.event_name", data.EventName);
        activity.SetTag("messaging.system", "rabbitmq");
        activity.SetTag("messaging.destination", data.Exchange);
        activity.SetTag("messaging.rabbitmq.routing_key", data.RoutingKey);
        
        // 记录原始消息内容
        if (!string.IsNullOrEmpty(data.RawContent))
        {
            activity.SetTag("eventbus.raw_content", data.RawContent);
        }
        
        if (data.Exception != null)
        {
            activity.SetStatus(ActivityStatusCode.Error, data.Exception.Message);
        }
        else
        {
            activity.SetStatus(ActivityStatusCode.Ok);
        }
    }

    private void HandleReceived(ConsumeEventData? data)
    {
        if (data == null) return;

        var activityName = $"{data.EventBusType}.Consume";
        using var activity = _activitySource.StartActivity(activityName, ActivityKind.Consumer);
        if (activity == null) return;

        activity.SetTag("eventbus.event_type", data.EventType);
        activity.SetTag("eventbus.event_name", data.EventName);
        activity.SetTag("messaging.system", "rabbitmq");
        activity.SetTag("messaging.source", data.Exchange);
        activity.SetTag("messaging.rabbitmq.queue", data.Queue);
        
        // 记录原始消息内容
        if (!string.IsNullOrEmpty(data.RawContent))
        {
            activity.SetTag("eventbus.raw_content", data.RawContent);
        }
        
        activity.SetStatus(ActivityStatusCode.Ok);
    }

    private void HandleProcessed(ProcessEventData? data)
    {
        if (data == null) return;

        var activityName = $"{data.EventBusType}.Process";
        using var activity = _activitySource.StartActivity(activityName, ActivityKind.Internal);
        if (activity == null) return;

        activity.SetTag("eventbus.event_type", data.EventType);
        activity.SetTag("eventbus.event_name", data.EventName);
        activity.SetTag("eventbus.handler_type", data.HandlerType);
        
        // 记录原始消息内容
        if (!string.IsNullOrEmpty(data.RawContent))
        {
            activity.SetTag("eventbus.raw_content", data.RawContent);
        }
        
        if (data.Exception != null)
        {
            activity.SetStatus(ActivityStatusCode.Error, data.Exception.Message);
        }
        else
        {
            activity.SetStatus(ActivityStatusCode.Ok);
        }
    }

    public void Dispose()
    {
        _allListenersSubscription?.Dispose();
        
        lock (_lock)
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
            _subscriptions.Clear();
        }
        
        _activitySource.Dispose();
    }
}
