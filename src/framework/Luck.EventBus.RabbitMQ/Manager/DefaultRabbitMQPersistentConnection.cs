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
    /// 
    /// </summary>
    public class DefaultRabbitMqPersistentConnection
        : IRabbitMqPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DefaultRabbitMqPersistentConnection> _logger;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private readonly List<AmqpTcpEndpoint>? _tcpEndpoints;
        private readonly uint _maxPoolCount;
        private readonly int _retryCount;
        private IChannelPool? _channelPool;
        private IConnection? _connection;
        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        private readonly object _syncRoot = new object();


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
            _tcpEndpoints = tcpEndpoints;
            _maxPoolCount = maxChannelCount < 1 ? (uint)Environment.ProcessorCount : maxChannelCount;
        }


        public bool IsConnected => _connection is { IsOpen: true } && !_disposed;

        public void ReturnChannel(IModel channel)
        {
            throw new NotImplementedException();
        }

        public IModel GetChannel()
        {
            throw new NotImplementedException();
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("RabbitMQ连接失败");
            }

            if (_connection is null)
                throw new InvalidOperationException("RabbitMQ连接未创建");
            return _connection.CreateModel();
        }


        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ客户端尝试连接");
            _connectionLock.Wait();

            try
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (ex, time) =>
                        {
                            _logger.LogWarning(ex, "RabbitMQ客户端无法连接 {TimeOut}s ({ExceptionMessage})",
                                $"{time.TotalSeconds:n1}", ex.Message);
                        }
                    );

                policy.Execute(() =>
                {
                    _connection = _tcpEndpoints is not null &&
                                  _tcpEndpoints.Count > 0
                        ? _connectionFactory.CreateConnection(_tcpEndpoints)
                        : _connectionFactory.CreateConnection();
                });

                if (IsConnected && _connection is not null)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation("RabbitMQ Client获得了一个持久连接 '{HostName}' 并且订阅了故障事件",
                        _connection.Endpoint.HostName);
                    _channelPool = new ChannelPool(_connection, _maxPoolCount);
                    _logger.LogInformation("RabbitBus channel pool max count: {Count}", _maxPoolCount);
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
                _ = _connectionLock.Release();
            }
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                if (_connection is not null)
                {
                    try
                    {
                        _connection.ConnectionShutdown -= OnConnectionShutdown;
                        _connection.CallbackException -= OnCallbackException;
                        _connection.ConnectionBlocked -= OnConnectionBlocked;
                        _connection.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical("{Message}", ex.Message);
                    }
                }
            }

            if (_channelPool is null)
            {
                return;
            }

            try
            {
                _channelPool.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("{Message}", ex.Message);
            }

            _disposed = true;
        }


        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("RabbitMQ连接关闭。正在尝试重新连接...");
            _ = TryConnect();
        }

        void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("RabbitMQ连接抛出异常。在重试...");
            _ = TryConnect();
        }

        void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("RabbitMQ连接处于关闭状态。正在尝试重新连接...");

            _ = TryConnect();
        }
    }
}