using System;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 定义模块加载接口
    /// </summary>
    public interface IAppModule : IApplicationInitialization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void ConfigureServices(ConfigureServicesContext context);
        /// <summary>
        /// 服务依赖集合
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        Type[] GetDependedTypes(Type? moduleType = null);
        /// <summary>
        /// 
        /// </summary>
        bool Enable { get;  set; }
    }
}