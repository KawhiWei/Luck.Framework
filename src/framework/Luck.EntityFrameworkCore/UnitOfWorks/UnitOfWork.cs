using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.UnitOfWorks
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly LuckDbContext _dbContext;

        public UnitOfWork(ILuckDbContext dbContext)
        {
            _dbContext = dbContext as LuckDbContext ?? throw new NotSupportedException();
        }
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var e in _dbContext.ChangeTracker.Entries())
                {
                    //if (e.Entity is IAggregateRoot r)
                    //{
                    //    var domainEvents = r.GetDomainEvents();
                    //    foreach (var domainEvent in domainEvents)
                    //    {
                    //        //await _mediator.PublishAsync(notification, cancellationToken);
                    //    }
                    //    r.ClearDomainEvents();
                    //}
                }
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                _dbContext.Rollback();

                if (exception is DbUpdateConcurrencyException)
                {

                }

                throw;
            }

        }
    }
}
