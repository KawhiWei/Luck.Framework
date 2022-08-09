using System.Data;

namespace Luck.DDD.Domain.SqlRepositories;

public interface ISqlWriteRepository<TEntity, TKey> where TEntity : IEntity
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="entity"></param>
    /// <param name="transaction"></param>
    int Add(string sql,TEntity entity, IDbTransaction? transaction = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="entity"></param>
    /// <param name="transaction"></param>
    int Update(string sql,TEntity entity, IDbTransaction? transaction = null);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="entity"></param>
    /// <param name="transaction"></param>
    int Remove(string sql,TEntity entity, IDbTransaction? transaction = null);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="entity"></param>
    /// <param name="transaction"></param>
    Task<int> AddAsync(string sql,TEntity entity, IDbTransaction? transaction = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="entity"></param>
    /// <param name="transaction"></param>
    Task<int> UpdateAsync(string sql,TEntity entity, IDbTransaction? transaction = null);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="entity"></param>
    /// <param name="transaction"></param>
    Task<int> RemoveAsync(string sql,TEntity entity, IDbTransaction? transaction = null);
}