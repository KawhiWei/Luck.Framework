using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;

namespace Luck.Dove.Logging;

public class DoveLoggerProcessor:IDoveLoggerProcessor
{
    private BufferBlock<(DateTimeOffset, string)> _bufferBlock;
    private CancellationTokenSource? _tokenSource = null;
    private Task? _task = null;

    private readonly ConcurrentQueue<(string,string)> _concurrentQueue = new ConcurrentQueue<(string,string)>();
    public DoveLoggerProcessor()
    {
        _bufferBlock = new BufferBlock<(DateTimeOffset, string)>(new DataflowBlockOptions { BoundedCapacity = 10_000 });
        Task.Factory.StartNew(RunningAsync,TaskCreationOptions.LongRunning);
    }

    public void Start()
    {
        _tokenSource = new CancellationTokenSource();
        
    }

    public void Stop(CancellationToken cancellationToken)
    {
        _tokenSource?.Cancel();
    }

    public void Enqueue(string categoryName,string message)
    {
        _concurrentQueue.Enqueue((categoryName,message));
    }


    private async Task RunningAsync(object state)
    {
        
    }
    private async Task RunningAsync()
    {
        while (true)
        {
            if (_concurrentQueue.TryDequeue(out var item))
            {
                Console.WriteLine($"{item.Item2}-----------{item.Item2}");
                await Task.Delay(1000);
            }
            else
            {
                await Task.Delay(1000);
            }
        }
    }
}