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
        protected abstract void AddDbContextWithUnitOfWork(IServiceCollection services);

        protected abstract void AddDbDriven(IServiceCollection service);
    }
}
