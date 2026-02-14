using RabbitMQ.Client;

namespace Luck.EventBus.RabbitMQ.Abstraction;

/// <summary>
/// RabbitMQ 持久化连接接口（纯异步）
/// </summary>
public interface IRabbitMqPersistentConnection : IAsyncDisposable
{
    /// <summary>
    /// 是否已连接
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// 尝试连接（异步）
    /// </summary>
    Task<bool> TryConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 从发布通道池中获取 Channel（异步）
    /// </summary>
    Task<IChannel> GetPublishChannelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 从消费通道池中获取 Channel（异步）
    /// </summary>
    Task<IChannel> GetConsumerChannelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建新的 Channel（不使用连接池，异步）
    /// </summary>
    Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 归还发布 Channel 到连接池
    /// </summary>
    void ReturnPublishChannel(IChannel channel);

    /// <summary>
    /// 归还消费 Channel 到连接池
    /// </summary>
    void ReturnConsumerChannel(IChannel channel);
}
