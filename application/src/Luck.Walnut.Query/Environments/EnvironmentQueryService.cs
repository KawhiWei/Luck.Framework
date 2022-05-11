using Luck.DDD.Domain.Repositories;
using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Query.Environments
{
    public class EnvironmentQueryService : IEnvironmentQueryService
    {
        private readonly IAggregateRootRepository<AppEnvironment, string> _appEnvironmentRepository;
        private readonly IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> _applicationRepository;

        public EnvironmentQueryService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository, IAggregateRootRepository<Application, string> applicationRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _applicationRepository = applicationRepository;
        }


        public async Task<List<AppEnvironmentPageListOutputDto>> GetAppEnvironmentConfigurationPageAsync(string environmentId)
        {

            var list = await _appEnvironmentRepository.FindAll().Where(o => o.Id == environmentId).Include(o => o.Configurations).SelectMany(o => o.Configurations).Select(a => new AppEnvironmentPageListOutputDto
            {

                Id = a.Id,
                IsOpen = a.IsOpen,
                IsPublish = a.IsPublish,
                Key = a.Key,
                Type = a.Type,
                Value = a.Value,

            }).ToListAsync();
            return list;
        }


    }
}
