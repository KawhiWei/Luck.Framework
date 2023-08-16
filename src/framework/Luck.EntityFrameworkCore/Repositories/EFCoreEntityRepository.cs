using Luck.DDD.Domain;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Luck.DDD.Domain.Domain.Entities;

namespace Luck.EntityFrameworkCore.Repositories
{
    public class EfCoreEntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey> where TEntity : class, IEntityWithIdentity where TKey : IEquatable<TKey>
    {

        private readonly LuckDbContextBase _dbContext;

        public LuckDbContextBase DbContext => _dbContext;

        public EfCoreEntityRepository(ILuckDbContext dbContext)
        {
            _dbContext = dbContext as LuckDbContextBase ?? throw new NotSupportedException();
        }

        public TEntity? Find(TKey primaryKey)
        {
            return _dbContext.Find<TEntity>(primaryKey);
        }

        public IQueryable<TEntity> FindAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return FindAll().Where(predicate);
        }

        public  ValueTask<TEntity?> FindAsync(TKey primaryKey)
        {
            return  _dbContext.FindAsync<TEntity>(primaryKey);
        }

        public  Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return  _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
    }
}
