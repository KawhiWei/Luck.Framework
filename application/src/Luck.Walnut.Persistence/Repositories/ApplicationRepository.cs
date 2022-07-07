using System.Linq.Expressions;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.EntityFrameworkCore.Repositories;
using Luck.Framework.Exceptions;
using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.Walnut.Domain.Repositories;
using Luck.Walnut.Dto;
using Luck.Walnut.Dto.Applications;
using Microsoft.EntityFrameworkCore;

namespace Luck.Walnut.Persistence.Repositories;

public class ApplicationRepository : EFCoreAggregateRootRepository<Application, string>, IApplicationRepository
{
    public ApplicationRepository(ILuckDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Application?> FindFirstOrDefaultByIdAsync(string id)
    {
        return await FindAll(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Application?> FindFirstOrDefaultByAppIdAsync(string appId)
    {
        return await FindAll(x => x.AppId == appId).FirstOrDefaultAsync();
    }

    
    public async Task<IEnumerable<ApplicationOutputDto>> FindListAsync(PageInput input)
    {
        return await  FindAll()
            .Select(c => new ApplicationOutputDto
            {
                Id = c.Id,
                AppId = c.AppId,
                Status = c.Status,
                EnglishName = c.EnglishName,
                ChinessName = c.ChinessName,
                DepartmentName = c.DepartmentName,
                LinkMan = c.LinkMan,
            }).ToPage(input.PageIndex,input.PageSize).ToArrayAsync();
    }
}