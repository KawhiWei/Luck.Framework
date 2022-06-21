using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.Threading;
using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Query.Environments
{
    public class EnvironmentQueryService : IEnvironmentQueryService
    {
        private readonly IAggregateRootRepository<AppEnvironment, string> _appEnvironmentRepository;

        private readonly IAggregateRootRepository<Application, string>
            _applicationRepository;

        private readonly IEntityRepository<AppConfiguration, string> _appConfigurationRepository;
        private readonly ICancellationTokenProvider _cancellationTokenProvider; //当中断请求时，所以有操作同时也中断
        private const string FindAppConfigurationNotExistErrorMsg = "配置数据不存在!!!!";

        public EnvironmentQueryService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository,
            ICancellationTokenProvider cancellationTokenProvider,
            IAggregateRootRepository<Application, string> applicationRepository,
            IEntityRepository<AppConfiguration, string> appConfigurationRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _applicationRepository = applicationRepository;
            _appConfigurationRepository = appConfigurationRepository;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task<PageBaseResult<AppEnvironmentPageListOutputDto>> GetAppEnvironmentConfigurationPageAsync(
            string environmentId, PageInput input)
        {
            var list = await _appEnvironmentRepository.FindAll().Where(o => o.Id == environmentId)
                .Include(o => o.Configurations).SelectMany(o => o.Configurations).Select(a =>
                    new AppEnvironmentPageListOutputDto
                    {
                        Id = a.Id,
                        IsOpen = a.IsOpen,
                        IsPublish = a.IsPublish,
                        Key = a.Key,
                        Type = a.Type,
                        Value = a.Value,
                    }).Skip((input.PageCount - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
            var total = await _appEnvironmentRepository.FindAll().Where(o => o.Id == environmentId)
                .Include(o => o.Configurations).SelectMany(o => o.Configurations).CountAsync();
            return new PageBaseResult<AppEnvironmentPageListOutputDto>(total, list.ToArray());
        }

        public async Task<AppConfigurationOutput> GetConfigurationDetailForConfigurationIdAsync(string configurationId)
        {
            var appconfigutation = await GetConfigurationDetailByIdAsync(configurationId);
            return new AppConfigurationOutput()
            {
                Id = appconfigutation.Id,
                Key = appconfigutation.Key,
                Value = appconfigutation.Value,
                IsOpen = appconfigutation.IsOpen,
                IsPublish = appconfigutation.IsPublish,
                Type = appconfigutation.Type,
            };
        }

        private async Task<AppConfiguration> GetConfigurationDetailByIdAsync(string configurationId)
        {
            var appConfiguration = await _appConfigurationRepository.FindAsync(configurationId);
            if (appConfiguration is null) throw new BusinessException(FindAppConfigurationNotExistErrorMsg);
            return appConfiguration;
        }

        public async Task<AppEnvironmentOutputDto> GetAppConfigurationByAppIdAndEnvironmentNameAsync(string appId,
            string environmentName)
        {
            var appEnvironment = await _appEnvironmentRepository
                .FindAll(x => x.AppId == appId && x.EnvironmentName == environmentName).Include(x => x.Configurations)
                .FirstOrDefaultAsync();
            if (appEnvironment is null)
            {
                throw new BusinessException($"{appId}不存在此环境");
            }

            var configs = appEnvironment.Configurations.Where(x=>x.IsPublish).Select(x => new AppConfigurationOutput()
            {
                Key = x.Key, Value = x.Value, Type = x.Type,
            }).ToList();
            return new AppEnvironmentOutputDto()
            {
                EnvironmentName = appEnvironment.EnvironmentName,
                Version = appEnvironment.Version,
                AppId = appEnvironment.AppId,
                Configs = configs,
            };
        }
    }
}