using System;
using Luck.AppModule;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;

namespace Luck.UnitTest.MongoDbTests;

/// <summary>
/// 
/// </summary>
public class MongoDbTestModule : LuckAppModule,IDisposable
{
    private readonly MongoDbRunner _runner = MongoDbRunner.Start();

    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddMongoDbContext<TestMongoDbMemoryDataBaseContext>(options =>
        {
            options.ConnectionString =_runner.ConnectionString;
        });
    }
    
    public void Dispose()
    {
        _runner.Dispose();
    }
}