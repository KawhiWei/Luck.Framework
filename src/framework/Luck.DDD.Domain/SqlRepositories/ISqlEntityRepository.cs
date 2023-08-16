using Luck.DDD.Domain.Domain.Entities;

namespace Luck.DDD.Domain.SqlRepositories;

public interface ISqlEntityRepository<TEntity, in TKey> :
    ISqlRepository<TEntity, TKey> where TEntity : IEntityWithIdentity
{

}