using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Query.Environments
{
    public interface IEnvironmentQueryService : IScopedDependency
    {
        /// <summary>
        /// 根据应用Id和环境名称获取配置列表
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        Task<List<AppConfigurationOutput>> GetAppConfigurationByAppIdAndEnvironmentNameAsync(string appId, string environmentName);
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
