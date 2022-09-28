using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.Framework.Infrastructure.DependencyInjectionModule;

namespace Module.Sample.EventHandlers;

[RabbitMq("fanout_test_exchange", ExchangeType.FanOut, "", "fanout_test_queue_001")]
public class TestIntegrationFanOutOrderEvent: IntegrationEvent
{
    public string Name { get; set; } = default!;
        
}

[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class TestIntegrationFanOutOrderEventHandler : IIntegrationEventHandler<TestIntegrationFanOutOrderEvent>
{
    private readonly ILogger<TestIntegrationEventHandler> _logger;

    public TestIntegrationFanOutOrderEventHandler(ILogger<TestIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(TestIntegrationFanOutOrderEvent @event)
    {
        _logger.DoveLogInformation($"{@event.Name}---A2---{DateTime.Now}");
        return Task.CompletedTask; 
    }
}