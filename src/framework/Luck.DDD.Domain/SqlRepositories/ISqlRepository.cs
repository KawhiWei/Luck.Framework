namespace Luck.DDD.Domain.SqlRepositories;

public interface ISqlRepository<TEntity, in TKey> where TEntity : IEntity
{
    IEnumerable<TEntity> FindAll(string sql,object? param);
    
    TEntity? Find(string sql,  object? param);

    ValueTask<TEntity?> FindAsync(string sql,  object? param);
    
    
    Task<IEnumerable<TEntity>> FindAllAsync(string sql,object? param);

}