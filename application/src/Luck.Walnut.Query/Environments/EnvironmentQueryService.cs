using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.Threading;
using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Query.Environments
{
    public class EnvironmentQueryService : IEnvironmentQueryService
    {
        private readonly IAggregateRootRepository<AppEnvironment, string> _appEnvironmentRepository;
        private readonly IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> _applicationRepository;
        private readonly IEntityRepository<AppConfiguration, string> _appConfigurationRepository;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;  //当中断请求时，所以有操作同时也中断
        private const string FindAppConfigurationNotExistErrorMsg = "配置数据不存在!!!!";

        public EnvironmentQueryService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository, ICancellationTokenProvider cancellationTokenProvider,IAggregateRootRepository<Application, string> applicationRepository, IEntityRepository<AppConfiguration, string> appConfigurationRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _applicationRepository = applicationRepository;
            _appConfigurationRepository=appConfigurationRepository;
            _cancellationTokenProvider = cancellationTokenProvider;
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

        public async Task<AppConfigurationOutput> GetConfigurationDetailForConfigurationIdAsync(string configurationId)
        {
            var appconfigutation= await GetConfigurationDetailByIdAsync(configurationId);
            return new AppConfigurationOutput()
            {
                Id=appconfigutation.Id,
                Key=appconfigutation.Key,
                Value=appconfigutation.Value,
                IsOpen=appconfigutation.IsOpen,
                IsPublish=appconfigutation.IsPublish,
                Type= appconfigutation.Type,
            };
        }

        private async Task<AppConfiguration> GetConfigurationDetailByIdAsync(string configurationId)
        {

            var appConfiguration=await _appConfigurationRepository.FindAsync(configurationId);
            if(appConfiguration is null)
                throw new BusinessException(FindAppConfigurationNotExistErrorMsg);
            return appConfiguration;
        }
        
        public async Task<List<AppConfigurationOutput>> GetAppConfigurationByAppIdAndEnvironmentNameAsync(string appId, string environmentName)
        {
            var application = await _applicationRepository.FindAll(x => x.AppId == appId).FirstOrDefaultAsync();
            if (application is null)
                throw new BusinessException($"{appId}应用不存在");
            return await _appEnvironmentRepository.FindAll(x => x.AppId == application.Id && x.EnvironmentName == environmentName).Include(x => x.Configurations).SelectMany(x => x.Configurations).Select(a => new AppConfigurationOutput
            {
                Key = a.Key,
                Value = a.Value,
                Type = a.Type,
                Group = a.Group,
            }).ToListAsync(_cancellationTokenProvider.Token);
        }
    }
}
