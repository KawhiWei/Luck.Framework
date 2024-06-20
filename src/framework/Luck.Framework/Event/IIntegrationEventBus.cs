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
        /// <param name="priority"></param>
        /// <typeparam name="TEvent"></typeparam>
        void Publish<TEvent>(TEvent @event, byte? priority = 1) where TEvent : IIntegrationEvent;


        /// <summary>
        /// 订阅者开启
        /// </summary>
        void Subscribe();
    }
}