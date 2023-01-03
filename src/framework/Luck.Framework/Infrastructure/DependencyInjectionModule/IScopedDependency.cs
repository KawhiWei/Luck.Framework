
namespace Luck.Framework.Infrastructure.DependencyInjectionModule
{
    /// <summary>
    /// 实现此接口的类型将自动注册为 ServiceLifetime.Scoped 模式
    /// </summary>
    [IgnoreDependency]
    public interface IScopedDependency
    {
    }
}