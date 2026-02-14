namespace OpenTelemetry.Luck.EventBus;

/// <summary>
/// 用于订阅单个 DiagnosticListener 事件的观察者
/// </summary>
internal class EventBusDiagnosticObserver : IObserver<KeyValuePair<string, object?>>
{
    private readonly LuckEventBusInstrumentation _instrumentation;

    public EventBusDiagnosticObserver(LuckEventBusInstrumentation instrumentation)
    {
        _instrumentation = instrumentation;
    }

    public void OnNext(KeyValuePair<string, object?> value)
    {
        _instrumentation.OnEvent(value.Key, value.Value);
    }

    public void OnError(Exception error) { }
    public void OnCompleted() { }
}
