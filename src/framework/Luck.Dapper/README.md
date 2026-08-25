# Luck.Dapper

`Luck.Dapper` 封装 Dapper 的 SQL 仓储抽象。它不绑定具体数据库连接实现，由应用通过 `IDapperDrivenProvider` 提供同步和异步 `IDbConnection`。

## 安装

```bash
dotnet add package Luck.Dapper --version 2.0.14
```

包依赖 `Luck.DDD.Domain`、`Luck.Framework` 和 `Dapper`，支持 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。本包不包含 SQL Server、MySQL、PostgreSQL 或 SQLite 的 ADO.NET 驱动。

下面的可运行示例额外使用 SQLite 连接实现：

```bash
dotnet add package Microsoft.Data.Sqlite
```

## 最小示例

```csharp
using System.Data;
using Dapper;
using Luck.Dapper.DbConnectionFactories;
using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.DDD.Domain.SqlRepositories;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

public sealed class Note : IAggregateRootBase
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public sealed class SqliteProvider : IDapperDrivenProvider
{
    private const string ConnectionString = "Data Source=luck-dapper-demo.db";

    public IDbConnection GetDbConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public async Task<IDbConnection> GetDbConnectionAsync()
    {
        var connection = new SqliteConnection(ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}

var connectionProvider = new SqliteProvider();
using (var connection = connectionProvider.GetDbConnection())
{
    connection.Execute("CREATE TABLE IF NOT EXISTS Notes (Id INTEGER PRIMARY KEY, Text TEXT NOT NULL)");
}

var services = new ServiceCollection()
    .AddSingleton<IDapperDrivenProvider>(connectionProvider)
    .AddDefaultSqlRepository();
using var provider = services.BuildServiceProvider();

var repository = provider.GetRequiredService<ISqlAggregateRootRepository<Note, int>>();
var note = new Note { Id = 1, Text = "hello" };
repository.Add("INSERT INTO Notes (Id, Text) VALUES (@Id, @Text)", note);
var saved = repository.Find("SELECT Id, Text FROM Notes WHERE Id = @Id", new { note.Id });
Console.WriteLine(saved?.Text);
```

## 主要 API

### 连接提供者

应用实现 `IDapperDrivenProvider`：

- `GetDbConnection()` 返回已准备使用的同步 `IDbConnection`。
- `GetDbConnectionAsync()` 返回已准备使用的异步 `IDbConnection`。

仓储会在没有外部事务时创建并释放连接，因此提供者返回的连接应由调用方或仓储拥有；示例返回新连接。

### 仓储注册与方法

```csharp
services.AddDefaultSqlRepository(ServiceLifetime.Scoped);
```

该扩展默认注册：

- `ISqlAggregateRootRepository<TEntity, TKey>` -> `DapperAggregateRootRepositoryBase<TEntity, TKey>`。
- `ISqlEntityRepository<TEntity, TKey>` -> `DapperEntityRepository<TEntity, TKey>`。

两类仓储都提供 `Find`、`FindAsync`、`FindAll` 和 `FindAllAsync`，SQL 和参数对象直接传给 Dapper。聚合根仓储另外提供同步/异步 `Add`、`Update`、`Remove`，可传入 `IDbTransaction`。

## 限制与注意事项

- 本包不负责连接字符串、连接池、事务边界或数据库迁移；这些由 `IDapperDrivenProvider` 和应用负责。
- 聚合根写入方法只有在传入事务且事务包含连接时才复用该连接；否则每次操作都会创建独立连接。读取方法始终自行获取连接。
- SQL、表名、列名和参数都由调用方提供，没有额外的 SQL 注入保护；动态 SQL 必须使用参数化查询。
- `DapperEntityRepository` 仅实现查询接口；需要写入时使用 `ISqlAggregateRootRepository` 或自行扩展仓储。
