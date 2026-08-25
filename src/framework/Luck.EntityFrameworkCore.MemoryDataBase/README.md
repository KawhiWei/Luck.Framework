# Luck.EntityFrameworkCore.MemoryDataBase

`Luck.EntityFrameworkCore.MemoryDataBase` 为 `Luck.EntityFrameworkCore` 提供 EF Core InMemory 驱动，适合单元测试和本地快速验证。

## 安装

```bash
dotnet add package Luck.EntityFrameworkCore.MemoryDataBase --version 2.0.14
```

包依赖 `Luck.EntityFrameworkCore` 和与目标框架同主版本的 Microsoft.EntityFrameworkCore.InMemory，支持 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。

## 最小示例

```csharp
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.EntityFrameworkCore.MemoryDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class TestDbContext(DbContextOptions options) : LuckDbContextBase(options)
{
    public DbSet<Note> Notes => Set<Note>();
}

public sealed class Note
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

var services = new ServiceCollection();
services.AddLuckDbContext<TestDbContext>(options =>
{
    options.ConnectionString = "luck-test";
    options.Type = DataBaseType.MemoryDataBase;
});
services.AddMemoryDriven();

using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<TestDbContext>();
context.Notes.Add(new Note { Text = "in memory" });
await context.SaveChangesAsync();
```

## 主要 API

### `AddMemoryDriven`

```csharp
services.AddMemoryDriven();
```

该扩展注册单例 `IDbContextDrivenProvider`，实现为 `MemoryDrivenProvider`，其 `Type` 为 `DataBaseType.MemoryDataBase`。`AddLuckDbContext` 的 `EfDbContextConfig.ConnectionString` 会传给 `UseInMemoryDatabase`，因此它是内存数据库名称，而不是网络连接字符串。

## 限制与注意事项

- InMemory 数据库不是关系型数据库，SQL 翻译、约束、事务和真实数据库行为都可能不同，不应替代生产数据库集成测试。
- 使用相同数据库名称的上下文可能共享同一内存存储；测试之间建议使用唯一名称，避免数据相互污染。
- `MemoryDrivenProvider.Builder` 接收 `QuerySplittingBehavior` 参数但不会应用它。
- `LuckDbContextBase` 会继续执行其默认的修改时间和软删除处理；自定义上下文重写 `OnModelCreating` 时要调用 `base.OnModelCreating(modelBuilder)`。
