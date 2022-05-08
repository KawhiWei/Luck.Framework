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

        /// <summary>
        /// 得到环境列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<AppEnvironmentOptputListDto>> GetEnvironmentList()=>_environmentService.GetEnvironmentListAsync();

        /// <summary>
        /// 得到环境下配置列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{environmentId}")]
        public Task<List<AppEnvironmentPageListOutputDto>> GetAppEnvironmentAndConfigurationPage(string environmentId) => _environmentService.GetAppEnvironmentConfigurationPageAsync(environmentId);


        /// <summary>
        /// 添加环境
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task AddEnvironment([FromBody] AppEnvironmentInputDto input) => _environmentService.AddAppEnvironmentAsync(input);


        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="environmentId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("{environmentId}")]
   
        public Task AddAppConfiguration(string environmentId,[FromBody] AppConfigurationInput input) => _environmentService.AddAppConfigurationAsync(environmentId, input);


        /// <summary>
        ///更新配置
        /// </summary>
        /// <param name="environmentId"></param>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{environmentId}/{id}")]
        public Task UpdageAppConfiguration(string environmentId, string id, [FromBody] AppConfigurationInput input) => _environmentService.UpdateAppConfigurationAsync(environmentId, id, input);

        /// <summary>
        /// 删除环境
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task DeleteEnvironment(string id) => _environmentService.DeleteAppEnvironmentAsnyc(id);

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="environmentId"></param>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        [HttpDelete("{environmentId}/{configurationId}")]
        public Task DeleteAppConfiguration(string environmentId, string configurationId) => _environmentService.DeleteAppConfigurationAsync
            (environmentId, configurationId);




    }
}
