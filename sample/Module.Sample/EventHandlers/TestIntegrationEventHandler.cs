
using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.DependencyInjectionModule;

namespace Module.Sample.EventHandlers
{
    [RabbitMQ("ddtest", ExchangeType.Routing, "test_0001", "testqueue1")]
    public class TestIntegrationEvent : IntegrationEvent
    {

       public string Name { get; set; }
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
            _logger.LogInformation($"{@event.Name}");
            return Task.CompletedTask; 
        }
    }
}
