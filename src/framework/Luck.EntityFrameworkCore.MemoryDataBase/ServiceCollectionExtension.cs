using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore.MemoryDatabase;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMemoryDriven(this IServiceCollection services)
    {
        services.AddSingleton<IDbContextDrivenProvider, MemoryDrivenProvider>();
        return services;
    }
}