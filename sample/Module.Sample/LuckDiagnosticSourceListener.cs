using System.Diagnostics;
using Luck.EventBus.RabbitMQ.Diagnostics;
using Luck.Framework.Extensions;

namespace Module.Sample;

/// <summary>
/// 
/// </summary>
internal class LuckDiagnosticSourceListener : IObserver<KeyValuePair<string, object>>
{
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(KeyValuePair<string, object> value)
    {
        // _handlerFactory(value.Key);
        if (value.Key.StartsWith("Luck"))
        {
            Console.WriteLine($"当前事件Key:「{value.Key}」Value：{value.Value.Serialize()}");
        }
        Activity.Current?.AddEvent(new ActivityEvent(value.Key, DateTimeOffset.Now, new ActivityTagsCollection { new(value.Key, value.Value) }));
    }
}