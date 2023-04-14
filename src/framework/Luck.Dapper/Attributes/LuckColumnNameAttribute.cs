namespace Luck.Dapper.Attributes;

/// <summary>
/// 数据库列名
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class LuckColumnNameAttribute: Attribute
{
    /// <summary>
    /// column Name
    /// </summary>
    /// <param name="columnName"></param>
    public LuckColumnNameAttribute(string columnName) => ColumnName = columnName;

    /// <summary>
    /// name of column
    /// </summary>
    public string ColumnName { get; private set; }
}