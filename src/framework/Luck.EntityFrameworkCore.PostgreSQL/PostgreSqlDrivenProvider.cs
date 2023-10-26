using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.PostgreSQL
{
    /// <summary>
    ///  PostgreSql驱动
    /// </summary>
    public class PostgreSqlDrivenProvider : IDbContextDrivenProvider
    {
        public DataBaseType Type => DataBaseType.PostgreSql;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <param name="querySplittingBehavior">拆分查询配置，默认使用拆分查询</param>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        public DbContextOptionsBuilder Builder<TDbContext>(DbContextOptionsBuilder builder,string connectionString,QuerySplittingBehavior querySplittingBehavior=QuerySplittingBehavior.SplitQuery) where TDbContext : ILuckDbContext
        {
            builder.UseNpgsql(connectionString, opt => 
                opt.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name)
                    .UseQuerySplittingBehavior(querySplittingBehavior)
            ).EnableSensitiveDataLogging().UseSnakeCaseNamingConvention(); 
            return builder;
        }
    }
}
