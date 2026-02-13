namespace OpenTelemetry.Luck.EventBus;

internal class LuckDiagnosticListenerObserver : IObserver<KeyValuePair<string, object?>>
{
    private readonly LuckEventBusInstrumentation _instrumentation;

    public LuckDiagnosticListenerObserver(LuckEventBusInstrumentation instrumentation)
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
