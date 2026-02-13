namespace Luck.Framework.Event;

/// <summary>
/// 消费消息事件数据
/// </summary>
public class ConsumeEventData : LuckEventData
{
    /// <summary>
    /// 初始化消费事件数据
    /// </summary>
    public ConsumeEventData(
        LuckEventDefinition eventDefinition,
        EventBusType eventBusType,
        string eventType,
        string eventName,
        string exchange,
        string routingKey,
        string queue,
        string messageId,
        int contentSize,
        string? consumerTag = null)
        : base(eventDefinition, eventBusType)
    {
        EventType = eventType;
        EventName = eventName;
        Exchange = exchange;
        RoutingKey = routingKey;
        Queue = queue;
        MessageId = messageId;
        ContentSize = contentSize;
        ConsumerTag = consumerTag;
    }

    /// <summary>
    /// 事件类型全名
    /// </summary>
    public virtual string EventType { get; }

    /// <summary>
    /// 事件名称
    /// </summary>
    public virtual string EventName { get; }

    /// <summary>
    /// 交换机名称
    /// </summary>
    public virtual string Exchange { get; }

    /// <summary>
    /// 路由键
    /// </summary>
    public virtual string RoutingKey { get; }

    /// <summary>
    /// 队列名称
    /// </summary>
    public virtual string Queue { get; }

    /// <summary>
    /// 消息ID
    /// </summary>
    public virtual string MessageId { get; }

    /// <summary>
    /// 内容大小（字节）
    /// </summary>
    public virtual int ContentSize { get; }

    /// <summary>
    /// 消费者标签
    /// </summary>
    public virtual string? ConsumerTag { get; }
}
