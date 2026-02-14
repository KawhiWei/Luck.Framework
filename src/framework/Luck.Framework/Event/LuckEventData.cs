using System.Diagnostics;

namespace Luck.Framework.Event;

/// <summary>
/// Luck 事件数据基类
/// </summary>
public abstract class LuckEventData
{
    /// <summary>
    /// 事件定义
    /// </summary>
    public LuckEventDefinition EventDefinition { get; }

    /// <summary>
    /// 事件总线类型
    /// </summary>
    public EventBusType EventBusType { get; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// 当前 Activity
    /// </summary>
    public Activity? Activity { get; } = Activity.Current;

    /// <summary>
    /// 原始内容（序列化后的 JSON）
    /// </summary>
    public string? RawContent { get; set; }

    /// <summary>
    /// 初始化事件数据
    /// </summary>
    protected LuckEventData(LuckEventDefinition eventDefinition, EventBusType eventBusType)
    {
        EventDefinition = eventDefinition;
        EventBusType = eventBusType;
    }

    /// <summary>
    /// 事件 ID
    /// </summary>
    public LuckEventId LuckEventId => EventDefinition.LuckEventId;

    /// <summary>
    /// 日志级别
    /// </summary>
    public LuckLogLevel Level => EventDefinition.Level;
}
