using Microsoft.Extensions.DependencyInjection;
using System;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public interface IStartupModuleRunner : IModuleApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        void Initialize(IServiceProvider service);
    }
}