namespace Luck.EventBus.RabbitMQ
{
    public class RabbitMqConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultVHost = "/";
        
        
        public const int UseDefaultPort = 5672;
        
        
        public string Host { get; set; } = default!;

        public string PassWord { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public int RetryCount { get; set; } = default!;

        public int Port { get; set; } = UseDefaultPort;

        public string VirtualHost { get; set; } = DefaultVHost;

        /// <summary>
        /// 是否启用诊断事件
        /// 启用后会将事件发布和消费的详细信息发送到 DiagnosticListener，可被 OpenTelemetry 等采集器收集
        /// </summary>
        public bool EnableDiagnosticEvents { get; set; } = false;
    }
}