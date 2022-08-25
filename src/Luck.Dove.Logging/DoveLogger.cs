using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLogger : ILogger
{
    private readonly string _categoryName;
    private readonly IDoveLoggerProcessor _doveLoggerProcessor;

    public DoveLogger(string categoryName, IDoveLoggerProcessor doveLoggerProcessor)
    {
        this._categoryName = categoryName;
        _doveLoggerProcessor = doveLoggerProcessor;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _doveLoggerProcessor.Enqueue(_categoryName,$"{state?.ToString()}");
    }

    private bool IsMicrosoftWirte()
    {
        return new List<string>() { "Microsoft.Hosting.Lifetime" }.Any(x => x == _categoryName);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state) => default!;
}