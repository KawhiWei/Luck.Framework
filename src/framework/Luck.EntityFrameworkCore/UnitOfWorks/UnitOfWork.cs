using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Luck.EntityFrameworkCore.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextFactory<LuckDbContextBase> _dbContextFactory;
        protected LuckDbContextBase DbContext { get; }

        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(ILogger<UnitOfWork> logger, IDbContextFactory<LuckDbContextBase> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            DbContext = dbContextFactory.CreateDbContext();
            _logger = logger ?? throw new NotSupportedException();
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
    }
}