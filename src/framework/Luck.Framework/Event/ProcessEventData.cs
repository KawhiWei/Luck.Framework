namespace Luck.Framework.Event;

/// <summary>
/// 处理消息事件数据
/// </summary>
public class ProcessEventData : LuckEventData
{
    /// <summary>
    /// 初始化处理事件数据
    /// </summary>
    public ProcessEventData(
        LuckEventDefinition eventDefinition,
        EventBusType eventBusType,
        string eventType,
        string eventName,
        string handlerType,
        string? eventContent = null,
        TimeSpan? duration = null,
        Exception? exception = null)
        : base(eventDefinition, eventBusType)
    {
        EventType = eventType;
        EventName = eventName;
        HandlerType = handlerType;
        EventContent = eventContent;
        Duration = duration;
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
    /// 处理器类型
    /// </summary>
    public virtual string HandlerType { get; }

    /// <summary>
    /// 事件内容（JSON）
    /// </summary>
    public virtual string? EventContent { get; }

    /// <summary>
    /// 处理持续时间
    /// </summary>
    public virtual TimeSpan? Duration { get; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public virtual Exception? Exception { get; }
}
