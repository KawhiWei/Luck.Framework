using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Luck.AppModule;

namespace Luck.AutoDependencyInjection
{
    /// <summary>
    /// 自动注入模块，继承与AppModuleBase类进行实现
    /// </summary>
    public class AutoDependencyAppModule : LuckAppModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            var services = context.Services;
            AddAutoInjection(services);
        }


        private void AddAutoInjection(IServiceCollection services)
        {
            var baseTypes = new Type[]
                { typeof(IScopedDependency), typeof(ITransientDependency), typeof(ISingletonDependency) };
            var types = AssemblyHelper.FindTypes(type =>
                type is { IsClass: true, IsAbstract: false } 
                && (baseTypes.Any(b => b.IsAssignableFrom(type))) 
                || type.GetCustomAttribute<DependencyInjectionAttribute>() != null);
            foreach (var implementedInterType in types)
            {
                var attr = implementedInterType.GetCustomAttribute<DependencyInjectionAttribute>();
                var typeInfo = implementedInterType.GetTypeInfo();
                var serviceTypes = typeInfo.ImplementedInterfaces
                    .Where(x => x.HasMatchingGenericArity(typeInfo)
                                && !x.HasAttribute<IgnoreDependencyAttribute>()
                                && x != typeof(IDisposable))
                    .Select(t => t.GetRegistrationType(typeInfo)).ToList();
                var lifetime = GetServiceLifetime(implementedInterType);
                if (lifetime == null)
                {
                    break;
                }
                if (!serviceTypes.Any())
                {
                    services.Add(new ServiceDescriptor(implementedInterType, implementedInterType, lifetime.Value));
                    continue;
                }

                if (attr?.AddSelf is true)
                {
                    services.Add(new ServiceDescriptor(implementedInterType, implementedInterType, lifetime.Value));
                }

                foreach (var serviceType in serviceTypes.Where(o => !o.HasAttribute<IgnoreDependencyAttribute>()))
                {
                    services.Add(new ServiceDescriptor(serviceType, implementedInterType, lifetime.Value));
                }
            }
        }

        private static ServiceLifetime? GetServiceLifetime(Type type)
        {
            var attr = type.GetCustomAttribute<DependencyInjectionAttribute>();
            return attr?.Lifetime ??
                   (typeof(IScopedDependency).IsAssignableFrom(type)
                       ? ServiceLifetime.Scoped
                       : typeof(ITransientDependency).IsAssignableFrom(type)
                           ? ServiceLifetime.Transient
                           : typeof(ISingletonDependency).IsAssignableFrom(type)
                               ? ServiceLifetime.Singleton
                               : null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ApplicationInitialization(ApplicationContext context)
        {
            context.GetApplicationBuilder();
        }
    }
}