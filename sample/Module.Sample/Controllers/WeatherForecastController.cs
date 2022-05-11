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
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        public async Task CreateAsync()
        {
            await _orderService.CreateAsync();
        }
    }
}