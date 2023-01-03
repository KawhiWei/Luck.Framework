namespace Luck.Framework.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public BusinessException()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public BusinessException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        public BusinessException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public BusinessException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 
        /// </summary>
        public string? ErrorCode { get; set; }
    }
}
