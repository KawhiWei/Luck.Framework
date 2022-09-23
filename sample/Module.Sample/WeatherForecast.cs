using System.Reflection;
using System.Runtime.InteropServices;

namespace Module.Sample
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        public static string GetLogFilePath()
        {
            var path = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var appUK = Environment.GetEnvironmentVariable("DAOKEAPPUK");
                var deployNo = string.Concat(Environment.GetEnvironmentVariable("DAOKEID"), Environment.GetEnvironmentVariable("MESSOS_TASK_ID"));

                path = $"/data/logs/skynet-{appUK}-{deployNo}/app/app.log";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly is not null)
                {
                    var codeBase = assembly.CodeBase;
                    if (codeBase is not null)
                    {
                        var localPath = new Uri(codeBase).LocalPath;
                        var logPath = Path.GetDirectoryName(localPath);

                        if (logPath is not null)
                        {
                            path = Path.Combine(logPath, "logs", $"{assembly.GetName().Name}.log");
                        }
                    }
                    
                }
            }

            return path;
        }
    }
}