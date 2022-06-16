using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore
{
    public abstract class EntityFrameworkCoreBaseModule : AppModule
    {

        public override void ConfigureServices(ConfigureServicesContext context)
        {
            context.Services.AddDefaultRepository();
            context.Services.AddUnitOfWork();
            AddDbContextWithUnitOfWork(context.Services);
            AddDbDriven(context.Services);
        }
        public abstract void AddDbContextWithUnitOfWork(IServiceCollection services);

        public abstract void AddDbDriven(IServiceCollection service);
    }
}
