using Luck.AspNetCore.ApiResults;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up ApiResult in an Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    public static class ApiResultServiceCollectionExtensions
    {
        public static void AddApiResult(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSingleton<IApiResultWrapAttribute, ApiResultWrapAttribute>();
        }
    }
}