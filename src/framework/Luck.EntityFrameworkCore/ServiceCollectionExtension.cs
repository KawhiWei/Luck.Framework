using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.EntityFrameworkCore.Repositories;
using Luck.EntityFrameworkCore.UnitOfWorks;
using Luck.Framework.Exceptions;
using Luck.Framework.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        public static IServiceCollection AddDefaultRepository(this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.Add(new ServiceDescriptor(typeof(IAggregateRootRepository<,>),
                typeof(EfCoreAggregateRootRepository<,>), lifetime));
            services.Add(new ServiceDescriptor(typeof(IEntityRepository<,>), typeof(EfCoreEntityRepository<,>),
                lifetime));
            return services;
        }

        public static IServiceCollection AddLuckDbContext<TDbContext>(this IServiceCollection services,
            Action<EfDbContextConfig> efDbContextAction,
            Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction = null)
            where TDbContext : LuckDbContextBase
        {
            if (efDbContextAction == null)
                throw new LuckException(nameof(efDbContextAction));

            // var serviceDescriptor = services.FirstOrDefault<ServiceDescriptor>((Func<ServiceDescriptor, bool>) (d => d.ServiceType == typeof (TContextImplementation)));
            
            services.AddDbContext<LuckDbContextBase, TDbContext>((provider, dbContextBuilder) =>
            {
                var config = new EfDbContextConfig();
                efDbContextAction.Invoke(config);
                var dbType = config.Type;
                var drivenProvider = provider.GetServices<IDbContextDrivenProvider>()
                    .FirstOrDefault(x => x.Type.Equals(dbType));

                if (drivenProvider == null)
                    throw new LuckException($"{nameof(drivenProvider)}没有对应的{dbType}的实现！");
                var builder = drivenProvider.Builder(dbContextBuilder, config.ConnectionString,
                    config.QuerySplittingBehavior);
                optionsAction?.Invoke(provider, builder);
            });

            return services;
        }
        public static IServiceCollection AddLuckDbContextPool<TDbContext>(this IServiceCollection services,
            Action<EfDbContextConfig> efDbContextAction,
            Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction = null)
            where TDbContext : LuckDbContextBase
        {
            if (efDbContextAction == null)
                throw new LuckException(nameof(efDbContextAction));

            // var serviceDescriptor = services.FirstOrDefault<ServiceDescriptor>((Func<ServiceDescriptor, bool>) (d => d.ServiceType == typeof (TContextImplementation)));
            
            services.AddDbContextPool<LuckDbContextBase, TDbContext>((provider, dbContextBuilder) =>
            {
                var config = new EfDbContextConfig();
                efDbContextAction.Invoke(config);
                var dbType = config.Type;
                var drivenProvider = provider.GetServices<IDbContextDrivenProvider>()
                    .FirstOrDefault(x => x.Type.Equals(dbType));

                if (drivenProvider == null)
                    throw new LuckException($"{nameof(drivenProvider)}没有对应的{dbType}的实现！");
                var builder = drivenProvider.Builder(dbContextBuilder, config.ConnectionString,
                    config.QuerySplittingBehavior);
                optionsAction?.Invoke(provider, builder);
            });

            return services;
        }
        

        [Obsolete("存在部分问题未解决")]
        public static IServiceCollection AddPooledLuckDbContextFactory<TDbContext>(this IServiceCollection services,
            Action<EfDbContextConfig> efDbContextAction,
            Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction = null)
            where TDbContext : LuckDbContextBase
        {
            if (efDbContextAction == null)
                throw new LuckException(nameof(efDbContextAction));

            //services.AddDbContext<ILuckDbContext, TDbContext>()
            services.TryAdd(new ServiceDescriptor(typeof(ILuckDbContext), typeof(TDbContext),
                ServiceLifetime.Scoped));
            services.AddPooledDbContextFactory<TDbContext>((provider, dbContextBuilder) =>
            {
                var config = new EfDbContextConfig();
                efDbContextAction.Invoke(config);
                var dbType = config.Type;
                var drivenProvider = provider.GetServices<IDbContextDrivenProvider>()
                    .FirstOrDefault(x => x.Type.Equals(dbType));

                if (drivenProvider == null)
                    throw new LuckException($"{nameof(drivenProvider)}没有对应的{dbType}的实现！");
                var builder = drivenProvider.Builder(dbContextBuilder, config.ConnectionString,
                    config.QuerySplittingBehavior);
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