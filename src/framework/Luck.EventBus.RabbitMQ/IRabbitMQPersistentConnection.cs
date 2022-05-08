using RabbitMQ.Client;

namespace Luck.EventBus.RabbitMQ
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get;  }

        bool TryConnect();

        IModel CreateModel();
    }



}