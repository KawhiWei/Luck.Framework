using Luck.Framework.Extensions;
using Microsoft.AspNetCore.Http;
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

        public TestsController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet]
        public Task TestEnumToList()
        {

          var list=  typeof(TestEnum).TypeToEnumList();
          
           return Task.FromResult(list);
        }

        [HttpPost]
        public  Task CreateAndEventAsync()
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
