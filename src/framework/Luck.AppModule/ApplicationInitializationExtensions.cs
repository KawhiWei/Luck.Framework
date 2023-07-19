using Luck.Framework.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.AppModule
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationInitializationExtensions
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <returns></returns>
        public static IApplicationBuilder GetApplicationBuilder(this ApplicationContext applicationContext)
        {
            return applicationContext.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
        }
    }
}