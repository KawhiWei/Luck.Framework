using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLogger: ILogger
{
    private readonly string _categoryName;

    public DoveLogger(string categoryName)
    {
        this._categoryName = categoryName;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Console.WriteLine($"{_categoryName}====== {logLevel} - {eventId.Id} : " + formatter(state, exception));
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state) => default!;
   
}