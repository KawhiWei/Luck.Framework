namespace Luck.AspNetCore.ApiResults
{
    public class ApiResult
    {
        public ApiResult() { }
        public ApiResult(string errorCode, string errorMessage)
        {
            Success = false;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public ApiResult(object? result)
        {
            Success = true;
            Result = result;
        }

        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public object? Result { get; set; }
    }
}