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
    }
}