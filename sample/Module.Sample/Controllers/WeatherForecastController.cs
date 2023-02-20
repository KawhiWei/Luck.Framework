using Luck.Framework.Utilities;
using Microsoft.AspNetCore.Mvc;
using Module.Sample.Services;

namespace Module.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly IOrderService _orderService;
        public WeatherForecastController(IOrderService orderService)
        {
      
            _orderService = Check.NotNull(orderService,nameof(orderService)) ;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<object?>  Get()
        {

           return await _orderService.TestQuerySplittingBehavior();
        }
        [HttpPost]
        public async Task CreateAsync()
        {
            await _orderService.CreateAsync();
        }
        
       
    }
}