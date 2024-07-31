using Luck.DDD.Domain;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Luck.DDD.Domain.Domain.Entities;
using Luck.Framework.UnitOfWorks;

namespace Luck.EntityFrameworkCore.Repositories
{
    public class EfCoreEntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>
        where TEntity : class, IEntityWithIdentity where TKey : IEquatable<TKey>
    {
        private readonly LuckDbContextBase _luckDbContextBase;

        public EfCoreEntityRepository(IUnitOfWork unitOfWork)
        {
            _luckDbContextBase = unitOfWork.GetLuckDbContext() as LuckDbContextBase ??
                                 throw new ArgumentNullException(nameof(ILuckDbContext));
        }

        public TEntity? Find(TKey primaryKey)
        {
            return _luckDbContextBase.Find<TEntity>(primaryKey);
        }

        public IQueryable<TEntity> FindAll()
        {
            return _luckDbContextBase.Set<TEntity>();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return FindAll().Where(predicate);
        }

        public ValueTask<TEntity?> FindAsync(TKey primaryKey)
        {
            return _luckDbContextBase.FindAsync<TEntity>(primaryKey);
        }

        public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _luckDbContextBase.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
    }
}