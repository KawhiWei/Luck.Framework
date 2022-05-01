using Grpc.Core;
using Luck.Walnut.V1;

namespace Luck.Walnut.Api.GrpcServices
{
    public class GetConfigService: GetConfig.GetConfigBase
    {



        public override Task<ApplicationConfigResponse> GetAppliactionConfig(ApplicationConfigRequest request, ServerCallContext context)
        {
            return base.GetAppliactionConfig(request, context);
        }
    }
}
