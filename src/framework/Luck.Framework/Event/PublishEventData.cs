namespace Luck.Framework.Event;

/// <summary>
/// 发布消息事件数据
/// </summary>
public class PublishEventData : LuckEventData
{
    /// <summary>
    /// 初始化发布事件数据
    /// </summary>
    public PublishEventData(
        LuckEventDefinition eventDefinition,
        EventBusType eventBusType,
        string eventType,
        string eventName,
        string? eventContent,
        string exchange,
        string routingKey,
        string? queue = null,
        string? messageId = null,
        int? contentSize = null,
        Exception? exception = null)
        : base(eventDefinition, eventBusType)
    {
        EventType = eventType;
        EventName = eventName;
        EventContent = eventContent;
        Exchange = exchange;
        RoutingKey = routingKey;
        Queue = queue;
        MessageId = messageId;
        ContentSize = contentSize;
        Exception = exception;
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
    /// 事件内容（JSON）
    /// </summary>
    public virtual string? EventContent { get; }

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
    public virtual string? Queue { get; }

    /// <summary>
    /// 消息ID
    /// </summary>
    public virtual string? MessageId { get; }

    /// <summary>
    /// 内容大小（字节）
    /// </summary>
    public virtual int? ContentSize { get; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public virtual Exception? Exception { get; }
}
