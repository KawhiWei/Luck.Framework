using Luck.Framework.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Luck.EventBus.RabbitMQ
{


    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddEventBusRabbitMQ(this ServiceCollection service, Action<RabbitMQConfig> action)
        {


            RabbitMQConfig config=new RabbitMQConfig();
            action.Invoke(config);


            service.AddRabbitMQPersistentConnection(config);
            service.AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbitMQ>(serviceProvider =>
            {




                return new IntegrationEventBusRabbitMQ();

            });

            return service;
        }


        public static IServiceCollection AddRabbitMQPersistentConnection(this ServiceCollection service, RabbitMQConfig config)
        {
            service.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {

                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = config.Host,
                    DispatchConsumersAsync = true,
                    UserName = config.Name,
                    Password = config.PassWord,
                };

                return new DefaultRabbitMQPersistentConnection(factory, logger, config.RetryCount);
            });
            return service;
        }
    }
}