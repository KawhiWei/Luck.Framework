using System.ComponentModel;
using Luck.Framework.Extensions;

namespace Luck.EventBus.RabbitMQ.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RabbitMQAttribute : Attribute
    {
        public RabbitMQAttribute(string exchange, ExchangeType exchangeType, string routingKey, string? queue = null)
        {
            Exchange = exchange;
            Type = exchangeType.ToDescription() ?? "direct";
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


    public enum ExchangeType
    {
        /// <summary>
        /// 路由模式
        /// </summary>
        [Description("direct")]
        Routing = 1,
        ///// <summary>
        ///// 主题模式
        ///// </summary>
        //[Description("topic")]
        //Topic = 2,
        /// <summary>
        /// 订阅模式
        /// </summary>
        [Description("fanout")]
        FanOut = 3,
    
    }
}