using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.EntityFrameworkCore.Repositories;
using Luck.EntityFrameworkCore.UnitOfWorks;
using Luck.Framework.Exceptions;
using Luck.Framework.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
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

        public static IServiceCollection AddLuckDbContext<TDbContext>(this IServiceCollection services, Action<EFDbContextConfig> efDbContextAction, Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction = null) where TDbContext : LuckDbContextBase
        {
            if (efDbContextAction == null)
                throw new LuckException(nameof(efDbContextAction));

            services.AddDbContext<ILuckDbContext, TDbContext>((provider, dbcontextbuilder) =>
            {
                EFDbContextConfig config = new EFDbContextConfig();
                efDbContextAction.Invoke(config);
                var dbType = config.Type;
                var drivenProvider = provider.GetServices<IDbContextDrivenProvider>().FirstOrDefault(x => x.Type.Equals(dbType));

                if (drivenProvider == null)
                    throw new LuckException($"{nameof(drivenProvider)}没有对应的{dbType}的实现！");
                var builder = drivenProvider.Builder<TDbContext>(dbcontextbuilder, config.ConnnectionString);
                optionsAction?.Invoke(provider, builder);
            });

            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
