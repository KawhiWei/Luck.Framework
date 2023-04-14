using System;
using System.Collections.Generic;
using System.Data;
using Luck.Dapper.ClickHouse;
using Luck.Dapper.DbConnectionFactories;
using Luck.Framework.Infrastructure;
using Luck.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace Luck.UnitTest.ClickHouse_Tests;

public class ClickHouseTest : IntegratedTest<ClickHouseTestModule>
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClickHouseTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Connection_Test_Open()
    {
        var hostEnvironment = ServiceProvider.GetService<IHostEnvironment>();
        var dbConnectionFactory = ServiceProvider.GetService<IDbConnectionFactory>();
        if (dbConnectionFactory is null)
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        var dbConnection = dbConnectionFactory.GetDbConnection();
        dbConnection.Open();
        Assert.True(dbConnection.State == ConnectionState.Open);
    }
}

/// <summary>
/// 
/// </summary>
public class ClickHouseTestModule : ClickHouseBaseModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        base.ConfigureServices(context);
        var connectionOptionList = new List<ConnectionStringOptions>
        {
            new ConnectionStringOptions()
            {
                Host = "192.168.31.20",
                Port = 9000,
                User = "kawhi",
                Password = "wzw0126..",
                Database = "luck_asa",
            }
        };
        context.Services.AddClickHouseDbConnectionString(x =>
        {
            x.IsCluster = false;
            x.ConnectionOptionList = connectionOptionList;
        });
    }
}