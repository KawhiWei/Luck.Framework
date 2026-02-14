using System.Diagnostics;

namespace Luck.EventBus.OpenTelemetry;

/// <summary>
/// 用于订阅所有 DiagnosticListener 的观察者
/// </summary>
internal class AllDiagnosticListenersObserver : IObserver<DiagnosticListener>
{
    private readonly LuckEventBusInstrumentation _instrumentation;

    public AllDiagnosticListenersObserver(LuckEventBusInstrumentation instrumentation)
    {
        _instrumentation = instrumentation;
    }

    public void OnNext(DiagnosticListener listener)
    {
        _instrumentation.SubscribeToListener(listener);
    }

    public void OnError(Exception error) { }
    public void OnCompleted() { }
}
