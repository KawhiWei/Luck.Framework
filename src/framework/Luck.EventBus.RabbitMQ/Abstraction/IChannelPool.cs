using RabbitMQ.Client;

namespace Luck.EventBus.RabbitMQ.Abstraction;

/// <summary>
/// Channel 连接池接口（纯异步）
/// </summary>
public interface IChannelPool : IAsyncDisposable
{
    /// <summary>
    /// 从池中获取 Channel（异步）
    /// </summary>
    Task<IChannel> GetChannelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 归还 Channel 到池或者释放
    /// </summary>
    void ReturnChannel(IChannel channel);
}
