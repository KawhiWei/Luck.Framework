using Luck.Dapper.DbConnectionFactories;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.Dapper.ClickHouse;

public class ClickHouseBaseModule: AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddDefaultSqlRepository();
        context.Services.AddSingleton<IDbConnectionFactory, ClickHouseDbConnectionFactory>();
    }
}