# Luck.EventBus.RabbitMQ 使用文档

## 概述

Luck.EventBus.RabbitMQ 是一个完全异步的 RabbitMQ 事件总线实现，支持诊断事件和 OpenTelemetry 分布式追踪。

**特点：**
- 完全异步 API，无阻塞
- 发布/消费通道完全隔离
- 内置诊断事件系统
- 支持 OpenTelemetry 集成
- 自动重连和故障恢复

## 快速开始

### 1. 安装

```bash
dotnet add package Luck.EventBus.RabbitMQ
```

### 2. 注册服务

```csharp
using Microsoft.Extensions.DependencyInjection;

builder.Services.AddLuckEventBusRabbitMq(config =>
{
    config.Host = "localhost";
    config.Port = 5672;
    config.UserName = "guest";
    config.PassWord = "guest";
    config.VirtualHost = "/";
    config.RetryCount = 5;
});
```

### 3. 定义事件

```csharp
using Luck.Framework.Event;
using Luck.EventBus.RabbitMQ.Attributes;
using Luck.EventBus.RabbitMQ.Enums;

[RabbitMq(EWorkModel.PublishSubscribe, "my_exchange", ExchangeType.Direct, "my_routing_key", "my_queue")]
public class OrderCreatedEvent : IntegrationEvent
{
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

### 4. 定义事件处理器

```csharp
using Luck.Framework.Event;

public class OrderCreatedEventHandler : IIntegrationEventHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(OrderCreatedEvent @event)
    {
        _logger.LogInformation("处理订单创建事件: {OrderId}, 金额: {Amount}", 
            @event.OrderId, @event.Amount);
        
        // 处理业务逻辑
        
        return Task.CompletedTask;
    }
}
```

### 5. 注册事件处理器

```csharp
builder.Services.AddTransient<IIntegrationEventHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();
```

### 6. 发布事件

```csharp
using Luck.Framework.Event;

public class OrderService
{
    private readonly IIntegrationEventBus _eventBus;

    public OrderService(IIntegrationEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task CreateOrderAsync(CreateOrderRequest request)
    {
        // 创建订单逻辑...
        
        var orderEvent = new OrderCreatedEvent
        {
            OrderId = Guid.NewGuid().ToString(),
            Amount = request.Amount
        };

        // 发布事件（异步）
        await _eventBus.PublishAsync(orderEvent);
    }
}
```

## 配置选项

### RabbitMqConfig

```csharp
public class RabbitMqConfig
{
    public string Host { get; set; } = "localhost";           // RabbitMQ 主机
    public int Port { get; set; } = 5672;                      // RabbitMQ 端口
    public string UserName { get; set; } = "guest";           // 用户名
    public string PassWord { get; set; } = "guest";           // 密码
    public string VirtualHost { get; set; } = "/";            // 虚拟主机
    public int RetryCount { get; set; } = 5;                  // 重试次数
}
```

## 交换机类型

```csharp
public enum ExchangeType
{
    Direct,    // 直接路由
    Fanout,    // 广播
    Topic,     // 主题路由
    Headers,   // 头路由
    Routing    // 路由
}
```

## 工作模式

```csharp
public enum EWorkModel
{
    None,               // 无交换机
    PublishSubscribe,   // 发布订阅模式
    Routing,            // 路由模式
    Topics,             // 主题模式
    Rpc                 // RPC 模式
}
```

## OpenTelemetry 集成

### 1. 安装 OpenTelemetry 包

```bash
dotnet add package Luck.EventBus.OpenTelemetry
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol
```

### 2. 配置 OpenTelemetry

```csharp
using Luck.EventBus.OpenTelemetry;
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
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                // OpenObserve gRPC 端点
                options.Endpoint = new Uri("http://localhost:5081");
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                options.Headers = "Authorization=Basic cm9vdEBleGFtcGxlLmNvbTp3U1J6RDJpSDltR2RWSENs,organization=default,stream-name=default";
            });
    });
