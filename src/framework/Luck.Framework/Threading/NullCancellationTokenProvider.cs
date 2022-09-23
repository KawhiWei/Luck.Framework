namespace Luck.Framework.Threading
{
    /// <summary>
    /// 空的取消标记提供者
    /// </summary>
    public class NullCancellationTokenProvider : ICancellationTokenProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();
        /// <summary>
        /// 
        /// </summary>
        public CancellationToken Token { get; } = CancellationToken.None;

        private NullCancellationTokenProvider()
        {

        }
    }
}
