using Luck.EventBus.RabbitMQ;
using Luck.Framework.Event;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventBusRabbitMq(this IServiceCollection service, Action<RabbitMqConfig> action)
        {
            RabbitMqConfig config = new RabbitMqConfig();
            action.Invoke(config);


            service.AddRabbitMqPersistentConnection(config)
                .AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbitMq>(serviceProvider
                => new IntegrationEventBusRabbitMq(config.RetryCount, serviceProvider)
            );
            service.AddSingleton<IIntegrationEventBusSubscriptionsManager, RabbitMqEventBusSubscriptionsManager>();
            service.AddHostedService<RabbitMqIntegrationEventBusBackgroundServiceSubscribe>();
            return service;
        }


        private static IServiceCollection AddRabbitMqPersistentConnection(this IServiceCollection service, RabbitMqConfig config)
        {
            service.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = config.Host,
                    DispatchConsumersAsync = true,
                    UserName = config.UserName,
                    Password = config.PassWord,
                    Port = config.Port,
                    VirtualHost = config.VirtualHost,
                };
                return new DefaultRabbitMQPersistentConnection(factory, logger, config.RetryCount);
            });
            return service;
        }
    }
}