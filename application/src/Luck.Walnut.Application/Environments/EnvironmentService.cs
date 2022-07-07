using Luck.Framework.Exceptions;
using Luck.Framework.Extensions;
using Luck.Framework.Threading;
using Luck.Framework.UnitOfWorks;
using Luck.Walnut.Application.Environments.Events;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Domain.Repositories;
using Luck.Walnut.Dto.Environments;
using MediatR;

namespace Luck.Walnut.Application.Environments
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly IEnvironmentRepository _appEnvironmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ICancellationTokenProvider _cancellationTokenProvider; //当中断请求时，所以有操作同时也中断

        public EnvironmentService(IEnvironmentRepository appEnvironmentRepository,
            IUnitOfWork unitOfWork, ICancellationTokenProvider cancellationTokenProvider, IMediator mediator)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _unitOfWork = unitOfWork;
            _cancellationTokenProvider = cancellationTokenProvider;
            _mediator = mediator;
        }

        private const string FindEnvironmentNotExistErrorMsg = "环境数据不存在!!!!";

        public async Task AddAppEnvironmentAsync(AppEnvironmentInputDto input)
        {
            await CheckAppEnvironmentExistAsync(input.EnvironmentName);
            var appEnvironment = new AppEnvironment(input.EnvironmentName, input.AppId);
            _appEnvironmentRepository.Add(appEnvironment);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }

        public async Task DeleteAppEnvironmentAsync(string environmentId)
        {
            var appEnvironment = await FindAppEnvironmentByIdAsync(environmentId);

            await CheckAppEnvironmentExistByIdAsync(environmentId);
            //只删除环境？ 要不要把配置也删除？级联删除？
            _appEnvironmentRepository.Remove(appEnvironment);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }
        
        public async Task AddAppConfigurationAsync(string environmentId, AppConfigurationInput input)
        {
            var appEnvironment = await _appEnvironmentRepository.FirstOrDefaultByIdAsync(environmentId);
            if (appEnvironment is null)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }
            if (appEnvironment.Configurations.Any(x => x.Key == input.Key))
            {
                throw new BusinessException($"{input.Key}已存在");
            }

            appEnvironment.AddConfiguration(input.Key, input.Value, input.Type, input.IsOpen, input.Group);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }



        public async Task DeleteAppConfigurationAsync(string environmentId, string configurationId)
        {
            environmentId.NotNullOrEmpty(nameof(environmentId));
            configurationId.NotNullOrEmpty(nameof(configurationId));
            var environment = await _appEnvironmentRepository.FirstOrDefaultByIdAsync(environmentId);
            if (environment is null)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }

            var configuration = environment.Configurations.FirstOrDefault(o => o.Id == configurationId);
            if (configuration is null)
            {
                throw new BusinessException($"配置不存在！！！");
            }

            environment.Configurations.Remove(configuration);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }

        public async Task UpdateAppConfigurationAsync(string environmentId, string id, AppConfigurationInput input)
        {
            id.NotNullOrEmpty(nameof(id));
            environmentId.NotNullOrEmpty(nameof(environmentId));
            var environment = await _appEnvironmentRepository.FirstOrDefaultByIdAsync(environmentId);
            if (environment is null)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }
            environment.UpdateConfiguration(id, input.Key, input.Value, input.Type, input.IsOpen, input.Group)
                .UpdateVersion("");
            _appEnvironmentRepository.Update(environment);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }

        public async Task PublishAsync(string environmentId, List<string> configurationId)
        {
            var appEnvironment = await FindAppEnvironmentByIdAsync(environmentId);
            appEnvironment.Publish(configurationId);
            appEnvironment.UpdateVersion(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
            await _unitOfWork.CommitAsync();
            var appConfigurationEvent = new AppConfigurationEvent()
            {
                AppId = appEnvironment.AppId
            };
            await _mediator.Publish(appConfigurationEvent, _cancellationTokenProvider.Token);
        }


        private async Task CheckAppEnvironmentExistAsync(string environmentName)
        {
            var appEnvironment = await _appEnvironmentRepository.FirstOrDefaultByEnvironmentNameAsync(environmentName);
            if (appEnvironment is not null)
            {
                throw new BusinessException($"[{environmentName}]已存在");
            }
        }
        
        private async Task CheckAppEnvironmentExistByIdAsync(string id)
        {
            var appEnvironment = await _appEnvironmentRepository.FirstOrDefaultByIdAsync(id);
            if (appEnvironment is not null)
            {
                throw new BusinessException($"{FindEnvironmentNotExistErrorMsg}");
            }
        }
        

        private async Task<AppEnvironment> FindAppEnvironmentByIdAsync(string environmentId)
        {
            environmentId.NotNullOrEmpty(nameof(environmentId));
            var appEnvironment = await _appEnvironmentRepository.FindAll(x => x.Id == environmentId)
                .Include(x => x.Configurations).FirstOrDefaultAsync();
            if (appEnvironment is null)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }

            return appEnvironment;
        }
    }
}