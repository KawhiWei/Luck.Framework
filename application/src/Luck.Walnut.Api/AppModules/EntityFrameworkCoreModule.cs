

using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.Walnut.Infrastructure;

namespace Luck.Walnut.Api.AppModules
{
    public class EntityFrameworkCoreModule : EntityFrameworkCoreBaseModule
    {
        public override void AddDbContextWithUnitOfWork(IServiceCollection services)
        {
            services.AddLuckDbContext<WalnutDbContext>(x =>
            {
                x.ConnnectionString = "server=127.0.0.1;userid=root;pwd=123456;database=luck.walnut;connectiontimeout=3000;port=3306;Pooling=true;Max Pool Size=300; Min Pool Size=5;";
                x.Type = DataBaseType.MySql;
                x.MigrationsAssemblyName = "Luck.Walnut.Api";
            });
        }
    }
}
