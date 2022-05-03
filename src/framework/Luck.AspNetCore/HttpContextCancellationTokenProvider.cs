using Luck.Framework.Threading;
using Luck.Framework.Utilities;
using Microsoft.AspNetCore.Http;

namespace Luck.AspNetCore
{
    /// <summary>
    /// Http请求中断
    /// </summary>
    public class HttpContextCancellationTokenProvider : ICancellationTokenProvider
    {


        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CancellationToken Token => _httpContextAccessor?.HttpContext?.RequestAborted ?? NullCancellationTokenProvider.Instance.Token;
    }
}
