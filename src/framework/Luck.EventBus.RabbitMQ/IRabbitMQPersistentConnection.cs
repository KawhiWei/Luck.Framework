using RabbitMQ.Client;

namespace Luck.EventBus.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get;  }

        bool TryConnect();

        IModel CreateModel();
    }



}