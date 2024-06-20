using Luck.EventBus.RabbitMQ.Enums;
using Luck.Framework.Extensions;

namespace Luck.EventBus.RabbitMQ.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RabbitMqAttribute : Attribute
    {
        public RabbitMqAttribute(EWorkModel workModel, string exchange, ExchangeType exchangeType, string routingKey, string? queue = null)
        {
            WorkModel = workModel;
            Exchange = exchange;
            Type = exchangeType.ToDescription() ?? "direct";
            RoutingKey = routingKey;
            Queue = queue;
        }
        public EWorkModel WorkModel { get; }

        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 交换机模式
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 路由键《路由键和队列名称配合使用》
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 队列名称《队列名称和路由键配合使用》
        /// </summary>
        public string? Queue { get; set; }
    }
}