using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luck.DDD.Domain
{
    public class AggregateRootBase : IAggregateRootBase, IDomainEvents
    {
        private string? _aggregateRootType;


        private string AggregateRootName
        {
            get
            {
                var aggregateRootType = GetType();
                _aggregateRootType = aggregateRootType.FullName;
                if (_aggregateRootType is null)
                {
                    throw new ArgumentNullException(nameof(_aggregateRootType));
                }

                return _aggregateRootType;
            }
        }


        [NotMapped]
        private readonly Queue<INotification> domainEvents = new();
        public void AddDomainEvent(INotification notification)
        {
            domainEvents.Enqueue(notification);
        }

        public void AddDomainEventIfAbsent(INotification notification)
        {
            if (!domainEvents.Contains(notification))
            {
                domainEvents.Enqueue(notification);
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
