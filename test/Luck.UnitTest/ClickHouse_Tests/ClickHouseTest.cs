using System;
using System.Data;
using Luck.Dapper.DbConnectionFactories;
using Luck.TestBase;
using Microsoft.Extensions.DependencyInjection;
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
        var dbConnectionFactory = ServiceProvider.GetService<IDapperDrivenProvider>();
        if (dbConnectionFactory is null)
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        var dbConnection = dbConnectionFactory.GetDbConnection();
        dbConnection.Open();
        Assert.True(dbConnection.State == ConnectionState.Open);
    }
}