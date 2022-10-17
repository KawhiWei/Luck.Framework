using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Luck.Framework.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLoggerProcessor : IDoveLoggerProcessor
{
    private BufferBlock<(DateTimeOffset, string)> _bufferBlock;
    private CancellationTokenSource? _tokenSource = null;
    private Task? _task = null;
    private readonly ConcurrentQueue<(string categoryName, string message)> _concurrentQueue = new();

    public DoveLoggerProcessor()
    {
        _bufferBlock = new BufferBlock<(DateTimeOffset, string)>(new DataflowBlockOptions { BoundedCapacity = 10_000 });
        Task.Factory.StartNew(RunningAsync, TaskCreationOptions.LongRunning);
    }

    public void Start()
    {
        _tokenSource = new CancellationTokenSource();
    }

    public void Stop(CancellationToken cancellationToken)
    {
        _tokenSource?.Cancel();
    }

    public void Enqueue(string categoryName, string message)
    {
        _concurrentQueue.Enqueue((categoryName, message));
    }
    
    private async Task RunningAsync()
    {
        while (true)
        {
            if (_concurrentQueue.TryDequeue(out var item))
            {
                try
                {
                    var appId = Environment.GetEnvironmentVariable("AppId");
                    var logger = item.message.Deserialize<DoveLoggerModule>();
                    if (logger is null)
                    {
                        continue;
                    }

                    logger.CategoryName = item.categoryName.Split(".").Last();
                    var dictionary = new Dictionary<string, string>
                    {
                        { "appId", appId ?? "" },
                        { "categoryName", logger.CategoryName },
                        { "body", logger.Body },
                        { "method", logger.Method },
                        { "businessFilter", logger.BusinessFilter },
                        // { "businessFilter", logger.BusinessFilter }
                    };

                    Console.WriteLine($"[{appId}][{logger.CategoryName}][{logger.Method}][{logger.Body}][{logger.BusinessFilter}][{logger.Exception}]");
                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                await Task.Delay(1000);
            }
        }
    }
}