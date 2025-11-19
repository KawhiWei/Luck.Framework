using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework
{
    /// <summary>
    /// 业务ServiceKey:如辅营使用辅营Scope来进行配置ServiceKey
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BusinessServiceKeyAttribute(Type serviceType, string serviceKey, ServiceLifetime lifetime)
        : Attribute
    {
        public Type ServiceType { get; } = serviceType;

        public string ServiceKey { get; } = serviceKey;

        public ServiceLifetime Lifetime { get; } = lifetime;
    }
}