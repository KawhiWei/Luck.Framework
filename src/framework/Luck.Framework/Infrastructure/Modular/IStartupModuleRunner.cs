using Microsoft.Extensions.DependencyInjection;
using System;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public interface IStartupModuleRunner : IModuleApplication
    {
        void ConfigureServices(IServiceCollection services);

        void Initialize(IServiceProvider service);
    }
}