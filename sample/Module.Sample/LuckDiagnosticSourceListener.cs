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
        if (value.Key.Equals("Luck.EventBus.RabbitMQ.WritePublishBefore"))
        {
            Console.WriteLine($"当前事件Key:「{value.Key}」------Value{value.Value.Serialize()}");
        }
        
        if (value.Key.Equals("Microsoft.EntityFrameworkCore"))
        {
            Console.WriteLine($"当前事件Key:「{value.Key}」------Value{value.Value.Serialize()}");
        }
    }
}