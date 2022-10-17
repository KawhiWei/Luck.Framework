using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLoggerProvider : ILoggerProvider
{
    private ConcurrentDictionary<string, DoveLogger> _doveLoggers = new();
    private readonly IDoveLoggerProcessor _doveLoggerProcessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DoveLoggerProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _doveLoggerProcessor = new DoveLoggerProcessor();
    }

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _doveLoggers.GetOrAdd(categoryName, _ => new DoveLogger(categoryName, _doveLoggerProcessor,_httpContextAccessor));
    }
}