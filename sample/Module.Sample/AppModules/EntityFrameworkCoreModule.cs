﻿using Luck.EntityFrameworkCore;
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
                    "User ID=postgres;Password=wzw0126..;Host=39.101.165.187;Port=8832;Database=module.test";
                x.Type = DataBaseType.PostgreSql;
            });
        }

        protected override void AddDbDriven(IServiceCollection service)
        {
            service.AddPostgreSQLDriven();
        }
    }
}