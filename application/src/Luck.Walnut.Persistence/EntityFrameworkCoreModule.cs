

using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.Walnut.Persistence
{
    public class EntityFrameworkCoreModule : EntityFrameworkCoreBaseModule
    {
        public override void AddDbContextWithUnitOfWork(IServiceCollection services)
        {
            services.AddLuckDbContext<WalnutDbContext>(x =>
            {
                x.ConnnectionString = "User ID=postgres;Password=&duyu789;Host=101.34.26.221;Port=40011;Database=luck.walnut";
                x.Type = DataBaseType.PostgreSQL;
            });
        }

        public override void AddDbDriven(IServiceCollection service)
        {
            service.AddPostgreSQLDriven();
        }
    }
}
