using Luck.Framework.Domian;
using System.Linq.Expressions;

namespace Luck.Framework.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : IEntity
    {

        TEntity? Find(TKey primaryKey);

        Task<TEntity?> FindAsync(TKey primaryKey);

        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> FindAll();

        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
    }
}
