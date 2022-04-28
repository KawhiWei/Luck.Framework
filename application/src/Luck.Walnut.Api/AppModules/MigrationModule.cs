using Luck.Framework.Infrastructure;
using Luck.Walnut.Infrastructure;

namespace Luck.Walnut.Api.AppModules
{

    [DependsOn(
        typeof(EntityFrameworkCoreModule)
    )]

    public class MigrationModule : AppModule
    {

        public override void ApplicationInitialization(ApplicationContext context)
        {
         
            var moduleDbContext = context.ServiceProvider.GetService<WalnutDbContext>();
            moduleDbContext?.Database.EnsureCreated();
        }
    }
}
