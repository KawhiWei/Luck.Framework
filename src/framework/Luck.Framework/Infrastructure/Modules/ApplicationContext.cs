using Luck.Framework.Extensions;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 自定义应用上下文
    /// </summary>
    public class ApplicationContext : IServiceProviderAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ApplicationContext(IServiceProvider serviceProvider)
        {
            serviceProvider.NotNull(nameof(serviceProvider));
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }
    }
}