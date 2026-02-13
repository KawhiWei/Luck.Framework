# OpenTelemetry.Luck.EventBus

Luck.EventBus 的 OpenTelemetry 集成包。

## 安装

```bash
dotnet add package Luck.EventBus.RabbitMQ
dotnet add package OpenTelemetry.Luck.EventBus
dotnet add package OpenTelemetry
dotnet add package OpenTelemetry.Exporter.Jaeger
```

## 使用

```csharp
// 1. 配置 RabbitMQ
builder.Services.AddEventBusRabbitMq(config =>
{
    config.Host = "localhost";
    config.UserName = "guest";
    config.PassWord = "guest";
});

// 2. 配置 OpenTelemetry（一行代码）
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddLuckEventBusInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddJaegerExporter();
    });
```

## 生成的 Span

| Span 名称 | Kind | 说明 |
|-----------|------|------|
| RabbitMQ.Publish | Producer | 消息发布 |
| RabbitMQ.Consume | Consumer | 消息消费 |
| RabbitMQ.Process | Internal | 消息处理 |

## Tags

- `eventbus.event_type` - 事件类型
- `eventbus.event_name` - 事件名称
- `eventbus.handler_type` - 处理器类型
- `messaging.system` - "rabbitmq"
- `messaging.destination` - 交换机
- `messaging.rabbitmq.routing_key` - 路由键
- `messaging.rabbitmq.queue` - 队列名

## 关键要点

1. **一行代码启用**：`.AddLuckEventBusInstrumentation()`
2. **只依赖 OpenTelemetry.Api**（轻量级）
