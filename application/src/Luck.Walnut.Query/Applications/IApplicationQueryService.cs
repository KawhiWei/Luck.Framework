using Luck.Walnut.Dto.Applications;

namespace Luck.Walnut.Query.Applications
{
    public interface IApplicationQueryService : IScopedDependency
    {

        Task<IEnumerable<ApplicationOutputDto>> GetApplicationListAsync();

        Task<ApplicationOutput> GetApplicationDetailAndEnvironmentAsync(string id);

        Task<ApplicationDetailOutputDto> GetApplicationDetailForIdAsync(string id);
    }
}
