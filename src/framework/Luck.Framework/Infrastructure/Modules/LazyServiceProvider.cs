using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
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
        /// <param name="service"></param>
        /// <returns></returns>
        public object LazyGetRequiredService(Type service)
        {
            return CacheServices.GetOrAdd(service, serviceType => ServiceProvider.GetRequiredService(serviceType));
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
        /// <param name="service"></param>
        /// <returns></returns>
        public object LazyGetService(Type? service)
        {
            return CacheServices!.GetOrAdd(service, serviceType =>
            {
                if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
                return ServiceProvider.GetService(serviceType);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LazyGetService<T>(T? defaultValue)
        {
            return (T)LazyGetService(typeof(T), defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public object LazyGetService(Type service, object? defaultValue)
        {
            return LazyGetService(service) ?? defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public object LazyGetService(Type service, Func<IServiceProvider, object> factory)
        {
            return CacheServices.GetOrAdd(service, serviceType => factory(ServiceProvider));
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
