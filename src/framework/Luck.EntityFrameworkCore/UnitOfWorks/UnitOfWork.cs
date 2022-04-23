using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.UnitOfWorks
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly LuckDbContextBase _dbContext;

        public UnitOfWork(ILuckDbContext dbContext)
        {
            _dbContext = dbContext as LuckDbContextBase ?? throw new NotSupportedException();
        }
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
             
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
