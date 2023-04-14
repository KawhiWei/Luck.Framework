using Luck.Dapper.ClickHouse;
using Luck.Dapper.Repositories;
using Luck.DDD.Domain.SqlRepositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDefaultSqlRepository(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add(new ServiceDescriptor(typeof(ISqlAggregateRootRepository<,>), typeof(DapperAggregateRootRepositoryBase<,>), lifetime));
        services.Add(new ServiceDescriptor(typeof(ISqlEntityRepository<,>), typeof(DapperEntityRepository<,>), lifetime));
        return services;
    }
    
    public static IServiceCollection AddClickHouseDbConnectionString(this IServiceCollection services,
        Action<ClickHouseConnectionConfig> action)
    {
        services.Configure<ClickHouseConnectionConfig>(action);
        return services;
    }
}