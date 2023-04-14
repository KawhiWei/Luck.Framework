using System.Data;
// using ClickHouse.Client.ADO;
using Luck.Dapper.DbConnectionFactories;
using Luck.Framework.Exceptions;
using Microsoft.Extensions.Options;
using Octonica.ClickHouseClient;

namespace Luck.Dapper.ClickHouse;

public class ClickHouseDbConnectionFactory : IDbConnectionFactory
{
    private readonly ClickHouseConnectionConfig _clickHouseConnectionConfig;

    public ClickHouseDbConnectionFactory(IOptions<ClickHouseConnectionConfig> clickHouseConfig)
    {
        _clickHouseConnectionConfig = clickHouseConfig.Value;
    }

    public IDbConnection GetDbConnection()
    {
        var conn = new ClickHouseConnection(GetClickHouseConnectionStringBuilder());
        conn.Open();
        return conn;
    }

    public async Task<IDbConnection> GetDbConnectionAsync()
    {
        var conn = new ClickHouseConnection("");
        await conn.OpenAsync();
        return conn;
    }

    private string GetClickHouseConnectionStringBuilder()
    {
        string connectionString;
        ConnectionStringOptions? connectionOption;
        if (!_clickHouseConnectionConfig.IsCluster)
        {
            connectionOption = _clickHouseConnectionConfig.ConnectionOptionList.FirstOrDefault();

            if (connectionOption is null)
            {
                throw new LuckException("无法找到数据库链接字符串，请检查数据库连接字符串是否配置正确！");
            }

            connectionString =
                $"Host={connectionOption.Host};Port={connectionOption.Port};User={connectionOption.User};Password={connectionOption.Password};Database={connectionOption.Database}";
        }
        else
        {
            connectionOption = _clickHouseConnectionConfig.ConnectionOptionList[0];
            connectionString =
                $"Host={connectionOption.Host};Port={connectionOption.Port};User={connectionOption.User};Password={connectionOption.Password};Database={connectionOption.Database}";
        }

        return connectionString;
    }
}