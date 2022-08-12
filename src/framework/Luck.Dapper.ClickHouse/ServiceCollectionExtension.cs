using Luck.Dapper.ClickHouse;
using Luck.Dapper.Repositories;
using Luck.DDD.Domain.SqlRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDefaultSqlRepository(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add(new ServiceDescriptor(typeof(ISqlAggregateRootRepository<,>), typeof(DapperAggregateRootRepositoryBase<,>), lifetime));
        return services;
    }
    
    public static IServiceCollection AddClickHouseDbConnectionString(this IServiceCollection services,
        Action<ClickHouseConfig> action)
    {
        services.Configure<ClickHouseConfig>(action);
        return services;
    }
}