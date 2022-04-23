using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Luck.EntityFrameworkCore.UnitOfWorks
{
    public class UnitOfWork: IUnitOfWork
    {

        private readonly LuckDbContextBase _dbContext;

        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(ILuckDbContext dbContext, ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext as LuckDbContextBase ?? throw new NotSupportedException();
            _logger = logger ?? throw new NotSupportedException();


        }

        public  async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
             
                return  await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _dbContext.Rollback();

                if (exception is DbUpdateConcurrencyException)
                {
                    _logger?.LogError(exception.Message);
                }

                throw;
            }

        }
    }
}
