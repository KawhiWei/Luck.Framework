namespace Luck.DDD.Domain.SqlRepositories;

public interface ISqlEntityRepository<TEntity, in TKey> :
    ISqlRepository<TEntity, TKey> where TEntity : IEntityWithIdentity
{

}