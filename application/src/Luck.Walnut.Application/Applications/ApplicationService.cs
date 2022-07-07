using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.UnitOfWorks;
using Luck.Walnut.Domain.Repositories;
using Luck.Walnut.Dto.Applications;

namespace Luck.Walnut.Application.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _applicationRepository = applicationRepository;
        }

        public async Task AddApplicationAsync(ApplicationInputDto input)
        {
            await CheckAppIdAsync(input.AppId);
            var application = new Domain.AggregateRoots.Applications.Application(input.EnglishName,
                input.DepartmentName, input.ChinessName, input.LinkMan, input.AppId, input.Status);
            _applicationRepository.Add(application);
            await _unitOfWork.CommitAsync();
        }

        private async Task CheckAppIdAsync(string appId)
        {
            await GetApplicationByAppIdAsync(appId);
        }

        public async Task UpdateApplicationAsync(string id, ApplicationInputDto input)
        {
            var application =  await GetApplicationByIdAsync(id);
            application.UpdateInfo(input.EnglishName, input.DepartmentName, input.ChinessName, input.LinkMan,
                input.AppId, input.Status);
            await _unitOfWork.CommitAsync();
        }
        



        public async Task DeleteApplicationAsync(string id)
        {
            var application = await GetApplicationByIdAsync(id);
            _applicationRepository.Remove(application);
            await _unitOfWork.CommitAsync();
        }


        private async Task<Domain.AggregateRoots.Applications.Application> GetApplicationByAppIdAsync(string appId)
        {
            var application = await _applicationRepository.FindFirstOrDefaultByAppIdAsync(appId);
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
        
        private async Task<Domain.AggregateRoots.Applications.Application> GetApplicationByIdAsync(string id)
        {
            var application = await _applicationRepository.FindFirstOrDefaultByIdAsync(id);
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
    }
}