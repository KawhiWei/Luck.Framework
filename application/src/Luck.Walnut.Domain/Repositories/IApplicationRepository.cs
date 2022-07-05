using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.DDD.Domain.Repositories;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Luck.Walnut.Dto;

namespace Luck.Walnut.Domain.Repositories;


public interface IApplicationRepository : IAggregateRootRepository<Application,string>,IScopedDependency
{
    Task<Application> FindFirstOrDefaultByIdAsync(string id);

    Task<Application> FindFirstOrDefaultByAppIdAsync(string appId);

    Task<List<Application>> FindListAsync(PageInput input);
}