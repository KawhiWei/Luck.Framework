using Luck.Framework.Infrastructure;

namespace Module.Sample
{
    [DependsOn(
        typeof(DependencyAppModule),
        typeof(TestModule)
    )]
    public class AppWebModule: AppModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {

            base.ConfigureServices(context);
        }
    }
}
