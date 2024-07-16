namespace Luck.Framework.PipelineAbstract;

public enum Interruptible
{
    /// <summary>
    /// 终止, 不进行任何后续处理
    /// </summary>
    Cancel = 1001,

    /// <summary>
    /// 终止, 自动补偿重试
    /// </summary>
    Retry = 2001,

    /// <summary>
    /// 终止, 进入监控,不自动补偿.
    /// </summary>
    Observe = 2002,
    
    /// <summary>
    /// 跳过执行当前Pipe
    /// </summary>
    Skip = 3002,
}