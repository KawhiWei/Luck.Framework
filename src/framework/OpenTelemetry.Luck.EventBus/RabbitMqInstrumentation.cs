using System.Diagnostics;
using Luck.Framework.Event;

namespace OpenTelemetry.Luck.EventBus;

/// <summary>
/// RabbitMQ EventBus OpenTelemetry 仪表化
/// </summary>
public class LuckEventBusInstrumentation : IDisposable
{
    private readonly ActivitySource _activitySource;
    private readonly DiagnosticListener _diagnosticListener;
    private IDisposable? _subscription;

    public const string SourceName = DiagnosticConstants.DiagnosticListenerName;

    public LuckEventBusInstrumentation(string? activitySourceName = null)
    {
        _activitySource = new ActivitySource(activitySourceName ?? SourceName);
        _diagnosticListener = new DiagnosticListener(DiagnosticConstants.DiagnosticListenerName);
        _subscription = _diagnosticListener.Subscribe(new LuckDiagnosticListenerObserver(this));
    }

    internal void OnEvent(string eventName, object? eventData)
    {
        switch (eventName)
        {
            case nameof(EventIds.Published):
                HandlePublished(eventData as PublishEventData);
                break;
            case nameof(EventIds.Received):
                HandleReceived(eventData as ConsumeEventData);
                break;
            case nameof(EventIds.Processed):
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
        _subscription?.Dispose();
        _activitySource.Dispose();
    }
}
