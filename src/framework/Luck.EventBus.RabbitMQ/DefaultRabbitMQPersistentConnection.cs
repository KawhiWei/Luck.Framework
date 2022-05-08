using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace Luck.EventBus.RabbitMQ
{
    public class DefaultRabbitMQPersistentConnection
    : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;
        private readonly int _retryCount;
        IConnection? _connection;
        bool _disposed;

        object sync_root = new object();



        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<DefaultRabbitMQPersistentConnection> logger, int retryCount = 5)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
        }



        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
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

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            if (_connection is not null)
            {
                try
                {
                    _connection.ConnectionShutdown -= OnConnectionShutdown;
                    _connection.CallbackException -= OnCallbackException;
                    _connection.ConnectionBlocked -= OnConnectionBlocked;
                    _connection.Dispose();
                }
                catch (IOException ex)
                {
                    _logger.LogCritical(ex.ToString());
                }
            }
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ客户端尝试连接");

            lock (sync_root)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "RabbitMQ客户端无法连接 {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    }
                );

                policy.Execute(() =>
                {
                    _connection = _connectionFactory
                            .CreateConnection();
                });

                if (IsConnected && _connection is not null)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation("RabbitMQ Client获得了一个持久连接 '{HostName}' 并且订阅失败了事件", _connection.Endpoint.HostName);
                    return true;
                }
                else
                {
                    _logger.LogCritical("RabbitMQ连接不能被创建和打开");

                    return false;
                }
            }
        }


        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ连接关闭。正在尝试重新连接...");

            TryConnect();
        }

        void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ连接抛出异常。在重试...");

            TryConnect();
        }

        void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ连接处于关闭状态。正在尝试重新连接...");

            TryConnect();
        }
    }



}