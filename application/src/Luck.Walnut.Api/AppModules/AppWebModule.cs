using Luck.Framework.Infrastructure;


namespace Luck.Walnut.Api.AppModules
{
    [DependsOn(
        typeof(DependencyAppModule),
        typeof(EntityFrameworkCoreModule),
        typeof(MigrationModule),
        typeof(SerilogModule)
    )]
    public class AppWebModule: AppModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            base.ConfigureServices(context);
            
        }
    }
}
