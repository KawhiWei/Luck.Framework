using Luck.Framework.Infrastructure;
using Module.Sample.DbContexts;

namespace Module.Sample.AppModules
{
    public class MigrationModule : AppModule
    {

        public override void ApplicationInitialization(ApplicationContext context)
        {
            var app = context.GetApplicationBuilder();
            var moduleDbContext = context.ServiceProvider.GetService<ModuleDbContext>();
            moduleDbContext?.Database.EnsureCreated();
        }
    }
}
