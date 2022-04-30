using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.Extensions;
using Luck.Framework.UnitOfWorks;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Application.Environments
{
    public interface IEnvironmentService: IScopedDependency
    {

        /// <summary>
        /// 添加环境
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddAppEnvironment(AppEnvironmentInputDto input);

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
        Task UpdateEnvironmentAsnyc(string environmentId, AppEnvironmentInputDto input);

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddAppConfiguration(string environmentId, AppConfigurationInput input);

    }

    public class EnvironmentService : IEnvironmentService
    {
        private readonly IAggregateRootRepository<AppEnvironment, string> _appEnvironmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public EnvironmentService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository, IUnitOfWork unitOfWork)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAppEnvironment(AppEnvironmentInputDto input)
        {
            var appEnvironment = new AppEnvironment(input.EnvironmentName, input.ApplicationId);
            _appEnvironmentRepository.Add(appEnvironment);
            await _unitOfWork.CommitAsync();
        }

        public async Task AddAppConfiguration(string environmentId, AppConfigurationInput input)
        {
            var appEnvironment = await _appEnvironmentRepository.FindAsync(environmentId);
            _ = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            //appEnvironment = Check.NotNull(appEnvironment, nameof(appEnvironment));
            appEnvironment.AddConfiguration(input.Key, input.Value, input.Type, input.IsOpen);
            await _unitOfWork.CommitAsync();
        }


        private const string FindEnvironmentNotExistErrorMsg = "环境数据不存在!!!!";
        private async Task<AppEnvironment> FindAppEnvironmentById(string environmentId)
        {
            environmentId.NotNullOrEmpty(nameof(environmentId));
            var appEnvironment = await _appEnvironmentRepository.FindAsync(environmentId);
            if (appEnvironment is null)
            {
                throw new BusinessException(FindEnvironmentNotExistErrorMsg);
            }

            return appEnvironment;
        }
        public async Task DeleteAppEnvironmentAsnyc(string environmentId)
        {
            var appEnvironment = await FindAppEnvironmentById(environmentId);

            //只删除环境？ 要不要把配置也删除？级联删除？
            _appEnvironmentRepository.Remove(appEnvironment);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateEnvironmentAsnyc(string environmentId, AppEnvironmentInputDto input)
        {
            var appEnvironment = await FindAppEnvironmentById(environmentId);
         
        }
    }
}
