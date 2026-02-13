using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Luck.AutoDependencyInjection;
using Luck.EventBus.RabbitMQ.Enums;

namespace Module.Sample.EventHandlers;

[RabbitMq(EWorkModel.None, "fanout_test_exchange", ExchangeType.FanOut, "", "fanout_test_queue_002")]
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
        return Task.CompletedTask;
    }
}