# Luck.EntityFrameworkCore

`Luck.EntityFrameworkCore` 为 Entity Framework Core 提供统一的数据库驱动选择、工作单元和仓储实现，并在 `LuckDbContextBase` 中集成修改时间与软删除处理。

## 安装

```bash
dotnet add package Luck.EntityFrameworkCore --version 2.0.14
```

项目目标框架为 `net10.0`。包依赖 `Luck.AppModule`、`Luck.DDD.Domain`、`Luck.Framework`、Microsoft.EntityFrameworkCore 10.0.11 和 EFCore.NamingConventions 10.0.1。

`Luck.EntityFrameworkCore` 本身只定义驱动接口，不包含 MySQL、PostgreSQL 或内存数据库驱动。请同时安装一个驱动包，并在容器中注册对应的 `IDbContextDrivenProvider`。例如，下面的示例使用内存数据库：

```bash
dotnet add package Luck.EntityFrameworkCore.MemoryDataBase --version 2.0.14
```

## 最小示例

下面的示例使用 `Luck.EntityFrameworkCore.MemoryDataBase`，但仓储、工作单元和上下文注册均来自本包。

```csharp
using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.EntityFrameworkCore.MemoryDatabase;
using Luck.Framework.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class Note : IAggregateRootBase
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public sealed class AppDbContext(DbContextOptions options) : LuckDbContextBase(options)
{
    public DbSet<Note> Notes => Set<Note>();
}

var services = new ServiceCollection();
services.AddLogging();
services.AddLuckDbContext<AppDbContext>(options =>
{
    options.ConnectionString = "luck-demo";
    options.Type = DataBaseType.MemoryDataBase;
});
services.AddMemoryDriven();
services.AddUnitOfWork();
services.AddDefaultRepository();

using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var repository = scope.ServiceProvider
    .GetRequiredService<IAggregateRootRepository<Note, int>>();
var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

var note = new Note { Text = "hello" };
repository.Add(note);
await unitOfWork.CommitAsync();
var saved = await repository.FindAsync(note.Id);
Console.WriteLine(saved?.Text);
```

## 主要 API

### 注册上下文和基础设施

- `AddLuckDbContext<TDbContext>(Action<EfDbContextConfig>, Action<IServiceProvider, DbContextOptionsBuilder>?)` 注册普通 EF Core 上下文。
- `AddLuckDbContextPool<TDbContext>(...)` 使用 EF Core 上下文池，参数与普通注册一致。
- `AddDefaultRepository(ServiceLifetime lifetime = Scoped)` 注册 `IAggregateRootRepository<,>` 和 `IEntityRepository<,>`，默认生命周期为 `Scoped`。
- `AddUnitOfWork()` 注册 `IUnitOfWork`，实现为 `UnitOfWork`，生命周期为 `Scoped`。

`EfDbContextConfig` 的配置项如下：

| 配置项 | 说明 |
| --- | --- |
| `ConnectionString` | 传给所选驱动的连接字符串；内存驱动将其作为数据库名称。 |
| `Type` | `DataBaseType.MySql`、`PostgreSql`、`MemoryDataBase` 或 `SqlServer`。驱动必须已注册。 |
| `QuerySplittingBehavior` | EF Core 拆分查询策略，默认是 `QuerySplittingBehavior.SplitQuery`。是否生效由具体驱动决定。 |

`AddLuckDbContext` 和 `AddLuckDbContextPool` 在构建选项时按 `Type` 查找 `IDbContextDrivenProvider`。找不到匹配驱动会抛出 `LuckException`。`AddPooledLuckDbContextFactory` 已标记为过时，不建议使用。

### 上下文、工作单元和仓储

自定义上下文应继承 `LuckDbContextBase`，构造函数接收 `DbContextOptions`。重写 `OnModelCreating` 时应先调用 `base.OnModelCreating(modelBuilder)`，再添加自己的映射。基类会：

- 在保存前调用实现 `IUpdatable` 的实体的 `UpdateCreation()` 或 `UpdateModification()`。
- 对实现 `ISoftDeletable` 的实体添加名为 `Deleted` 的阴影属性和全局查询过滤器；删除操作会转换为更新并调用 `UpdateDeletion()`。
- 提供 `Rollback()`，通过 `ChangeTracker.Clear()` 清空当前跟踪状态。

`EfCoreAggregateRootRepository<TEntity, TKey>` 支持 `Add`、`Attach`、`Update`、`Remove`、按主键或谓词查询；`EfCoreEntityRepository<TEntity, TKey>` 提供实体查询。前者要求实体实现 `IAggregateRootBase`，后者要求实体实现 `IEntityWithIdentity`。

### JSON 属性转换

在 EF Core 实体配置中可以使用以下扩展把引用类型序列化为 JSON 字符串：

```csharp
using Luck.EntityFrameworkCore.Extensions;

modelBuilder.Entity<Profile>().Property(x => x.Settings).HasJsonConversion();
modelBuilder.Entity<Profile>().Property(x => x.RequiredSettings).HasJsonConversionNotNull();
```

`HasJsonConversion<T>` 适用于可空引用类型，`HasJsonConversionNotNull<T>` 适用于非空引用类型。转换使用 `System.Text.Json`，同时配置值比较器；类型应能够被 `System.Text.Json` 序列化和反序列化。

## 限制与注意事项

- `DataBaseType.SqlServer` 已定义，但当前解决方案没有对应的 SQL Server 驱动包；需要自行实现并注册 `IDbContextDrivenProvider`。
- MySQL、PostgreSQL 驱动默认启用 `EnableSensitiveDataLogging()`，生产环境可能记录敏感数据，请结合实际环境覆写或调整日志配置。
- 软删除会占用名为 `Deleted` 的阴影属性；实体已有同名属性时，模型创建会抛出 `EntityFrameworkCorePropertyException`。值对象和继承实体会跳过这段处理。
- `LuckDbConnectionInterceptor.ConnectionOpeningAsync` 当前会把连接字符串设置为空，除非已确认这一行为符合需求，否则不要直接注册该拦截器。
