namespace Luck.AspNetCore.ApiResults
{
    public class ApiResultWithStackTrace : ApiResult
    {
        public ApiResultWithStackTrace()
        {
        }

        public ApiResultWithStackTrace(object result) : base(result)
        {
        }

        public ApiResultWithStackTrace(string errorCode, string errorMessage, string? stackTrace = null) : base(errorCode, errorMessage)
        {
            StackTrace = stackTrace;
        }

        public string? StackTrace { get; set; }
    }
}