using Luck.Framework.Infrastructure;
using Luck.Redis.StackExchange;
using Module.Sample.AppModules;

namespace Module.Sample
{
    [DependsOn(
        typeof(DependencyAppModule),
        typeof(EntityFrameworkCoreModule),
        typeof(MigrationModule),
        typeof(StackExchangeRedisModule)
    )]
    public class AppWebModule: AppModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            
            base.ConfigureServices(context);
            
        }
    }
}
