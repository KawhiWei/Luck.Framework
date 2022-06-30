using MediatR;

namespace Module.Sample.EventHandlers
{
    public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEto>
    {

        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(OrderCreatedEto notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"创建订单成功,订单Id={notification.Id},Name={notification.Name},创建时间：{DateTime.Now}");
            return Task.CompletedTask;
        }
    }

    public class OrderCreatedEto : INotification
    {


        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;

    }
}
