using Luck.DDD.Domain;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Luck.DDD.Domain.Domain.AggregateRoots;

namespace Luck.EntityFrameworkCore.Repositories
{
    public class EfCoreAggregateRootRepository<TEntity, TKey> : AggregateRootRepositoryBase<TEntity, TKey>
        where TEntity : class, IAggregateRootBase
        where TKey : IEquatable<TKey>
    {
        private readonly IDbContextFactory<LuckDbContextBase> _dbContextFactory;
        protected LuckDbContextBase DbContext { get; }

        public EfCoreAggregateRootRepository(
            IDbContextFactory<LuckDbContextBase> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            DbContext = dbContextFactory.CreateDbContext();
        }

        public override void Add(TEntity entity)
        {
            DbContext.Add(entity);
        }


        public override void Attach(TEntity entity)
        {
            DbContext.Attach(entity);
        }


        public override void Update(TEntity entity)
        {
            DbContext.Update(entity);
        }


        public override void Remove(TEntity entity)
        {
            DbContext.Remove(entity);
        }


        public override TEntity? Find(TKey primaryKey)
        {
            return DbContext.Find<TEntity>(primaryKey);
        }

        public override ValueTask<TEntity?> FindAsync(TKey primaryKey)
        {
            return DbContext.FindAsync<TEntity>(primaryKey);
        }

        public override Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        protected override IQueryable<TEntity> FindQueryable()
        {
            return DbContext.Set<TEntity>();
        }
    }
}