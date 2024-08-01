using Luck.AppModule;
using Luck.Framework.Infrastructure;
using Module.Sample.DbContexts;

namespace Module.Sample.AppModules
{
    public class MigrationModule : LuckAppModule
    {

        public override void ApplicationInitialization(ApplicationContext context)
        {
            var moduleDbContext = context.ServiceProvider.GetService<ModuleDbContext>();
            moduleDbContext?.Database.EnsureCreated();
        }
    }
}
