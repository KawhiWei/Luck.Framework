using Luck.Framework.Repositories;
using Luck.FrameworkCore.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加工作单元
        /// </summary>
        /// <param name="services"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultRepository(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.Add(new ServiceDescriptor(typeof(IAggregateRootRepository<,>), typeof(EFCoreAggregateRootRepository<,>), lifetime));
            services.Add(new ServiceDescriptor(typeof(IEntityRepository<,>), typeof(EFCoreEntityRepository<,>), lifetime));
            return services;
        }
    }
}
