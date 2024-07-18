using Luck.DDD.Domain;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.Framework.UnitOfWorks;

namespace Luck.EntityFrameworkCore.Repositories
{
    public class EfCoreAggregateRootRepository<TEntity, TKey> : AggregateRootRepositoryBase<TEntity, TKey>
        where TEntity : class, IAggregateRootBase
        where TKey : IEquatable<TKey>
    {
        private readonly IDbContextFactory<LuckDbContextBase> _dbContextFactory;
        private readonly LuckDbContextBase _luckDbContextBase;
        protected LuckDbContextBase DbContext => _luckDbContextBase;

        public EfCoreAggregateRootRepository(IUnitOfWork unitOfWork)
        {
            _luckDbContextBase = unitOfWork.GetLuckDbContext() as LuckDbContextBase ??
                                 throw new ArgumentNullException(nameof(ILuckDbContext));
        }

        public override void Add(TEntity entity)
        {
            _luckDbContextBase.Add(entity);
        }

        public override void Attach(TEntity entity)
        {
            _luckDbContextBase.Attach(entity);
        }

        public override void Update(TEntity entity)
        {
            _luckDbContextBase.Update(entity);
        }

        public override void Remove(TEntity entity)
        {
            _luckDbContextBase.Remove(entity);
        }

        public override TEntity? Find(TKey primaryKey)
        {
            return _luckDbContextBase.Find<TEntity>(primaryKey);
        }

        public override ValueTask<TEntity?> FindAsync(TKey primaryKey)
        {
            return _luckDbContextBase.FindAsync<TEntity>(primaryKey);
        }

        public override Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _luckDbContextBase.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        protected override IQueryable<TEntity> FindQueryable()
        {
            return _luckDbContextBase.Set<TEntity>();
        }
    }
}