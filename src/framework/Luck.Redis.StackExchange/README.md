# Luck.Redis.StackExchange

`Luck.Redis.StackExchange` 基于 StackExchange.Redis，提供 `ConnectionMultiplexer` 的依赖注入配置，以及 Redis Hash/List 的同步和异步操作封装。

## 安装

```bash
dotnet add package Luck.Redis.StackExchange --version 2.0.14
```

项目依赖 `Luck.Framework`、`Luck.AppModule` 和 `StackExchange.Redis`，支持 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。

## 注册连接

`AddRedis` 位于 `Microsoft.Extensions.DependencyInjection` 命名空间：

```csharp
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddRedis(options =>
{
    options.Host = "localhost:6379";
    options.Password = "secret";
    options.Timeout = 5000;
});
```

该扩展会注册一个单例 `ConnectionMultiplexer`，并固定设置 `KeepAlive = 15`、`ResolveDns = false` 和 `AbortOnConnectFail = false`。`Timeout` 会设置同步超时，连接超时为它的 5 倍；未设置时使用 StackExchange.Redis 默认值。

## 注册 Hash/List 服务

`StackExchangeRedisModule` 会把操作封装注册为单例：

```csharp
using Luck.AppModule;
using Luck.Framework.Infrastructure;
using Luck.Redis.StackExchange;

[DependsOn(typeof(StackExchangeRedisModule))]
public sealed class AppModule : LuckAppModule
{
}
```

也可以手动注册模块中的两个实现：

```csharp
services.AddSingleton<IRedisHash, StackExchangeRedisHash>();
services.AddSingleton<IRedisList, StackExchangeRedisList>();
```

应用模块方式还需要通过 `Luck.AutoDependencyInjection` 的 `AddApplication<T>()` 启动模块运行器。

## Hash 操作

容器注册的 `Luck.Framework.Infrastructure.Caching.Interface.IRedisHash` 当前暴露异步 Hash API，支持字符串和泛型 JSON 值：

```csharp
public sealed class CounterService(IRedisHash hash)
{
    public async Task<long> IncrementAsync()
    {
        await hash.HashSetAsync("orders", "status", "created");
        return await hash.HashIncrementAsync("orders", "count", 1);
    }
}
```

主要异步方法包括 `HashSetAsync`、`HashSetNxAsync`、`HashMSetAsync`、`HashGetAsyncByFieldAsync`、`HashGetAllAsync`、`HashGetKeysAsync`、`HashGetValueAsync`、`HashExistsAsync`、`DeleteAsync`、`HashIncrementAsync`、`HashDecrementAsync` 和 `HashLenAsync`。泛型重载使用 `Luck.Framework.Extensions` 中的 JSON 序列化扩展。同步同名方法存在于 `StackExchangeRedisHash` 具体类中，但当前注册接口不会声明这些成员。

## List 操作

容器注册的 `Luck.Framework.Infrastructure.Caching.Interface.IRedisList` 当前暴露同步 List API，提供两端入队/出队、按索引读写、范围读取、长度、修剪和移除：

```csharp
public sealed class QueueService(IRedisList list)
{
    public void Enqueue(Order order)
    {
        list.RPush("orders", order);
    }

    public Order? Dequeue()
    {
        return list.LPop<Order>("orders");
    }
}

public sealed record Order(string Id);
```

对应方法以 `LPush`、`RPush`、`LPop`、`RPop`、`GetRange`、`GetLen`、`SetByIndex`、`LTrim`、`LRemove` 开头，并提供泛型重载。异步方法以 `Async` 结尾，存在于 `StackExchangeRedisList` 具体实现中；如果需要注入异步 API，请额外注册具体类并将其注入。Redis List 的 `start`/`end` 是 StackExchange.Redis 的闭区间索引。

## 当前边界与注意事项

- `IRedisString` 和 `StackExchangeRedisString` 当前没有公开操作成员；本包暂不提供 String 数据结构封装。
- 连接和 Hash/List 实现均以单例注册，`ConnectionMultiplexer` 由容器复用。
- 泛型值通过 Luck.Framework 的 JSON 扩展序列化；调用方应保证写入和读取时使用兼容的类型及序列化设置。
- 连接字符串为空或格式错误时，`ConfigurationOptions.Parse`/`ConnectionMultiplexer.Connect` 会抛出异常。
- 当前源码把部分 Hash/List 的同步与异步 partial 接口声明放在不同命名空间；注入的接口类型以实现注册所引用的 `Luck.Framework.Infrastructure.Caching.Interface` 命名空间为准。因此 Hash 接口主要暴露异步方法，List 接口主要暴露同步方法。需要同时使用两组方法时，应额外注册 `StackExchangeRedisHash`/`StackExchangeRedisList` 具体类型，并自行评估版本兼容性。

## 许可证

本项目采用 [LGPL-3.0-only](../../../LICENSE) 许可证。
