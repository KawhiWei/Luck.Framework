# Luck.DDD.Domain

`Luck.DDD.Domain` 提供轻量的领域模型和仓储契约：实体、聚合根、创建/修改/删除时间标记、普通表达式查询仓储，以及基于 SQL 的读写仓储接口。它不包含 ORM、数据库连接或仓储的具体实现。

## 安装

```bash
dotnet add package Luck.DDD.Domain --version 2.0.9
```

该包依赖 `Luck.Framework`，目标框架为 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。

## 领域模型

### 实体与聚合根

核心接口和基类关系如下：

| 类型 | 说明 |
| --- | --- |
| `IEntity` | 所有领域实体的最小标记接口 |
| `EntityWithIdentity<TKey>` | 带 `Id` 的实体基类，构造函数要求传入主键 |
| `FullEntity` | 使用 `string` 雪花 ID，并实现时间和软删除标记 |
| `IAggregateRootBase` | 聚合根标记接口，继承 `IEntity` |
| `AggregateRootBase` | 聚合根基类 |
| `AggregateRootWithIdentity<TKey>` | 带泛型主键的聚合根基类 |
| `FullAggregateRoot` | 使用 `string` 雪花 ID，并实现时间和软删除标记 |

一个典型的聚合可以这样定义：

```csharp
using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.DDD.Domain.Domain.Entities;

public sealed class Order : FullAggregateRoot
{
    public Order(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();
}

public sealed class OrderItem : FullEntity
{
    public OrderItem(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
    public Order Order { get; private set; } = default!;
}
```

`FullEntity` 和 `FullAggregateRoot` 的构造函数会调用 `SnowflakeId.GenerateNewStringId()` 生成字符串 ID。`CreationTime`、`LastModificationTime` 和 `DeletionTime` 默认值不会自动填充；调用以下方法才会写入 UTC 时间：

```csharp
order.UpdateCreation();
order.UpdateModification();
order.UpdateDeletion();
```

这些方法分别来自 `IUpdatable` 和 `ISoftDeletable`。库不会自动在新增、修改或删除时调用它们，调用时机需要由 ORM 拦截器、仓储或应用代码负责。

## 普通仓储契约

`Luck.DDD.Domain.Repositories` 提供以下接口：

- `IRepository<TEntity, TKey>`：`Find`、`FindAsync`、`FindAll` 以及按表达式查询。
- `IEntityRepository<TEntity, TKey>`：面向 `IEntityWithIdentity` 的只读仓储标记。
- `IWriteRepository<TEntity, TKey>`：`Attach`、`Add`、`Update`、`Remove` 写操作。
- `IAggregateRootRepository<TEntity, TKey>`：聚合根读写仓储，组合 `IRepository` 和 `IWriteRepository`。

应用或适配包注册实现后，可按以下方式使用：

```csharp
using Luck.DDD.Domain.Repositories;
using Luck.Framework.UnitOfWorks;

public sealed class OrderApplicationService(
    IAggregateRootRepository<Order, string> orders,
    IUnitOfWork unitOfWork)
{
    public async Task<Order?> GetAsync(string id)
    {
        return await orders.FindAsync(id);
    }

    public async Task CreateAsync(Order order)
    {
        orders.Add(order);
        await unitOfWork.CommitAsync();
    }
}
```

`AggregateRootRepositoryBase<TEntity, TKey>` 已实现两个 `FindAll` 重载，其他读写方法和 `FindQueryable()` 仍为抽象成员，适合由 EF Core 等适配器继承实现。

## SQL 仓储契约

`Luck.DDD.Domain.SqlRepositories` 提供不依赖具体 ORM 的 SQL API：

- `ISqlRepository<TEntity, TKey>`：`Find`、`FindAll` 的同步和异步 SQL 查询。
- `ISqlEntityRepository<TEntity, TKey>`：面向带身份实体的 SQL 只读仓储。
- `ISqlWriteRepository<TEntity, TKey>`：带可选 `IDbTransaction` 的 `Add`、`Update`、`Remove` 同步/异步操作。
- `ISqlAggregateRootRepository<TEntity, TKey>`：聚合根 SQL 读写仓储。
- `SqlAggregateRootRepositoryBase<TEntity, TKey>`：聚合根 SQL 仓储抽象基类，所有 SQL 执行成员都需要子类实现。

例如自定义实现需要自行决定参数对象、连接和事务管理：

```csharp
public abstract class OrderSqlRepository : SqlAggregateRootRepositoryBase<Order, string>
{
    // 实现 Add/Update/Remove、Find/FindAll 及其异步重载。
    // 具体 SQL 执行器不由 Luck.DDD.Domain 提供。
}
```

## 领域异常

`Luck.DDD.Domain.Exceptions.DomainException` 只有一个 `DomainException(string message)` 构造函数，可用于表达领域规则失败：

```csharp
if (order.Items.Count == 0)
{
    throw new DomainException("订单至少需要一项明细");
}
```

## 依赖与注意事项

- 本包只提供模型和接口，不会注册仓储或 `IUnitOfWork`。需要由应用、`Luck.EntityFrameworkCore` 等适配包完成注册和实现。
- `IUnitOfWork` 定义在 `Luck.Framework.UnitOfWorks`，不是本包的命名空间；如果应用需要事务提交，请同时引用并注册对应实现。
- `FullEntity` 与 `FullAggregateRoot` 的 ID 是十六进制字符串，不是数据库自增整数；数据库映射时请使用字符串列类型。
- `Entity` 的历史命名空间为 `Luck.DDD.Domain.Domian.Entities`（`Domian` 拼写保持现状）。新代码应优先使用 `EntityWithIdentity<TKey>` 或 `FullEntity`，避免直接依赖这个低层类型。
- 仓储返回 `IQueryable<TEntity>` 时，查询的执行、连接生命周期和异常行为取决于具体实现；请在有效的作用域内枚举结果。
