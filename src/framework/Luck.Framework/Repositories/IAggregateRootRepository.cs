using Luck.Framework.Domian;

namespace Luck.Framework.Repositories
{
    public interface IAggregateRootRepository<TEntity, TKey> :
        IWriteRepository<TEntity,TKey>, IRepository<TEntity, TKey> where TEntity : IAggregateRootBase
    {

    }
}
