using System.Diagnostics;

namespace Luck.EventBus.OpenTelemetry;

/// <summary>
/// 包装单个 DiagnosticListener 订阅的辅助类
/// </summary>
internal class SingleDiagnosticListenerSubscription : IDisposable
{
    public DiagnosticListener Listener { get; }
    private readonly IDisposable _subscription;

    public SingleDiagnosticListenerSubscription(DiagnosticListener listener, IDisposable subscription)
    {
        Listener = listener;
        _subscription = subscription;
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}
