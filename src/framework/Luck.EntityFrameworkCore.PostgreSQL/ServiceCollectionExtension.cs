﻿using Luck.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore.DbContextDrivenProvides;

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
        services.AddSingleton<IDbContextDrivenProvider, PostgreSqlDbContextDrivenProvider>();
        return services;
    }
}