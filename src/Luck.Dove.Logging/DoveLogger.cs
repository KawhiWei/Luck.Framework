using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLogger : ILogger
{
    private readonly string _categoryName;
    private readonly IDoveLoggerProcessor _doveLoggerProcessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DoveLogger(string categoryName, IDoveLoggerProcessor doveLoggerProcessor, IHttpContextAccessor httpContextAccessor)
    {
        this._categoryName = categoryName;
        _doveLoggerProcessor = doveLoggerProcessor;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _doveLoggerProcessor.Enqueue(_categoryName, $"{state?.ToString()}");
    }

    public bool IsEnabled(LogLevel logLevel) => true;


    public IDisposable BeginScope<TState>(TState state) => default!;
}