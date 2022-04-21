
using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.Domian;
using Luck.Framework.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Luck.EntityFrameworkCore.Repositories
{
    public class EFCoreAggregateRootRepository<TEntity, TKey> : AggregateRootRepositoryBase<TEntity, TKey> where TEntity : class, IAggregateRootBase
        where TKey : IEquatable<TKey>
    {
        private readonly LuckDbContext _dbContext;

        public LuckDbContext DbContext => _dbContext;

        public EFCoreAggregateRootRepository(ILuckDbContext dbContext)
        {
            _dbContext = dbContext as LuckDbContext ?? throw new NotSupportedException();
        }

        public override void Add(TEntity entity)
        {
            _dbContext.Add(entity);
        }

        public override void AddRange(params object[] entities)
        {
            _dbContext.AddRange(entities);
        }

        public override void Attach(TEntity entity)
        {
            _dbContext.Attach(entity);
        }

        public override void AttachRange(params object[] entities)
        {
            _dbContext.AttachRange(entities);
        }

        public override void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }

        public override void UpdateRange(params object[] entities)
        {
            _dbContext.UpdateRange(entities);
        }

        public override void Remove(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public override void RemoveRange(params object[] entities)
        {
            _dbContext.RemoveRange(entities);
        }

        public override void BulkInsert(params object[] entities)
        {
            throw new NotImplementedException();
        }

        public override TEntity? Find(TKey primaryKey)
        {
            return _dbContext.Find<TEntity>(primaryKey);
        }

        public async override Task<TEntity?> FindAsync(TKey primaryKey)
        {
            return await _dbContext.FindAsync<TEntity>(primaryKey);
        }

        public override async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        protected override IQueryable<TEntity> DoFindQueryable()
        {
            return _dbContext.Set<TEntity>();
        }
    }
}
