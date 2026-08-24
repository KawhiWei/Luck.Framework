# Luck.MongoDB

`Luck.MongoDB` 封装 MongoDB 客户端、上下文基类和依赖注入注册，并注册一组全局 BSON 约定，减少 MongoDB 实体的重复配置。

## 安装

```bash
dotnet add package Luck.MongoDB --version 2.0.9
```

包依赖 `Luck.Framework`、`MongoDB.Bson` 和 `MongoDB.Driver`（仓库当前中央版本为 `2.28.0`），支持 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。

## 最小示例

```csharp
using Luck.MongoDB;
using Luck.MongoDB.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

public sealed class AppMongoContext(MongoContextOptions options) : MongoDbContextBase(options)
{
    public IMongoCollection<User> Users => Collection<User>("users");
}

public sealed class User
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

var services = new ServiceCollection();
services.AddMongoDbContext<AppMongoContext>(options =>
{
    options.ConnectionString = "mongodb://localhost:27017/luck";
});

using var provider = services.BuildServiceProvider();
var context = provider.GetRequiredService<AppMongoContext>();
await context.Users.InsertOneAsync(new User { Name = "Ada" });
var user = await context.Users
    .Find(x => x.Name == "Ada")
    .FirstOrDefaultAsync();
Console.WriteLine(user?.Id);
```

`AddMongoDbContext<TContext>` 会把 `TContext` 和 `MongoDbContextBase` 以单例注册，因此无需额外注册上下文。

## 主要 API

### `AddMongoDbContext`

```csharp
services.AddMongoDbContext<AppMongoContext>(options =>
{
    options.ConnectionString = "mongodb://localhost:27017/luck";
});
```

`MongoContextOptions.ConnectionString` 是必填的 MongoDB URI。`MongoDbContextBase` 会从 URI 读取数据库名；URI 没有数据库名时使用默认数据库名 `Luck`。

### 上下文成员

- `Database` 返回 `IMongoDatabase`。
- `MongoClient` 返回当前数据库使用的 `IMongoClient`。
- `Collection<TEntity>()` 使用实体类型名称的小写形式作为集合名。
- `Collection<TEntity>(string tableName)` 使用调用方指定的集合名。

注册时还会加入以下全局 BSON 约定：属性名使用 camelCase；忽略实体中不存在的字段；`Id` 或 `ID` 映射到 `_id`；没有自定义生成器的 `string` 类型 Id 使用 ObjectId 生成器并以 ObjectId BSON 类型存储。

## 限制与注意事项

- 连接字符串为空或仅为空字符串时，创建上下文会抛出参数异常；URI 中没有数据库名时不会报错，而是连接到 `Luck` 数据库。
- 上下文服务和 `MongoContextOptions` 都以单例注册；请确保使用的实体、序列化配置和应用生命周期适合单例访问。
- `Database` 每次访问都会根据连接字符串创建 `MongoClient`，高频访问时应考虑在应用层缓存 `Database` 或覆写 `GetDbContext()`。
- BSON 约定通过 `ConventionRegistry.Register` 全局注册，每次调用 `AddMongoDbContext` 都会增加一组带随机名称的约定；多个容器或测试在同一进程中注册时应注意全局状态。
- 本包只提供上下文和 MongoDB 客户端封装，不提供工作单元、仓储、迁移或事务抽象；这些能力需由应用直接使用 MongoDB Driver 实现。
