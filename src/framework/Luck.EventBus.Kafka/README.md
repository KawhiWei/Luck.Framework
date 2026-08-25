# Luck.EventBus.Kafka

`Luck.EventBus.Kafka` 当前只包含 Kafka 事件的元数据特性和连接配置模型。它不提供 Kafka producer、consumer、服务注册扩展或后台消费服务；需要完整 Kafka 事件总线的应用必须在此基础上自行实现，或使用其他适配包。

## 安装

```bash
dotnet add package Luck.EventBus.Kafka --version 2.0.14
```

项目依赖 `Luck.Framework`，目标框架与解决方案一致：`net10.0`。

## 配置模型

`KafkaMqConfig` 位于 `Luck.EventBus.Kafka` 命名空间，包含连接信息：

```csharp
using Luck.EventBus.Kafka;

var config = new KafkaMqConfig
{
    Host = "localhost",
    Port = KafkaMqConfig.UseDefaultPort,
    UserName = "kafka-user",
    PassWord = "kafka-password",
    RetryCount = 5
};
```

`Port` 默认值为 `9092`。当前包不会读取配置文件，也不会把 `KafkaMqConfig` 自动注册到依赖注入容器；请由应用负责绑定和注册。

## 事件元数据

`KafkaAttribute` 可标注在事件类型上，用于保存 topic 和 tag：

```csharp
using Luck.EventBus.Kafka.Attributes;
using Luck.Framework.Event;

[Kafka("orders", "order.created")]
public sealed class OrderCreated : IntegrationEvent
{
    public string OrderId { get; init; } = string.Empty;
}
```

运行时可以通过反射读取 `Topic` 和 `Tag`，再将其转换为应用自己的 Kafka producer/consumer 配置。该特性不会自动创建 topic，也不会触发发布或订阅。

## 当前边界

- 包中没有 `AddKafka`、`IIntegrationEventBus` 的 Kafka 实现或 `Confluent.Kafka` 包引用。
- `KafkaMqConfig` 的 `Host`、`UserName` 和 `PassWord` 默认是未初始化字符串，使用前必须显式设置。
- 配置模型中的 `RetryCount` 只保存调用方提供的值，不会自动执行重试。
- 事件基类、处理器契约和诊断模型来自 `Luck.Framework.Event`；RabbitMQ 的实现请查看 [`Luck.EventBus.RabbitMQ`](../Luck.EventBus.RabbitMQ/README.md)。

## 许可证

本项目采用 [LGPL-3.0-only](../../../LICENSE) 许可证。
