using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace Luck.Logging.Serilog;

/// <summary>Configures shared Serilog sinks for Luck hosts.</summary>
public static class LoggingExtensions
{
    /// <summary>Registers bootstrap and host loggers for a web application.</summary>
    public static WebApplicationBuilder AddLuckSerilog(this WebApplicationBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        builder.Services.AddLuckSerilog(builder.Configuration, builder.Environment);
        return builder;
    }

    /// <summary>Registers Serilog with the application's service collection.</summary>
    public static IServiceCollection AddLuckSerilog(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        var options = LuckLoggingOptions.FromConfiguration(configuration, environment.ContentRootPath);
        Log.Logger = CreateLogger(options, environment.ContentRootPath);
        services.AddSerilog((_, loggerConfiguration) =>
        {
            ConfigureLogger(loggerConfiguration, options, environment.ContentRootPath);
        });

        return services;
    }

    /// <summary>Records an unrecoverable exception during host startup or execution.</summary>
    public static void LogStartupFailure(Exception exception)
    {
        if (exception == null)
            throw new ArgumentNullException(nameof(exception));
        Log.Fatal(exception, "Host terminated unexpectedly during startup or execution.");
    }

    /// <summary>Flushes buffered events and releases Serilog sinks.</summary>
    public static void CloseAndFlush()
    {
        Log.CloseAndFlush();
    }

    private static Logger CreateLogger(LuckLoggingOptions options, string contentRootPath)
    {
        var configuration = new LoggerConfiguration();
        ConfigureLogger(configuration, options, contentRootPath);
        return configuration.CreateLogger();
    }

    private static void ConfigureLogger(
        LoggerConfiguration loggerConfiguration,
        LuckLoggingOptions options,
        string contentRootPath)
    {
        loggerConfiguration
            .MinimumLevel.Is(options.MinimumLevel)
            .Enrich.FromLogContext()
            .Enrich.With(new RequiredLogPropertiesEnricher(options.Module))
            .WriteTo.Console(outputTemplate: LuckLogTemplates.Output)
            .WriteTo.File(
                ResolveFilePath(options.FilePath, contentRootPath),
                outputTemplate: LuckLogTemplates.Output,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: options.RollOnFileSizeLimit,
                fileSizeLimitBytes: options.FileSizeLimitBytes,
                retainedFileCountLimit: options.RetainedFileCountLimit,
                shared: options.Shared,
                flushToDiskInterval: TimeSpan.FromSeconds(options.FlushIntervalSeconds));

        foreach (var item in options.MinimumLevelOverrides)
            loggerConfiguration.MinimumLevel.Override(item.Key, item.Value);
    }

    private static string ResolveFilePath(string filePath, string contentRootPath)
    {
        var resolvedPath = Path.IsPathRooted(filePath) ? filePath : Path.Combine(contentRootPath, filePath);
        var directory = Path.GetDirectoryName(resolvedPath);
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);
        return resolvedPath;
    }

}
