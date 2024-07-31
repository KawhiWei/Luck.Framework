using Luck.Framework.PipelineAbstract;

namespace Luck.Pipeline;

public abstract class Context : IContext
{
    public Context(string uniqueKey)
    {
        UniqueKey = uniqueKey;
    }
    public string UniqueKey { get; }

    /// <summary>
    /// 扩展属性对象 
    /// </summary>
    public Dictionary<string, object> Properties { get; } = new();

    public bool IsInterruptible { get; private set; }

    public Interruptible Interruptible { get; private set; }

    public string InterruptibleReason { get; private set; } = null!;


    public void SetInterruptible(Interruptible interruptible, string interruptibleReason = "")
    {
        this.IsInterruptible = true;
        this.Interruptible = interruptible;
        this.InterruptibleReason = interruptibleReason ?? "无";
    }
}