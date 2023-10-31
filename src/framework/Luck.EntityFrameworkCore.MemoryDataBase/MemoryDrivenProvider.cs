using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.MemoryDatabase;

public class MemoryDrivenProvider: IDbContextDrivenProvider
{
    public DataBaseType Type => DataBaseType.MemoryDataBase;

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
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        return builder;
    }
}