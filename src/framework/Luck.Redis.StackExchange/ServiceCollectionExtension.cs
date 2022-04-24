using Luck.Framework.Infrastructure;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, Action<RedisEndpoint> redisEndpoint)
        {

            if (redisEndpoint is null)
                throw new ArgumentNullException($"{nameof(redisEndpoint)} Redis 连接字符串不可为空");

            var endpoint = new RedisEndpoint();

            redisEndpoint.Invoke(endpoint);

            var options = ConfigurationOptions.Parse(endpoint.Host);

            if (!string.IsNullOrWhiteSpace(endpoint.Password))
                options.Password = endpoint.Password;

            if (endpoint.Timeout is not null && endpoint.Timeout > 0)
            {
                options.SyncTimeout = endpoint.Timeout.Value;
                //options.ResponseTimeout = endpoint.Timeout.Value * 2;
                options.ConnectTimeout = endpoint.Timeout.Value * 5;
            }

            options.KeepAlive = 15;
            options.ResolveDns = false;
            options.AbortOnConnectFail = false;

            services.AddSingleton(x =>
            {
                return ConnectionMultiplexer.Connect(options);
            });
            return services;
        }
    }
}
