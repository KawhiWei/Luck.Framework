using System.Collections.Concurrent;
using Luck.EventBus.RabbitMQ.Abstraction;
using RabbitMQ.Client;

namespace Luck.EventBus.RabbitMQ.Manager;

internal sealed class ChannelPool(IConnection connection, uint maxSize) : IChannelPool
{
    private readonly ConcurrentBag<IModel> _channels = new();

    public void Dispose()
    {
        foreach (var channel in _channels)
        {
            channel.Dispose();
        }
    }

    /// <inheritdoc />
    public IModel GetChannel()
    {
        return _channels.TryTake(out var channel) ? channel : connection.CreateModel();
    }

    /// <inheritdoc />
    public void ReturnChannel(IModel channel)
    {
        if (_channels.Count <= maxSize)
        {
            _channels.Add(channel);
            return;
        }

        channel.Dispose();
    }
}