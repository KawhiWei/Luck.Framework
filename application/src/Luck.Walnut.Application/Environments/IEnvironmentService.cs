using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Application.Environments
{
    public interface IEnvironmentService : IScopedDependency
    {

        /// <summary>
        /// 添加环境
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddAppEnvironmentAsync(AppEnvironmentInputDto input);

        /// <summary>
        /// 删除环境
        /// </summary>
        /// <param name="environmentId"></param>
        /// <returns></returns>
        Task DeleteAppEnvironmentAsnyc(string environmentId);


        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddAppConfigurationAsync(string environmentId, AppConfigurationInput input);


        /// <summary>
        ///删除
        /// </summary>
        /// <param name="environmentId"></param>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        Task DeleteAppConfigurationAsync(string environmentId, string configurationId);
      
        /// <summary>
        /// 根据应用Id和环境名称获取配置列表
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        Task<List<AppConfigurationOutput>> GetAppConfigurationByAppIdAndEnvironmentName(string appId, string environmentName);

        Task UpdateAppConfigurationAsync(string environmentId, string id, AppConfigurationInput input);


    }
}