```

### 3. 查看链路追踪

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

访问 http://localhost:5080 查看链路追踪数据。

## 诊断事件

### 事件类型

| 事件名称 | 说明 | 数据 |
|---------|------|------|
| Published | 消息发布成功 | PublishEventData |
| PublishFailed | 消息发布失败 | PublishEventData |
| Received | 收到消息 | ConsumeEventData |
| Processed | 消息处理成功 | ProcessEventData |
| ProcessFailed | 消息处理失败 | ProcessEventData |

### 事件数据结构

所有事件数据都继承自 `LuckEventData`，包含以下公共属性：

```csharp
public abstract class LuckEventData
{
    public LuckEventDefinition EventDefinition { get; }  // 事件定义
    public EventBusType EventBusType { get; }            // 事件总线类型
    public DateTimeOffset Timestamp { get; }             // 时间戳
    public Activity? Activity { get; }                   // 当前 Activity
    public string? RawContent { get; set; }              // 原始消息内容（JSON）
    public LuckEventId LuckEventId { get; }              // 事件 ID
    public LuckLogLevel Level { get; }                   // 日志级别
}
```

### 订阅诊断事件

```csharp
using System.Diagnostics;
using Luck.Framework.Event;

public class DiagnosticEventSubscriber : IObserver<DiagnosticListener>
{
    public void Subscribe()
    {
        DiagnosticListener.AllListeners.Subscribe(this);
    }

    public void OnNext(DiagnosticListener listener)
    {
        if (listener.Name == "Luck.EventBus.Diagnostics")
        {
            listener.Subscribe(new EventObserver());
        }
    }

    private class EventObserver : IObserver<KeyValuePair<string, object?>>
    {
        public void OnNext(KeyValuePair<string, object?> value)
        {
            var eventName = value.Key;
            var eventData = value.Value as LuckEventData;
            
            Console.WriteLine($"[Event] {eventName}");
            Console.WriteLine($"  Type: {eventData?.EventBusType}");
            Console.WriteLine($"  Timestamp: {eventData?.Timestamp}");
            Console.WriteLine($"  RawContent: {eventData?.RawContent}");
        }

        public void OnError(Exception error) { }
        public void OnCompleted() { }
    }

    public void OnError(Exception error) { }
    public void OnCompleted() { }
}
```

## 最佳实践

### 1. 连接配置

```csharp
builder.Services.AddLuckEventBusRabbitMq(config =>
{
    // 使用环境变量或配置中心
    config.Host = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
    config.Port = builder.Configuration.GetValue<int>("RabbitMQ:Port", 5672);
    config.UserName = builder.Configuration["RabbitMQ:UserName"] ?? "guest";
    config.PassWord = builder.Configuration["RabbitMQ:PassWord"] ?? "guest";
    config.RetryCount = 5;
});
```

### 2. 异常处理

```csharp
try
{
    await _eventBus.PublishAsync(orderEvent);
}
catch (Exception ex)
{
    _logger.LogError(ex, "发布事件失败");
    // 处理失败逻辑，如重试或记录到数据库
}
```

### 3. 性能考虑

- 发布和消费使用独立的通道池，避免相互影响
- 所有操作都是异步的，不会阻塞线程
- 诊断事件包含原始消息内容，便于调试

## 故障排查

### 连接失败

1. 检查 RabbitMQ 服务是否运行
2. 检查主机名、端口、用户名和密码
3. 检查网络连接和防火墙设置

### 收不到消息

1. 确认事件处理器已注册到 DI
2. 检查交换机、队列和路由键配置
3. 查看 RabbitMQ 管理界面（http://localhost:15672）

### OpenTelemetry 无数据

1. 确认 OpenObserve 容器已启动（端口 5080/5081）
2. 检查 OTLP 端点配置是否正确
3. 查看 API Key 是否有效
4. 检查控制台是否有错误日志

## 示例项目

参考 `sample/EventBus.TestApi` 项目获取完整的示例代码。

启动示例：
```bash
cd sample/EventBus.TestApi
dotnet run
```

发送测试事件：
```bash
curl -X POST http://localhost:5000/api/test/send \
  -H "Content-Type: application/json" \
  -d '{"message": "Hello RabbitMQ!"}'
```
