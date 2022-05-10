using Luck.Framework.Event;


namespace Luck.EventBus.RabbitMQ
{


    public class RabbitMQEventBusSubscriptionsManager: IIntegrationEventBusSubscriptionsManager
    {

        private readonly Dictionary<string, List<Type>> _handlers= new Dictionary<string, List<Type>>();

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Type> GetHandlersForEvent<T>() where T : IIntegrationEvent
        {
            throw new NotImplementedException();
        }

        public bool HasSubscriptionsForEvent(string eventName)
        {
            throw new NotImplementedException();
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }
    }
}
