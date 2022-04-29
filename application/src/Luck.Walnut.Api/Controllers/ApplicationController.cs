using Luck.Walnut.Dto.Applications;
using Microsoft.AspNetCore.Mvc;

namespace Luck.Walnut.Api.Controllers
{
    [ApiController]
    [Route("api/applications")]
    public class ApplicationController : BaseController
    {
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(ILogger<ApplicationController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
           
        //}

        [HttpPost]
        public Task AddApplication([FromBody] ApplicationInputDto input)
        {
            return Task.CompletedTask;
        }
    }
}