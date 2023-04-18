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
        var connectionOptionList = new List<ConnectionStringOptions>
        {
            new()
            {
                Host = "192.168.31.20",
                Port = 9000,
                User = "kawhi",
                Password = "wzw0126..",
                Database = "luck_asa",
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