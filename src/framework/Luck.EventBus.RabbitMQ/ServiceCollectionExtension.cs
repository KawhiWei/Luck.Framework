using Luck.EventBus.RabbitMQ;
using Luck.Framework.Event;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddEventBusRabbitMq(this IServiceCollection service, Action<RabbitMQConfig> action)
        {


            RabbitMQConfig config = new RabbitMQConfig();
            action.Invoke(config);


            service.AddRabbitMqPersistentConnection(config);
            service.AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbitMq>(serviceProvider =>
            {

                var rabbitMqPersistentConnection = serviceProvider.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = serviceProvider.GetRequiredService<ILogger<IntegrationEventBusRabbitMq>>();
                var subsManager=    serviceProvider.GetRequiredService<IIntegrationEventBusSubscriptionsManager>();

                if (rabbitMqPersistentConnection is null)
                {
                    throw new ArgumentNullException(nameof(rabbitMqPersistentConnection));
                }
                    

                if (logger is null)
                {
                    throw new ArgumentNullException(nameof(rabbitMqPersistentConnection));
                }
                return new IntegrationEventBusRabbitMq(rabbitMqPersistentConnection, logger, config.RetryCount, subsManager, serviceProvider);

            });
            service.AddSingleton<IIntegrationEventBusSubscriptionsManager, RabbitMqEventBusSubscriptionsManager>();
            service.AddHostedService<RabbitMqIntegrationEventBusBackgroundServiceSubscribe>();
            return service;
        }


        private static IServiceCollection AddRabbitMqPersistentConnection(this IServiceCollection service, RabbitMQConfig config)
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
                };
                return new DefaultRabbitMQPersistentConnection(factory, logger, config.RetryCount);
            });
            return service;
        }
       


    }
}