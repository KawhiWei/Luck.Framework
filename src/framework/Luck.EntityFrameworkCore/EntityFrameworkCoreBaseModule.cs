using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore
{
    public abstract class EntityFrameworkCoreBaseModule : AppModule
    {

        public override void ConfigureServices(ConfigureServicesContext context)
        {
            this.AddDbDriven(context.Services);
            context.Services.AddDefaultRepository();
            context.Services.AddUnitOfWork();

            AddDbContextWithUnitOfWork(context.Services);
        }
        /// <summary>
        /// 添加DB驱动
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        protected virtual IServiceCollection AddDbDriven(IServiceCollection services)
        {
            services.AddSingleton<IDbContextDrivenProvider, MySqlDbContextDrivenProvider>();
            //services.AddSingleton<IDbContextDrivenProvider, SqlServerDbContextDrivenProvider>();
            //services.AddSingleton<IDbContextDrivenProvider, NpgSqlDbContextDrivenProvider>();
            return services;
        }

        public abstract void AddDbContextWithUnitOfWork(IServiceCollection services);
    }
}
