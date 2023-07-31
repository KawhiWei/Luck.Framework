using Microsoft.Extensions.DependencyInjection;

namespace Luck.AutoDependencyInjection.PropertyInjection
{
    /// <summary>
    /// 属性注入服务提供者工厂
    /// </summary>
    internal class PropertyInjectionServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        /// <summary>
        /// 创建服务提供者
        /// </summary>
        /// <param name="containerBuilder">容器建造者</param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            return new PropertyInjectionServiceProvider(containerBuilder);
        }

        /// <summary>
        /// 创建构建器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns></returns>

        public IServiceCollection CreateBuilder(IServiceCollection? services)
        {

            return services ?? new ServiceCollection();
        }

    }
}
