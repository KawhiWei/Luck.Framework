using OpenTelemetry.Trace;

namespace OpenTelemetry.Luck.EventBus;

/// <summary>
/// RabbitMQ EventBus OpenTelemetry 扩展方法
/// </summary>
public static class LuckTracerProviderBuilderExtensions
{
    /// <summary>
    /// 添加 Luck EventBus 仪表化
    /// </summary>
    public static TracerProviderBuilder AddLuckEventBusInstrumentation(this TracerProviderBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.AddSource(LuckEventBusInstrumentation.SourceName);

        var instrumentation = new LuckEventBusInstrumentation();

        return builder.AddInstrumentation(() => instrumentation);
    }
}
