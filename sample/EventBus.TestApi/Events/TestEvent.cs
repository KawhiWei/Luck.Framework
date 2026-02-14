using Luck.Framework.Event;
using Luck.EventBus.RabbitMQ.Attributes;
using Luck.EventBus.RabbitMQ.Enums;

namespace EventBus.TestApi.Events;

/// <summary>
/// 测试事件
/// </summary>
[RabbitMq(EWorkModel.PublishSubscribe, "test_exchange", ExchangeType.Routing, "test_routing_key", "test_queue")]
public class TestEvent : IntegrationEvent
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
