using Luck.Framework.Event;
using Microsoft.AspNetCore.Mvc;
using EventBus.TestApi.Events;

namespace EventBus.TestApi.Controllers;

/// <summary>
/// 测试事件控制器
/// </summary>
[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly IIntegrationEventBus _eventBus;
    private readonly ILogger<TestController> _logger;

    public TestController(IIntegrationEventBus eventBus, ILogger<TestController> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    /// <summary>
    /// 发送测试事件
    /// </summary>
    [HttpPost("send-event")]
    public async Task<IActionResult> SendEvent([FromBody] SendEventRequest request)
    {
        var testEvent = new TestEvent
        {
            Message = request.Message ?? $"Hello from TestApi at {DateTime.UtcNow}",
            SentAt = DateTime.UtcNow
        };

        _logger.LogInformation("正在发送测试事件: {EventId}", testEvent.EventId);
        
        await _eventBus.PublishAsync(testEvent);

        return Ok(new
        {
            EventId = testEvent.EventId,
            Message = testEvent.Message,
            SentAt = testEvent.SentAt,
            Status = "Published"
        });
    }

    /// <summary>
    /// 批量发送测试事件
    /// </summary>
    [HttpPost("send-batch")]
    public async Task<IActionResult> SendBatchEvents([FromBody] SendBatchEventRequest request, CancellationToken cancellationToken)
    {
        var count = request.Count ?? 5;
        var events = new List<TestEvent>();

        for (int i = 0; i < count; i++)
        {
            var testEvent = new TestEvent
            {
                Message = $"Batch message {i + 1} at {DateTime.UtcNow}",
                SentAt = DateTime.UtcNow
            };
            events.Add(testEvent);
            await _eventBus.PublishAsync(testEvent, cancellationToken: cancellationToken);
        }

        return Ok(new
        {
            Count = count,
            Events = events.Select(e => new { e.EventId, e.Message }),
            Status = "Published"
        });
    }

    /// <summary>
    /// 获取服务状态
    /// </summary>
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            Service = "EventBus.TestApi",
            Status = "Running",
            OpenTelemetry = "Enabled",
            EventBus = "RabbitMQ"
        });
    }
}

/// <summary>
/// 发送事件请求
/// </summary>
public class SendEventRequest
{
    public string? Message { get; set; }
}

/// <summary>
/// 批量发送事件请求
/// </summary>
public class SendBatchEventRequest
{
    public int? Count { get; set; }
}
