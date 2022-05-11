namespace Module.Sample
{
    public class MyBackgroundService : BackgroundService
    {

        private readonly ILogger<MyBackgroundService> _logger;

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {

                while (true)
                {
                    _logger.LogInformation("进入定时任务");
                }
            }
            return Task.CompletedTask;
        }
    }
}
