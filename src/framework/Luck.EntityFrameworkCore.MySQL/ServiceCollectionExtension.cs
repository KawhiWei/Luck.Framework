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
    public virtual IServiceCollection AddMySQLDriven(IServiceCollection services)
    {
        services.AddSingleton<IDbContextDrivenProvider, MySqlDbContextDrivenProvider>();
        return services;
    }
}