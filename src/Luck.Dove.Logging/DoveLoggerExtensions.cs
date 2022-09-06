using Luck.Dove.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class DoveLoggerExtensions
{
    public static IServiceCollection AddDoveLogger(this IServiceCollection service)
    {
        service.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DoveLoggerProvider>());
        // service.AddSingleton<IDoveLoggerProcessor, DoveLoggerProcessor>();
        return service;
    }
}