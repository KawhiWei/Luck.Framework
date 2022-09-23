using Luck.Framework.Event;
using Microsoft.AspNetCore.Mvc;
using Module.Sample.EventHandlers;

namespace Module.Sample.Controllers
{
    [Route("api/integrationevents")]
    [ApiController]
    public class TestIntegrationEventController : ControllerBase
    {
        private readonly IIntegrationEventBus _integrationEventBus;

        public TestIntegrationEventController(IIntegrationEventBus integrationEventBus)
        {
            _integrationEventBus = integrationEventBus;
        }

        [HttpPost("add")]
        public void CreateOrder()
        {
            var orderEvent = new CreateOrderIntegrationEvent { OrderNo = "亮白风格-图解网络-小林coding-v2.0.pdf" };
            _integrationEventBus.Publish(orderEvent);

        }


        [HttpGet("getdetail")]
        public void CreateTestIntegrationEvent()
        {
            var test = new TestIntegrationEvent() { Name="亮白风格-图解网络-小林coding-v2.0.pdf" };
            _integrationEventBus.Publish(test);

        }
        
        
        [HttpGet("testfanout")]
        public void CreateFanoutTestIntegrationEvent()
        {
            var test = new TestIntegrationFanOutOrderEvent { Name="亮白风格-图解网络-小林coding-00012-v2.0.pdf" };
            _integrationEventBus.Publish(test);

        }
    }
}
