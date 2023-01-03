namespace Luck.Framework.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public NotFoundException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NotFoundException(string? message) : base(message)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
