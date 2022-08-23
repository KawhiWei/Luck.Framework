using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLoggerManager : IDoveLoggerManager
{
    private readonly ConcurrentQueue<string> _queue = new();
    private CancellationTokenSource? _tokenSource = null;
    private Task? _task = null;

    public DoveLoggerManager()
    {
    }

    public void Start()
    {
        _tokenSource = new CancellationTokenSource();
        Task.Factory.StartNew(() => Running(_tokenSource.Token), TaskCreationOptions.LongRunning);
    }

    public void Stop(CancellationToken cancellationToken)
    {
        _tokenSource?.Cancel();
    }

    public void Enqueue(string message)
    {
        _queue.Enqueue(message);
    }


    private async Task Running(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_queue.TryDequeue(out var message))
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    await Task.CompletedTask;
                    Console.WriteLine($"后台日志服务打印了日志：「{message}」");
                    //日志后续处理
                }
                else
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}