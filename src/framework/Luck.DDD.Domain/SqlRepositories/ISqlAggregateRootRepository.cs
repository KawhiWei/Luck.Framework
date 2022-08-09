namespace Luck.DDD.Domain.SqlRepositories;

public interface ISqlAggregateRootRepository<TEntity, TKey>:ISqlWriteRepository<TEntity, TKey>,ISqlRepository<TEntity, TKey>where TEntity : IAggregateRootBase
{
    
}