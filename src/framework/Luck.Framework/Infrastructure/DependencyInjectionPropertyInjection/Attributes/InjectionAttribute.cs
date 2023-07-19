namespace Luck.Framework.Infrastructure.DependencyInjectionPropertyInjection.Attributes
{
    /// <summary>
    /// 实现属性注入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class InjectionAttribute : Attribute
    {

    }
}
