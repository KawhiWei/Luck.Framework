using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Luck.Logging.Serilog;

/// <summary>Configures shared Serilog sinks and HTTP request enrichment for Luck hosts.</summary>
public static class LoggingExtensions
{
    /// <summary>Configures bootstrap and host loggers for a web application.</summary>
    public static WebApplicationBuilder UseLuckSerilog(this WebApplicationBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        var bootstrapOptions = LuckLoggingOptions.FromConfiguration(
            builder.Configuration,
            builder.Environment.ContentRootPath);
        Log.Logger = CreateLogger(bootstrapOptions, builder.Environment.ContentRootPath);
        builder.Host.UseLuckSerilog();
        return builder;
    }

    /// <summary>Configures the Serilog logger used when an <see cref="IHostBuilder"/> builds its host.</summary>
    public static IHostBuilder UseLuckSerilog(this IHostBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        builder.UseSerilog((context, _, loggerConfiguration) =>
        {
            var options = LuckLoggingOptions.FromConfiguration(
                context.Configuration,
                context.HostingEnvironment.ContentRootPath);
            ConfigureLogger(loggerConfiguration, options, context.HostingEnvironment.ContentRootPath);
        });

        return builder;
    }

    /// <summary>Adds request logging and per-request structured fields.</summary>
    public static IApplicationBuilder UseLuckRequestLogging(this IApplicationBuilder app)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        app.Use(async (context, next) =>
        {
            using (LogContext.PushProperty(LuckLogPropertyNames.TraceId, GetTraceId(context)))
            using (LogContext.PushProperty(LuckLogPropertyNames.Filter1, context.Request.Method))
                await next();
        });

        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = LuckLogTemplates.HttpRequestCompleted;
            options.GetLevel = (context, _, exception) => GetRequestLogLevel(context, exception);
            options.EnrichDiagnosticContext = (diagnosticContext, context) =>
            {
                diagnosticContext.Set(LuckLogPropertyNames.TraceId, GetTraceId(context));
                diagnosticContext.Set(LuckLogPropertyNames.Category, "HTTP");
                diagnosticContext.Set(LuckLogPropertyNames.Subcategory, GetPath(context));
                diagnosticContext.Set(LuckLogPropertyNames.Filter1, context.Request.Method);
                diagnosticContext.Set(LuckLogPropertyNames.Filter2,
                    context.Response.StatusCode.ToString(CultureInfo.InvariantCulture));
            };
        });

        return app;
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

    private static LogEventLevel GetRequestLogLevel(HttpContext context, Exception? exception)
    {
        if (exception != null || context.Response.StatusCode >= StatusCodes.Status500InternalServerError)
            return LogEventLevel.Error;
        return context.Response.StatusCode >= StatusCodes.Status400BadRequest
            ? LogEventLevel.Warning
            : LogEventLevel.Information;
    }

    private static string GetTraceId(HttpContext context)
    {
        var traceId = Activity.Current == null ? null : Activity.Current.TraceId.ToString();
        return string.IsNullOrWhiteSpace(traceId) || traceId.All(character => character == '0')
            ? context.TraceIdentifier
            : traceId;
    }

    private static string GetPath(HttpContext context)
    {
        return context.Request.Path.HasValue ? context.Request.Path.Value! : "/";
    }
}
