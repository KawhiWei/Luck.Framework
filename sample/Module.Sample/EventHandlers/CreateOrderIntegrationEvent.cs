using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.Framework.Extensions;
using Luck.AutoDependencyInjection;
using Luck.EventBus.RabbitMQ.Enums;

namespace Module.Sample.EventHandlers
{
    [RabbitMq(EWorkModel.None, "test002", ExchangeType.Routing, "createorder", "testqueue")]
    public class CreateOrderIntegrationEvent : IntegrationEvent
    {

        public string OrderNo { get; set; } = default!;
    }

    [DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
    public class CreateOrderIntegrationHandler : IIntegrationEventHandler<CreateOrderIntegrationEvent>
    {
        private readonly ILogger<CreateOrderIntegrationHandler> _logger;
        public CreateOrderIntegrationHandler(ILogger<CreateOrderIntegrationHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(CreateOrderIntegrationEvent @event)
        {
            // _logger.LogInformation($"{@event.Serialize()}");
            return Task.CompletedTask;
        }


    }
}
