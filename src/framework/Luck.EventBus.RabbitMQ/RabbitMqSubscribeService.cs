using Luck.Framework.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Luck.EventBus.RabbitMQ
{
    /// <summary>
    /// 后台任务进行事件订阅
    /// </summary>
    public class RabbitMqSubscribeService(IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = serviceProvider.CreateScope())
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