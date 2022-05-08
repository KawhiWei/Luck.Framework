namespace Luck.EventBus.RabbitMQ.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RabbitMQAttribute : Attribute
    {
        public RabbitMQAttribute(string exchange, string type, string routingKey, string? queue = null)
        {
            Exchange = exchange;
            Type = type;
            RoutingKey = routingKey;
            Queue = queue;
        }

        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; } = default!;

        /// <summary>
        /// 交换机模式
        /// </summary>
        public string Type { get; set; }=default!;

        /// <summary>
        /// 路由键《路由键和队列名称配合使用》
        /// </summary>
        public string RoutingKey { get; set; } = default!;

        /// <summary>
        /// 队列名称《队列名称和路由键配合使用》
        /// </summary>
        public string? Queue { get; set; }
    }
}