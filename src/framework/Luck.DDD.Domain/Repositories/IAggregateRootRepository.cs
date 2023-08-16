using Luck.DDD.Domain.Domain.AggregateRoots;

namespace Luck.DDD.Domain.Repositories
{
    public interface IAggregateRootRepository<TEntity, TKey> :
        IWriteRepository<TEntity, TKey>, IRepository<TEntity, TKey> where TEntity : IAggregateRootBase
    {

    }
}
