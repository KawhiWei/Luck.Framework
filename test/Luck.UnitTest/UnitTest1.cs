using System;
using Luck.Framework;
using Luck.TestBase.SourceGenerators;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Luck.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddBusinessServices();
            
            var serviceProvider = services.BuildServiceProvider();

            var test = serviceProvider.GetKeyedService<IAncillaryPaySuccessWithAncillaryScopeProvider>("70");
            
            if(test is null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}