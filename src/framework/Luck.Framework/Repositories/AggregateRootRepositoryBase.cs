using Luck.Framework.Domian;
using System.Linq.Expressions;

namespace Luck.Framework.Repositories
{
    public abstract class AggregateRootRepositoryBase<TEntity, TKey> :
        IAggregateRootRepository<TEntity, TKey>
        where TEntity :class, IAggregateRootBase
    {

        public AggregateRootRepositoryBase()
        {

        }
        public abstract void Add(TEntity entity);

        public abstract void AddRange(params object[] entities);

        public abstract void Remove(TEntity entity);

        public abstract void RemoveRange(params object[] entities);

        public abstract void Update(TEntity entity);

        public abstract void UpdateRange(params object[] entities);

        public abstract void Attach(TEntity entity);

        public abstract void AttachRange(params object[] entities);

        public abstract void BulkInsert(params object[] entities);

        public abstract TEntity? Find(TKey primaryKey);

        public IQueryable<TEntity> FindAll()
        {
            return DoFindQueryable();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return DoFindQueryable().Where(predicate);
        }

        public abstract Task<TEntity?> FindAsync(TKey primaryKey);

        public abstract Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);


        protected abstract  IQueryable<TEntity> DoFindQueryable();
    }
}
