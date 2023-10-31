using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore.MySQL;

public class ServiceCollectionExtension
{
    
    /// <summary>
    /// 添加DB驱动
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public virtual IServiceCollection AddMemoryDataBase(IServiceCollection services)
    {
        services.AddSingleton<IDbContextDrivenProvider, MySqlDrivenProvider>();
        return services;
    }
}