using Dapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.MemoryDatabase;
using Luck.EntityFrameworkCore.MySQL;
using Luck.EntityFrameworkCore.PostgreSQL;
using Luck.AutoDependencyInjection;
using Luck.Dapper.ClickHouse;
using Luck.Dapper.DbConnectionFactories;
using Luck.Framework.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Luck.UnitTest;

public class DatabaseProviderUpgradeTests
{
    [Fact]
    public async Task InMemory_provider_can_persist_and_query_entities()
    {
        var provider = new MemoryDrivenProvider();
        var databaseName = $"upgrade-test-{Guid.NewGuid():N}";
        var options = provider.Builder(new DbContextOptionsBuilder<UpgradeTestContext>(), databaseName).Options;

        await using var context = new UpgradeTestContext(options);
        context.Records.Add(new UpgradeRecord { Name = "in-memory" });
        await context.SaveChangesAsync();

        var saved = await context.Records.SingleAsync();
        Assert.Equal("in-memory", saved.Name);
        Assert.Equal(DataBaseType.MemoryDataBase, provider.Type);
    }

    [Fact]
    public void PostgreSql_provider_configures_npgsql_without_connecting()
    {
        var provider = new PostgreSqlDrivenProvider();
        var builder = provider.Builder(new DbContextOptionsBuilder(),
            "Host=localhost;Port=5432;Database=luck_tests;Username=postgres;Password=postgres");

        Assert.Equal(DataBaseType.PostgreSql, provider.Type);
        Assert.Contains(builder.Options.Extensions, extension => extension.GetType().Name.Contains("Npgsql", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Category", "DatabaseIntegration")]
    public async Task MySql_provider_can_connect_when_connection_string_is_configured()
    {
        var connectionString = Environment.GetEnvironmentVariable("LUCK_MYSQL_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        var provider = new MySqlDrivenProvider();
        var options = provider.Builder(new DbContextOptionsBuilder<UpgradeTestContext>(), connectionString).Options;
        await using var context = new UpgradeTestContext(options);

        await AssertDatabaseCrudAsync(context);
    }

    [Fact]
    [Trait("Category", "DatabaseIntegration")]
    public async Task PostgreSql_provider_can_connect_when_connection_string_is_configured()
    {
        var connectionString = Environment.GetEnvironmentVariable("LUCK_POSTGRES_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        var provider = new PostgreSqlDrivenProvider();
        var options = provider.Builder(new DbContextOptionsBuilder<UpgradeTestContext>(), connectionString).Options;
        await using var context = new UpgradeTestContext(options);

        await AssertDatabaseCrudAsync(context);
    }

    [Fact]
    public async Task Dapper_can_execute_parameterized_query_and_map_result()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await connection.ExecuteAsync("create table records (id integer primary key, name text not null)");
        await connection.ExecuteAsync("insert into records (id, name) values (@Id, @Name)", new { Id = 7, Name = "dapper" });

        var record = await connection.QuerySingleAsync<SqliteRecord>("select id as Id, name as Name from records where id = @Id", new { Id = 7 });

        Assert.Equal("dapper", record.Name);
    }

    [Fact]
    public void ClickHouse_provider_is_registered_without_opening_a_network_connection()
    {
        var services = new ServiceCollection();
        services.AddClickHouseDbConnectionString(options =>
        {
            options.ConnectionOptionList.Add(new ConnectionStringOptions { Host = "localhost", Port = 9000, Database = "default" });
        });
        services.AddClickHouseDapperDriven();

        using var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IDapperDrivenProvider>();

        Assert.IsType<DapperClickHouseDrivenProvider>(provider);
    }

    [Fact]
    [Trait("Category", "DatabaseIntegration")]
    public async Task ClickHouse_provider_can_execute_query_when_connection_is_configured()
    {
        var host = Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_HOST");
        if (string.IsNullOrWhiteSpace(host))
            return;

        var services = new ServiceCollection();
        services.AddClickHouseDbConnectionString(options =>
        {
            options.ConnectionOptionList.Add(new ConnectionStringOptions
            {
                Host = host,
                Port = uint.Parse(Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_PORT") ?? "9000"),
                User = Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_USER") ?? "default",
                Password = Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_PASSWORD") ?? string.Empty,
                Database = Environment.GetEnvironmentVariable("LUCK_CLICKHOUSE_DATABASE") ?? "default"
            });
        });
        services.AddClickHouseDapperDriven();

        using var serviceProvider = services.BuildServiceProvider();
        using var connection = serviceProvider.GetRequiredService<IDapperDrivenProvider>().GetDbConnection();
        Assert.Equal(1, await connection.QuerySingleAsync<int>("select 1"));
    }

    [Fact]
    public void Auto_dependency_module_registers_annotated_services()
    {
        var services = new ServiceCollection();
        new AutoDependencyAppModule().ConfigureServices(new ConfigureServicesContext(services));

        using var serviceProvider = services.BuildServiceProvider();
        Assert.IsType<AnnotatedDependency>(serviceProvider.GetRequiredService<IAnnotatedDependency>());
    }

    private sealed class UpgradeTestContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<UpgradeRecord> Records => Set<UpgradeRecord>();
    }

    private static async Task AssertDatabaseCrudAsync(UpgradeTestContext context)
    {
        await context.Database.EnsureCreatedAsync();

        var record = new UpgradeRecord { Name = $"created-{Guid.NewGuid():N}" };
        context.Records.Add(record);
        await context.SaveChangesAsync();
        var saved = await context.Records.SingleAsync(item => item.Id == record.Id);
        Assert.Equal(record.Name, saved.Name);

        saved.Name = "updated";
        await context.SaveChangesAsync();
        Assert.Equal("updated", await context.Records
            .Where(item => item.Id == record.Id)
            .Select(item => item.Name)
            .SingleAsync());

        context.Records.Remove(saved);
        await context.SaveChangesAsync();
        Assert.False(await context.Records.AnyAsync(item => item.Id == record.Id));
    }

    private sealed class UpgradeRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private sealed class SqliteRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private interface IAnnotatedDependency
    {
    }

    [DependencyInjection(ServiceLifetime.Singleton)]
    private sealed class AnnotatedDependency : IAnnotatedDependency
    {
    }
}
