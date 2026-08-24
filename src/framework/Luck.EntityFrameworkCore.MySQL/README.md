# Luck.EntityFrameworkCore.MySQL

`Luck.EntityFrameworkCore.MySQL` 为 `Luck.EntityFrameworkCore` 提供基于 Pomelo 的 MySQL/MariaDB EF Core 驱动。

## 安装

```bash
dotnet add package Luck.EntityFrameworkCore.MySQL --version 2.0.9
```

包依赖 `Luck.EntityFrameworkCore` 和 `Pomelo.EntityFrameworkCore.MySql`。项目支持 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`；Pomelo 版本随目标框架选择，`net9.0` 和 `net10.0` 使用仓库中央版本配置。

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

该方法注册单例 `IDbContextDrivenProvider`，实现为 `MySqlDrivenProvider`，其 `Type` 为 `DataBaseType.MySql`。驱动使用 `ServerVersion.AutoDetect(connectionString)`，随后调用 `UseMySql` 和 `UseSnakeCaseNamingConvention`。

## 限制与注意事项

- `ServerVersion.AutoDetect` 需要根据连接字符串探测服务器版本；应用首次构建上下文选项时应能访问 MySQL/MariaDB 实例。
- 驱动默认启用 `EnableSensitiveDataLogging()`，生产环境请评估敏感数据泄露风险。
- `MySqlDrivenProvider.Builder` 接收 `QuerySplittingBehavior` 参数，但当前实现没有传给 Pomelo 配置；`EfDbContextConfig.QuerySplittingBehavior` 在此驱动下不会生效。
- `LuckDbContextBase` 默认启用修改时间和软删除处理；自定义 `OnModelCreating` 时必须调用基类实现。
