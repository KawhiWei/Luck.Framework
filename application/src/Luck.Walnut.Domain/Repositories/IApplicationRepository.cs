using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.DDD.Domain.Repositories;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Luck.Walnut.Dto;
using Luck.Walnut.Dto.Applications;

namespace Luck.Walnut.Domain.Repositories;


public interface IApplicationRepository : IAggregateRootRepository<Application,string>,IScopedDependency
{
    Task<Application?> FindFirstOrDefaultByIdAsync(string id);

    Task<Application?> FindFirstOrDefaultByAppIdAsync(string appId);

    Task<IEnumerable<ApplicationOutputDto>> FindListAsync(PageInput input);
}