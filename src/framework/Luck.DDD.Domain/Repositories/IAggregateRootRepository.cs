namespace Luck.DDD.Domain.Repositories
{
    public interface IAggregateRootRepository<TEntity, TKey> :
        IWriteRepository<TEntity, TKey>, IRepository<TEntity, TKey> where TEntity : IAggregateRootBase
    {

    }
}
