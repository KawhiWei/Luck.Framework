# Luck.EventBus.RabbitMQ

`Luck.EventBus.RabbitMQ` 是 `Luck.Framework.Event` 的 RabbitMQ 实现，提供异步事件发布、程序集扫描式处理器订阅、独立的发布/消费通道，以及 `DiagnosticListener` 诊断事件。

## 安装

```bash
dotnet add package Luck.EventBus.RabbitMQ --version 2.0.9
```

项目依赖 `Luck.Framework`、`RabbitMQ.Client` 和 `Polly`，并引用 `Microsoft.AspNetCore.App` 以运行后台订阅服务。目标框架为 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。

## 快速开始

### 注册总线和处理器

```csharp
using Luck.EventBus.RabbitMQ.Enums;
using Luck.Framework.Event;
using Microsoft.Extensions.DependencyInjection;

builder.Services.AddLuckEventBusRabbitMq(options =>
{
    options.Host = "localhost";
    options.Port = 5672;
    options.UserName = "guest";
    options.PassWord = "guest";
    options.VirtualHost = "/";
    options.RetryCount = 5;
});

builder.Services.AddTransient<IIntegrationEventHandler<OrderCreated>, OrderCreatedHandler>();
```

`AddLuckEventBusRabbitMq` 会注册 `IIntegrationEventBus`、订阅管理器、持久连接和 `RabbitMqSubscribeService`。后台服务启动后会自动调用 `SubscribeAsync`，应用通常不需要手动调用订阅方法。

### 定义事件和路由

事件必须标记 `RabbitMqAttribute`，否则发布和订阅都会失败。当前源码中可用的交换机类型是 `Routing` 和 `FanOut`：

```csharp
using Luck.EventBus.RabbitMQ.Attributes;
using Luck.EventBus.RabbitMQ.Enums;
using Luck.Framework.Event;

[RabbitMq(
    EWorkModel.Routing,
    exchange: "orders",
    exchangeType: ExchangeType.Routing,
    routingKey: "order.created",
    queue: "orders.worker")]
public sealed class OrderCreated : IntegrationEvent
{
    public string OrderId { get; init; } = string.Empty;
}

public sealed class OrderCreatedHandler : IIntegrationEventHandler<OrderCreated>
{
    public Task HandleAsync(OrderCreated @event)
    {
        Console.WriteLine(@event.OrderId);
        return Task.CompletedTask;
    }
}
```

### 发布事件

通过 `IIntegrationEventBus` 发布，API 是纯异步的：

```csharp
public sealed class OrderService(IIntegrationEventBus eventBus)
{
    public Task CreateAsync(string orderId, CancellationToken cancellationToken)
    {
        return eventBus.PublishAsync(
            new OrderCreated { OrderId = orderId },
            cancellationToken: cancellationToken);
    }
}
```

发布前会校验 RabbitMQ 连接和事件特性，事件 JSON 使用 `System.Text.Json` 序列化；消息使用持久化投递模式，默认优先级为 `1`。

## 配置

`RabbitMqConfig` 的属性如下：

| 属性 | 默认值 | 说明 |
| --- | --- | --- |
| `Host` | 未初始化 | RabbitMQ 主机名，使用前必须设置 |
| `Port` | `5672` | RabbitMQ 端口 |
| `UserName` | 未初始化 | 用户名 |
| `PassWord` | 未初始化 | 密码 |
| `VirtualHost` | `/` | RabbitMQ 虚拟主机 |
| `RetryCount` | `0` | 发布阶段 Polly 重试次数 |
| `EnableDiagnosticEvents` | `false` | 预留配置项，当前实现不会据此关闭诊断事件 |

连接失败时持久连接会尝试建立连接；发布阶段的 Polly 策略只处理 `BrokerUnreachableException` 和 `SocketException`，按指数退避执行 `RetryCount` 次。重试次数为 `0` 时不会执行重试。

## 交换机和工作模式

`RabbitMqAttribute` 的构造函数是：

```csharp
RabbitMqAttribute(
    EWorkModel workModel,
    string exchange,
    ExchangeType exchangeType,
    string routingKey,
    string queue = "")
```

当前枚举值：

| 枚举 | 生成的 RabbitMQ 类型 | 用途 |
| --- | --- | --- |
| `ExchangeType.Routing` | `direct` | 精确路由 |
| `ExchangeType.FanOut` | `fanout` | 广播 |
| `EWorkModel.None` | 空字符串 | 使用默认交换机；发布端不声明交换机 |
| `EWorkModel.PublishSubscribe` | `fanout` | 发布/订阅 |
| `EWorkModel.Routing` | `direct` | 路由 |
| `EWorkModel.Topics` | `topic` | 主题 |
| `EWorkModel.Delayed` | `x-delayed-message` | 延迟交换机，需要 RabbitMQ 插件和额外参数 |

`RabbitMqAttribute` 同时接收 `EWorkModel` 和 `ExchangeType`，源码只把 `ExchangeType` 转换为实际的 exchange type 字符串；应确保两者语义匹配。当前 `ExchangeType` 没有 `Direct`、`Topic` 或 `Headers` 成员。

消费端会声明交换机和队列，并使用 `routingKey` 绑定队列。消费场景应提供非空的 exchange、queue 和 routing key；`EWorkModel.None` 只适合明确使用默认交换机的发布场景。

## 处理器发现和确认

`SubscribeAsync` 会通过 `AssemblyHelper` 查找当前加载程序集中的非抽象处理器类，并从其 `IIntegrationEventHandler<T>` 接口推断事件类型。因此处理器必须：

- 是具体的非抽象类；
- 实现 `IIntegrationEventHandler<T>`；
- 通过 DI 注册，或者由 `Luck.AutoDependencyInjection` 自动注册；
- 事件类型带有 `RabbitMqAttribute`。

消息只有在处理器成功完成后才会 `BasicAckAsync`。处理器异常会记录 `ProcessFailed` 诊断事件并重新抛出，消息不会在成功前确认；具体重投递行为由 RabbitMQ 消费者配置决定。

## 诊断事件

事件总线创建名称为 `Luck.EventBus.Diagnostics` 的 `DiagnosticListener`，写入以下事件：

| 事件名 | 数据类型 | 触发时机 |
| --- | --- | --- |
| `Published` | `PublishEventData` | 发布成功 |
| `PublishFailed` | `PublishEventData` | 发布重试回调记录失败 |
| `Received` | `ConsumeEventData` | 收到 RabbitMQ 消息 |
| `Processed` | `ProcessEventData` | 处理器完成并确认消息 |
| `ProcessFailed` | `ProcessEventData` | 处理器抛出异常 |

`EnableDiagnosticEvents` 当前只是配置模型属性，`RabbitMqEventBus` 始终写入上述诊断事件。OpenTelemetry 集成见 [`Luck.EventBus.OpenTelemetry`](../Luck.EventBus.OpenTelemetry/README.md)。

## 注意事项

- `RabbitMqEventBus` 的实现类是内部类型，应用应依赖 `IIntegrationEventBus`，不要直接构造实现类。
- 事件 key 默认使用事件类型名称；同一事件类型的重复处理器注册会抛出 `ArgumentException`。
- 发布和消费使用不同通道池；连接、通道和后台订阅服务由容器管理。
- 诊断事件包含原始 JSON 内容，可能包含业务敏感数据。接入日志或追踪后应配置采样、脱敏或限制导出。
- 当前实现的订阅循环会持续运行直到取消令牌触发；应用退出时应让宿主正常停止后台服务。

## 许可证

本项目采用 [LGPL-3.0-only](../../../LICENSE) 许可证。
