using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.AutoDependencyInjection;

namespace Module.Sample.EventHandlers;

[RabbitMq("fanout_test_exchange", ExchangeType.FanOut, "", "fanout_test_queue_002")]
public class TestIntegrationFanOutPayEvent : IntegrationEvent
{
    public string Name { get; set; } = default!;

}


[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class TestIntegrationFanOutPayEventHandler : IIntegrationEventHandler<TestIntegrationFanOutPayEvent>
{
    private readonly ILogger<TestIntegrationEventHandler> _logger;

    public TestIntegrationFanOutPayEventHandler(ILogger<TestIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(TestIntegrationFanOutPayEvent @event)
    {
        _logger.DoveLogInformation($"{@event.Name}---A1---{DateTime.Now}");
        return Task.CompletedTask;
    }
}