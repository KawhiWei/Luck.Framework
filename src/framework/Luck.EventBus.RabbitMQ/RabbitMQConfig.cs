namespace Luck.EventBus.RabbitMQ
{
    public class RabbitMQConfig
    {
        public string Host { get; set; } = default!;

        public string PassWord { get; set; } = default!;

        public string Name { get; set; } = default!;

        public int RetryCount { get; set; } = default!;
    }
}