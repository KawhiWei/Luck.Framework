using System.Collections.Concurrent;
using Luck.EventBus.RabbitMQ.Abstraction;
using RabbitMQ.Client;

namespace Luck.EventBus.RabbitMQ.Manager;

/// <summary>
/// Channel 连接池实现（纯异步版本）
/// </summary>
internal sealed class ChannelPool : IChannelPool
{
    private readonly IConnection _connection;
    private readonly uint _maxSize;
    private readonly ConcurrentBag<IChannel> _channels = new();
    private readonly SemaphoreSlim _createLock = new(1, 1);
    private bool _disposed;

    public ChannelPool(IConnection connection, uint maxSize)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _maxSize = maxSize;
    }

    public async Task<IChannel> GetChannelAsync(CancellationToken cancellationToken = default)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(ChannelPool));
        }

        // 尝试从池中获取
        if (_channels.TryTake(out var channel) && channel.IsOpen)
        {
            return channel;
        }

        // 池中没有可用 channel，创建新的
        await _createLock.WaitAsync(cancellationToken);
        try
        {
            return await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            _createLock.Release();
        }
    }

    public void ReturnChannel(IChannel channel)
    {
        if (_disposed || !channel.IsOpen)
        {
            channel.Dispose();
            return;
        }

        if (_channels.Count < _maxSize)
        {
            _channels.Add(channel);
        }
        else
        {
            channel.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        _disposed = true;

        foreach (var channel in _channels)
        {
            try
            {
                await channel.DisposeAsync();
            }
            catch
            {
                // 忽略释放时的错误
            }
        }

        _createLock.Dispose();
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
