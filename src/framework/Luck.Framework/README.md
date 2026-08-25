# Luck.Framework

`Luck.Framework` 是 Luck.Framework 解决方案的基础契约库。它提供模块化、依赖注入约定、事件总线模型、管道抽象、取消标记、工作单元契约以及常用扩展方法；它本身不提供 ASP.NET Core 启动流程、数据库驱动或消息中间件实现。

## 安装

```bash
dotnet add package Luck.Framework --version 2.0.14
```

项目目标框架为 `net10.0`，Microsoft.Extensions 依赖统一使用 10.0.11 稳定版。

## 模块与依赖注入契约

### 模块契约

`Luck.Framework.Infrastructure` 命名空间中的 `IAppModule` 定义了模块的两个生命周期方法：

- `ConfigureServices(ConfigureServicesContext context)`：向 `context.Services` 注册服务。
- `ApplicationInitialization(ApplicationContext context)`：服务提供者构建后执行初始化。

模块依赖使用 `DependsOnAttribute` 声明：

```csharp
using Luck.AppModule;
using Luck.AutoDependencyInjection;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

[DependsOn(typeof(AutoDependencyAppModule))]
public sealed class AppModule : LuckAppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddLogging();
    }

    public override void ApplicationInitialization(ApplicationContext context)
    {
        // var service = context.ServiceProvider.GetRequiredService<IMyService>();
    }
}
```

`Luck.Framework` 只提供 `IAppModule`、`DependsOnAttribute` 和上下文类型。实际扫描、实例化和运行模块的实现位于 `Luck.AppModule`；ASP.NET Core 中的 `AddApplication<T>()` 和 `InitializeApplication()` 位于 `Luck.AutoDependencyInjection`。

### 自动注册标记

以下接口位于 `Luck.Framework.Infrastructure.DependencyInjectionModule`：

| 接口 | 约定的生命周期 |
| --- | --- |
| `IScopedDependency` | `ServiceLifetime.Scoped` |
| `ITransientDependency` | `ServiceLifetime.Transient` |
| `ISingletonDependency` | `ServiceLifetime.Singleton` |

这些接口只是约定，不会单独触发注册。需要引入 `Luck.AutoDependencyInjection` 并让启动模块依赖 `AutoDependencyAppModule` 才会执行扫描。`IgnoreDependencyAttribute` 可用于排除类型或接口。

## 集成事件契约

`Luck.Framework.Event` 提供事件模型，具体的 RabbitMQ/Kafka 实现由其他包提供。

```csharp
using Luck.Framework.Event;

public sealed class OrderCreated : IntegrationEvent
{
    public string OrderNo { get; init; } = string.Empty;
}

public sealed class OrderCreatedHandler : IIntegrationEventHandler<OrderCreated>
{
    public Task HandleAsync(OrderCreated @event)
    {
        Console.WriteLine(@event.OrderNo);
        return Task.CompletedTask;
    }
}
```

主要类型如下：

- `IntegrationEvent` 自动生成 `EventId`（雪花 ID 字符串）和 `EventCreationDate`。
- `IIntegrationEvent` 是事件最小契约；`IIntegrationEventHandler<T>` 定义 `HandleAsync`。
- `IIntegrationEventBus` 提供 `PublishAsync<TEvent>` 与 `SubscribeAsync`，实现需要由事件总线适配包注册。
- `IIntegrationEventBusSubscriptionsManager` 管理事件类型与处理器类型的订阅关系。
- `LuckEventData`、`PublishEventData`、`ConsumeEventData`、`ProcessEventData` 携带诊断数据；`DiagnosticConstants.DiagnosticListenerName` 为 `Luck.EventBus.Diagnostics`。
- `EventIds` 提供发布、接收和处理阶段的 `LuckEventId`；`LuckEventId` 可以与 `int` 隐式转换。

## 管道抽象

`Luck.Framework.PipelineAbstract` 只定义 `IContext`、`IPipe<TContext>`、`IPipelineFactory`、委托类型和 `Interruptible` 枚举。可直接使用的实现位于 `Luck.Pipeline`，详见该项目的 README。

## 通用工具

常用 API 包括：

```csharp
using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure;
using Luck.Framework.Utilities;

Check.NotNull(order, nameof(order));
Check.NotNullOrEmpty(orderNo, nameof(orderNo));

var json = order.Serialize();
var copy = json.Deserialize<Order>();
var id = SnowflakeId.GenerateNewStringId();
```

- `Check.NotNull` 和 `Check.NotNullOrEmpty` 在参数无效时抛出参数异常。
- `JsonExtension.Serialize` 默认使用缩进 JSON 与 `UnsafeRelaxedJsonEscaping`；传入 `null` 时返回空字符串。`Deserialize` 对空字符串返回默认值。
- `SnowflakeId.GenerateNewId()` 返回值类型，`GenerateNewStringId()` 返回十六进制字符串。
- `ServiceProviderExtension` 提供 `GetLogger<T>()`、`GetServiceOrCreateInstance`、`CreateInstance` 和 `CreateScoped` 等扩展；它们仍遵循 Microsoft.Extensions.DependencyInjection 的生命周期规则。
- `AssemblyHelper` 可按条件查找已加载的程序集和类型。它在类型初始化时缓存程序集列表；调用 `AddExcludeLibs` 前应确认不会影响自动依赖注入扫描。
- `EnumExtensions.ToDescription()`、`TypeExtension`、`CollectionExtensions` 和 `StringExtension` 提供枚举、反射、集合及字符串辅助方法。

项目还包含以下契约/特性：

- `ICancellationTokenProvider` 与 `NullCancellationTokenProvider`：统一获取取消标记；HTTP 实现见 `Luck.AspNetCore`。
- `IUnitOfWork` 与 `ILuckDbContext`：定义 `CommitAsync` 和数据库上下文访问，具体实现由数据库包提供。
- `Luck.Dapper.Attributes.LuckTableAttribute` 与 `LuckColumnNameAttribute`：为 Dapper 适配器提供表名、列名元数据。
- `BusinessException`、`LuckException`、`NotFoundException`：通用异常类型；ASP.NET Core 的 API 包会识别其中部分类型。

## 注意事项

1. 本包不会自动把实现类加入 DI 容器，也不会自动创建 `IIntegrationEventBus`、`IUnitOfWork` 或 Redis 客户端。
2. 事件处理器、数据库仓储和缓存接口都需要相应适配包或应用自行实现。
3. `IAppModule` 的实现应提供公共无参构造函数，以便模块运行器通过反射创建。
4. `JsonExtension` 的默认序列化选项和雪花 ID 的机器/进程信息是当前实现的一部分；跨服务使用时请按业务要求验证格式与唯一性。
