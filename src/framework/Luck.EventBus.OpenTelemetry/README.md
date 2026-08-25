# Luck.EventBus.OpenTelemetry

`Luck.EventBus.OpenTelemetry` 把 Luck 事件总线的 `DiagnosticListener` 事件转换为 OpenTelemetry `Activity`。当前实现监听名称为 `Luck.EventBus.Diagnostics` 的诊断源，主要用于 [`Luck.EventBus.RabbitMQ`](../Luck.EventBus.RabbitMQ/README.md)。

## 安装

```bash
dotnet add package Luck.EventBus.OpenTelemetry --version 2.0.14
dotnet add package Luck.EventBus.RabbitMQ --version 2.0.14
dotnet add package OpenTelemetry.Extensions.Hosting --version 1.18.0
dotnet add package OpenTelemetry.Exporter.Console --version 1.18.0
```

本项目本身引用 `OpenTelemetry.Api` 和 `Microsoft.AspNetCore.App`。`AddOpenTelemetry()`、`AddConsoleExporter()`、OTLP exporter 以及 ASP.NET Core instrumentation 来自应用额外安装的 OpenTelemetry 包，请按实际导出目标添加。

## 快速开始

RabbitMQ 事件总线会创建诊断监听器；OpenTelemetry 扩展会自动发现并订阅它：

```csharp
using Luck.EventBus.OpenTelemetry;
using Luck.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLuckEventBusRabbitMq(options =>
{
    options.Host = "localhost";
    options.Port = 5672;
    options.UserName = "guest";
    options.PassWord = "guest";
    options.VirtualHost = "/";
    options.RetryCount = 5;
});

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Orders.Api"))
        .AddLuckEventBusInstrumentation()
        .AddConsoleExporter());
```

`.AddLuckEventBusInstrumentation()` 会把 `Luck.EventBus.Diagnostics` 加入 tracer source，并注册一个 `LuckEventBusInstrumentation` 实例。无需手动调用 `DiagnosticListener.AllListeners.Subscribe`。

## Span

Activity 名称和类型固定如下：

| Activity 名称 | Kind | 来源 |
| --- | --- | --- |
| `RabbitMQ.Publish` | `Producer` | `Published` 或 `PublishFailed` |
| `RabbitMQ.Consume` | `Consumer` | `Received` |
| `RabbitMQ.Process` | `Internal` | `Processed` 或 `ProcessFailed` |

如果应用没有注册 tracer provider，或采样策略丢弃该 Activity，`StartActivity` 会返回 `null`，不会产生导出数据。

## 标签

所有存在值的原始消息都会写入 `eventbus.raw_content`。RabbitMQ 发布 Span 的标签：

| 标签 | 值 |
| --- | --- |
| `eventbus.event_type` | 事件类型全名 |
| `eventbus.event_name` | 事件类型名称 |
| `messaging.system` | `rabbitmq` |
| `messaging.destination` | 交换机 |
| `messaging.rabbitmq.routing_key` | 路由键 |
| `eventbus.raw_content` | 原始 JSON |

RabbitMQ 消费 Span 的标签：

| 标签 | 值 |
| --- | --- |
| `eventbus.event_type` | 事件类型全名 |
| `eventbus.event_name` | 事件类型名称 |
| `messaging.system` | `rabbitmq` |
| `messaging.source` | 交换机 |
| `messaging.rabbitmq.queue` | 队列 |
| `eventbus.raw_content` | 原始 JSON |

处理 Span 的标签：

| 标签 | 值 |
| --- | --- |
| `eventbus.event_type` | 事件类型全名 |
| `eventbus.event_name` | 事件类型名称 |
| `eventbus.handler_type` | 处理器类型全名 |
| `eventbus.raw_content` | 原始 JSON |

`PublishFailed` 和 `ProcessFailed` 会把 Activity 状态设为 `Error` 并使用异常消息；成功事件状态设为 `Ok`。当前实现不会把 `Consume` 的消息异常状态设置为 Error，因为消费异常由 RabbitMQ event bus 在后续处理阶段写入 `ProcessFailed`。

## 导出器示例

### OTLP/gRPC

安装 `OpenTelemetry.Exporter.OpenTelemetryProtocol` 后：

```csharp
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddLuckEventBusInstrumentation()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:4317");
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        }));
```

### Jaeger 或其他后端

请安装对应的 OpenTelemetry exporter，并把 exporter 添加到同一个 `TracerProviderBuilder`。本包不启动 OpenObserve、Jaeger 或 OTLP 服务，也不保存追踪数据。

## 原始诊断事件

如果不需要 OpenTelemetry，也可以直接订阅 `DiagnosticListener`：

```csharp
using System.Diagnostics;
using Luck.Framework.Event;

DiagnosticListener.AllListeners.Subscribe(new Observer());

sealed class Observer : IObserver<DiagnosticListener>
{
    public void OnNext(DiagnosticListener listener)
    {
        if (listener.Name == DiagnosticConstants.DiagnosticListenerName)
            listener.Subscribe(new EventObserver());
    }

    public void OnError(Exception error) { }
    public void OnCompleted() { }
}

sealed class EventObserver : IObserver<KeyValuePair<string, object?>>
{
    public void OnNext(KeyValuePair<string, object?> value)
    {
        var data = value.Value as LuckEventData;
        Console.WriteLine($"{value.Key}: {data?.RawContent}");
    }

    public void OnError(Exception error) { }
    public void OnCompleted() { }
}
```

直接订阅时由调用方负责释放 `IDisposable` 订阅。使用本包的 `AddLuckEventBusInstrumentation()` 时，instrumentation 会在 tracer provider 释放时清理诊断监听器订阅。

## 注意事项

- RabbitMQ 端的 `RabbitMqConfig.EnableDiagnosticEvents` 当前未用于控制诊断事件；事件总线会始终写入诊断监听器。
- `eventbus.raw_content` 包含完整业务 JSON，可能泄露敏感信息；生产环境应谨慎导出、脱敏或关闭对应采集。
- Activity source 名称为 `Luck.EventBus.Diagnostics`，扩展方法已经调用 `AddSource`，不要用另一个名称重复配置。
- 该实现当前只处理 `Published`、`PublishFailed`、`Received`、`Processed` 和 `ProcessFailed` 事件；`Processing` 事件定义存在于基础契约，但 RabbitMQ 实现不会写出它。

## 许可证

本项目采用 [LGPL-3.0-only](../../../LICENSE) 许可证。
