# Luck.EntityFrameworkCore.PostgreSQL

`Luck.EntityFrameworkCore.PostgreSQL` 为 `Luck.EntityFrameworkCore` 提供基于 Npgsql 的 PostgreSQL EF Core 驱动。

## 安装

```bash
dotnet add package Luck.EntityFrameworkCore.PostgreSQL --version 2.0.14
```

包依赖 `Luck.EntityFrameworkCore` 和 `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.3，目标框架为 `net10.0`。

## 最小示例

```csharp
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.EntityFrameworkCore.PostgreSQL;
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
        "Host=localhost;Port=5432;Database=luck;Username=luck;Password=secret";
    options.Type = DataBaseType.PostgreSql;
});
services.AddPostgreSQLDriven();

using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var notes = await context.Notes.ToListAsync();
```

## 主要 API

### `AddPostgreSQLDriven`

```csharp
services.AddPostgreSQLDriven();
```

该扩展注册单例 `IDbContextDrivenProvider`，实现为 `PostgreSqlDrivenProvider`，其 `Type` 为 `DataBaseType.PostgreSql`。驱动使用 `UseNpgsql`，应用 `QuerySplittingBehavior`，并启用蛇形命名约定。

## 限制与注意事项

- PostgreSQL 服务器必须可访问，连接字符串格式由 Npgsql 驱动解释。
- 驱动默认启用 `EnableSensitiveDataLogging()`，生产环境请评估敏感数据泄露风险。
- `LuckDbContextBase` 默认启用修改时间和软删除处理；自定义 `OnModelCreating` 时必须调用基类实现。
