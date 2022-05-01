using Luck.Framework.Utilities;
using Luck.Walnut.Application.Environments;
using Luck.Walnut.Dto.Environments;
using Microsoft.AspNetCore.Mvc;

namespace Luck.Walnut.Api.Controllers
{
    [Route("api/environment")]
    public class EnvironmentController :BaseController
    {

        private readonly IEnvironmentService _environmentService;

        public EnvironmentController(IEnvironmentService environmentService)
        {
            _environmentService = Check.NotNull(environmentService, nameof(environmentService));
            
        }

        [HttpPost]
        public Task AddEnvironment([FromBody] AppEnvironmentInputDto input) => _environmentService.AddAppEnvironmentAsync(input);


        [HttpPost("{environmentId}")]
   
        public Task AddAppConfiguration(string environmentId,[FromBody] AppConfigurationInput input) => _environmentService.AddAppConfigurationAsync(environmentId, input);



        [HttpPut("{environmentId}/{id}")]
        public Task UpdageAppConfiguration(string environmentId, string id, [FromBody] AppConfigurationInput input) => _environmentService.UpdateAppConfigurationAsync(environmentId, id, input);

        [HttpDelete("{id}")]
        public Task DeleteEnvironment(string id) => _environmentService.DeleteAppEnvironmentAsnyc(id);

    }
}
