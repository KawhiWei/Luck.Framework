using Luck.Framework.Extensions;
using Luck.Walnut.Dto.Applications;
using Microsoft.EntityFrameworkCore;

namespace Luck.Walnut.Application.Applications
{
    public interface IApplicationService : IScopedDependency
    {
        Task AddApplicationAsync(ApplicationInputDto input);

        Task CheckAppIdAsync(string appid, bool isUpdate = false, string? id = null);

        Task UpdateApplicationAsync(string id, ApplicationInputDto input);


        Task DeleteApplicationAsync(string id);
    }
}
