using System.Threading.Tasks;
using Luck.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.UnitTest.SourceGeneratorTest;

[BusinessServiceKey(typeof(IAncillaryPaySuccessWithAncillaryScopeProvider), "70",
    ServiceLifetime.Scoped)]
public class BusExpressAncillaryPaySuccessWithAncillaryScopeProvider : IAncillaryPaySuccessWithAncillaryScopeProvider
{
    public async Task<(bool, string)> AncillaryPaySuccessProviderAsync(string request,
        string originMessage)
    {
        await Task.CompletedTask;
        return  (true, "非单售辅营暂不处理");
    }
}