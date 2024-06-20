using RabbitMQ.Client;

namespace Luck.EventBus.RabbitMQ.Abstraction
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get;  }

        bool TryConnect();

        void ReturnChannel(IModel channel);
        
        /// <summary>
        /// 从池中获取Channel
        /// </summary>
        /// <returns></returns>
        IModel GetChannel();

        IModel CreateModel();
    }



}