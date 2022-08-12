using System.Data;
using Luck.Dapper.DbConnectionFactories;
using Microsoft.Extensions.Options;
using Octonica.ClickHouseClient;

namespace Luck.Dapper.ClickHouse;

public class ClickHouseDbConnectionFactory : IDbConnectionFactory
{
    private readonly ClickHouseConfig _clickHouseConfig;

    public ClickHouseDbConnectionFactory(IOptions<ClickHouseConfig> clickHouseConfig)
    {
        _clickHouseConfig = clickHouseConfig.Value;
    }

    public IDbConnection GetDbConnection()
    {
        var conn = new ClickHouseConnection(GetClickHouseConnectionStringBuilder());
        conn.Open();
        return conn;
    }

    public async Task<IDbConnection> GetDbConnectionAsync()
    {
        var conn = new ClickHouseConnection(GetClickHouseConnectionStringBuilder());
        await conn.OpenAsync();
        return conn;
    }

    private ClickHouseConnectionStringBuilder GetClickHouseConnectionStringBuilder()
    {
        var clickHouseConnectionString = new ClickHouseConnectionStringBuilder
        {
            Host = _clickHouseConfig.Host,
            User = _clickHouseConfig.User,
            Password = _clickHouseConfig.Password,
            Port = _clickHouseConfig.Port,
            Database = _clickHouseConfig.Database,
            ReadWriteTimeout = _clickHouseConfig.ReadWriteTimeout,
            BufferSize = _clickHouseConfig.BufferSize,
            Compress = _clickHouseConfig.Compress
        };
        return clickHouseConnectionString;
    }
}