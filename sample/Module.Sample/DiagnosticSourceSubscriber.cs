using System.Diagnostics;

namespace Module.Sample;

/// <summary>
/// Diagnostic Event Name Subscriber
/// 诊断事件处理者
/// </summary>
internal class  DiagnosticSourceSubscriber: IObserver<DiagnosticListener>
{
    private readonly List<IDisposable> _listenerSubscriptions;
    private readonly Func<string, LuckDiagnosticSourceListener> _handlerFactory;
    private IDisposable _allSourcesSubscription;

    public DiagnosticSourceSubscriber(LuckDiagnosticSourceListener handlerFactory)
    {
        _handlerFactory = _=>handlerFactory;
        this._listenerSubscriptions = new List<IDisposable>();
    }
    public void Subscribe()
    {
        if (this._allSourcesSubscription == null)
        {
            this._allSourcesSubscription = DiagnosticListener.AllListeners.Subscribe(this);
        }
    }
    
    
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(DiagnosticListener value)
    {
        var listener=new LuckDiagnosticSourceListener();
        value.Subscribe(listener);

    }
}