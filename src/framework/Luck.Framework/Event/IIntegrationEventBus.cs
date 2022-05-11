namespace Luck.Framework.Event
{
    /// <summary>
    /// 发送事件接口定义
    /// </summary>
    public interface IIntegrationEventBus
    {
        void Publish<TEvent>(IIntegrationEvent @event) where TEvent : IIntegrationEvent;






        void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
    }
}
