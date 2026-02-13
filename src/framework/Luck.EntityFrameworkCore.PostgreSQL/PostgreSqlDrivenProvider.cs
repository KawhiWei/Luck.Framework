using Luck.EntityFrameworkCore.DbContextDrivenProvides;
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
        /// <returns></returns>
        public DbContextOptionsBuilder Builder(DbContextOptionsBuilder builder, string connectionString,
            QuerySplittingBehavior querySplittingBehavior = QuerySplittingBehavior.SplitQuery)
        {
            builder.UseNpgsql(connectionString, opt =>
                opt.UseQuerySplittingBehavior(querySplittingBehavior)
            ).EnableSensitiveDataLogging().UseSnakeCaseNamingConvention();
            return builder;
        }
    }
}