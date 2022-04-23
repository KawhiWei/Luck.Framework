using System.Linq.Expressions;

namespace Luck.DDD.Domain.Repositories
{
    public abstract class AggregateRootRepositoryBase<TEntity, TKey> :
        IAggregateRootRepository<TEntity, TKey>
        where TEntity : class, IAggregateRootBase
    {

     
        public abstract void Add(TEntity entity);

     

        public abstract void Remove(TEntity entity);



        public abstract void Update(TEntity entity);


        public abstract void Attach(TEntity entity);



        public abstract TEntity? Find(TKey primaryKey);

        public IQueryable<TEntity> FindAll()
        {
            return FindQueryable();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return FindQueryable().Where(predicate);
        }

        public abstract ValueTask<TEntity?> FindAsync(TKey primaryKey);

        public abstract Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);


        protected abstract IQueryable<TEntity> FindQueryable();
    }
}
