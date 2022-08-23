using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLoggerBackgroundServiceSubscribe : BackgroundService
{
    private readonly IDoveLoggerManager _loggerManager;
    public DoveLoggerBackgroundServiceSubscribe(IDoveLoggerManager loggerManager)
    {
        _loggerManager = loggerManager;
    }

    protected override  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // _logger.LogInformation("自定义日志后台处理服务准备启动");
        _loggerManager.Start();
        // _logger.LogInformation("自定义日志后台处理服务启动完成");
        return Task.CompletedTask;
    }
}