using Luck.DDD.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore
{
    public static class MediatorExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx, CancellationToken cancellationToken = default)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<IDomainEvents>()
                .Where(x => x.Entity.GetDomainEvents().Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents())
                .ToList();//加ToList()是为立即加载，否则会延迟执行，到foreach的时候已经被ClearDomainEvents()了

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
