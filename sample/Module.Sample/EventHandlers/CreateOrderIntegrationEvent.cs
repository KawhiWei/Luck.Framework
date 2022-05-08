using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;

namespace Module.Sample.EventHandlers
{
    [RabbitMQ("vvtest", "direct", "createorder","testqueue")]
    public class CreateOrderIntegrationEvent : IntegrationEvent
    {

        public string OrderNo { get; set; } = default!;
    }
}
