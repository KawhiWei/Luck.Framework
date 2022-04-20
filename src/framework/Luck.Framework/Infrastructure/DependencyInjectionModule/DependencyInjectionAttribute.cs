using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Framework.Infrastructure.DependencyInjectionModule
{
    /// <summary>
    /// 配置此特性将自动进行注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyInjectionAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lifetime">注入类型(Scoped\Singleton\Transient)</param>
        public DependencyInjectionAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }

        /// <summary>
        /// 
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// 获取或设置 是否注册自身类型，默认没有接口的类型会注册自身，当此属性值为true时，也会注册自身
        /// </summary>
        public bool AddSelf { get; set; }
    }
}
