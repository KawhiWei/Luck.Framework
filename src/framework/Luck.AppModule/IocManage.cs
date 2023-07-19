using Luck.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Luck.AppModule
{
    /// <summary>
    /// IOC管理
    /// </summary>
    public class IocManage
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        private IServiceProvider _provider;

        /// <summary>
        /// 服务集合
        /// </summary>
        private IServiceCollection _services;

        /// <summary>
        /// 创建懒加载Ioc管理实例
        /// </summary>
        private static readonly Lazy<IocManage> InstanceLazy = new Lazy<IocManage>(() => new IocManage());

        /// <summary>
        /// 构造方法
        /// </summary>
        private IocManage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static IocManage Instance => InstanceLazy.Value;

        /// <summary>
        /// 设置应用程序服务提供者
        /// </summary>
        internal void SetApplicationServiceProvider(IServiceProvider provider)
        {
            provider.NotNull(nameof(provider));
            _provider = provider;
        }

        /// <summary>
        /// 设置应用程序服务集合
        /// </summary>
        public void SetServiceCollection(IServiceCollection services)
        {
            services.NotNull(nameof(services));
            _services = services;
        }
        /// <summary>
        /// 得到服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public T? GetService<T>()
        {
            _provider.NotNull(nameof(_provider));
            _services.NotNull(nameof(_services));
            return _provider.GetService<T>();
        }

        /// <summary>
        /// 得到日志记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ILogger? GetLogger<T>()
        {
            ILoggerFactory? factory = _provider.GetService<ILoggerFactory>();
            return factory?.CreateLogger<T>();
        }
    }
}