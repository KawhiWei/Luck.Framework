using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.Extensions;
using Luck.Framework.UnitOfWorks;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Application.Environments
{
    public interface IEnvironmentService : IScopedDependency
    {

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
        Task<List<AppEnvironmentPageListOutputDto>> GetAppEnvironmentPageAsync();

        /// <summary>
        ///删除
        /// </summary>
        /// <param name="environmentId"></param>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        Task DeleteAppConfigurationAsync(string environmentId, string configurationId);




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
        public EnvironmentService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository, IUnitOfWork unitOfWork, IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> applicationRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _unitOfWork = unitOfWork;
            _applicationRepository = applicationRepository;
        }

        public async Task AddAppEnvironmentAsync(AppEnvironmentInputDto input)
        {
            var appEnvironment = new AppEnvironment(input.EnvironmentName, input.ApplicationId);
            _appEnvironmentRepository.Add(appEnvironment);
            await _unitOfWork.CommitAsync();
        }

        public async Task AddAppConfigurationAsync(string environmentId, AppConfigurationInput input)
        {
            var appEnvironment = await _appEnvironmentRepository.FindAsync(environmentId);
            _ = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            //appEnvironment = Check.NotNull(appEnvironment, nameof(appEnvironment));
            appEnvironment.AddConfiguration(input.Key, input.Value, input.Type, input.IsOpen);
            await _unitOfWork.CommitAsync();
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
            await _unitOfWork.CommitAsync();
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


        public async Task<List<AppEnvironmentPageListOutputDto>> GetAppEnvironmentPageAsync()
        {

            var list = (await _appEnvironmentRepository.FindAll().Include(o => o.Configurations)


                 .Select(o => new AppEnvironmentPageListOutputDto()
                 {

                     Id = o.Id,
                     EnvironmentName = o.EnvironmentName,
                     AppConfigurationOutputDtos = o.Configurations.Select(a => new AppConfigurationOutputDto()
                     {

                         Id = a.Id,
                         Key = a.Key,
                         Value = a.Value,
                         IsOpen = a.IsOpen,
                         Type = a.Type
                     }),


                 })
                 .ToListAsync());
            return list;
        }

        public async Task DeleteAppConfigurationAsync(string environmentId, string configurationId)
        {
            environmentId.NotNullOrEmpty(nameof(environmentId));
            configurationId.NotNullOrEmpty(nameof(configurationId));
            var environment = await _appEnvironmentRepository.FindAll(o => o.Id == environmentId).Include(o => o.Configurations).FirstOrDefaultAsync();

            if (environment is null)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }


            if (!environment?.Configurations.Any()==true )
            {
                throw new BusinessException("环境下没有对应的配置");
            }

            var configuration = environment?.Configurations.FirstOrDefault(o => o.Id == configurationId);

            if (configuration is null)
            {
                throw new BusinessException($"{configurationId}没有找到对应的配置");
            }


            //要在映射上添加AppEnvironmentId这个，不是操作导航属性Remove,会把AppEnvironmentId字段值清空!!
            environment?.Configurations.Remove(configuration);

            await _unitOfWork.CommitAsync();
        }

    

        public async Task<List<AppConfigurationOutput>> GetAppConfigurationByAppIdAndEnvironmentName(string appId, string environmentName)
        {
            var application =await  _applicationRepository.FindAll(x => x.AppId == appId).FirstOrDefaultAsync();
            if (application is null)
                throw new BusinessException($"{appId}应用不存在");


           return await  _appEnvironmentRepository.FindAll(x => x.ApplicationId == application.Id && x.EnvironmentName == environmentName).Include(x => x.Configurations).SelectMany(x =>  x.Configurations).Select(a=>new AppConfigurationOutput
            {
                Key= a.Key,
                Value=a.Value,
                Type=a.Type
            }).ToListAsync();

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
            environment?.UpdateConfiguration(id, input.Key, input.Value, input.Type, input.IsOpen, input.IsPublish);
            _appEnvironmentRepository.Update(environment);
            await _unitOfWork.CommitAsync();
        }
    }
}
