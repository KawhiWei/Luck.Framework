using Luck.AppModule;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IRedisHash = Luck.Framework.Infrastructure.Caching.Interface.IRedisHash;
using IRedisList = Luck.Framework.Infrastructure.Caching.Interface.IRedisList;

namespace Luck.Redis.StackExchange
{
    public class StackExchangeRedisModule : LuckAppModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            var service = context.Services;

            service.TryAddSingleton<IRedisHash, StackExchangeRedisHash>();
            service.TryAddSingleton<IRedisList, StackExchangeRedisList>();
        }
    }
}
