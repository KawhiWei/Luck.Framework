using Luck.Framework.Extensions;
using Luck.Logging.Serilog;
using Microsoft.AspNetCore.Mvc;
using Module.Sample.Services;
using System.ComponentModel;

namespace Module.Sample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<TestsController> _logger;

        public TestsController(IOrderService orderService, ILogger<TestsController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public Task TestEnumToList()
        {

            var list = typeof(TestEnum).TypeToEnumList();

            return Task.FromResult(list);
        }

        [HttpGet]
        public async Task LogRequestContextAsync()
        {
            _logger.LogLuckInformation("Controller is calling the application service.");
            await _orderService.LogRequestContextAsync();
        }

        [HttpPost]
        public Task CreateAndEventAsync()
        {

            return _orderService.CreateAndEventAsync();
        }

        [HttpPost]
        public Task CreateOrder()
        {

            return _orderService.CreateAndEventAsync();
        }

    }


    public enum TestEnum
    {
        [Description("大黄瓜")]
        A,
        [Description("大黄瓜1")]
        B,
        C
    }
}
