using Luck.Dapper.DbConnectionFactories;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.Dapper.ClickHouse;

public abstract class ClickHouseBaseModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddDefaultSqlRepository();
        AddDbDriven(context.Services);
        AddConnectionString(context.Services);
    }

    protected abstract void AddConnectionString(IServiceCollection service);
    
    protected abstract void AddDbDriven(IServiceCollection service);
}