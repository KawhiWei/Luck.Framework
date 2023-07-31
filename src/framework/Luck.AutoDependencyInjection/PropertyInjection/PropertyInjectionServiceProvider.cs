using Luck.AutoDependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.AutoDependencyInjection.PropertyInjection
{
    /// <summary>
    /// 属性注入提供者接口
    /// </summary>
    internal class PropertyInjectionServiceProvider : IPropertyInjectionServiceProvider
    {
        private readonly IPropertyInjector _propertyInjector;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public PropertyInjectionServiceProvider(IServiceCollection service)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            service.AddSingleton<IPropertyInjectionServiceProvider>(this);
            _serviceProvider = service.BuildServiceProvider();
            _propertyInjector = new PropertyInjector(this);
        }



        public object? GetService(Type serviceType)
        {
            var instance = _serviceProvider.GetService(serviceType);
            return instance is null ? null : _propertyInjector.InjectProperties(instance);
        }

        public object GetRequiredService(Type serviceType)
        {
            var service = GetService(serviceType);
            return service;
        }
    }
}
