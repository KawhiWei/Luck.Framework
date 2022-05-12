using Luck.Framework.Event;


namespace Luck.EventBus.RabbitMQ
{


    public class RabbitMQEventBusSubscriptionsManager : IIntegrationEventBusSubscriptionsManager
    {

        private readonly Dictionary<string, List<Type>> _handlers = new Dictionary<string, List<Type>>();

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventKey = GetEventKey<T>();
            DoAddSubscription(typeof(TH), eventKey);
        }

        public void AddSubscription(Type eventType, Type handlerType)
        {
            var eventKey = GetEventKey(eventType);
            DoAddSubscription(handlerType, eventKey);
        }

        private void DoAddSubscription(Type handlerType, string eventName)
        {

            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(o => o == handlerType))
            {
                throw new ArgumentException(
                   $"处理器类型 {handlerType.Name} 已经注册 '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);
        }



        public IEnumerable<Type> GetHandlersForEvent<T>() where T : IIntegrationEvent
        {

            var eventKey = GetEventKey<T>();
            return GetHandlersForEvent(eventKey);

        }

        public IEnumerable<Type> GetHandlersForEvent(string eventName)
        {

            return _handlers[eventName];
        }

        public bool HasSubscriptionsForEvent(string eventName)
        {


            return _handlers.ContainsKey(eventName);
        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {

            var eventKey = GetEventKey<T>();
            return HasSubscriptionsForEvent(eventKey);
        }



        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {

            if (!HasSubscriptionsForEvent<T>())
            {
                return;
            }

            var eventName = GetEventKey<T>();
            _handlers[eventName].Remove(typeof(TH));
            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
                EventRemovedEventArgs args = new EventRemovedEventArgs();
                args.EventType = typeof(T);
                OnEventRemoved?.Invoke(this, args);
            }

        }

        public event EventHandler<EventRemovedEventArgs>? OnEventRemoved;

        public bool IsEmpty => !_handlers.Keys.Any();

        public void Clear() => _handlers.Clear();


        public string GetEventKey<T>()
        {


            return GetEventKey(typeof(T));
        }
        public string GetEventKey(Type type)
        {

            return type.Name;
        }

   
    }
}
