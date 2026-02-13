using Luck.Framework.Event;

namespace EventBus.TestApi.Events;

/// <summary>
/// 测试事件处理器
/// </summary>
public class TestEventHandler : IIntegrationEventHandler<TestEvent>
{
    private readonly ILogger<TestEventHandler> _logger;

    public TestEventHandler(ILogger<TestEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(TestEvent @event)
    {
        _logger.LogInformation("========================================");
        _logger.LogInformation("收到测试事件: {EventId}", @event.EventId);
        _logger.LogInformation("消息内容: {Message}", @event.Message);
        _logger.LogInformation("发送时间: {SentAt}", @event.SentAt);
        _logger.LogInformation("========================================");
        
        return Task.CompletedTask;
    }
}
