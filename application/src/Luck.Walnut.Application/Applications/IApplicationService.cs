using Luck.Framework.Extensions;
using Luck.Walnut.Dto.Applications;
using Microsoft.EntityFrameworkCore;

namespace Luck.Walnut.Application.Applications
{
    public interface IApplicationService : IScopedDependency
    {
        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddApplicationAsync(ApplicationInputDto input);
        
        /// <summary>
        /// 修改应用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateApplicationAsync(string id, ApplicationInputDto input);
        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteApplicationAsync(string id);
    }
}
