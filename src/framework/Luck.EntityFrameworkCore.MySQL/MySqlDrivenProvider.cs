using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.MySQL
{
    /// <summary>
    /// Mysql驱动
    /// </summary>
    public class MySqlDrivenProvider : IDbContextDrivenProvider
    {
        public DataBaseType Type => DataBaseType.MySql;

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
#if NET10_0_OR_GREATER
            builder.UseMySQL(connectionString)
#else
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            builder.UseMySql(connectionString, serverVersion)
#endif
                .EnableSensitiveDataLogging()
                .UseSnakeCaseNamingConvention();
            return builder;
        }
    }
}
