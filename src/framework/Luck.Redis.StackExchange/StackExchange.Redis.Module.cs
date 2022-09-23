using Luck.Framework.Infrastructure;
using Luck.Framework.Infrastructure.Caching;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IRedisHash = Luck.Framework.Infrastructure.Caching.Interface.IRedisHash;
using IRedisList = Luck.Framework.Infrastructure.Caching.Interface.IRedisList;

namespace Luck.Redis.StackExchange
{
    public class StackExchangeRedisModule: AppModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            var service = context.Services;

            service.TryAddSingleton<IRedisHash, StackExchangeRedisHash>();
            service.TryAddSingleton<IRedisList, StackExchangeRedisList>();
        }
    }
}
