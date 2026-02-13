# Luck.EventBus.RabbitMQ 诊断事件使用说明

## 概述

参照 Microsoft.EntityFrameworkCore 的诊断事件实现模式，Luck.EventBus.RabbitMQ 现在支持通过 `DiagnosticListener` 发送详细的诊断事件数据，可被 OpenTelemetry、Application Insights 等采集器收集。

**特点：**
- 无需引用 OpenTelemetry 包
- 使用 .NET 内置的 `DiagnosticListener` 机制
- 通过 `RabbitMqConfig.EnableDiagnosticEvents` 控制开关
- 支持发布、消费、处理全流程追踪

## 快速开始

### 1. 启用诊断事件

```csharp
services.AddEventBusRabbitMq(config =>
{
    config.Host = "localhost";
    config.UserName = "guest";
    config.PassWord = "guest";
    config.Port = 5672;
    config.VirtualHost = "/";
    config.RetryCount = 5;
    // 启用诊断事件
    config.EnableDiagnosticEvents = true;
});
```

### 2. 订阅诊断事件

```csharp
using System.Diagnostics;
using Luck.EventBus.RabbitMQ.Diagnostics;

public class RabbitMqDiagnosticObserver : IObserver<DiagnosticListener>, IObserver<KeyValuePair<string, object>>
{
    private readonly List<IDisposable> _subscriptions = new();

    public void Subscribe()
    {
        DiagnosticListener.AllListeners.Subscribe(this);
    }

    public void OnNext(DiagnosticListener listener)
    {
        // 订阅 RabbitMQ 诊断监听器
        if (listener.Name == RabbitMqDiagnosticConstants.DiagnosticListenerName)
        {
            var subscription = listener.Subscribe(this);
            _subscriptions.Add(subscription);
        }
    }

    public void OnNext(KeyValuePair<string, object> value)
    {
        switch (value.Key)
        {
            case nameof(RabbitMqEventId.MessagePublished):
                HandleMessagePublished(value.Value as RabbitMqPublishEventData);
                break;

            case nameof(RabbitMqEventId.MessagePublishFailed):
                HandleMessagePublishFailed(value.Value as RabbitMqPublishEventData);
                break;

            case nameof(RabbitMqEventId.MessageReceived):
                HandleMessageReceived(value.Value as RabbitMqConsumeEventData);
                break;

            case nameof(RabbitMqEventId.MessageProcessed):
                HandleMessageProcessed(value.Value as RabbitMqProcessEventData);
                break;

            case nameof(RabbitMqEventId.MessageProcessingFailed):
                HandleMessageProcessingFailed(value.Value as RabbitMqProcessEventData);
                break;
        }
    }

    private void HandleMessagePublished(RabbitMqPublishEventData? data)
    {
        if (data == null) return;

        Console.WriteLine($"[发布成功] Event: {data.EventName}");
        Console.WriteLine($"  Type: {data.EventType}");
        Console.WriteLine($"  Exchange: {data.Exchange}");
        Console.WriteLine($"  RoutingKey: {data.RoutingKey}");
        Console.WriteLine($"  ContentSize: {data.ContentSize} bytes");
        Console.WriteLine($"  Timestamp: {data.Timestamp}");
        Console.WriteLine($"  TraceId: {data.Activity?.TraceId}");

        if (!string.IsNullOrEmpty(data.EventContent))
        {
            Console.WriteLine($"  Content: {data.EventContent.Substring(0, Math.Min(100, data.EventContent.Length))}...");
        }
    }

    private void HandleMessagePublishFailed(RabbitMqPublishEventData? data)
    {
        if (data == null) return;

        Console.WriteLine($"[发布失败] Event: {data.EventName}");
        Console.WriteLine($"  Error: {data.Exception?.Message}");
    }

    private void HandleMessageReceived(RabbitMqConsumeEventData? data)
    {
        if (data == null) return;

        Console.WriteLine($"[收到消息] Event: {data.EventName}");
        Console.WriteLine($"  Queue: {data.Queue}");
        Console.WriteLine($"  MessageId: {data.MessageId}");
        Console.WriteLine($"  ContentSize: {data.ContentSize} bytes");
    }

    private void HandleMessageProcessed(RabbitMqProcessEventData? data)
    {
        if (data == null) return;

        Console.WriteLine($"[处理成功] Event: {data.EventName}");
        Console.WriteLine($"  Handler: {data.HandlerType}");
        Console.WriteLine($"  Duration: {data.Duration?.TotalMilliseconds} ms");
    }

    private void HandleMessageProcessingFailed(RabbitMqProcessEventData? data)
    {
        if (data == null) return;

        Console.WriteLine($"[处理失败] Event: {data.EventName}");
        Console.WriteLine($"  Handler: {data.HandlerType}");
        Console.WriteLine($"  Error: {data.Exception?.Message}");
    }

    public void OnCompleted() { }
    public void OnError(Exception error) { }

    public void Dispose()
    {
        foreach (var subscription in _subscriptions)
        {
            subscription.Dispose();
        }
        _subscriptions.Clear();
    }
}
```

