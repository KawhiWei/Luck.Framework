namespace Luck.EventBus.Kafka
{
    public class KafkaMqConfig
    {

        
        
        public const int UseDefaultPort = 9092;
        
        
        public string Host { get; set; } = default!;

        public string PassWord { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public int RetryCount { get; set; } = default!;

        public int Port { get; set; } = UseDefaultPort;

    }
}