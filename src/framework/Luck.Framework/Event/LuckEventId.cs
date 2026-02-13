namespace Luck.Framework.Event;

/// <summary>
/// Luck 事件 ID 结构体，避免与 Microsoft.Extensions.Logging.EventId 冲突
/// </summary>
public readonly struct LuckEventId : IEquatable<LuckEventId>
{
    /// <summary>
    /// 获取事件 ID 数值
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// 获取事件名称
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// 初始化 LuckEventId
    /// </summary>
    public LuckEventId(int id, string? name = null)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// 转换为字符串
    /// </summary>
    public override string ToString()
    {
        return Name ?? Id.ToString();
    }

    /// <summary>
    /// 隐式转换为 int
    /// </summary>
    public static implicit operator int(LuckEventId luckEventId) => luckEventId.Id;

    /// <summary>
    /// 隐式转换为 LuckEventId
    /// </summary>
    public static implicit operator LuckEventId(int i) => new(i);

    /// <summary>
    /// 相等比较
    /// </summary>
    public bool Equals(LuckEventId other)
    {
        return Id == other.Id;
    }

    /// <summary>
    /// 相等比较
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is LuckEventId other && Equals(other);
    }

    /// <summary>
    /// 获取哈希码
    /// </summary>
    public override int GetHashCode()
    {
        return Id;
    }

    /// <summary>
    /// 相等运算符
    /// </summary>
    public static bool operator ==(LuckEventId left, LuckEventId right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// 不等运算符
    /// </summary>
    public static bool operator !=(LuckEventId left, LuckEventId right)
    {
        return !left.Equals(right);
    }
}
