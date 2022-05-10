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
            throw new NotImplementedException();
        }

        public string GetEventKey<T>()
        {

            return typeof(T).Name;
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
            throw new NotImplementedException();
        }


    }
}
