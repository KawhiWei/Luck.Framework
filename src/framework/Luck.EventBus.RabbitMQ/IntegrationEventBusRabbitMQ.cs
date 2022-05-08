using Luck.EventBus.RabbitMQ.Attributes;
using Luck.Framework.Event;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;

namespace Luck.EventBus.RabbitMQ
{
    public class IntegrationEventBusRabbitMQ : IIntegrationEventBus
    {

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<IntegrationEventBusRabbitMQ> _logger;
        private readonly int _retryCount;

        public IntegrationEventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, ILogger<IntegrationEventBusRabbitMQ> logger, int retryCount)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _retryCount = retryCount;
        }

        public void Publish<TEvent>(IIntegrationEvent @event) where TEvent : IIntegrationEvent
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }


            var type = @event.GetType();

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(ex, "发布集成事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
            });

            _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.Id, type.Name);


            var rabbitMQAttribute = type.GetCustomAttribute<RabbitMQAttribute>();

            if (rabbitMQAttribute is null)
                throw new ArgumentNullException($"{nameof(@event)}未设置<RabbitMQAttribute>特性,无法发布事件");
            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

            using (var channel = _persistentConnection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();

                channel.ExchangeDeclare(exchange: rabbitMQAttribute.Exchange, type: rabbitMQAttribute.Type);

                if (!string.IsNullOrEmpty(rabbitMQAttribute.RoutingKey) && !string.IsNullOrEmpty(rabbitMQAttribute.Queue))
                {
                    channel.QueueBind(rabbitMQAttribute.Queue, rabbitMQAttribute.Exchange, rabbitMQAttribute.RoutingKey);
                }
                policy.Execute(() =>
                {
                    properties.DeliveryMode = 2;
                    _logger.LogTrace("向RabbitMQ发布事件: {EventId}", @event.Id);
                    channel.BasicPublish(exchange: rabbitMQAttribute.Exchange, routingKey: rabbitMQAttribute.RoutingKey, mandatory: true, basicProperties: properties, body: body);
                });
            }
        }
    }
}