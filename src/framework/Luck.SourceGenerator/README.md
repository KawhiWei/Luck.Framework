# Luck.SourceGenerator

`Luck.SourceGenerator` 是一个基于 Roslyn `IIncrementalGenerator` 的编译期源代码生成器。当前实现面向仓库中的 keyed DI 测试场景：它扫描特定命名空间下的 `BusinessServiceKeyAttribute`，生成批量注册方法 `AddBusinessServices()`。

## 安装

```bash
dotnet add package Luck.SourceGenerator --version 2.0.14
```

项目目标框架为 `netstandard2.0`，使用 Microsoft.CodeAnalysis 5.9 与 Microsoft.Net.Compilers.Toolset 5.9 构建。它是 analyzer/source generator 项目，不是运行时服务库；请把包作为 analyzer 引用到支持 Roslyn 增量生成器的项目中。

## 当前扫描条件

生成器只接受同时满足以下条件的类：

1. 类声明包含至少一个 attribute。
2. attribute 的完整类型名必须是 `Luck.UnitTest.SourceGeneratorTest.BusinessServiceKeyAttribute`。
3. attribute 构造参数依次提供服务类型、服务键和 `ServiceLifetime`。

仓库中对应的测试特性定义位于 `test/Luck.UnitTest/SourceGeneratorTest`。通用业务项目不能直接使用任意名称的 `BusinessServiceKeyAttribute` 触发本生成器；若需要通用能力，必须先调整生成器源码中的完整类型名匹配逻辑。

## 生成内容

当发现至少一个目标类时，生成器会添加 `ServiceCollectionExtensions.g.cs`，内容位于 `Luck.TestBase.SourceGenerators` 命名空间，包含：

```csharp
public static IServiceCollection AddBusinessServices(this IServiceCollection services)
```

每个标记类会生成一个 keyed `ServiceDescriptor`，生命周期映射如下：

| `ServiceLifetime` 数值 | 生成的生命周期 |
| --- | --- |
| `1` | `Scoped` |
| `2` | `Transient` |
| 其他值 | `Singleton` |

生成结果使用 `ServiceDescriptor(typeof(TService), serviceKey, typeof(TImplementation), lifetime)`，因此运行时项目需要引用包含 keyed DI API 的 .NET 版本（通常为 .NET 8 或更高版本）。

## 示例

示例特性必须使用生成器当前硬编码的完整名称：

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.UnitTest.SourceGeneratorTest;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class BusinessServiceKeyAttribute(
    Type serviceType,
    string serviceKey,
    ServiceLifetime lifetime) : Attribute
{
    public Type ServiceType { get; } = serviceType;
    public string ServiceKey { get; } = serviceKey;
    public ServiceLifetime Lifetime { get; } = lifetime;
}
```

```csharp
[BusinessServiceKey(typeof(IUserService), "user", ServiceLifetime.Scoped)]
public sealed class UserService : IUserService
{
}

var services = new ServiceCollection();
services.AddBusinessServices();
var provider = services.BuildServiceProvider();
var userService = provider.GetKeyedService<IUserService>("user");
```

生成器只负责生成注册代码，不负责实现特性、不负责验证服务类型、不负责创建容器，也不负责检查 service key 的重复。重复键和无效类型会在编译或运行时由生成代码及 DI 容器报告。

## 调试与限制

- 生成器不生成任何目标类时不会添加扩展文件。
- 生成 namespace 和扩展方法名称是固定的，引用方必须引入 `Luck.TestBase.SourceGenerators` 命名空间或使用全限定名。
- service key 直接插入生成的字符串字面量；包含特殊引号的 key 可能生成无效 C#，使用前应限制 key 字符集。
- 仓库中的 `SourceGenerator/README.md` 是早期业务示例；以上说明以当前 `ServiceRegistrationGenerator.cs` 实现为准。

## 许可证

本项目采用 [LGPL-3.0-only](../../../LICENSE) 许可证。
