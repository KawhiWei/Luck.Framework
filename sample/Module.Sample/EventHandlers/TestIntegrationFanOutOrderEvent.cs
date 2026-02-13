using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.AutoDependencyInjection;
using Luck.EventBus.RabbitMQ.Enums;

namespace Module.Sample.EventHandlers;

[RabbitMq(EWorkModel.None, "fanout_test_exchange", ExchangeType.FanOut, "", "fanout_test_queue_001")]
public class TestIntegrationFanOutOrderEvent : IntegrationEvent
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
        return Task.CompletedTask;
    }
}