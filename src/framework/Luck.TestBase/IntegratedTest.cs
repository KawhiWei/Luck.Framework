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
        private IModuleApplication? Application { get; }

        private IServiceProvider RootServiceProvider { get; }

        private IServiceScope TestServiceScope { get; }

        protected override IServiceProvider ServiceProvider => Application is null ? throw new InvalidOperationException() : Application.ServiceProvider;

        protected IntegratedTest()
        {
            Environment.SetEnvironmentVariable("appid", "apppidaasdasd");
            var services = CreateServiceCollection();

            BeforeAddApplication(services);
            var application = services.AddApplication<TStartup>();
            Application = services.GetBuildService<IModuleApplication>();
            if (Application is null)
            {
                throw new InvalidOperationException();
            }

            AfterAddApplication(services);
            RootServiceProvider = CreateServiceProvider(services);
            TestServiceScope = RootServiceProvider.CreateScope();
            ((StartupModuleRunner)Application!)?.Initialize(TestServiceScope.ServiceProvider);
        }

        private IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        private void BeforeAddApplication(IServiceCollection services)
        {
        }

        private void AfterAddApplication(IServiceCollection services)
        {
        }

        private IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProviderFromFactory();
            return serviceProvider ?? throw new InvalidOperationException();
        }

        private IServiceProvider ConfigureProvider(Action<IServiceCollection> configure)
        {
            var services = new ServiceCollection();

            configure(services);
            var serviceProvider = services.BuildServiceProviderFromFactory();
            return serviceProvider ?? throw new InvalidOperationException();
        }
    }
}