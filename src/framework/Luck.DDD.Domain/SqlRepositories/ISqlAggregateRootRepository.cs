using Luck.DDD.Domain.Domain.AggregateRoots;

namespace Luck.DDD.Domain.SqlRepositories;

public interface ISqlAggregateRootRepository<TEntity, TKey>:ISqlWriteRepository<TEntity, TKey>,ISqlRepository<TEntity, TKey>where TEntity : IAggregateRootBase
{
    
}