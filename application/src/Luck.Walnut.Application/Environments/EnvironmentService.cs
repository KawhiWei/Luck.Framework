using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.Extensions;
using Luck.Framework.Threading;
using Luck.Framework.UnitOfWorks;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Application.Environments
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly IAggregateRootRepository<AppEnvironment, string> _appEnvironmentRepository;
        private readonly IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> _applicationRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICancellationTokenProvider _cancellationTokenProvider;  //当中断请求时，所以有操作同时也中断
        public EnvironmentService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository, IUnitOfWork unitOfWork, ICancellationTokenProvider cancellationTokenProvider, IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> applicationRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _unitOfWork = unitOfWork;
            _cancellationTokenProvider = cancellationTokenProvider;
            _applicationRepository = applicationRepository;
        }


        public async Task AddAppEnvironmentAsync(AppEnvironmentInputDto input)
        {
            var appEnvironment = new AppEnvironment(input.EnvironmentName, input.ApplicationId);
            _appEnvironmentRepository.Add(appEnvironment);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }

        public async Task AddAppConfigurationAsync(string environmentId, AppConfigurationInput input)
        {
            var appEnvironment = await _appEnvironmentRepository.FindAsync(environmentId);
            _ = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            //appEnvironment = Check.NotNull(appEnvironment, nameof(appEnvironment));
            appEnvironment.AddConfiguration(input.Key, input.Value, input.Type, input.IsOpen);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }


        private const string FindEnvironmentNotExistErrorMsg = "环境数据不存在!!!!";
        private async Task<AppEnvironment?> FindAppEnvironmentByIdAsync(string environmentId)
        {
            environmentId.NotNullOrEmpty(nameof(environmentId));
            var appEnvironment = await _appEnvironmentRepository.FindAsync(environmentId);
            IsBusinessException(appEnvironment is null, FindEnvironmentNotExistErrorMsg);
            //if (appEnvironment is null)
            //{
            //    throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            //}

            return appEnvironment;
        }
        public async Task DeleteAppEnvironmentAsnyc(string environmentId)
        {
            var appEnvironment = await FindAppEnvironmentByIdAsync(environmentId);

            if (appEnvironment is  null)
            {
                throw new BusinessException("没有找到对应环境");
            }
            //只删除环境？ 要不要把配置也删除？级联删除？
            _appEnvironmentRepository.Remove(appEnvironment);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);

        }



        private void IsBusinessException(bool isExp, string msg)
        {
            if (isExp)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }

        }


        

        public async Task DeleteAppConfigurationAsync(string environmentId, string configurationId)
        {
            environmentId.NotNullOrEmpty(nameof(environmentId));
            configurationId.NotNullOrEmpty(nameof(configurationId));
            var environment = await _appEnvironmentRepository.FindAll(o => o.Id == environmentId).Include(o => o.Configurations).FirstOrDefaultAsync();

            if (environment is  null)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }

            var configuration = environment.Configurations.FirstOrDefault(o => o.Id == configurationId);
            if (configuration is null)
            {
                throw new BusinessException($"{configurationId}没有找到对应的配置");
            }
            
            //要在映射上添加AppEnvironmentId这个，不是操作导航属性Remove,会把AppEnvironmentId字段值清空!!
            environment.Configurations.Remove(configuration);
           
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }



        public async Task<List<AppConfigurationOutput>> GetAppConfigurationByAppIdAndEnvironmentName(string appId, string environmentName)
        {
            var application = await _applicationRepository.FindAll(x => x.AppId == appId).FirstOrDefaultAsync();
            if (application is null)
                throw new BusinessException($"{appId}应用不存在");


            return await _appEnvironmentRepository.FindAll(x => x.ApplicationId == application.Id && x.EnvironmentName == environmentName).Include(x => x.Configurations).SelectMany(x => x.Configurations).Select(a => new AppConfigurationOutput
            {
                Key = a.Key,
                Value = a.Value,
                Type = a.Type
            }).ToListAsync(_cancellationTokenProvider.Token);

        }


        public async Task UpdateAppConfigurationAsync(string environmentId, string id, AppConfigurationInput input)
        {
            id.NotNullOrEmpty(nameof(id));
            environmentId.NotNullOrEmpty(nameof(environmentId));
            var environment = await _appEnvironmentRepository.FindAll(o => o.Id == environmentId).Include(o => o.Configurations).FirstOrDefaultAsync();
            if (environment is null)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }
            environment.UpdateConfiguration(id, input.Key, input.Value, input.Type, input.IsOpen);
            _appEnvironmentRepository.Update(environment);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }
    }
}
