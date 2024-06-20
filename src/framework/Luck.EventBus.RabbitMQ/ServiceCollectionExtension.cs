using Luck.EventBus.RabbitMQ;
using Luck.EventBus.RabbitMQ.Abstraction;
using Luck.EventBus.RabbitMQ.Manager;
using Luck.Framework.Event;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventBusRabbitMq(this IServiceCollection service, Action<RabbitMqConfig> action)
        {
            var config = new RabbitMqConfig();
            action.Invoke(config);
            
            service.AddRabbitMqPersistentConnection(config)
                .AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbitMq>(serviceProvider
                => new IntegrationEventBusRabbitMq(config.RetryCount, serviceProvider)
            );
            service.AddSingleton<IIntegrationEventBusSubscriptionsManager, RabbitMqEventBusSubscriptionsManager>();
            service.AddHostedService<RabbitMqSubscribeService>();
            return service;
        }


        private static IServiceCollection AddRabbitMqPersistentConnection(this IServiceCollection service, RabbitMqConfig config)
        {
            service.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = config.Host,
                    DispatchConsumersAsync = true,
                    UserName = config.UserName,
                    Password = config.PassWord,
                    Port = config.Port,
                    VirtualHost = config.VirtualHost,
                };
                return new DefaultRabbitMqPersistentConnection(factory, logger, config.RetryCount);
            });
            return service;
        }
    }
}