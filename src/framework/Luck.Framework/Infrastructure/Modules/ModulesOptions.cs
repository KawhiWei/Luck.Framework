using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class ModulesOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection Service { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public ModulesOptions(IServiceCollection service)
        {
            Service = service;
        }
    }
}