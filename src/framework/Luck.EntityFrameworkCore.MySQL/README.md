# Luck.EntityFrameworkCore.MySQL

`Luck.EntityFrameworkCore.MySQL` 为 `Luck.EntityFrameworkCore` 提供 MySQL EF Core 驱动：net6-net9 使用 Pomelo，net10 使用 Oracle Connector/NET。

## 安装

```bash
dotnet add package Luck.EntityFrameworkCore.MySQL --version 2.0.14
```

包仅支持 `net10.0`，并引用 `MySql.EntityFrameworkCore` 10.0.9（Oracle Connector/NET）。

## 最小示例

MySQL 驱动注册方法目前是 `ServiceCollectionExtension` 的实例方法，不是扩展方法，因此需要显式创建实例：

```csharp
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.EntityFrameworkCore.MySQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class AppDbContext(DbContextOptions options) : LuckDbContextBase(options)
{
    public DbSet<Note> Notes => Set<Note>();
}

public sealed class Note
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

var services = new ServiceCollection();
services.AddLuckDbContext<AppDbContext>(options =>
{
    options.ConnectionString =
        "Server=localhost;Port=3306;Database=luck;User Id=luck;Password=secret";
    options.Type = DataBaseType.MySql;
});
new ServiceCollectionExtension().AddMySqlDriven(services);

using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var notes = await context.Notes.ToListAsync();
```

## 主要 API

### `AddMySqlDriven`

```csharp
new ServiceCollectionExtension().AddMySqlDriven(services);
```

该方法注册单例 `IDbContextDrivenProvider`，实现为 `MySqlDrivenProvider`，其 `Type` 为 `DataBaseType.MySql`。net6-net9 使用 Pomelo 的 `ServerVersion.AutoDetect` 与 `UseMySql`；net10 使用 Oracle provider 的 `UseMySQL(connectionString)`。两个路径都会启用敏感数据日志和 `UseSnakeCaseNamingConvention`。

## 限制与注意事项

- net6-net9 的 Pomelo provider 会通过 `ServerVersion.AutoDetect` 探测服务器版本；net10 的 Oracle provider 直接接收连接字符串。
- 驱动默认启用 `EnableSensitiveDataLogging()`，生产环境请评估敏感数据泄露风险。
- `MySqlDrivenProvider.Builder` 接收 `QuerySplittingBehavior` 参数，但当前实现没有传给 provider 配置；`EfDbContextConfig.QuerySplittingBehavior` 在此驱动下不会生效。
- `LuckDbContextBase` 默认启用修改时间和软删除处理；自定义 `OnModelCreating` 时必须调用基类实现。
