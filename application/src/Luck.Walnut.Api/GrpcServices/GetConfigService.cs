using Grpc.Core;
using Luck.Walnut.Application.Environments;
using Luck.Walnut.V1;

namespace Luck.Walnut.Api.GrpcServices
{
    public class GetConfigService : GetConfig.GetConfigBase
    {
        private readonly IEnvironmentService _environmentService;

        public GetConfigService(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public override Task<ApplicationConfigResponse> GetAppliactionConfig(ApplicationConfigRequest request, ServerCallContext context)
        {
            //_environmentService.GetAppEnvironmentPageAsync();

            return Task.FromResult(new ApplicationConfigResponse());
        }
    }
}
