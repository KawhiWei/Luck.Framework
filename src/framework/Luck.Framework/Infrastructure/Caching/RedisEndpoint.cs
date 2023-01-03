namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisEndpoint
    {
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; } = default!;
        
        /// <summary>
        /// 
        /// </summary>
        public int? Timeout { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? Password { get; set; }
    }
}
