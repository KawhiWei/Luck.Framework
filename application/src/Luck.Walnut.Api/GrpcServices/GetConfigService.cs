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

        public override async Task<ApplicationConfigResponse> GetAppliactionConfig(ApplicationConfigRequest request, ServerCallContext context)
        {
            var configs = await _environmentService.GetAppConfigurationByAppIdAndEnvironmentName(request.AppId, request.EnvironmentName);
            var results = configs.Select(config => new Result
            {
                Key = config.Key,
                Value = config.Value,
                Type = config.Type,
            });
            var response = new ApplicationConfigResponse();
            response.Result.AddRange(results);
            return response;
        }
    }
}
