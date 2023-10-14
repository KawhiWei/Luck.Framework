using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Luck.AppModule;
using Luck.AutoDependencyInjection;

namespace Luck.TestBase
{
    public abstract class IntegratedTest<TStartup> : TestBaseWithServiceProvider where TStartup : IAppModule
    {
        protected IModuleApplication Application { get; }

        protected IServiceProvider RootServiceProvider { get; }

        protected IServiceScope TestServiceScope { get; }

        protected override IServiceProvider ServiceProvider => Application.ServiceProvider;

        protected IntegratedTest()
        {
            Environment.SetEnvironmentVariable("appid", "apppidaasdasd");
            var services = CreateServiceCollection();

            BeforeAddApplication(services);
            var application = services.AddApplication<TStartup>();
            Application = services.GetBuildService<IModuleApplication>();
            AfterAddApplication(services);
            RootServiceProvider = CreateServiceProvider(services);
            TestServiceScope = RootServiceProvider.CreateScope();
            ((StartupModuleRunner)Application).Initialize(TestServiceScope.ServiceProvider);
        }

        protected virtual IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        protected virtual void BeforeAddApplication(IServiceCollection services)
        {
        }

        protected virtual void AfterAddApplication(IServiceCollection services)
        {
        }

        protected virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProviderFromFactory();
        }

        protected virtual IServiceProvider ConfigureProvider(Action<IServiceCollection> configure)
        {
            var services = new ServiceCollection();

            configure(services);

            return services.BuildServiceProviderFromFactory();
        }
    }
}
