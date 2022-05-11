using Luck.Framework.Exceptions;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtension
    {
        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <typeparam name="T">非静态强类型</typeparam>
        /// <returns>日志对象</returns>
        public static ILogger<T> GetLogger<T>(this IServiceProvider provider)
        {
            ILoggerFactory? factory = provider.GetService<ILoggerFactory>();
            return factory?.CreateLogger<T>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILogger? GetLogger(this IServiceProvider provider, Type type)
        {
            ILoggerFactory? factory = provider.GetService<ILoggerFactory>();
            return factory?.CreateLogger(type);
        }

        public static object? GetInstance(this IServiceProvider provider, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }
           
            if (descriptor.ImplementationType != null)
            {
                return provider.GetServiceOrCreateInstance(descriptor.ImplementationType);
            }
            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory(provider);
            }
            return null;
        }
        /// <summary>
        /// 获取日志记录器
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILogger GetLogger(this ILazyServiceProvider provider, Type type)
        {
            ILoggerFactory factory = provider.LazyGetService<ILoggerFactory>();
            return factory.CreateLogger(type);
        }
        public static object GetServiceOrCreateInstance(this IServiceProvider provider, Type type)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(provider, type);
        }

        public static object CreateInstance(this IServiceProvider provider, Type type, params object[] arguments)
        {
            return ActivatorUtilities.CreateInstance(provider, type, arguments);
        }

        public static void GetService<T>(this IServiceProvider provider, Action<T> action)
        {
            if(action==null)
                throw new ArgumentNullException(nameof(action));
            var t = provider.GetService<T>();
            if (t == null)
                throw new ArgumentNullException(nameof(action));
            action(t);
        }

        public static void CreateScoped(this IServiceProvider provider, Action<IServiceProvider> callback)
        {
            using var scope = provider.CreateScope();
            callback(scope.ServiceProvider);
        }
    }
}
