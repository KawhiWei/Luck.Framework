using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.Extensions;
using Luck.Framework.Threading;
using Luck.Framework.UnitOfWorks;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Application.Environments
{
    public interface IEnvironmentService : IScopedDependency
    {


        Task<IEnumerable<AppEnvironmentOptputListDto>> GetEnvironmentListAsync();

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
        /// 分页
        /// </summary>
        /// <returns></returns>
        Task<List<AppEnvironmentPageListOutputDto>> GetAppEnvironmentConfigurationPageAsync(string environmentId);

        /// <summary>
        ///删除
        /// </summary>
        /// <param name="environmentId"></param>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        Task DeleteAppConfigurationAsync(string environmentId, string configurationId);

        /// <summary>
        /// 根据配置项id获取详情
        /// </summary>
        /// <param name="configId">配置项id</param>
        /// <returns></returns>
        Task<AppEnvironmentDetailOutPutDto> GetAppEnvironmentConfigurationDetail(string configId);

      
        /// <summary>
        /// 根据应用Id和环境名称获取配置列表
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        Task<List<AppConfigurationOutput>> GetAppConfigurationByAppIdAndEnvironmentName(string appId, string environmentName);

        Task UpdateAppConfigurationAsync(string environmentId, string id, AppConfigurationInput input);


    }

    public class EnvironmentService : IEnvironmentService
    {
        private readonly IAggregateRootRepository<AppEnvironment, string> _appEnvironmentRepository;
        private readonly IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> _applicationRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICancellationTokenProvider _cancellationTokenProvider;  //当中断请求时，所以有操作同时也中断

        private readonly IEntityRepository<AppConfiguration, string> _configRepository;
        public EnvironmentService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository, IUnitOfWork unitOfWork, ICancellationTokenProvider cancellationTokenProvider, IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> applicationRepository, IEntityRepository<AppConfiguration, string> configRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _unitOfWork = unitOfWork;
            _cancellationTokenProvider = cancellationTokenProvider;
            _applicationRepository = applicationRepository;
            _configRepository = configRepository;
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
        private async Task<AppEnvironment?> FindAll(string environmentId)
        {

            var appEnvironment = await _appEnvironmentRepository.FindAll(o => o.Id == environmentId).Include(o => o.Configurations).FirstOrDefaultAsync();
            IsBusinessException(appEnvironment is null, FindEnvironmentNotExistErrorMsg);
            return appEnvironment;
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

        public async Task<AppEnvironmentDetailOutPutDto> GetAppEnvironmentConfigurationDetail(string configId)
        {
            var info = await _configRepository.FindAsync(p => p.Id == configId);
            var result = new AppEnvironmentDetailOutPutDto
            {
                Id = info.Id,
                IsOpen = info.IsOpen,
                IsPublish = info.IsPublish,
                Key = info.Key,
                Type = info.Type,
                Value = info.Value,
            };
            return result;
        }


        public async Task DeleteAppConfigurationAsync(string environmentId, string configurationId)
        {
            environmentId.NotNullOrEmpty(nameof(environmentId));
            configurationId.NotNullOrEmpty(nameof(configurationId));
            var environment = await _appEnvironmentRepository.FindAll(o => o.Id == environmentId).Include(o => o.Configurations).FirstOrDefaultAsync();

            if (environment is not null)
            {
                var configuration = environment.Configurations.FirstOrDefault(o => o.Id == configurationId);
                if (configuration is null)
                {
                    throw new BusinessException($"{configurationId}没有找到对应的配置");
                }

                //要在映射上添加AppEnvironmentId这个，不是操作导航属性Remove,会把AppEnvironmentId字段值清空!!
                environment.Configurations.Remove(configuration);

                await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
            }
            throw new BusinessException(FindEnvironmentNotExistErrorMsg);

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
            environment.UpdateConfiguration(id, input.Key, input.Value, input.Type, input.IsOpen, input.IsPublish);
            _appEnvironmentRepository.Update(environment);
            await _unitOfWork.CommitAsync(_cancellationTokenProvider.Token);
        }

        public async Task<IEnumerable<AppEnvironmentOptputListDto>> GetEnvironmentListAsync()
        {


            return await _appEnvironmentRepository.FindAll().Select(o => new AppEnvironmentOptputListDto()
            {


                Id = o.Id,
                ApplicationId = o.ApplicationId,
                EnvironmentName = o.EnvironmentName,
                AppId = _applicationRepository.FindAll().FirstOrDefault(a => a.Id == o.ApplicationId).AppId
            }).ToListAsync();

        }
    }
}