### 3. 注册观察者

```csharp
// 在 Startup.cs 或 Program.cs 中
var observer = new RabbitMqDiagnosticObserver();
observer.Subscribe();

// 或者注册为单例服务
services.AddSingleton<RabbitMqDiagnosticObserver>(provider =>
{
    var observer = new RabbitMqDiagnosticObserver();
    observer.Subscribe();
    return observer;
});
```

## 数据结构

### RabbitMqPublishEventData

```csharp
public class RabbitMqPublishEventData : RabbitMqEventData
{
    public string EventType { get; }        // 事件类型全名
    public string EventName { get; }        // 事件名称
    public string? EventContent { get; }    // 事件内容（JSON）
    public string Exchange { get; }         // 交换机
    public string RoutingKey { get; }       // 路由键
    public string? Queue { get; }           // 队列名
    public string? MessageId { get; }       // 消息ID
    public int? ContentSize { get; }        // 内容大小（字节）
    public Exception? Exception { get; }    // 异常信息
}
```

### RabbitMqConsumeEventData

```csharp
public class RabbitMqConsumeEventData : RabbitMqEventData
{
    public string EventType { get; }        // 事件类型全名
    public string EventName { get; }        // 事件名称
    public string Exchange { get; }         // 交换机
    public string RoutingKey { get; }       // 路由键
    public string Queue { get; }            // 队列名
    public string MessageId { get; }        // 消息ID
    public int ContentSize { get; }         // 内容大小（字节）
    public string? ConsumerTag { get; }     // 消费者标签
}
```

### RabbitMqProcessEventData

```csharp
public class RabbitMqProcessEventData : RabbitMqEventData
{
    public string EventType { get; }        // 事件类型全名
    public string EventName { get; }        // 事件名称
    public string HandlerType { get; }      // 处理器类型
    public string? EventContent { get; }    // 事件内容（JSON）
    public TimeSpan? Duration { get; }      // 处理持续时间
    public Exception? Exception { get; }    // 异常信息
}
```

### 基类 RabbitMqEventData

```csharp
public abstract class RabbitMqEventData : LuckEventData
{
    public LuckEventDefinition EventDefinition { get; }     // 事件定义
    public string Message { get; }                           // 事件消息
    public DateTimeOffset Timestamp { get; }                // 时间戳
    public Activity? Activity { get; }                      // 当前 Activity
    public LuckEventId LuckEventId { get; }                 // 事件 ID
    public LuckLogLevel Level { get; }                      // 日志级别
}
```

## 与 OpenTelemetry 集成

虽然项目不直接引用 OpenTelemetry，但你可以在外部应用中配置 OpenTelemetry 来采集这些诊断事件：

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("MyService"))
            // 添加自定义处理器来采集 RabbitMQ 诊断事件
            .AddProcessor(new RabbitMqTracingProcessor())
            .AddConsoleExporter()
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "localhost";
                options.AgentPort = 6831;
            });
    });

// 自定义处理器
public class RabbitMqTracingProcessor : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        // 处理 Activity 数据
    }
}
```

## 事件 ID

参照 EF Core 的模式，定义了以下事件 ID：

| 事件名称 | ID | 级别 | 说明 |
|---------|-----|------|------|
| MessagePublished | 100001 | Information | 消息发布成功 |
| MessagePublishFailed | 100003 | Error | 消息发布失败 |
| MessageReceived | 100004 | Information | 收到消息 |
| MessageProcessed | 100006 | Information | 消息处理成功 |
| MessageProcessingFailed | 100007 | Error | 消息处理失败 |

## 最佳实践

1. **生产环境控制**
   ```csharp
   // 只在需要时启用
   config.EnableDiagnosticEvents = 
       Environment.GetEnvironmentVariable("ENABLE_DIAGNOSTICS") == "true";
   ```

2. **性能考虑**
   - 诊断事件会在发布和消费时序列化事件内容为 JSON
   - 如果事件内容很大，注意性能影响
   - 可以通过条件编译在 Release 模式禁用

3. **与 Activity 集成**
   - 每个诊断事件都包含当前的 `Activity` 对象
   - 可以获取 TraceId、SpanId 等信息
   - 支持分布式追踪传播

## 故障排查

### 看不到诊断事件

1. 确认 `EnableDiagnosticEvents` 设置为 `true`
2. 检查 `DiagnosticListener` 订阅是否正确
3. 确认监听名称匹配：`Luck.EventBus.RabbitMQ.Diagnostics`

### 性能下降

1. 禁用诊断事件
2. 减少事件内容的序列化
3. 使用异步处理器
