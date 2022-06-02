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

        Task UpdateAppConfigurationAsync(string environmentId, string id, AppConfigurationInput input);


    }
}
