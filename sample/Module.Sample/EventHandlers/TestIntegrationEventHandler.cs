
using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.Framework.Extensions;
using Luck.AutoDependencyInjection;
using Luck.EventBus.RabbitMQ.Enums;

namespace Module.Sample.EventHandlers
{
    [RabbitMq(EWorkModel.None, "ddtest", ExchangeType.Routing, "test_0001", "testqueue1")]
    public class TestIntegrationEvent : IntegrationEvent
    {

        public string Name { get; set; } = default!;
    }

    [DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
    public class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        private readonly ILogger<TestIntegrationEventHandler> _logger;

        public TestIntegrationEventHandler(ILogger<TestIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(TestIntegrationEvent @event)
        {
            _logger.DoveLogInformation($"{@event.Name}------{DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
