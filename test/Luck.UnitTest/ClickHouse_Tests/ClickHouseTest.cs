using System;
using System.Data;
using Luck.Dapper.ClickHouse;
using Luck.Dapper.DbConnectionFactories;
using Luck.Framework.Infrastructure;
using Luck.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Luck.UnitTest.ClickHouse_Tests;

public class ClickHouseTest : IntegratedTest<ClickHouseTestModule>
{
    [Fact]
    public void Connection_Test_Open()
    {
        var dbConnectionFactory = ServiceProvider.GetService<IDbConnectionFactory>();
        if (dbConnectionFactory is null)
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        var dbConnection = dbConnectionFactory.GetDbConnection();
        dbConnection.Open();
        Assert.True(dbConnection.State == ConnectionState.Open);
    }
}

public class ClickHouseTestModule : ClickHouseBaseModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        base.ConfigureServices(context);
        context.Services.AddClickHouseDbConnectionString(x =>
        {
            x.Host = "192.168.31.20";
            x.Port = 9000;
            x.User = "kawhi";
            x.Password = "wzw0126..";
            x.Database = "db_name";
        });
    }
}