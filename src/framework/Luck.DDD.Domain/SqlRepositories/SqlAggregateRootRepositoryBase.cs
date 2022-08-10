using System.Data;

namespace Luck.DDD.Domain.SqlRepositories;

public abstract class SqlAggregateRootRepositoryBase<TEntity, TKey> :
    ISqlAggregateRootRepository<TEntity, TKey>
    where TEntity : class, IAggregateRootBase
{
    public abstract int Add(string sql,TEntity entity, IDbTransaction? transaction = null);

    public abstract int Update(string sql,TEntity entity, IDbTransaction? transaction = null);

    public abstract int Remove(string sql,TEntity entity, IDbTransaction? transaction = null);
   

    public abstract Task<int> AddAsync(string sql,TEntity entity, IDbTransaction? transaction = null);


    public abstract Task<int> UpdateAsync(string sql,TEntity entity, IDbTransaction? transaction = null);

    public abstract Task<int> RemoveAsync(string sql,TEntity entity, IDbTransaction? transaction = null);

    public abstract IEnumerable<TEntity> FindAll(string sql, object? param);

    public abstract Task<IEnumerable<TEntity>> FindAllAsync(string sql,object? param);
    
    public abstract TEntity? Find(string sql,  object? param);

    public abstract ValueTask<TEntity?> FindAsync(string sql,  object? param);
    
    
}