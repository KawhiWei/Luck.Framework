namespace Luck.Framework.PipelineAbstract;

/// <summary>
/// 中断策略支持
/// </summary>
public interface IInterruptibleContext
{
    bool IsInterruptible { get; }

    Interruptible Interruptible { get; }

    /// <summary>
    /// 中断原因
    /// </summary>
    string InterruptibleReason { get; }

    public void SetInterruptible(Interruptible interruptible, string interruptibleReason);
}