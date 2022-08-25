using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLoggerLoggerProvider : ILoggerProvider
{
    private ConcurrentDictionary<string, DoveLogger> _doveLoggers = new ConcurrentDictionary<string, DoveLogger>();
    private readonly IDoveLoggerProcessor _doveLoggerProcessor;

    public DoveLoggerLoggerProvider()
    {
        _doveLoggerProcessor = new DoveLoggerProcessor();
    }

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _doveLoggers.GetOrAdd(categoryName, _ => new DoveLogger(categoryName, _doveLoggerProcessor));
    }
}