# ServiceRegistrationGenerator

`ServiceRegistrationGenerator` 是一个 `IIncrementalGenerator`，用于生成 keyed DI 注册扩展。当前实现仍是面向仓库测试模型的实验性生成器，不是可直接复用的通用扫描器。

## 当前匹配规则

生成器只处理带有以下完整类型名特性的类：

```text
Luck.UnitTest.SourceGeneratorTest.BusinessServiceKeyAttribute
```

特性构造参数按顺序解释为服务类型、服务键和 `ServiceLifetime` 数值。生命周期映射为：`0` 或其他值对应 Singleton，`1` 对应 Scoped，`2` 对应 Transient。

## 生成结果

存在匹配类时，生成器添加 `ServiceCollectionExtensions.g.cs`，其中包含：

```csharp
namespace Luck.TestBase.SourceGenerators;

public static class ServiceInfoServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(
        this IServiceCollection services)
    {
        services.Add(new ServiceDescriptor(
            typeof(IMyService),
            "service-key",
            typeof(MyService),
            ServiceLifetime.Scoped));
        return services;
    }
}
```

消费项目应以 analyzer 方式引用生成器项目或包，并引用 `Microsoft.Extensions.DependencyInjection`。

## 已知限制

- 特性完整名称被硬编码为仓库测试命名空间；其他项目定义的同名特性不会被识别。
- 生成代码命名空间固定为 `Luck.TestBase.SourceGenerators`。
- 当前单元测试项目没有定义上述特性或匹配类，因此解决方案构建只能验证生成器可被编译器加载，不能验证注册代码实际生成。
- 生成器未报告无效构造参数、重复服务键或无法解析服务类型的诊断。
