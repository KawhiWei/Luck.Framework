using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Luck.Logging.Serilog;

/// <summary>Provides structured logging scopes for the Luck output template.</summary>
public static class LuckLoggerExtensions
{
    /// <summary>Writes an information event with the calling method as its subcategory.</summary>
    public static void LogLuckInformation(
        this ILogger logger,
        string message,
        object?[]? values = null,
        [CallerMemberName] string? methodName = null)
    {
        LogWithMethodScope(logger, LogLevel.Information, null, message, values, methodName);
    }

    /// <summary>Writes a warning event with the calling method as its subcategory.</summary>
    public static void LogLuckWarning(
        this ILogger logger,
        string message,
        object?[]? values = null,
        [CallerMemberName] string? methodName = null)
    {
        LogWithMethodScope(logger, LogLevel.Warning, null, message, values, methodName);
    }

    /// <summary>Writes an error event with the calling method as its subcategory.</summary>
    public static void LogLuckError(
        this ILogger logger,
        Exception exception,
        string message,
        object?[]? values = null,
        [CallerMemberName] string? methodName = null)
    {
        LogWithMethodScope(logger, LogLevel.Error, exception, message, values, methodName);
    }

    /// <summary>
    /// Adds the calling method name as the subcategory for the returned scope.
    /// </summary>
    public static IDisposable BeginLuckMethodScope(
        this ILogger logger,
        [CallerMemberName] string? methodName = null)
    {
        return logger.BeginLuckLogScope(subcategory: methodName);
    }

    /// <summary>
    /// Adds the supplied Luck log fields to all events written within the returned scope.
    /// </summary>
    /// <param name="logger">The logger that writes the scoped events.</param>
    /// <param name="module">An optional module name.</param>
    /// <param name="category">An optional log category.</param>
    /// <param name="subcategory">An optional log subcategory.</param>
    /// <param name="requestTraceId">An optional request or business trace identifier.</param>
    /// <param name="filter1">An optional first business filter.</param>
    /// <param name="filter2">An optional second business filter.</param>
    /// <returns>A scope that must be disposed after the related log events are written.</returns>
    public static IDisposable BeginLuckLogScope(
        this ILogger logger,
        string? module = null,
        string? category = null,
        string? subcategory = null,
        string? requestTraceId = null,
        string? filter1 = null,
        string? filter2 = null)
    {
        ArgumentNullException.ThrowIfNull(logger);

        var properties = new Dictionary<string, object?>();
        AddIfNotEmpty(properties, LuckLogPropertyNames.Module, module);
        AddIfNotEmpty(properties, LuckLogPropertyNames.Category, category);
        AddIfNotEmpty(properties, LuckLogPropertyNames.Subcategory, subcategory);
        AddIfNotEmpty(properties, LuckLogPropertyNames.TraceId, requestTraceId);
        AddIfNotEmpty(properties, LuckLogPropertyNames.Filter1, filter1);
        AddIfNotEmpty(properties, LuckLogPropertyNames.Filter2, filter2);

        return logger.BeginScope(properties)!;
    }

    private static void AddIfNotEmpty(Dictionary<string, object?> properties, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            properties.Add(name, value);
    }

    private static void LogWithMethodScope(
        ILogger logger,
        LogLevel level,
        Exception? exception,
        string message,
        object?[]? values,
        string? methodName)
    {
        ArgumentNullException.ThrowIfNull(logger);

        using var scope = logger.BeginLuckMethodScope(methodName);
        logger.Log(level, exception, message, values ?? []);
    }
}
