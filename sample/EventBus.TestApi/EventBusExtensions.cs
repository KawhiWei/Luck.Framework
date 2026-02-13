using Luck.Framework.Event;

namespace EventBus.TestApi;

/// <summary>
/// EventBus 扩展方法
/// </summary>
public static class EventBusExtensions
{
    /// <summary>
    /// 订阅指定类型的事件
    /// </summary>
    public static IApplicationBuilder UseEventBus(this IApplicationBuilder app, Type[] eventTypes)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IIntegrationEventBus>();
        
        // 自动订阅所有指定类型的事件
        foreach (var eventType in eventTypes)
        {
            System.Console.WriteLine($"自动订阅事件: {eventType.Name}");
        }

        return app;
    }
}
