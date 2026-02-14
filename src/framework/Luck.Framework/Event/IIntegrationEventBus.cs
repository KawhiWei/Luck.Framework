namespace Luck.Framework.Event
{
    /// <summary>
    /// 发送事件接口定义（纯异步）
    /// </summary>
    public interface IIntegrationEventBus
    {
        /// <summary>
        /// 发布事件（异步）
        /// </summary>
        Task PublishAsync<TEvent>(TEvent @event, byte? priority = 1, CancellationToken cancellationToken = default) 
            where TEvent : IIntegrationEvent;

        /// <summary>
        /// 订阅者开启（异步）
        /// </summary>
        Task SubscribeAsync(CancellationToken cancellationToken = default);
    }
}