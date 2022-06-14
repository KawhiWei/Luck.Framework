using System.Reflection;
using Luck.Framework.Event;
using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Luck.EventBus.RabbitMQ
{
    /// <summary>
    /// 后台任务进行事件订阅
    /// </summary>
    public class RabbitMqIntegrationEventBusBackgroundServiceSubscribe : BackgroundService
    {

        private readonly IServiceProvider _rootServiceProvider;

        public RabbitMqIntegrationEventBusBackgroundServiceSubscribe(IServiceProvider serviceProvider)
        {
            _rootServiceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _rootServiceProvider.CreateScope())
            {
                var eventBus= scope.ServiceProvider.GetService<IIntegrationEventBus>();
                if (eventBus is null)
                {
                    throw new Exception("RabbitMQ集成事件总线没有注册");
                }
                eventBus.Subscribe();
                while (true && !stoppingToken.IsCancellationRequested)
                {
                    await  Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}