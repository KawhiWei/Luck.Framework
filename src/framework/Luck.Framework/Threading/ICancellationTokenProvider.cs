namespace Luck.Framework.Threading
{
    /// <summary>
    /// 异步任务取消提供者
    /// </summary>
    public interface ICancellationTokenProvider
    {

        /// <summary>
        /// 获取异步任务取消标识
        /// </summary>
        CancellationToken Token { get; }
    }
}
