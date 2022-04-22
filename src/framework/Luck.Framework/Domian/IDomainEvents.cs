using MediatR;
using System.Collections.Generic;

namespace Luck.Framework.Domian
{
    public interface IDomainEvents
    {
        IEnumerable<INotification> GetDomainEvents();
        void AddDomainEvent(INotification notification);

        void AddDomainEventIfAbsent(INotification notification);
        public void ClearDomainEvents();
    }
}
