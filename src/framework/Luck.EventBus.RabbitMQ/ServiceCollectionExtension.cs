using Luck.EventBus.RabbitMQ;
using Luck.Framework.Event;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddEventBusRabbitMQ(this IServiceCollection service, Action<RabbitMQConfig> action)
        {


            RabbitMQConfig config = new RabbitMQConfig();
            action.Invoke(config);


            service.AddRabbitMQPersistentConnection(config);
            service.AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbitMQ>(serviceProvider =>
            {

                var rabbitMQPersistentConnection = serviceProvider.GetRequiredService<IRabbitMQPersistentConnection>();

                if (rabbitMQPersistentConnection is null)
                    throw new ArgumentNullException(nameof(rabbitMQPersistentConnection));
                var logger = serviceProvider.GetRequiredService<ILogger<IntegrationEventBusRabbitMQ>>();
                if (logger is null)
                    throw new ArgumentNullException(nameof(rabbitMQPersistentConnection));

                return new IntegrationEventBusRabbitMQ(rabbitMQPersistentConnection, logger, config.RetryCount);

            });
            return service;
        }


        private static IServiceCollection AddRabbitMQPersistentConnection(this IServiceCollection service, RabbitMQConfig config)
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
                    Port=config.Port,
                };

                return new DefaultRabbitMQPersistentConnection(factory, logger, config.RetryCount);
            });
            return service;
        }
    }
}