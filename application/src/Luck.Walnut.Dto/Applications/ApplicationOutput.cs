using Luck.Walnut.Dto.Environments;

namespace Luck.Walnut.Dto.Applications
{
    public class ApplicationOutput
    {
        public ApplicationOutputDto Application { get; set; }=default!;

        public List<AppEnvironmentListOutputDto> EnvironmentLists { get; set; } = default!;
    }
}
