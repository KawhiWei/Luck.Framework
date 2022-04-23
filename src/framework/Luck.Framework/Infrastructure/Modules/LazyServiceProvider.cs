using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework.Infrastructure
{
    public class LazyServiceProvider : ILazyServiceProvider, ITransientDependency
    {
        protected Dictionary<Type, object> CacheServices { get; set; }
        protected IServiceProvider ServiceProvider { get; set; }

        public LazyServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            CacheServices = new Dictionary<Type, object>();
        }

        public T LazyGetRequiredService<T>()
        {
            return (T)LazyGetRequiredService(typeof(T));
        }

        public object LazyGetRequiredService(Type serviceType)
        {
            return CacheServices.GetOrAdd(serviceType, serviceType => ServiceProvider.GetRequiredService(serviceType));
        }

        public T LazyGetService<T>()
        {
            return (T)LazyGetService(typeof(T));
        }

        public object LazyGetService(Type serviceType)
        {
            return CacheServices.GetOrAdd(serviceType, serviceType => ServiceProvider.GetService(serviceType));
        }

        public T LazyGetService<T>(T defaultValue)
        {
            return (T)LazyGetService(typeof(T), defaultValue);
        }

        public object LazyGetService(Type serviceType, object defaultValue)
        {
            return LazyGetService(serviceType) ?? defaultValue;
        }

        public object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return CacheServices.GetOrAdd(serviceType, serviceType => factory(ServiceProvider));
        }

        public T LazyGetService<T>(Func<IServiceProvider, object> factory)
        {
            return (T)LazyGetService(typeof(T), factory);
        }
    }
}
