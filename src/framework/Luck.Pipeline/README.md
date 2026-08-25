# Luck.Pipeline

`Luck.Pipeline` 提供基于 `Luck.Framework.PipelineAbstract` 的顺序管道实现。它支持两种模式：通过 DI 按类型解析 `IPipe<TContext>` 的中间件管道，以及手动传入 `IDelegatePipe<TContext>` 的委托管道。

## 安装

```bash
dotnet add package Luck.Pipeline --version 2.0.14
```

该包依赖 `Luck.Framework`，目标框架为 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。项目没有提供 `IServiceCollection` 扩展，`PipelineFactory` 和每个管道节点都需要应用显式注册。

## 上下文

继承 `Luck.Pipeline.Context` 定义业务上下文：

```csharp
using Luck.Pipeline;

public sealed class OrderContext : Context
{
    public OrderContext(string uniqueKey) : base(uniqueKey)
    {
    }

    public string OrderNo { get; set; } = string.Empty;
}
```

`Context` 提供：

- `UniqueKey`：创建时传入的上下文唯一键。
- `Properties`：`Dictionary<string, object>`，用于在管道节点之间传递扩展数据。
- `IsInterruptible`、`Interruptible`、`InterruptibleReason`：中断状态。
- `SetInterruptible(Interruptible interruptible, string interruptibleReason = "")`：设置中断状态。

`Interruptible` 包含 `Cancel`、`Retry`、`Observe` 和 `Skip`。当前实现只负责设置状态并停止后续节点，不会自动执行重试、监控或补偿；这些语义需要由应用层处理。

## 中间件管道

继承 `DefaultPipe<TContext>` 实现节点：

```csharp
public sealed class LoadOrderPipe : DefaultPipe<OrderContext>
{
    protected override ValueTask Invoke(OrderContext context)
    {
        context.Properties["order"] = context.OrderNo;
        return ValueTask.CompletedTask;
    }
}

public sealed class ValidateOrderPipe : DefaultPipe<OrderContext>
{
    protected override ValueTask Invoke(OrderContext context)
    {
        if (string.IsNullOrWhiteSpace(context.OrderNo))
        {
            context.SetInterruptible(Interruptible.Cancel, "订单号为空");
        }

        return ValueTask.CompletedTask;
    }
}
```

注册工厂和节点，再通过 `IPipelineFactory.CreatePipelineBuilder<TContext>()` 构造：

```csharp
using Luck.Framework.PipelineAbstract;
using Luck.Pipeline;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddScoped<IPipelineFactory, PipelineFactory>()
    .AddScoped<LoadOrderPipe>()
    .AddScoped<ValidateOrderPipe>();

await using var provider = services.BuildServiceProvider();
var factory = provider.GetRequiredService<IPipelineFactory>();
var actuator = factory.CreatePipelineBuilder<OrderContext>()
    .UseMiddleware<LoadOrderPipe>()
    .UseMiddleware<ValidateOrderPipe>()
    .Build();

await actuator.InvokeAsync(new OrderContext("order-001") { OrderNo = "A001" });
```

`PipelineBuilder.Build()` 会按 `UseMiddleware` 的顺序从 DI 解析节点，并通过 `NextPipe` 串联。某个节点没有注册时会抛出 `NotImplementedException` 并提示检查 DI。`DefaultPipe` 的执行顺序是 `BeforeInvokeAsync`、`Invoke`、下一个节点、`AfterInvokeAsync`；当 `BeforeInvokeAsync` 返回 `false` 时跳过当前 `Invoke`，仍会继续后续节点。

## 委托管道

需要显式传入节点实例时，继承 `DefaultDelegatePipe<TContext>`：

```csharp
public sealed class WriteOrderPipe : DefaultDelegatePipe<OrderContext>
{
    protected override ValueTask Invoke(OrderContext context)
    {
        Console.WriteLine(context.OrderNo);
        return ValueTask.CompletedTask;
    }
}

var delegatePipeline = factory.CreateDelegatePipelineBuilder<OrderContext>()
    .UsePipe(provider.GetRequiredService<WriteOrderPipe>())
    .Build();

await delegatePipeline(new OrderContext("order-002") { OrderNo = "A002" });
```

委托模式通过 `UsePipe(IDelegatePipe<TContext>)` 保存节点，`Build()` 返回 `DelegatePipe<TContext>` 委托。节点执行 `BeforeInvokeAsync`、`Invoke` 和下一个委托；中断或异常会进入 `OnCancelled`。默认异常处理会以“节点类型名->异常消息”创建并重新抛出包装异常，可重写 `OnCancelled` 自定义行为。

## 中断与异常

```csharp
protected override ValueTask Invoke(OrderContext context)
{
    context.SetInterruptible(Interruptible.Retry, "下游暂时不可用");
    return ValueTask.CompletedTask;
}
```

中断状态会在当前节点的关键阶段检查；一旦设置，当前管道会调用 `OnCancelled` 并停止后续执行。`Retry`、`Observe` 等枚举值不会自动触发策略引擎。对于异常，默认 `OnCancelled` 会抛出新的 `Exception`，应用可以在自定义节点中覆盖它并记录或转换异常。

## 注意事项

- 所有 `IPipe<TContext>` 节点必须在同一个有效的 DI 作用域内注册；不要把依赖 scoped 服务的节点解析后缓存为单例。
- `DefaultPipe.ExecuteAsync(TContext context, IPipe<TContext> next)` 重载当前实现会在执行逻辑后抛出 `NotImplementedException`，请使用 `IPipeActuator.InvokeAsync` 或无 `next` 参数的 `ExecuteAsync`。
- `DefaultDelegatePipe` 中声明的 `AfterInvokeAsync` 当前不会在 `InvokeAsync` 中调用；若业务依赖后置逻辑，请在自定义实现中显式处理并验证行为。
- `DefaultPipeActuator` 只从列表的第一个节点开始执行；通常应通过 `PipelineBuilder.Build()` 构建完整链路，不要手动修改列表后假设会执行任意节点。
