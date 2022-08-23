
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLoggerLoggerProvider : ILoggerProvider
{
    private readonly IDoveLoggerManager _doveLoggerManager;

    public DoveLoggerLoggerProvider(IDoveLoggerManager doveLoggerManager)
    {
        _doveLoggerManager = doveLoggerManager;
    }

    public void Dispose()
    {
        
    }
    public ILogger CreateLogger(string categoryName)
    {
        return new DoveLogger(categoryName,_doveLoggerManager);
    }
}