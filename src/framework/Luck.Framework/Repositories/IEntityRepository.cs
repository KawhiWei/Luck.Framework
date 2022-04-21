using Luck.Framework.Domian;

namespace Luck.Framework.Repositories
{
    public interface IEntityRepository<TEntity, TKey> :
        IRepository<TEntity, TKey> where TEntity : IEntityWithIdentity
    {

    }
}
