using Luck.Dove.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class DoveLoggerExtensions
{
    public static IServiceCollection AddDoveLogger(this IServiceCollection service)
    {
        service.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DoveLoggerLoggerProvider>());
        service.AddSingleton<IDoveLoggerManager, DoveLoggerManager>();
        service.AddHostedService<DoveLoggerBackgroundServiceSubscribe>();
        return service;
    }
}