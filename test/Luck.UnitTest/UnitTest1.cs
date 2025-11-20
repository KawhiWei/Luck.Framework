using System;
using System.Threading.Tasks;
using Luck.Framework;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Luck.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            IServiceCollection services = new ServiceCollection();

            //TestBase.SourceGenerators.ServiceInfoServiceCollectionExtensions.AddBusinessServices(services);
            services.AddBusinessServices();
            var serviceProvider = services.BuildServiceProvider();
            //TestBase.SourceGenerators.ServiceInfoServiceCollectionExtensions.AddBusinessServices()
            //Luck.SourceGenerators.ServiceCollectionExtension.ServiceInfoServiceCollectionExtensions.AddBusinessServices()

            var test = serviceProvider.GetKeyedService<IAncillaryPaySuccessWithAncillaryScopeProvider>("50");
            if (test is null)
            {
                Assert.NotNull(test);
            }
            else
            {
                var result = await test.AncillaryPaySuccessProviderAsync("", "");
                Assert.True(result.Item1);
            }
        }
    }
}