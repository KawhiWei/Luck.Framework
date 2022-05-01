using Luck.Walnut.Application.Applications;
using Luck.Walnut.Dto.Applications;
using Microsoft.AspNetCore.Mvc;

namespace Luck.Walnut.Api.Controllers
{
    [ApiController]
    [Route("api/applications")]
    public class ApplicationController : BaseController
    {
        private readonly ILogger<ApplicationController> _logger;
        private readonly IApplicationService _applicationService;

        public ApplicationController(ILogger<ApplicationController> logger, IApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        [HttpPost]
        public Task AddApplication([FromBody] ApplicationInputDto input) => _applicationService.AddApplicationAsync(input);

        [HttpGet]
        public Task<IEnumerable<ApplicationOutputDto>> GetApplicationList() => _applicationService.GetApplicationListAsync();

        [HttpGet("{id}")]
        public Task<ApplicationDetailOutputDto> GetApplicationDetail(string id) => _applicationService.GetApplicationDetailAsync(id);

        [HttpPut("{id}")]
        public Task UpdateApplication(string id, [FromBody] ApplicationInputDto input) => _applicationService.UpdateApplicationAsync(id, input);

        [HttpDelete("{id}")]
        public Task DeleteApplication(string id) => _applicationService.DeleteApplicationAsync(id);


        


    }
}