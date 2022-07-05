using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Framework.UnitOfWorks;
using Luck.Walnut.Domain.Repositories;
using Luck.Walnut.Dto.Applications;

namespace Luck.Walnut.Application.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private  readonly  IApplicationRepository _applicationRepository;
        public ApplicationService(IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> repository,IApplicationRepository applicationRepository ,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _applicationRepository = applicationRepository;
        }

        public async Task AddApplicationAsync(ApplicationInputDto input)
        {
            await CheckAppIdAsync(input.AppId);
            var application = new Domain.AggregateRoots.Applications.Application(input.EnglishName, input.DepartmentName, input.ChinessName, input.LinkMan, input.AppId, input.Status);
            _repository.Add(application);
            await _unitOfWork.CommitAsync();
        }

        private async Task CheckAppIdAsync(string appid, bool isUpdate = false, string? id = null)
        {
            if (isUpdate && id is not null)
            {
                if (await _repository.FindAll(x => x.AppId == appid && x.Id != id).AnyAsync())
                    throw new BusinessException($"{appid}已存在");
            }
            else
            {
                if (await _repository.FindAll(x => x.AppId == appid).AnyAsync())
                    throw new BusinessException($"{appid}已存在");
            }
        }

        public async Task UpdateApplicationAsync(string id, ApplicationInputDto input)
        {
            await CheckAppIdAsync(input.AppId, true, id);
            var application =await _applicationRepository.FindFirstOrDefaultByIdAsync(id);
            application.UpdateInfo(input.EnglishName, input.DepartmentName, input.ChinessName, input.LinkMan, input.AppId, input.Status);
            await _unitOfWork.CommitAsync();

        }

        public async Task DeleteApplicationAsync(string id)
        {
            var application = await _applicationRepository.FindFirstOrDefaultByIdAsync(id);
            _repository.Remove(application);
            await _unitOfWork.CommitAsync();
        }

        
        private async Task<Domain.AggregateRoots.Applications.Application> GetApplicationByAppIdAsync(string appId)
        {
            var application = await _applicationRepository.FindFirstOrDefaultByAppIdAsync(appId);
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
    }
}
