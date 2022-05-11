using Luck.Framework.Event;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EventBus.RabbitMQ
{
    public static class ApplicationBuilderExtension
    {

        public static IApplicationBuilder UseEventBusRabbitMQ(this IApplicationBuilder builder)
        {
            var eventBus = builder.ApplicationServices.GetService<IIntegrationEventBus>();

            if (eventBus is null)
            { 
               throw new Exception("RabbitMQ集成事件总线没有注入");
            }
            return builder;
        }
    }
}
