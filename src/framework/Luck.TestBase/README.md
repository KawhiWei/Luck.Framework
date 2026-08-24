# Luck.TestBase

`Luck.TestBase` 为使用 Luck 模块系统的集成测试提供测试基类。它负责创建 `IServiceCollection`、注册启动模块、构建服务提供者并初始化模块；测试类可以通过受保护的服务解析方法访问应用容器。

## 安装

```bash
dotnet add package Luck.TestBase --version 2.0.9
```

项目支持 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`，并依赖 `Luck.Framework`、`Luck.AppModule`、`Luck.AutoDependencyInjection` 和 `Luck.Dapper.ClickHouse`。按被测模块追加相应的数据库、消息或缓存包。

## 集成测试

定义一个启动模块，再继承 `IntegratedTest<TStartup>`：

```csharp
using Luck.AppModule;
using Luck.Framework.Infrastructure;
using Luck.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public sealed class TestModule : LuckAppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddSingleton<IClock, TestClock>();
    }
}

public interface IClock
{
    DateTime UtcNow { get; }
}

public sealed class TestClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}

public sealed class ClockTests : IntegratedTest<TestModule>
{
    [Fact]
    public void Resolves_services_from_the_module_container()
    {
        var clock = GetRequiredService<IClock>();
        Assert.Equal(DateTimeKind.Utc, clock.UtcNow.Kind);
    }
}
```

`IntegratedTest<TStartup>` 的构造函数会：

1. 创建空的 `ServiceCollection`。
2. 调用应用模块扩展注册 `TStartup`。
3. 构建服务提供者并创建测试作用域。
4. 调用 `StartupModuleRunner.Initialize` 执行模块初始化。

基类提供两个受保护方法：`GetService<T>()` 返回可空服务，`GetRequiredService<T>()` 在服务未注册时抛出异常。测试类应通过这两个方法解析服务，不要依赖基类的内部容器字段。

## 测试前后的自定义注册

当前 `IntegratedTest<TStartup>` 中的 `BeforeAddApplication`、`AfterAddApplication` 和 `ConfigureProvider` 是私有方法，不能由派生测试类重写。若测试需要替换依赖，应在启动模块的 `ConfigureServices` 中注册测试实现，或直接创建自己的 `ServiceCollection`/fixture。

## `TestBaseWithServiceProvider`

需要自己管理服务提供者时，可以继承 `TestBaseWithServiceProvider` 并实现其受保护的 `ServiceProvider` 属性：

```csharp
using Microsoft.Extensions.DependencyInjection;

public sealed class ManualTest : TestBaseWithServiceProvider
{
    private readonly IServiceProvider _provider = new ServiceCollection()
        .AddSingleton("test")
        .BuildServiceProvider();

    protected override IServiceProvider ServiceProvider => _provider;

    public string? ReadValue() => GetService<string>();
}
```

`TestBaseModule` 是包内的内部模块，应用代码通常不需要直接引用。

## Luck.TestBase.SourceGenerator

仓库还包含 `Luck.TestBase.SourceGenerator.csproj`，它声明为 `netstandard2.0` Roslyn component，目标是为测试项目附加 `ServiceRegistrationGenerator` analyzer，并在构建时生成包。其生成器行为和限制见 [`Luck.SourceGenerator`](../Luck.SourceGenerator/README.md)。

当前 checkout 中该项目的 analyzer Include 指向 `SourceGenerators/ServiceRegistrationGenerator.cs`，而实际源文件位于 `Luck.SourceGenerator/SourceGenerator/ServiceRegistrationGenerator.cs`，因此在直接构建这个独立项目之前需要修正路径或补齐文件；本文不把它描述为开箱即用依赖。

## 注意事项

- 基类会把环境变量 `appid` 设置为固定的测试值；如果测试依赖此变量，请避免在同一进程并行运行会修改它的测试。
- 服务提供者和测试作用域由基类创建，但当前类没有公开释放方法；长生命周期外部资源应由测试 fixture 或应用服务自行管理。
- `IntegratedTest<TStartup>` 要求启动类型实现 `IAppModule`，并使用无参构造路径参与模块启动。

## 许可证

本项目采用 [LGPL-3.0-only](../../../LICENSE) 许可证。
