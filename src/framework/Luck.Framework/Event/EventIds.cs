namespace Luck.Framework.Event;

/// <summary>
/// 事件总线事件 ID 定义
/// </summary>
public static class EventIds
{
    /// <summary>
    /// 事件 ID 基础值
    /// </summary>
    public const int EventIdBase = 100000;

    /// <summary>
    /// 消息发布
    /// </summary>
    public static readonly LuckEventId Published = new(EventIdBase + 1, nameof(Published));

    /// <summary>
    /// 消息发布失败
    /// </summary>
    public static readonly LuckEventId PublishFailed = new(EventIdBase + 2, nameof(PublishFailed));

    /// <summary>
    /// 消息已接收
    /// </summary>
    public static readonly LuckEventId Received = new(EventIdBase + 3, nameof(Received));

    /// <summary>
    /// 消息处理中
    /// </summary>
    public static readonly LuckEventId Processing = new(EventIdBase + 4, nameof(Processing));

    /// <summary>
    /// 消息处理成功
    /// </summary>
    public static readonly LuckEventId Processed = new(EventIdBase + 5, nameof(Processed));

    /// <summary>
    /// 消息处理失败
    /// </summary>
    public static readonly LuckEventId ProcessFailed = new(EventIdBase + 6, nameof(ProcessFailed));
}
