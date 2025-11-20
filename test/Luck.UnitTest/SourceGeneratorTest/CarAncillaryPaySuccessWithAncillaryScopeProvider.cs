using System;
using System.Threading.Tasks;
using Luck.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.UnitTest.SourceGeneratorTest;

[BusinessServiceKey(typeof(IAncillaryPaySuccessWithAncillaryScopeProvider), "50",
    ServiceLifetime.Scoped)]
public class CarAncillaryPaySuccessWithAncillaryScopeProvider : IAncillaryPaySuccessWithAncillaryScopeProvider
{
    public async Task<(bool, string)> AncillaryPaySuccessProviderAsync(string request,
        string originMessage)
    {
        return (true, "非单售辅营暂不处理");
    }
}