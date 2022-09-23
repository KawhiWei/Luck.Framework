namespace Luck.Framework.Event
{
    /// <summary>
    /// 发送事件接口定义
    /// </summary>
    public interface IIntegrationEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="event"></param>
        /// <typeparam name="TEvent"></typeparam>
        void Publish<TEvent>(TEvent @event) where TEvent : IIntegrationEvent;
        
        
        /// <summary>
        /// 订阅者开启
        /// </summary>
        void Subscribe();
    }
}