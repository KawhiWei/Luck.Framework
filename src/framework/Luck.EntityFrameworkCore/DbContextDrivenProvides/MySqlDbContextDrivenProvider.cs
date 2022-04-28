using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.DbContextDrivenProvides
{
    public class MySqlDbContextDrivenProvider : IDbContextDrivenProvider
    {
        public DataBaseType Type => DataBaseType.MySql;

        public DbContextOptionsBuilder Builder<TDbContext>(DbContextOptionsBuilder builder, EFDbContextConfig contextConfig) where TDbContext : ILuckDbContext
        {
            var migrationsAssemblyName = contextConfig.MigrationsAssemblyName;
            if (contextConfig.MigrationsAssemblyName.IsNullOrEmpty())
            {

                migrationsAssemblyName = typeof(TDbContext).Assembly.GetName().Name;
            }
            builder.UseMySql(contextConfig.ConnnectionString, new MySqlServerVersion(new Version()), opt => opt.MigrationsAssembly(migrationsAssemblyName)).EnableSensitiveDataLogging().UseSnakeCaseNamingConvention();
            return builder;
        }
    }
}
