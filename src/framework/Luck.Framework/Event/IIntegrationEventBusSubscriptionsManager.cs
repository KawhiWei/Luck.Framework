namespace Luck.Framework.Event
{
    public interface IIntegrationEventBusSubscriptionsManager
    {
     


        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void AddSubscription<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 移除订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void RemoveSubscription<T, TH>()
                 where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 获取事件处理程序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        IEnumerable<Type> GetHandlersForEvent<T>()
           where T : IIntegrationEvent;

        /// <summary>
        /// 获取事件处理程序
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        IEnumerable<Type> GetHandlersForEvent(string eventName);


        /// <summary>
        /// 判断订阅者是否存在
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        bool HasSubscriptionsForEvent(string eventName);

        /// <summary>
        /// 获取事件密钥
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetEventKey<T>();
    }
}
