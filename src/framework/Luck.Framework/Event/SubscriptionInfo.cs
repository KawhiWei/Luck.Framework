namespace Luck.Framework.Event
{
    /// <summary>
    /// 订阅信息
    /// </summary>
    public class SubscriptionInfo
    {


        private SubscriptionInfo(Type handlerType, Type eventType)
        {
    
            HandlerType = handlerType;
            EventType = eventType;
        }

        public Type HandlerType { get; }

        public Type EventType { get; }

        public static SubscriptionInfo Typed(Type handlerType, Type eventType) =>
             new SubscriptionInfo(handlerType, eventType);
    }
}
