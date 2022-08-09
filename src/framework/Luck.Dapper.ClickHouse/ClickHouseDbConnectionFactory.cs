using System.Data;
using Luck.Dapper.DbConnectionFactories;
using Octonica.ClickHouseClient;

namespace Luck.Dapper.ClickHouse;

public class ClickHouseDbConnectionFactory : IDbConnectionFactory
{
    public IDbConnection GetDbConnection()
    {
        var sb = new ClickHouseConnectionStringBuilder();
        sb.Host = "192.168.31.20";
        sb.User = "kawhi";
        sb.Password = "wzw0126..";
        sb.Port = 9000;
        sb.Database = "db_name";
        var conn = new ClickHouseConnection(sb);
        conn.Open();
        return conn;
    }

    public async Task<IDbConnection> GetDbConnectionAsync()
    {
        var sb = new ClickHouseConnectionStringBuilder();
        sb.Host = "192.168.31.20";
        sb.User = "kawhi";
        sb.Password = "wzw0126..";
        sb.Port = 9000;
        sb.Database = "db_name";
        var conn = new ClickHouseConnection(sb);
        await conn.OpenAsync();
        return conn;
    }
}