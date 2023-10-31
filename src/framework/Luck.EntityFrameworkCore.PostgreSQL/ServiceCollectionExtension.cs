using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore.PostgreSQL;

public static class ServiceCollectionExtension
{
    
    /// <summary>
    /// 添加DB驱动
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public static IServiceCollection AddPostgreSQLDriven(this IServiceCollection services)
    {
        services.AddSingleton<IDbContextDrivenProvider, PostgreSqlDrivenProvider>();
        return services;
    }
}