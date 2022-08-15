using Luck.Dapper.DbConnectionFactories;
using Luck.DDD.Domain;
using Luck.DDD.Domain.SqlRepositories;

namespace Luck.Dapper.Repositories;

public class DapperEntityRepository<TEntity, TKey> :
    ISqlEntityRepository<TEntity, TKey> where TEntity : IEntityWithIdentity
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public DapperEntityRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public IEnumerable<TEntity> FindAll(string sql, object? param)
    {
        using var conn = _dbConnectionFactory.GetDbConnection();
        return conn.Query<TEntity>(sql, param);
    }

    public TEntity? Find(string sql, object? param)
    {
        using var conn = _dbConnectionFactory.GetDbConnection();
        return conn.QueryFirstOrDefault<TEntity>(sql,param);
    }

    public async ValueTask<TEntity?> FindAsync(string sql, object? param)
    {
        using var conn = await _dbConnectionFactory.GetDbConnectionAsync();
        return await conn.QueryFirstOrDefaultAsync<TEntity>(sql,param);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(string sql, object? param)
    {
        using var conn = await _dbConnectionFactory.GetDbConnectionAsync();
        return await conn.QueryAsync<TEntity>(sql, param);
    }
}