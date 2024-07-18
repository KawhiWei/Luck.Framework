using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.Exceptions;
using Luck.Framework.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Luck.EntityFrameworkCore.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        //private readonly IDbContextFactory<ILuckDbContext> _dbContextFactory;
        protected LuckDbContextBase DbContext { get; }

        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(ILogger<UnitOfWork> logger, ILuckDbContext dbContext)
        {
            DbContext = dbContext as LuckDbContextBase ?? throw new ArgumentNullException(nameof(ILuckDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger));
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                DbContext.Rollback();

                if (exception is DbUpdateConcurrencyException)
                {
                    _logger?.LogError(exception.Message);
                }

                throw;
            }
        }
        
        public ILuckDbContext GetLuckDbContext()
        {
            return DbContext;
        }
    }
}