using Luck.DDD.Domain.Domain.Entities;

namespace Luck.DDD.Domain.Repositories
{
    public interface IEntityRepository<TEntity, TKey> :
        IRepository<TEntity, TKey> where TEntity : IEntityWithIdentity
    {

    }
}
