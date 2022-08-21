
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLoggerLoggerProvider : ILoggerProvider
{
    public void Dispose()
    {
        
    }
    public ILogger CreateLogger(string categoryName)
    {
        return new DoveLogger(categoryName);
    }
}