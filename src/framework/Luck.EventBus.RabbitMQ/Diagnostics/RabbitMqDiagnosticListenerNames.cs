using Luck.Framework.Diagnostics;

namespace Luck.EventBus.RabbitMQ.Diagnostics;

public static class RabbitMqDiagnosticListenerNames
{
    /// <summary>
    /// DiagnosticListenerName用于在诊断中判断是哪个诊断器发出的事件
    /// </summary>
    public const string DiagnosticListenerName = $"{LuckDiagnosticListenerNames.LuckPrefix}EventBus.RabbitMQ.DiagnosticListener";
    
    /// <summary>
    /// 打开RabbitMQ连接前诊断名称
    /// </summary>
    public const string CreateRabbitMqConnectionBefore = $"{LuckDiagnosticListenerNames.LuckPrefix}EventBus.RabbitMQ.OpenRabbitMQConnectionBefore";
    
    /// <summary>
    /// 打开RabbitMQ连接后诊断名称
    /// </summary>
    public const string CreateRabbitMqConnectionAfter = $"{LuckDiagnosticListenerNames.LuckPrefix}EventBus.RabbitMQ.OpenRabbitMQConnectionAfter";
    
    /// <summary>
    /// 打开RabbitMQ连接后诊断名称
    /// </summary>
    public const string RabbitMqCreateModelBefore = $"{LuckDiagnosticListenerNames.LuckPrefix}EventBus.RabbitMQ.RabbitMqCreateModelBefore";
    /// <summary>
    /// 打开RabbitMQ连接后诊断名称
    /// </summary>
    public const string RabbitMqCreateModelAfter = $"{LuckDiagnosticListenerNames.LuckPrefix}EventBus.RabbitMQ.RabbitMqCreateModelAfter";

    
    
    /// <summary>
    /// 发布RabbitMQ集成事件前诊断名称
    /// </summary>
    public const string PublishIntegrationEventBusForRabbitMqBefore = $"{LuckDiagnosticListenerNames.LuckPrefix}EventBus.RabbitMQ.PublishIntegrationEventBusForRabbitMqBefore";
    
    /// <summary>
    /// 发布RabbitMQ集成事件前诊断名称
    /// </summary>
    public const string PublishIntegrationEventBusForRabbitMqAfter = $"{LuckDiagnosticListenerNames.LuckPrefix}EventBus.RabbitMQ.PublishIntegrationEventBusForRabbitMqAfter";
    
    /// <summary>
    /// 发布RabbitMQ集成事异常诊断名称
    /// </summary>
    public const string PublishIntegrationEventBusForRabbitMqError = $"{LuckDiagnosticListenerNames.LuckPrefix}EventBus.RabbitMQ.PublishIntegrationEventBusForRabbitMqError";
}