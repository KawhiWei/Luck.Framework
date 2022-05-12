using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Query.Environments
{
    public interface IEnvironmentQueryService : IScopedDependency
    {

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        Task<List<AppEnvironmentPageListOutputDto>> GetAppEnvironmentConfigurationPageAsync(string environmentId);

        /// <summary>
        /// 根据配置项id获取详情
        /// </summary>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        Task<AppConfigurationOutput> GetConfigurationDetailForConfigurationIdAsync(string configurationId);

    }
}
