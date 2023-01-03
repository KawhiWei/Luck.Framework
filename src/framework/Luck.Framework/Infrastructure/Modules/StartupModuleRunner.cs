using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class StartupModuleRunner : ModuleApplicationBase, IStartupModuleRunner
    {
        /// <summary>
        /// 程序启动运行时
        /// </summary>
        /// <param name="startupModuleType"></param>
        /// <param name="services"></param>
        public StartupModuleRunner(Type startupModuleType, IServiceCollection services)
            : base(startupModuleType, services)
        {
            services.AddSingleton<IStartupModuleRunner>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            IocManage.Instance.SetServiceCollection(services);
            var context = new ConfigureServicesContext(services);
            services.AddSingleton(context);
            foreach (var config in Modules)
            {
                services.AddSingleton(config);
                config.ConfigureServices(context);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public void Initialize(IServiceProvider service)
        {
            IocManage.Instance.SetApplicationServiceProvider(service);
            SetServiceProvider(service);
            using var scope = ServiceProvider.CreateScope();
            //using var scope = service.CreateScope();
            var ctx = new ApplicationContext(scope.ServiceProvider);
            foreach (var cfg in Modules)
            {
                cfg.ApplicationInitialization(ctx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (ServiceProvider is IDisposable disposableServiceProvider)
            {
                disposableServiceProvider.Dispose();
            }
        }
    }
}