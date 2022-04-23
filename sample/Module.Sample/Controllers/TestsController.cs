using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Sample.Services;

namespace Module.Sample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestsController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public TestsController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        public  Task CreateAndEventAsync()
        {

            return _orderService.CreateAndEventAsync();
        }

    }
}
