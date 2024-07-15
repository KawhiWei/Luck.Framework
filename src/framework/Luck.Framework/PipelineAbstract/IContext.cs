namespace Luck.Framework.PipelineAbstract;

public interface IContext : IInterruptibleContext
{
    string UniqueKey { get; }

    /// <summary>
    /// 扩展属性对象
    /// </summary>
    Dictionary<string, object> Properties { get; }
}