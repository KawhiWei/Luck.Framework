using Luck.DDD.Domain.Repositories;
using Luck.Framework.Exceptions;
using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Luck.Walnut.Dto.Applications;
using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Query.Applications
{
    public class ApplicationQueryService : IApplicationQueryService
    {
        private readonly IAggregateRootRepository<AppEnvironment, string> _appEnvironmentRepository;
        private readonly IAggregateRootRepository<Domain.AggregateRoots.Applications.Application, string> _applicationRepository;

        public ApplicationQueryService(IAggregateRootRepository<AppEnvironment, string> appEnvironmentRepository, IAggregateRootRepository<Application, string> applicationRepository)
        {
            _appEnvironmentRepository = appEnvironmentRepository;
            _applicationRepository = applicationRepository;
        }



        public async Task<IEnumerable<ApplicationOutputDto>> GetApplicationListAsync()
        {
            return await _applicationRepository.FindAll()
                 .Select(c => new ApplicationOutputDto
                 {
                     Id = c.Id,
                     AppId = c.AppId,
                     Status = c.Status,
                     EnglishName = c.EnglishName,
                     ChinessName = c.ChinessName,
                     DepartmentName = c.DepartmentName,
                     LinkMan = c.LinkMan,
                 }).ToListAsync();
        }





        public async Task<ApplicationOutput> GetApplicationDetailAndEnvironmentAsync(string id)
        {

            var application = await GetApplicationByIdAsync(id);
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








        private async Task<Domain.AggregateRoots.Applications.Application> GetApplicationByIdAsync(string id)
        {
            var application = await _applicationRepository.FindAsync(id);
            if (application is null)
                throw new BusinessException($"应用不存在");
            return application;
        }

        private IQueryable<AppEnvironmentOptputListDto> GetEnvironmentListForApplicationId(string applicationId)
        {
            return _appEnvironmentRepository.FindAll(x => x.ApplicationId == applicationId).Select(o => new AppEnvironmentOptputListDto()
            {
                Id = o.Id,
                ApplicationId = o.ApplicationId,
                EnvironmentName = o.EnvironmentName,
            });
        }
    }
}
