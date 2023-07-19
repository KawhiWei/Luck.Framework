using Luck.AppModule;
using Luck.AutoDependencyInjection;
using Luck.Framework.Infrastructure;
using Module.Sample.AppModules;

namespace Module.Sample
{
    [DependsOn(
        typeof(AutoDependencyAppModule),
        typeof(EntityFrameworkCoreModule),
        typeof(MigrationModule)
    )]
    public class AppWebModule : LuckAppModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {

            base.ConfigureServices(context);

        }
    }
}
