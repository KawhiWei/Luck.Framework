using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.MySQL
{
    /// <summary>
    /// Mysql驱动
    /// </summary>
    public class MySqlDbContextDrivenProvider : IDbContextDrivenProvider
    {
        public DataBaseType Type => DataBaseType.MySql;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <param name="querySplittingBehavior">拆分查询配置，默认使用拆分查询</param>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        public DbContextOptionsBuilder Builder<TDbContext>(DbContextOptionsBuilder builder, string connectionString, QuerySplittingBehavior querySplittingBehavior = QuerySplittingBehavior.SplitQuery) where TDbContext : ILuckDbContext
        {
            builder.UseMySql(connectionString, new MySqlServerVersion(new Version()), opt => opt.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name)).EnableSensitiveDataLogging().UseSnakeCaseNamingConvention();
            return builder;
        }
    }
}