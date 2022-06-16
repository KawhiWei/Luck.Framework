using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.UnitOfWorks;
using Luck.Walnut.Dto.Applications;

namespace Luck.Walnut.Application.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> _applicationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationService(IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> applicationRepository, IUnitOfWork unitOfWork)
        {
            _applicationRepository = applicationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddApplicationAsync(ApplicationInputDto input)
        {
            await CheckAppIdAsync(input.AppId);
            var application = new Domain.AggregateRoots.Applications.Application(input.EnglishName, input.DepartmentName, input.ChinessName, input.LinkMan, input.AppId, input.Status);
            _applicationRepository.Add(application);
            await _unitOfWork.CommitAsync();
        }

        private async Task CheckAppIdAsync(string appid, bool isUpdate = false, string? id = null)
        {
            if (isUpdate && id is not null)
            {
                if (await _applicationRepository.FindAll(x => x.AppId == appid && x.Id != id).AnyAsync())
                    throw new BusinessException($"{appid}已存在");
            }
            else
            {
                if (await _applicationRepository.FindAll(x => x.AppId == appid).AnyAsync())
                    throw new BusinessException($"{appid}已存在");
            }
        }

        public async Task UpdateApplicationAsync(string id, ApplicationInputDto input)
        {
            await CheckAppIdAsync(input.AppId, true, id);
            var application = await GetApplicationByIdAsync(id);
            application.UpdateInfo(input.EnglishName, input.DepartmentName, input.ChinessName, input.LinkMan, input.AppId, input.Status);
            await _unitOfWork.CommitAsync();

        }

        public async Task DeleteApplicationAsync(string id)
        {
            var application = await GetApplicationByIdAsync(id);
            _applicationRepository.Remove(application);
            await _unitOfWork.CommitAsync();
        }

        private async Task<Domain.AggregateRoots.Applications.Application> GetApplicationByIdAsync(string id)
        {
            var application = await _applicationRepository.FindAsync(id);
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
        
        private async Task<Domain.AggregateRoots.Applications.Application> GetApplicationByAppIdAsync(string appId)
        {
            var application = await _applicationRepository.FindAll(x=>x.AppId==appId).FirstOrDefaultAsync();
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
    }
}
