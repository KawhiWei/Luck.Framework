namespace Luck.Framework.Infrastructure.DependencyInjectionModule
{
    /// <summary>
    /// 实现此接口的类型将自动注册为 ServiceLifetime.Singleton 模式
    /// </summary>
    [IgnoreDependency]
    public interface ISingletonDependency
    {
    }
}