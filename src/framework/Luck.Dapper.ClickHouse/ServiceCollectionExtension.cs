using Luck.Dapper.ClickHouse;
using Luck.Dapper.DbConnectionFactories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClickHouseDbConnectionString(this IServiceCollection services,
        Action<ClickHouseConnectionConfig> action)
    {
        services.Configure(action);
        return services;
    }
    
    public static IServiceCollection AddClickHouseDapperDriven(this IServiceCollection services)
    {
        services.AddSingleton<IDapperDrivenProvider, DapperClickHouseDrivenProvider>();
        return services;
    }
}