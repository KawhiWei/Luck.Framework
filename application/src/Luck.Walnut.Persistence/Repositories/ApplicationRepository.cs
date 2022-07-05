using System.Linq.Expressions;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.EntityFrameworkCore.Repositories;
using Luck.Framework.Exceptions;
using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.Walnut.Domain.Repositories;
using Luck.Walnut.Dto;
using Microsoft.EntityFrameworkCore;

namespace Luck.Walnut.Persistence.Repositories;

public class ApplicationRepository : EFCoreAggregateRootRepository<Application, string>, IApplicationRepository
{
    public ApplicationRepository(ILuckDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Application> FindFirstOrDefaultByIdAsync(string id)
    {
        return await FindAll(x => x.Id == id).FirstOrDefaultAsync() ?? throw new BusinessException("应用不存在");
    }

    public async Task<Application> FindFirstOrDefaultByAppIdAsync(string appId)
    {
        return await FindAll(x => x.AppId == appId).FirstOrDefaultAsync() ?? throw new BusinessException("应用不存在");
    }

    
    public Task<List<Application>> FindListAsync(PageInput input)
    {
        return  FindAll().ToPage(input.PageIndex,input.PageSize).ToListAsync();
    }
}