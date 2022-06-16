using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.PostgreSQL
{
    /// <summary>
    ///  PostgreSql驱动
    /// </summary>
    public class PostgreSqlDbContextDrivenProvider : IDbContextDrivenProvider
    {
        public DataBaseType Type => DataBaseType.PostgreSQL;

        public DbContextOptionsBuilder Builder<TDbContext>(DbContextOptionsBuilder builder,string connnectionString) where TDbContext : ILuckDbContext
        {
            builder.UseNpgsql(connnectionString, opt => opt.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name)).EnableSensitiveDataLogging().UseSnakeCaseNamingConvention(); 
            return builder;
        }
    }
}
