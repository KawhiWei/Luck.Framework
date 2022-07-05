using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Module.Sample.DbContexts;

namespace Module.Sample
{
    public class EntityFrameworkCoreModule : EntityFrameworkCoreBaseModule
    {
        public override void AddDbContextWithUnitOfWork(IServiceCollection services)
        {
            services.AddLuckDbContext<ModuleDbContext>(x =>
            {
                x.ConnnectionString = "User ID=postgres;Password=&duyu789;Host=101.34.26.221;Port=40011;Database=module.test";
                x.Type = DataBaseType.PostgreSQL;
            });
        }

        public override void AddDbDriven(IServiceCollection service)
        {
            service.AddPostgreSQLDriven();
        }
    }
}
