using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.PostgreSQL;
using Module.Sample.DbContexts;

namespace Module.Sample
{
    public class EntityFrameworkCoreModule : EntityFrameworkCoreBaseModule
    {
        protected override void AddDbContextWithUnitOfWork(IServiceCollection services)
        {
            services.AddLuckDbContext<ModuleDbContext>(x =>
            {
                x.ConnectionString =
                    "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=module.test";
                x.Type = DataBaseType.PostgreSql;
            });
        }

        protected override void AddDbDriven(IServiceCollection service)
        {
            service.AddPostgreSQLDriven();
        }
    }
}