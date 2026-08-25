using System;
using System.Collections.Generic;
using Luck.Dapper.ClickHouse;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.UnitTest.ClickHouse_Tests;

/// <summary>
/// 
/// </summary>
public class ClickHouseTestModule : ClickHouseBaseModule
{
    protected override void AddConnectionString(IServiceCollection service)
    {
        var port = uint.TryParse(Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_PORT"), out var configuredPort)
            ? configuredPort
            : 9000;
        var connectionOptionList = new List<ConnectionStringOptions>
        {
            new()
            {
                Host = Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_HOST") ?? "localhost",
                Port = port,
                User = Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_USER") ?? "default",
                Password = Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_PASSWORD") ?? string.Empty,
                Database = Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_DATABASE") ?? "default",
            }
        };
        service.AddClickHouseDbConnectionString(x =>
        {
            x.IsCluster = false;
            x.ConnectionOptionList = connectionOptionList;
        });
    }

    protected override void AddDbDriven(IServiceCollection service)
    {
        service.AddClickHouseDapperDriven();
    }
}
