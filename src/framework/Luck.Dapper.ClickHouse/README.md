# Luck.Dapper.ClickHouse

`Luck.Dapper.ClickHouse` 为 `Luck.Dapper` 提供基于 `Octonica.ClickHouseClient` 的 ClickHouse 连接提供者和依赖注入配置。

## 安装

```bash
dotnet add package Luck.Dapper.ClickHouse --version 2.0.14
```

包依赖 `Luck.Dapper`、`Luck.AppModule` 和 Octonica.ClickHouseClient 4.1.4，目标框架为 `net10.0`。

## 最小示例

下面的配置注册 ClickHouse 驱动和 SQL 仓储，并执行同步查询。需要一个可访问的 ClickHouse 实例。

```csharp
using Luck.Dapper.ClickHouse;
using Luck.Dapper.DbConnectionFactories;
using Luck.DDD.Domain.Domain.Entities;
using Luck.DDD.Domain.SqlRepositories;
using Microsoft.Extensions.DependencyInjection;

public sealed class EventRow : IEntityWithIdentity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

var services = new ServiceCollection();
services.AddClickHouseDbConnectionString(options =>
{
    options.IsCluster = false;
    options.ConnectionOptionList = new()
    {
        new ConnectionStringOptions
        {
            Host = "localhost",
            Port = 9000,
            User = "default",
            Password = "",
            Database = "default"
        }
    };
});
services.AddClickHouseDapperDriven();
services.AddDefaultSqlRepository();

using var provider = services.BuildServiceProvider();
var repository = provider.GetRequiredService<ISqlEntityRepository<EventRow, long>>();
var rows = repository.FindAll("SELECT Id, Name FROM events LIMIT 10", null);
```

也可以继承 `ClickHouseBaseModule`。重写 `AddConnectionString(IServiceCollection)` 调用 `AddClickHouseDbConnectionString`，重写 `AddDbDriven(IServiceCollection)` 调用 `AddClickHouseDapperDriven`；基类会自动注册 SQL 仓储。

## 主要 API

### 配置项

`ClickHouseConnectionConfig`：

| 配置项 | 说明 |
| --- | --- |
| `ConnectionOptionList` | 连接选项列表；非集群模式使用第一项。 |
| `IsCluster` | 是否集群模式。当前实现仍只取列表第一项。 |

`ConnectionStringOptions` 的字段为 `Host`、`Port`、`User`、`Password`、`Database`、`ReadWriteTimeout` 和 `LoadWeight`。同步连接实际只使用前五项拼接 `Host`、`Port`、`User`、`Password`、`Database`。

### 服务注册

- `AddClickHouseDbConnectionString(Action<ClickHouseConnectionConfig>)` 使用 Options 模式保存连接配置。
- `AddClickHouseDapperDriven()` 注册单例 `IDapperDrivenProvider`，实现为 `DapperClickHouseDrivenProvider`。
- `AddDefaultSqlRepository()` 来自 `Luck.Dapper`，注册 SQL 仓储。

## 限制与注意事项

- 非集群模式没有连接选项时会抛出 `LuckException`；集群模式没有连接选项时当前实现会访问列表第一个元素并抛出索引异常。
- `IsCluster = true` 当前不会轮询或负载均衡，仍只使用 `ConnectionOptionList[0]`。
- `ReadWriteTimeout` 和 `LoadWeight` 当前没有写入生成的连接字符串，配置它们不会改变连接行为。
- `GetDbConnectionAsync()` 当前使用空连接字符串创建 `ClickHouseConnection`，没有复用已配置的连接选项；在该实现修复前不要依赖 ClickHouse 仓储的异步方法。
- 连接打开和 SQL 执行依赖外部 ClickHouse 服务，本包不负责建库、建表和迁移。
