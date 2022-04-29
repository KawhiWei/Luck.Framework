

namespace Luck.Framework.Exceptions
{
    public class LuckException : Exception
    {

        public LuckException()
        {
        }
        public LuckException(string message) : base(message)
        {
        }

        public LuckException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public LuckException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public string? ErrorCode { get; set; }
    }
}
