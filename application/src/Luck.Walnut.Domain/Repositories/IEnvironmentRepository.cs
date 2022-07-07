using Luck.DDD.Domain.Repositories;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto;
using Luck.Walnut.Dto.Applications;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Domain.Repositories;

public interface IEnvironmentRepository : IAggregateRootRepository<AppEnvironment,string>,IScopedDependency
{

    Task<AppEnvironment?> FirstOrDefaultByIdAsync(string id);
    
    Task<AppEnvironment?> FirstOrDefaultByEnvironmentNameAsync(string environmentName);

    /// <summary>
    /// 获取环境列表
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    Task<List<AppEnvironmentListOutputDto>> GetEnvironmentListForApplicationId(string appId);
    /// <summary>
    /// 获取配置列表
    /// </summary>
    /// <param name="environmentId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageBaseResult<AppConfigurationOutputDto>> GetAppConfigurationPageAsync(string environmentId, PageInput input);

}