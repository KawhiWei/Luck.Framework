# Luck.AppModule

`Luck.AppModule` 是 Luck.Framework 的模块运行时实现。它负责发现并实例化 `IAppModule`，读取 `[DependsOn]` 依赖，执行模块的服务配置和应用初始化，并提供懒加载服务访问。

## 安装

```bash
dotnet add package Luck.AppModule --version 2.0.14
```

该项目依赖 `Luck.Framework`，目标框架与解决方案一致：`net10.0`。

## 定义模块

继承 `LuckAppModule`，在 `ConfigureServices` 中注册服务，在 `ApplicationInitialization` 中读取已构建的服务：

```csharp
using Luck.AppModule;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public sealed class DataModule : LuckAppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddSingleton<IClock, SystemClock>();
    }

    public override void ApplicationInitialization(ApplicationContext context)
    {
        var clock = context.ServiceProvider.GetRequiredService<IClock>();
        Console.WriteLine(clock.UtcNow);
    }
}

public interface IClock
{
    DateTime UtcNow { get; }
}

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
```

`LuckAppModule` 默认 `Enable = true`，两个生命周期方法默认不执行任何操作。将 `Enable` 设为 `false` 可以让运行器跳过该模块。

使用 `DependsOnAttribute` 声明模块依赖：

```csharp
using Luck.AutoDependencyInjection;
using Luck.Framework.Infrastructure;

[DependsOn(typeof(DataModule), typeof(AutoDependencyAppModule))]
public sealed class AppModule : LuckAppModule
{
}
```

依赖类型可以继续声明自己的依赖，`LuckAppModule.GetDependedTypes()` 会递归展开并去重。依赖模块必须是具体、非抽象、非泛型并实现 `IAppModule` 的类型。

## 启动方式

`Luck.AppModule` 提供 `StartupModuleRunner`，但 ASP.NET Core 应用通常通过 `Luck.AutoDependencyInjection` 的扩展方法启动：

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication<AppModule>();

var app = builder.Build();
app.InitializeApplication();
app.Run();
```

上面示例中的 `AddApplication` 和 `InitializeApplication` 属于 `Luck.AutoDependencyInjection`，不是本包的扩展方法。应用模块必须能被程序集扫描发现，并且提供公共无参构造函数。

启动过程的主要阶段是：

1. `ModuleApplicationBase` 通过 `AssemblyHelper` 查找所有可实例化的模块，并把启用模块注册为单例。
2. `StartupModuleRunner.ConfigureServices` 创建 `ConfigureServicesContext`，依次调用已加载模块的 `ConfigureServices`。
3. `StartupModuleRunner.Initialize` 保存根服务提供者，并创建一个作用域，依次调用模块的 `ApplicationInitialization`。

运行器会把自身注册为 `IModuleApplication` 和 `IStartupModuleRunner`，可通过以下属性查看模块信息：

- `StartupModuleType`：启动模块类型。
- `Modules`：从启动模块和其依赖展开的模块列表。
- `Source`：扫描到的全部启用模块。
- `Services`、`ServiceProvider`：服务集合和初始化后的服务提供者。

## 服务访问

### `IocManage`

```csharp
var logger = IocManage.Instance.GetLogger<MyService>();
var service = IocManage.Instance.GetService<IMyService>();
```

`IocManage` 是进程级单例。运行器初始化服务集合和服务提供者后，`GetService<T>()` 与 `GetLogger<T>()` 才可正常使用；在初始化前调用会触发参数检查。优先在模块上下文或构造函数中使用显式的 `IServiceProvider`/构造函数注入，避免把全局单例作为常规依赖入口。

### `ILazyServiceProvider`

`LazyServiceProvider` 实现 `Luck.Framework.Infrastructure.ILazyServiceProvider`，自身标记为 `ITransientDependency`。它按服务类型缓存第一次解析的结果：

```csharp
public sealed class ReportService(ILazyServiceProvider services)
{
    public IClock Clock => services.LazyGetRequiredService<IClock>();
}
```

可用方法包括 `LazyGetRequiredService<T>()`、`LazyGetRequiredService(Type)`、`LazyGetService<T>()` 以及接受工厂的重载。当前实现的 `LazyGetService` 在找不到服务时也会抛出异常，而不是返回 `null`；请不要把它当作可空查询 API。

## 相关类型

| 类型 | 用途 |
| --- | --- |
| `ModuleApplicationBase` | 模块发现、实例化和模块列表管理的基类 |
| `StartupModuleRunner` | 执行 `ConfigureServices` 与 `ApplicationInitialization` |
| `LuckAppModule` | 最常用的模块基类 |
| `IocManage` | 进程级服务和日志访问器 |
| `LazyServiceProvider` | 带类型缓存的服务解析器 |

## 注意事项

- 模块扫描基于 `AssemblyHelper` 在类型初始化时缓存的程序集列表；模块所在程序集必须出现在应用的依赖上下文中。
- 模块实例通过无参构造函数创建，不能依赖构造函数参数注入。
- 本包不提供 ASP.NET Core 的 `WebApplication` 扩展，也不包含自动按标记扫描服务的实现；需要使用 `Luck.AutoDependencyInjection`。
- `StartupModuleRunner.Initialize` 创建并释放初始化作用域。不要在 `ApplicationInitialization` 中保存只在该作用域有效的 scoped 实例到单例对象。
