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

        public LuckException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
