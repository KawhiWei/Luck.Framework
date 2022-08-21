﻿using Luck.Framework.Event;
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

        [HttpPost]
        public void CreateOrder()
        {
            var orderEvent = new CreateOrderIntegrationEvent() { OrderNo = "亮白风格-图解网络-小林coding-v2.0.pdf" };
            _integrationEventBus.Publish<CreateOrderIntegrationEvent>(orderEvent);

        }


        [HttpGet]
        public void CreateTestIntegrationEvent()
        {
            var test = new TestIntegrationEvent() { Name="亮白风格-图解网络-小林coding-v2.0.pdf" };
            _integrationEventBus.Publish<TestIntegrationEvent>(test);

        }
    }
}
