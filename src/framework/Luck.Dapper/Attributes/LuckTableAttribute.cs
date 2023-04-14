namespace Luck.Dapper.Attributes;

/// <summary>
/// 数据库表名称
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class LuckTableAttribute: Attribute
{
    /// <summary>
    /// Optional Table attribute.
    /// </summary>
    /// <param name="tableName"></param>
    public LuckTableAttribute(string tableName) => Name = tableName;
    /// <summary>
    /// Name of the table
    /// </summary>
    public string Name { get; private set; }
}