# BusinessServiceRegistrationGenerator

这是一个基于 `IIncrementalGenerator` 的 SourceGenerator，用于自动注册带有 `BusinessServiceKeyAttribute` 特性的 `IAncillaryPaySuccessWithAncillaryScopeProvider` 接口实现类到 `IServiceCollection` 容器中。

## 功能特性

- 自动扫描所有实现了 `IAncillaryPaySuccessWithAncillaryScopeProvider` 接口的类
- 读取类上的 `BusinessServiceKeyAttribute` 特性信息
- 根据特性中的 `Lifetime` 和 `ServiceKey` 自动生成服务注册代码
- 生成扩展方法 `AddBusinessServices()` 用于批量注册服务

## 使用方法

### 1. 定义服务实现类

```csharp
[BusinessServiceKey(typeof(IAncillaryPaySuccessWithAncillaryScopeProvider), "70", ServiceLifetime.Scoped)]
public class BusExpressAncillaryPaySuccessWithAncillaryScopeProvider : IAncillaryPaySuccessWithAncillaryScopeProvider
{
    public async Task<(bool, string)> AncillaryPaySuccessProviderAsync(string request, string originMessage)
    {
        return (true, "非单售辅营暂不处理");
    }
}

[BusinessServiceKey(typeof(IAncillaryPaySuccessWithAncillaryScopeProvider), "50", ServiceLifetime.Scoped)]
public class CarAncillaryPaySuccessWithAncillaryScopeProvider : IAncillaryPaySuccessWithAncillaryScopeProvider
{
    public async Task<(bool, string)> AncillaryPaySuccessProviderAsync(string request, string originMessage)
    {
        return (true, "非单售辅营暂不处理");
    }
}
```

### 2. 注册服务

```csharp
// 在 Program.cs 或 Startup.cs 中
services.AddBusinessServices();
```

### 3. 使用服务

```csharp
// 通过 ServiceKey 获取特定的服务实现
var busExpressProvider = serviceProvider.GetKeyedService<IAncillaryPaySuccessWithAncillaryScopeProvider>("70");
var carProvider = serviceProvider.GetKeyedService<IAncillaryPaySuccessWithAncillaryScopeProvider>("50");
```

## 生成的代码示例

SourceGenerator 会自动生成类似以下的扩展方法：

```csharp
using Microsoft.Extensions.DependencyInjection;

namespace Luck.TestBase;

public static class BusinessServiceRegistrationExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<Luck.TestBase.IAncillaryPaySuccessWithAncillaryScopeProvider, Luck.TestBase.BusExpressAncillaryPaySuccessWithAncillaryScopeProvider>("70");
        services.AddKeyedScoped<Luck.TestBase.IAncillaryPaySuccessWithAncillaryScopeProvider, Luck.TestBase.CarAncillaryPaySuccessWithAncillaryScopeProvider>("50");
        return services;
    }
}
```

sb.AppendLine(
$"        services.Add(new ServiceDescriptor({service.ImplementationType}, {service.ServiceKey}, {service.Lifetime}));");


## 支持的生命周期

- `ServiceLifetime.Singleton` → `AddKeyedSingleton`
- `ServiceLifetime.Scoped` → `AddKeyedScoped`
- `ServiceLifetime.Transient` → `AddKeyedTransient`