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
    }
}
