# OpenTelemetry.Luck.EventBus

Luck.EventBus 的 OpenTelemetry 集成包，支持分布式链路追踪。

## 安装

```bash
dotnet add package OpenTelemetry.Luck.EventBus
```

## 快速开始

### 1. 基础配置

```csharp
using OpenTelemetry.Luck.EventBus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("MyService", "1.0.0"))
            .AddAspNetCoreInstrumentation()
            .AddLuckEventBusInstrumentation()  // 添加 EventBus 仪表化
            .AddConsoleExporter();
    });
```

### 2. 配置 RabbitMQ

```csharp
builder.Services.AddLuckEventBusRabbitMq(config =>
{
    config.Host = "localhost";
    config.UserName = "guest";
    config.PassWord = "guest";
    config.Port = 5672;
});
```

## 导出器配置

### Console Exporter

```csharp
.AddConsoleExporter()
```

输出到控制台，适用于开发和调试。

### OpenObserve (推荐)

```csharp
.AddOtlpExporter(options =>
{
    options.Endpoint = new Uri("http://localhost:5081");
    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
    options.Headers = "Authorization=Basic {api_key},organization=default,stream-name=default";
});
```

启动 OpenObserve:
```bash
docker run -d --name openobserve \
  -p 5080:5080 \
  -p 5081:5081 \
  -v openobserve-data:/data \
  -e ZO_DATA_DIR=/data \
  -e ZO_ROOT_USER_EMAIL="root@example.com" \
  -e ZO_ROOT_USER_PASSWORD="Complexpass#123" \
  openobserve/openobserve:latest
```

访问 http://localhost:5080 查看链路追踪。

### Jaeger

```csharp
.AddJaegerExporter(options =>
{
    options.AgentHost = "localhost";
    options.AgentPort = 6831;
})
```

启动 Jaeger:
```bash
docker run -d --name jaeger \
  -p 16686:16686 \
  -p 14268:14268 \
  -p 6831:6831/udp \
  jaegertracing/all-in-one:latest
```

访问 http://localhost:16686 查看链路追踪。

## 生成的 Span

| Span 名称 | Kind | 说明 |
|-----------|------|------|
| {EventBusType}.Publish | Producer | 消息发布 |
| {EventBusType}.Consume | Consumer | 消息消费 |
| {EventBusType}.Process | Internal | 消息处理 |

## Span 标签

### 通用标签

- `eventbus.event_type` - 事件类型全名
- `eventbus.event_name` - 事件名称
- `eventbus.raw_content` - 原始消息内容（JSON）
- `eventbus.handler_type` - 处理器类型（Process）

### RabbitMQ 标签

- `messaging.system` - "rabbitmq"
- `messaging.destination` - 交换机名称
- `messaging.rabbitmq.routing_key` - 路由键
- `messaging.rabbitmq.queue` - 队列名称
- `messaging.source` - 交换机名称（消费时）

## 工作原理

### 架构图

```
┌─────────────────┐     ┌──────────────────────┐     ┌──────────────────┐
│   Application   │     │   DiagnosticListener  │     │   OpenTelemetry  │
│                 │     │                      │     │                  │
│  Publish Event  │────▶│  Luck.EventBus       │────▶│  ActivitySource  │
│                 │     │  (RabbitMQEventBus)  │     │                  │
│  Consume Event  │────▶│                      │────▶│  OTLP Exporter   │────▶  OpenObserve
│                 │     │                      │     │                  │
│  Process Event  │────▶│                      │────▶│  Console Exporter│────▶  Console
└─────────────────┘     └──────────────────────┘     └──────────────────┘
```

### 事件流

1. **Publish**: 应用发布事件 → RabbitMQEventBus 发送消息 → 触发 Published 诊断事件 → 创建 Publish Span
2. **Consume**: RabbitMQ 收到消息 → 触发 Received 诊断事件 → 创建 Consume Span
3. **Process**: 事件处理器执行 → 触发 Processed 诊断事件 → 创建 Process Span

## 完整示例

```csharp
using Luck.EventBus.RabbitMQ;
using Luck.Framework.Event;
using OpenTelemetry.Luck.EventBus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// 配置 RabbitMQ EventBus
builder.Services.AddLuckEventBusRabbitMq(config =>
{
    config.Host = "localhost";
    config.UserName = "guest";
    config.PassWord = "guest";
    config.Port = 5672;
});

// 配置 OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("EventBusDemo", "1.0.0"))
            .AddAspNetCoreInstrumentation()
            .AddLuckEventBusInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:5081");
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            });
    });

// 注册事件处理器
builder.Services.AddTransient<IIntegrationEventHandler<TestEvent>, TestEventHandler>();

var app = builder.Build();
app.Run();
```

## 关键要点

1. **一行代码启用**: `.AddLuckEventBusInstrumentation()`
2. **自动发现**: 自动订阅 `Luck.EventBus.Diagnostics` 诊断监听器
3. **完整链路**: 包含 Publish、Consume、Process 三个阶段
4. **原始内容**: 所有 Span 包含 `eventbus.raw_content` 标签记录完整 JSON
5. **协议支持**: 支持 OTLP/gRPC 和 OTLP/HTTP 协议

## 故障排查

### 看不到链路数据

1. 确认 `AddLuckEventBusInstrumentation()` 已调用
2. 检查 Exporter 配置是否正确
3. 确认后端服务（OpenObserve/Jaeger）已启动
4. 查看控制台是否有错误日志

### 只有部分 Span

1. 如果只收到 Publish：检查消费者是否运行
2. 如果只收到 Consume/Process：检查发布端是否配置正确
3. 确认所有服务使用相同的诊断监听器名称

## 依赖项

- OpenTelemetry (>= 1.9.0)
- Luck.Framework
- Luck.EventBus.RabbitMQ

## 许可证

MIT
