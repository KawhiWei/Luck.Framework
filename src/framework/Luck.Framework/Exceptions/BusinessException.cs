namespace Luck.Framework.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException()
        {
        }
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public BusinessException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public string? ErrorCode { get; set; }
    }
}
