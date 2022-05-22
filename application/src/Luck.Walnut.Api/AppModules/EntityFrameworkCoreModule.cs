

using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;

namespace Luck.Walnut.Api.AppModules
{
    public class EntityFrameworkCoreModule : EntityFrameworkCoreBaseModule
    {
        public override void AddDbContextWithUnitOfWork(IServiceCollection services)
        {
            services.AddLuckDbContext<WalnutDbContext>(x =>
            {
                x.ConnnectionString = "server=47.100.213.49; userid=root; pwd=P@ssW0rd; database=luck.walnut; connectiontimeout=3000; port=3306; Pooling=true; MaxPoolSize=300; MinPoolSize=5;";
                x.Type = DataBaseType.MySql;
            });
        }
    }
}
