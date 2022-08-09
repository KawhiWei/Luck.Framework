namespace Luck.DDD.Domain.SqlRepositories;

public interface ISqlRepository<TEntity, in TKey> where TEntity : IEntity
{
    IEnumerable<TEntity> FindAll(string sql,object? param);
    
    TEntity? Find(TKey primaryKey);

    ValueTask<TEntity?> FindAsync(TKey primaryKey);
    
    
    Task<IEnumerable<TEntity>> FindAllAsync(string sql,object? param);

}