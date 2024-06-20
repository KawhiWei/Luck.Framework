using System.ComponentModel;

namespace Luck.EventBus.RabbitMQ.Enums;

public enum ExchangeType
{
    /// <summary>
    /// 路由模式
    /// </summary>
    [Description("direct")] Routing = 1,

    ///// <summary>
    ///// 主题模式
    ///// </summary>
    //[Description("topic")]
    //Topic = 2,
    /// <summary>
    /// 订阅模式
    /// </summary>
    [Description("fanout")] FanOut = 3,
}