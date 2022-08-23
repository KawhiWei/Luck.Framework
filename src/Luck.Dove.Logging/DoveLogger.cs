using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLogger: ILogger
{
    private readonly string _categoryName;
    private readonly IDoveLoggerManager _loggerManager;
    
    public DoveLogger(string categoryName, IDoveLoggerManager loggerManager)
    {
        this._categoryName = categoryName;
        _loggerManager = loggerManager;
    }
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _loggerManager.Enqueue($"[{_categoryName}][{logLevel}]{state?.ToString()}");
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state) => default!;
   
}