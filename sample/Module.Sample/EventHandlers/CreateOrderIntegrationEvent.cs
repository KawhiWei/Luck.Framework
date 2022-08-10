﻿using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.DependencyInjectionModule;

namespace Module.Sample.EventHandlers
{
    [RabbitMQ("test002", ExchangeType.Routing, "createorder","testqueue")]
    public class CreateOrderIntegrationEvent : IntegrationEvent
    {

        public string OrderNo { get; set; } = default!;
    }

    [DependencyInjection(ServiceLifetime.Transient, AddSelf=true)]
    public class CreateOrderIntegrationHandler : IIntegrationEventHandler<CreateOrderIntegrationEvent>
    {

        private readonly ILogger<CreateOrderIntegrationHandler> _logger;

        public CreateOrderIntegrationHandler(ILogger<CreateOrderIntegrationHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(CreateOrderIntegrationEvent @event)
        {
          
          _logger.LogInformation($"CreateOrderIntegrationEvent_Subscribe【{@event.Serialize()}】-----{DateTime.Now}");
          return  Task.CompletedTask;
        }


    }
}
