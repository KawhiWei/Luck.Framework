using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Domain.Repositories;
using Luck.Walnut.Dto;
using Luck.Walnut.Dto.Applications;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Query.Applications
{
    public class ApplicationQueryService : IApplicationQueryService
    {
        private readonly IEnvironmentRepository _appEnvironmentRepository;
        private  readonly  IApplicationRepository _applicationRepository;

        public ApplicationQueryService(IEnvironmentRepository appEnvironmentRepository,IApplicationRepository applicationRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _applicationRepository = applicationRepository;
        }



        public async Task<PageBaseResult<ApplicationOutputDto>> GetApplicationListAsync(PageInput input)
        {
            var totalCount= await _applicationRepository.FindAll().CountAsync();
            var data=await _applicationRepository.FindListAsync(input);

            return new PageBaseResult<ApplicationOutputDto>(totalCount, data.ToArray());
        }





        public async Task<ApplicationOutput> GetApplicationDetailAndEnvironmentAsync(string id)
        {

            var application = await GetApplicationByAppIdAsync(id);
            ApplicationOutput applicationOutput = new ApplicationOutput();

            applicationOutput.Application = new ApplicationOutputDto
            {
                Id = application.Id,
                ChinessName = application.ChinessName,
                EnglishName = application.EnglishName,
                LinkMan = application.LinkMan,
                AppId = application.AppId,
                Status = application.Status,
                DepartmentName = application.DepartmentName,
            };
            var environmentLists = await _appEnvironmentRepository.GetEnvironmentListForApplicationId(id);
            applicationOutput.EnvironmentLists = environmentLists;

            return applicationOutput;
        }
        public async Task<ApplicationDetailOutputDto> GetApplicationDetailForIdAsync(string id)
        {

            var application = await GetApplicationByIdAsync(id);

            return new ApplicationDetailOutputDto()
            {
                Id = application.Id,
                ChinessName = application.ChinessName,
                EnglishName = application.EnglishName,
                Status = application.Status,
                DepartmentName = application.DepartmentName,
                LinkMan = application.LinkMan,
                AppId = application.AppId,
            };
        }
       

        

        public async Task<ApplicationDetailOutputDto> GetApplicationDetailForAppIdAsync(string appId)
        {

            var application = await GetApplicationByAppIdAsync(appId);

            return new ApplicationDetailOutputDto()
            {
                Id = application.Id,
                ChinessName = application.ChinessName,
                EnglishName = application.EnglishName,
                Status = application.Status,
                DepartmentName = application.DepartmentName,
                LinkMan = application.LinkMan,
                AppId = application.AppId,
            };
        }
        private async Task<Application> GetApplicationByAppIdAsync(string appId)
        {
            var application = await _applicationRepository.FindAll(x=>x.AppId==appId).FirstOrDefaultAsync();
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
        
        private async Task<Application> GetApplicationByIdAsync(string id)
        {
            var application = await _applicationRepository.FindAsync(id);
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
    }
}
