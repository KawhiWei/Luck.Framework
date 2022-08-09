
using Luck.DDD.Domain;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Luck.EntityFrameworkCore.Repositories
{
    public class EfCoreAggregateRootRepository<TEntity, TKey> : AggregateRootRepositoryBase<TEntity, TKey> where TEntity : class, IAggregateRootBase
        where TKey : IEquatable<TKey>
    {
        private readonly LuckDbContextBase _dbContext;

        public LuckDbContextBase DbContext => _dbContext;

        public EfCoreAggregateRootRepository(ILuckDbContext dbContext)
        {
            _dbContext = dbContext as LuckDbContextBase ?? throw new NotSupportedException();
        }

        public override void Add(TEntity entity)
        {
            _dbContext.Add(entity);
        }

 
        public override void Attach(TEntity entity)
        {
            _dbContext.Attach(entity);
        }


        public override void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }


        public override void Remove(TEntity entity)
        {
            _dbContext.Remove(entity);
        }



        public override TEntity? Find(TKey primaryKey)
        {
            return _dbContext.Find<TEntity>(primaryKey);
        }

        public  override  ValueTask<TEntity?> FindAsync(TKey primaryKey)
        {
            return  _dbContext.FindAsync<TEntity>(primaryKey);
        }

        public override  Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return  _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        protected override IQueryable<TEntity> FindQueryable()
        {
            return _dbContext.Set<TEntity>();
        }
    }
}
