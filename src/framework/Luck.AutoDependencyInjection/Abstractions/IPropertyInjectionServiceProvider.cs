using Microsoft.Extensions.DependencyInjection;

namespace Luck.AutoDependencyInjection.Abstractions
{

    /// <summary>
    /// 属性注入提供者接口
    /// </summary>
    public interface IPropertyInjectionServiceProvider : IServiceProvider, ISupportRequiredService
    {

    }
}
