using System.Data;
using Luck.Dapper.DbConnectionFactories;
using Luck.DDD.Domain;
using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.DDD.Domain.SqlRepositories;

namespace Luck.Dapper.Repositories;

public class DapperAggregateRootRepositoryBase<TEntity, TKey> : SqlAggregateRootRepositoryBase<TEntity, TKey> where TEntity : class, IAggregateRootBase
    where TKey : IEquatable<TKey>
{
    private readonly IDapperDrivenProvider _dapperDrivenProvider;

    public DapperAggregateRootRepositoryBase(IDapperDrivenProvider dapperDrivenProvider)
    {
        _dapperDrivenProvider = dapperDrivenProvider;
    }

    public override int Add(string sql, TEntity entity, IDbTransaction? transaction = null)
    {
        IDbConnection conn;
        if (transaction?.Connection  is not null)
        {
            conn = transaction.Connection;
            return conn.Execute(sql, entity, transaction);
        }

        using (conn = _dapperDrivenProvider.GetDbConnection())
        {
            return conn.Execute(sql, entity);
        }
    }

    public override int Update(string sql, TEntity entity, IDbTransaction? transaction = null)
    {
        IDbConnection conn;
        if (transaction?.Connection  is not null)
        {
            conn = transaction.Connection;
            return conn.Execute(sql, entity, transaction);
        }

        using (conn = _dapperDrivenProvider.GetDbConnection())
        {
            return conn.Execute(sql, entity);
        }
    }

    public override int Remove(string sql, TEntity entity, IDbTransaction? transaction = null)
    {
        IDbConnection conn;
        if (transaction?.Connection  is not null)
        {
            conn = transaction.Connection;
            return conn.Execute(sql, entity, transaction);
        }

        using (conn = _dapperDrivenProvider.GetDbConnection())
        {
            return conn.Execute(sql, entity);
        }
    }

    public override async Task<int> AddAsync(string sql, TEntity entity, IDbTransaction? transaction = null)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        IDbConnection conn;
        if (transaction?.Connection  is not null)
        {
            conn = transaction.Connection;
            return await conn.ExecuteAsync(sql, entity, transaction);
        }

        using (conn = await _dapperDrivenProvider.GetDbConnectionAsync())
        {
            return await conn.ExecuteAsync(sql, entity);
        }
    }

    public override async Task<int> UpdateAsync(string sql, TEntity entity, IDbTransaction? transaction = null)
    {
        IDbConnection conn;
        if (transaction?.Connection  is not null)
        {
            conn = transaction.Connection;
            return await conn.ExecuteAsync(sql, entity, transaction);
        }

        using (conn = await _dapperDrivenProvider.GetDbConnectionAsync())
        {
            return await conn.ExecuteAsync(sql, entity);
        }
    }

    public override async Task<int> RemoveAsync(string sql, TEntity entity, IDbTransaction? transaction = null)
    {
        IDbConnection conn;
        if (transaction?.Connection  is not null)
        {
            conn = transaction.Connection;
            return await conn.ExecuteAsync(sql, entity, transaction);
        }

        using (conn = await _dapperDrivenProvider.GetDbConnectionAsync())
        {
            return await conn.ExecuteAsync(sql, entity);
        }
    }

    public override IEnumerable<TEntity> FindAll(string sql, object? param)
    {
        using var conn = _dapperDrivenProvider.GetDbConnection();
        return conn.Query<TEntity>(sql, param);
    }

    public override async Task<IEnumerable<TEntity>> FindAllAsync(string sql, object? param)
    {
        using var conn = await _dapperDrivenProvider.GetDbConnectionAsync();
        return await conn.QueryAsync<TEntity>(sql, param);
    }

    public override TEntity? Find(string sql,  object? param)
    {
        using var conn = _dapperDrivenProvider.GetDbConnection();
        return conn.QueryFirstOrDefault<TEntity>(sql,param);
    }

    public override async ValueTask<TEntity?> FindAsync(string sql,  object? param)
    {
        using var conn = await _dapperDrivenProvider.GetDbConnectionAsync();
        return await conn.QueryFirstOrDefaultAsync<TEntity>(sql,param);
    }
}