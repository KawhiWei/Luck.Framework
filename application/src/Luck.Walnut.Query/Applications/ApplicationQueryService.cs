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
        private readonly IAggregateRootRepository<AppEnvironment, string> _appEnvironmentRepository;
        private  readonly  IApplicationRepository _applicationRepository;

        public ApplicationQueryService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository,IApplicationRepository applicationRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _applicationRepository = applicationRepository;
        }



        public async Task<PageBaseResult<ApplicationOutputDto>> GetApplicationListAsync(PageInput input)
        {
            var totalCount= await _applicationRepository.FindAll().CountAsync();
            var data = (await _applicationRepository.FindListAsync(input))
                .Select(c => new ApplicationOutputDto
                {
                    Id = c.Id,
                    AppId = c.AppId,
                    Status = c.Status,
                    EnglishName = c.EnglishName,
                    ChinessName = c.ChinessName,
                    DepartmentName = c.DepartmentName,
                    LinkMan = c.LinkMan,
                });

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
            var environmentLists = await GetEnvironmentListForApplicationId(id).ToListAsync();
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
        private async Task<Domain.AggregateRoots.Applications.Application> GetApplicationByAppIdAsync(string appId)
        {
            var application = await _applicationRepository.FindAll(x=>x.AppId==appId).FirstOrDefaultAsync();
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
        
        private async Task<Domain.AggregateRoots.Applications.Application> GetApplicationByIdAsync(string id)
        {
            var application = await _applicationRepository.FindAsync(id);
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }
        

        private IQueryable<AppEnvironmentOptputListDto> GetEnvironmentListForApplicationId(string appId)
        {
            return _appEnvironmentRepository.FindAll(x => x.AppId == appId).Select(o => new AppEnvironmentOptputListDto()
            {
                Id = o.Id,
                ApplicationId = o.AppId,
                EnvironmentName = o.EnvironmentName,
            });
        }
    }
}
