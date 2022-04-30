using Luck.Framework.Infrastructure;
using Serilog;
using Serilog.Events;

namespace Luck.Walnut.Api.AppModules
{
    public class SerilogModule : AppModule
    {

        public override void ConfigureServices(ConfigureServicesContext context)
        {
            context.Services.AddLogging(builder =>
            {
       
                Log.Logger = new LoggerConfiguration()

                   .Enrich.FromLogContext()
                   .WriteTo.Console()
                      .WriteTo.File(Path.Combine("Logs", @$"{DateTime.Now.ToString("yyyy-MM-dd")}","log.txt"))
                      .CreateLogger();

                builder.AddSerilog();
            });

        }

    
    }
}
