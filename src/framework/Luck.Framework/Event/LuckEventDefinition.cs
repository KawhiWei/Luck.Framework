namespace Luck.Framework.Event;

/// <summary>
/// Luck 事件定义基类
/// </summary>
public class LuckEventDefinition
{
    /// <summary>
    /// 事件 ID
    /// </summary>
    public LuckEventId LuckEventId { get; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LuckLogLevel Level { get; }

    /// <summary>
    /// 事件定义名称
    /// </summary>
    public string Name => LuckEventId.Name!;

    /// <summary>
    /// 初始化事件定义
    /// </summary>
    public LuckEventDefinition(LuckEventId luckEventId, LuckLogLevel level)
    {
        LuckEventId = luckEventId;
        Level = level;
    }
}
