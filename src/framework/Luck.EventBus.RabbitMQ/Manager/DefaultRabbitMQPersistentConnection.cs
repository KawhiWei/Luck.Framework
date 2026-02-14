using System.Net.Sockets;
using Luck.EventBus.RabbitMQ.Abstraction;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Luck.EventBus.RabbitMQ.Manager
{
    /// <summary>
    /// RabbitMQ 持久化连接管理类（纯异步版本）
    /// </summary>
    public class DefaultRabbitMqPersistentConnection : IRabbitMqPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DefaultRabbitMqPersistentConnection> _logger;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private readonly List<AmqpTcpEndpoint>? _tcpEndpoints;
        private readonly uint _maxPoolCount;
        private readonly int _retryCount;
        
        // 发布通道池和消费通道池分离
        private IChannelPool? _publishChannelPool;
        private IChannelPool? _consumerChannelPool;
        private IConnection? _connection;
        private bool _disposed;

        public DefaultRabbitMqPersistentConnection(
            IConnectionFactory connectionFactory,
            ILogger<DefaultRabbitMqPersistentConnection> logger,
            int retryCount = 5,
            uint maxChannelCount = 10,
            List<AmqpTcpEndpoint>? tcpEndpoints = null
        )
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
            _tcpEndpoints = tcpEndpoints;
            _maxPoolCount = maxChannelCount < 1 ? (uint)Environment.ProcessorCount : maxChannelCount;
        }

        public bool IsConnected => _connection is { IsOpen: true } && !_disposed;

        public async Task<bool> TryConnectAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("RabbitMQ客户端尝试连接");
            await _connectionLock.WaitAsync(cancellationToken);

            try
            {
                if (IsConnected)
                {
                    return true;
                }

                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetryAsync(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (ex, time) =>
                        {
                            _logger.LogWarning(ex, "RabbitMQ客户端无法连接 {TimeOut}s ({ExceptionMessage})",
                                $"{time.TotalSeconds:n1}", ex.Message);
                        }
                    );

                await policy.ExecuteAsync(async ct =>
                {
                    _connection = _tcpEndpoints is not null && _tcpEndpoints.Count > 0
                        ? await _connectionFactory.CreateConnectionAsync(_tcpEndpoints, ct)
                        : await _connectionFactory.CreateConnectionAsync(ct);
                }, cancellationToken);

                if (IsConnected && _connection is not null)
                {
                    _connection.ConnectionShutdownAsync += OnConnectionShutdownAsync;
                    _connection.CallbackExceptionAsync += OnCallbackExceptionAsync;
                    _connection.ConnectionBlockedAsync += OnConnectionBlockedAsync;

                    _logger.LogInformation("RabbitMQ Client获得了一个持久连接 '{HostName}' 并且订阅了故障事件",
                        _connection.Endpoint.HostName);
                    
                    // 创建独立的发布通道池和消费通道池
                    _publishChannelPool = new ChannelPool(_connection, _maxPoolCount);
                    _consumerChannelPool = new ChannelPool(_connection, _maxPoolCount);
                    
                    _logger.LogInformation("RabbitBus 发布通道池最大数量: {Count}", _maxPoolCount);
                    _logger.LogInformation("RabbitBus 消费通道池最大数量: {Count}", _maxPoolCount);
                    
                    _disposed = false;
                    return true;
                }
                else
                {
                    _logger.LogCritical("RabbitMQ连接不能被创建和打开");
                    return false;
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async Task<IChannel> GetPublishChannelAsync(CancellationToken cancellationToken = default)
        {
            if (!IsConnected)
            {
                await TryConnectAsync(cancellationToken);
            }

            if (_publishChannelPool is null)
            {
                throw new InvalidOperationException("发布通道池未初始化");
            }

            return await _publishChannelPool.GetChannelAsync(cancellationToken);
        }

        public async Task<IChannel> GetConsumerChannelAsync(CancellationToken cancellationToken = default)
        {
            if (!IsConnected)
            {
                await TryConnectAsync(cancellationToken);
            }

            if (_consumerChannelPool is null)
            {
                throw new InvalidOperationException("消费通道池未初始化");
            }

            return await _consumerChannelPool.GetChannelAsync(cancellationToken);
        }

        public async Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("RabbitMQ连接失败");
            }

            if (_connection is null)
                throw new InvalidOperationException("RabbitMQ连接未创建");
            
            return await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        }

        public void ReturnPublishChannel(IChannel channel)
        {
            if (_disposed || !channel.IsOpen)
            {
                channel.Dispose();
                return;
            }

            _publishChannelPool?.ReturnChannel(channel);
        }

        public void ReturnConsumerChannel(IChannel channel)
        {
            if (_disposed || !channel.IsOpen)
            {
                channel.Dispose();
                return;
            }

            _consumerChannelPool?.ReturnChannel(channel);
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (_connection is not null)
            {
                try
                {
                    _connection.ConnectionShutdownAsync -= OnConnectionShutdownAsync;
                    _connection.CallbackExceptionAsync -= OnCallbackExceptionAsync;
                    _connection.ConnectionBlockedAsync -= OnConnectionBlockedAsync;
                    await _connection.DisposeAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("{Message}", ex.Message);
                }
            }

            if (_publishChannelPool is not null)
            {
                await _publishChannelPool.DisposeAsync();
            }
            if (_consumerChannelPool is not null)
            {
                await _consumerChannelPool.DisposeAsync();
            }
            _connectionLock.Dispose();
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        private Task OnConnectionBlockedAsync(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return Task.CompletedTask;

            _logger.LogWarning("RabbitMQ连接关闭。正在尝试重新连接...");
            _ = TryConnectAsync();
            return Task.CompletedTask;
        }

        private Task OnCallbackExceptionAsync(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return Task.CompletedTask;

            _logger.LogWarning("RabbitMQ连接抛出异常。在重试...");
            _ = TryConnectAsync();
            return Task.CompletedTask;
        }

        private Task OnConnectionShutdownAsync(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed) return Task.CompletedTask;

            _logger.LogWarning("RabbitMQ连接处于关闭状态。正在尝试重新连接...");
            _ = TryConnectAsync();
            return Task.CompletedTask;
        }
    }
}
