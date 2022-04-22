using MediatR;

namespace Luck.DDD.Domain
{
    public interface IDomainEvents
    {
        IEnumerable<INotification> GetDomainEvents();
        void AddDomainEvent(INotification notification);

        void AddDomainEventIfAbsent(INotification notification);
        public void ClearDomainEvents();
    }
}
