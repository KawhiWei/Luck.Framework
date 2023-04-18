using System.Data;

namespace Luck.Dapper.DbConnectionFactories;

public interface IDapperDrivenProvider
{
    /// <summary>
    /// 获取数据库连接
    /// </summary>
    /// <returns></returns>
    IDbConnection GetDbConnection();


    /// <summary>
    /// 异步获取数据库连接
    /// </summary>
    /// <returns></returns>
    Task<IDbConnection> GetDbConnectionAsync();
}