using Luck.DDD.Domain;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Luck.DDD.Domain.Domain.Entities;

namespace Luck.EntityFrameworkCore.Repositories
{
    public class EfCoreEntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>
        where TEntity : class, IEntityWithIdentity where TKey : IEquatable<TKey>
    {
        private readonly IDbContextFactory<LuckDbContextBase> _dbContextFactory;

        protected LuckDbContextBase DbContext { get; }

        public EfCoreEntityRepository(IDbContextFactory<LuckDbContextBase> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            DbContext = dbContextFactory.CreateDbContext();
        }

        public TEntity? Find(TKey primaryKey)
        {
            return DbContext.Find<TEntity>(primaryKey);
        }

        public IQueryable<TEntity> FindAll()
        {
            return DbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return FindAll().Where(predicate);
        }

        public ValueTask<TEntity?> FindAsync(TKey primaryKey)
        {
            return DbContext.FindAsync<TEntity>(primaryKey);
        }

        public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
    }
}