namespace Luck.MongoDB;
/// <summary>
/// MongoDb表名称特性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MongoDbTableAttribute: Attribute
{
    public MongoDbTableAttribute(string tableName)
    {
        TableName = tableName;
    }

    /// <summary>
    /// MongoDB表名称
    /// </summary>
    public string TableName { get; } 
}