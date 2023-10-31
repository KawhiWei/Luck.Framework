using Luck.AutoDependencyInjection.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Luck.AutoDependencyInjection.PropertyInjection
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 使用属性注入
        /// </summary>
        /// <param name="hostBuilder">host构建器</param>
        /// <returns></returns>
        public static void UsePropertyInjection(this IHostBuilder hostBuilder)
        {
            //hostBuilder.UseServiceProviderFactory(new PropertyInjectionServiceProviderFactory()).ConfigureServices(ConfigureServices);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.
                AddSingleton<IPropertyInjector, PropertyInjector>()
               .AddSingleton<IControllerFactory, PropertyInjectionControllerFactory>();
        }
    }
}
