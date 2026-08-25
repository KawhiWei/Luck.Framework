# Luck.AutoDependencyInjection

`Luck.AutoDependencyInjection` 为 `Luck.AppModule` 提供 ASP.NET Core 启动扩展，并根据约定或特性扫描程序集，把服务注册到 Microsoft.Extensions.DependencyInjection 容器。

## 安装

```bash
dotnet add package Luck.AutoDependencyInjection --version 2.0.14
```

该包依赖 `Luck.AppModule` 和 ASP.NET Core shared framework，目标框架为 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。

## 模块启动

应用模块需要依赖 `AutoDependencyAppModule`，然后使用两个扩展方法：

```csharp
using Luck.AppModule;
using Luck.AutoDependencyInjection;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

[DependsOn(typeof(AutoDependencyAppModule))]
public sealed class AppWebModule : LuckAppModule
{
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication<AppWebModule>();
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.InitializeApplication();
app.Run();
```

`AddApplication<T>()` 会创建 `StartupModuleRunner` 并执行模块的 `ConfigureServices`；`InitializeApplication()` 必须在 `builder.Build()` 之后调用，用于保存 `IApplicationBuilder` 并执行模块的 `ApplicationInitialization`。调用顺序错误会导致服务解析或初始化失败。

模块的 `ConfigureServices` 可以直接使用 `ConfigureServicesContext.Services`：

```csharp
public override void ConfigureServices(ConfigureServicesContext context)
{
    context.Services.AddHttpClient();
}
```

在 `ApplicationContext` 中可以取回应用构建器：

```csharp
public override void ApplicationInitialization(ApplicationContext context)
{
    IApplicationBuilder app = context.GetApplicationBuilder();
    // 在模块初始化阶段执行需要服务提供者的逻辑
}
```

## 自动注册服务

让实现类实现 `Luck.Framework.Infrastructure.DependencyInjectionModule` 中的标记接口：

```csharp
public interface IOrderService
{
    Task CreateAsync();
}

public sealed class OrderService : IOrderService, IScopedDependency
{
    public Task CreateAsync() => Task.CompletedTask;
}
```

当 `AutoDependencyAppModule` 执行扫描时，上例会以 `Scoped` 生命周期注册 `IOrderService -> OrderService`。对应规则为：

| 标记 | 注册生命周期 |
| --- | --- |
| `IScopedDependency` | `ServiceLifetime.Scoped` |
| `ITransientDependency` | `ServiceLifetime.Transient` |
| `ISingletonDependency` | `ServiceLifetime.Singleton` |

实现类的接口会作为服务类型注册；标记接口自身带有 `IgnoreDependencyAttribute`，不会被作为服务类型注册。实现类没有可注册接口时，会注册自身类型。

也可以使用 `DependencyInjectionAttribute` 显式指定生命周期：

```csharp
[DependencyInjection(ServiceLifetime.Singleton, AddSelf = true)]
public sealed class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
```

`AddSelf` 默认为 `false`。当类型有接口时，设置为 `true` 会额外注册 `Clock -> Clock`；没有接口的类型始终会注册自身。`DependencyInjectionAttribute` 的生命周期优先于标记接口。

在接口或类型上使用 `IgnoreDependencyAttribute` 可以排除自动映射：

```csharp
[IgnoreDependency]
public interface IMarkerOnly
{
}
```

## 扫描范围与生命周期

自动注入使用 `AssemblyHelper.FindTypes` 查找当前依赖上下文中已加载的程序集。服务项目必须作为应用依赖或输出程序集出现；只把 DLL 放在磁盘上并不保证会被扫描。扫描结果会受到 `AssemblyHelper.AddExcludeLibs` 的影响。

自动注册不会替代构造函数注入。请求作用域服务时仍应遵守 ASP.NET Core 的生命周期规则，不能从单例中长期保存 scoped 服务。

## 属性注入状态

项目包含 `InjectionAttribute` 和内部的属性注入服务提供者实现，设计上支持给属性或字段标记：

```csharp
public sealed class OrdersController : ControllerBase
{
    [Injection]
    private readonly IOrderService _orders = default!;
}
```

但当前源码中的 `UsePropertyInjection(this IHostBuilder)` 方法体为空，`PropertyInjectionServiceProviderFactory` 也是内部类型且没有被该扩展启用。因此当前版本不能依赖 `builder.Host.UsePropertyInjection()` 获得可工作的属性注入；示例项目中的调用不会改变默认服务提供者。生产代码应使用构造函数注入：

```csharp
public OrdersController(IOrderService orders)
{
    _orders = orders;
}
```

## 注意事项

- `AddApplication<T>()` 要求 `T` 实现 `IAppModule`，通常继承 `LuckAppModule`。
- 启动模块和依赖模块都需要公共无参构造函数，因为模块运行器通过反射创建实例。
- 本包只注册自动扫描到的服务；`IPipelineFactory`、`IIntegrationEventBus`、数据库上下文等仍须由对应包或应用显式注册。
- 属性注入部分是未完成的实验性实现，当前 API 不应写入正式使用说明或作为运行前提。
