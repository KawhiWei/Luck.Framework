using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 自动注入模块，继承与AppModuleBase类进行实现
    /// </summary>
    public class DependencyAppModule : AppModule
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
            var baseTypes = new Type[] { typeof(IScopedDependency), typeof(ITransientDependency), typeof(ISingletonDependency) };
            var types = AssemblyHelper.FindTypes(type => type.IsClass && !type.IsAbstract && (baseTypes.Any(b => b.IsAssignableFrom(type))) || type.GetCustomAttribute<DependencyInjectionAttribute>() != null);
            foreach (var implementedInterType in types)
            {
                var attr = implementedInterType.GetCustomAttribute<DependencyInjectionAttribute>();
                var typeInfo = implementedInterType.GetTypeInfo();
                var serviceTypes = typeInfo.ImplementedInterfaces.Where(x => x.HasMatchingGenericArity(typeInfo) && !x.HasAttribute<IgnoreDependencyAttribute>() && x != typeof(IDisposable)).Select(t => t.GetRegistrationType(typeInfo));
                var lifetime = GetServiceLifetime(implementedInterType);
                if (lifetime == null)
                {
                    break;
                }

                var enumerable = serviceTypes as Type[] ?? serviceTypes.ToArray();
                if (!enumerable.Any())
                {
                    services.Add(new ServiceDescriptor(implementedInterType, implementedInterType, lifetime.Value));
                    continue;
                }
                if (attr?.AddSelf == true)
                {
                    services.Add(new ServiceDescriptor(implementedInterType, implementedInterType, lifetime.Value));
                }
                foreach (var serviceType in enumerable.Where(o => !o.HasAttribute<IgnoreDependencyAttribute>()))
                {
                    services.Add(new ServiceDescriptor(serviceType, implementedInterType, lifetime.Value));
                }
            }
        }

        private ServiceLifetime? GetServiceLifetime(Type type)
        {
            var attr = type.GetCustomAttribute<DependencyInjectionAttribute>();
            if (attr != null)
            {
                return attr.Lifetime;
            }

            if (typeof(IScopedDependency).IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            if (typeof(ITransientDependency).IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (typeof(ISingletonDependency).IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ApplicationInitialization(ApplicationContext context)
        {
            var app = context.GetApplicationBuilder();
        }

    }
}