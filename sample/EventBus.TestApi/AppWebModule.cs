using Luck.AppModule;
using Luck.AutoDependencyInjection;
using Luck.Framework.Infrastructure;

namespace EventBus.TestApi
{
    [DependsOn(
        typeof(AutoDependencyAppModule)
    )]
    public class AppWebModule : LuckAppModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
