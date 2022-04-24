using Luck.Framework.Infrastructure;
using Module.Sample.AppModules;

namespace Module.Sample
{
    [DependsOn(
        typeof(DependencyAppModule),
        typeof(EntityFrameworkCoreModule),
        typeof(MigrationModule)
    )]
    public class AppWebModule: AppModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            
            base.ConfigureServices(context);
            
        }
    }
}
