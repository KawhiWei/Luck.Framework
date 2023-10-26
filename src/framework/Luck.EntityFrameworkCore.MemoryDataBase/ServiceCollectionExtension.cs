using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore.MemoryDatabase;

public class ServiceCollectionExtension
{
    public virtual IServiceCollection AddMemoryDriven(IServiceCollection services)
    {
        services.AddSingleton<IDbContextDrivenProvider, MemoryDrivenProvider>();
        return services;
    }
}