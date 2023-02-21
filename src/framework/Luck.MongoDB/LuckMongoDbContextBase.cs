using System.Diagnostics.CodeAnalysis;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.Exceptions;
using Luck.Framework.Extensions;
using MongoDB.Driver;

namespace Luck.MongoDB;

public class LuckMongoDbContextBase : IDisposable,ILuckDbContext
{
    private readonly MongoDbContextOption _options;

    protected LuckMongoDbContextBase([NotNull] MongoDbContextOption options)
    {
        _options = options;
    }

    /// <summary>
    /// 连接字符串
    /// </summary>
    private string ConnectionString => _options.ConnectionString;

    /// <summary>
    /// 文档库名称
    /// </summary>
    private IMongoDatabase Database { get;  set; } = default!;

    /// <summary>
    /// 获取一个文档
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public IMongoCollection<TEntity> Collection<TEntity>()
    {
        return Database.GetCollection<TEntity>(GetTableName<TEntity>());
    }

    /// <summary>
    /// 获取表名
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    private static string GetTableName<TEntity>()
    {
        var t = typeof(TEntity);
        var table = t.GetAttribute<MongoDbTableAttribute>();

        if (table == null)
        {
            return t.Name;
        }

        if (table.TableName.IsNullOrEmpty())
        {
            throw new LuckException("Table name does not exist!");
        }

        return table.TableName;
    }

    /// <summary>
    /// 连接字符串
    /// </summary>
    /// <returns></returns>
    private IMongoDatabase GetDbContext()
    {
        var mongoUrl = new MongoUrl(ConnectionString);
        var databaseName = mongoUrl.DatabaseName;
        if (databaseName.IsNullOrEmpty())
        {
            throw new LuckException($"{mongoUrl}不存DatabaseName名称");
        }

        var database = new MongoClient(mongoUrl).GetDatabase(databaseName);
        return database;
    }

    public void Dispose()
    {
    }
}