
﻿using MediatR;


﻿namespace Luck.DDD.Domain
{
    public class Entity : IEntityWithIdentity, IDomainEvents
    {

        private readonly IList<INotification> domainEvents = new List<INotification>();
        public void AddDomainEvent(INotification notification)
        {
            domainEvents.Add(notification);
        }

        public void AddDomainEventIfAbsent(INotification notification)
        {
            if (!domainEvents.Contains(notification))
            {
                domainEvents.Add(notification);
            }
        }

        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return domainEvents;
        }
    }
}
