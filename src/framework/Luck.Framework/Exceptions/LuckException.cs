

namespace Luck.Framework.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class LuckException : Exception
    {

        /// <summary>
        /// 
        /// </summary>
        public LuckException()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public LuckException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        public LuckException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public LuckException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public string? ErrorCode { get; private set; }
    }
}
