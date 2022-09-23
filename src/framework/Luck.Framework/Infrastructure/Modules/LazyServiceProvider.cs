using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework.Infrastructure
{
    public class LazyServiceProvider : ILazyServiceProvider, ITransientDependency
    {
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<Type, object> CacheServices { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public LazyServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            CacheServices = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LazyGetRequiredService<T>()
        {
            return (T)LazyGetRequiredService(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object LazyGetRequiredService(Type serviceType)
        {
            return CacheServices.GetOrAdd(serviceType, serviceType => ServiceProvider.GetRequiredService(serviceType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LazyGetService<T>()
        {
            return (T)LazyGetService(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object LazyGetService(Type serviceType)
        {
            return CacheServices.GetOrAdd(serviceType, serviceType => ServiceProvider.GetService(serviceType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LazyGetService<T>(T defaultValue)
        {
            return (T)LazyGetService(typeof(T), defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public object LazyGetService(Type serviceType, object defaultValue)
        {
            return LazyGetService(serviceType) ?? defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return CacheServices.GetOrAdd(serviceType, serviceType => factory(ServiceProvider));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LazyGetService<T>(Func<IServiceProvider, object> factory)
        {
            return (T)LazyGetService(typeof(T), factory);
        }
    }
}
