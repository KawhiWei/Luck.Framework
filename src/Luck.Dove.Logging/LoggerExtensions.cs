using System.Runtime.CompilerServices;
using Luck.Dove.Logging;
using Luck.Framework.Extensions;

namespace Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    #region Debug

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogDebug(this ILogger logger, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Debug, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogDebug(this ILogger logger, EventId eventId, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Debug, eventId, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogDebug(this ILogger logger, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Debug, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogDebug(this ILogger logger, EventId eventId, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Debug, eventId, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    #endregion

    #region Trace

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogTrace(this ILogger logger, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Trace, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogTrace(this ILogger logger, EventId eventId, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Trace, eventId, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogTrace(this ILogger logger, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Trace, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogTrace(this ILogger logger, EventId eventId, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Trace, eventId, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    #endregion

    #region Information

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogInformation(this ILogger logger, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Information, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogInformation(this ILogger logger, EventId eventId, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Information, eventId, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogInformation(this ILogger logger, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Information, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogInformation(this ILogger logger, EventId eventId, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Information, eventId, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    #endregion

    #region Warning

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogWarning(this ILogger logger, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Warning, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogWarning(this ILogger logger, EventId eventId, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Warning, eventId, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogWarning(this ILogger logger, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Warning, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogWarning(this ILogger logger, EventId eventId, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Warning, eventId, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    #endregion

    #region Error

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogError(this ILogger logger, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Error, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogError(this ILogger logger, EventId eventId, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Error, eventId, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogError(this ILogger logger, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Error, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogError(this ILogger logger, EventId eventId, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Error, eventId, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    #endregion

    #region Critical

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogCritical(this ILogger logger, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Critical, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogCritical(this ILogger logger, EventId eventId, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Critical, eventId, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogCritical(this ILogger logger, Exception? exception, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Critical, exception, $"{GetMessage(method, exception, message ?? "", businessFilter)}", args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventId"></param>
    /// <param name="message"></param>
    /// <param name="businessFilter"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    public static void DoveLogCritical(this ILogger logger, EventId eventId, string? message, string businessFilter = "", [CallerMemberName] string method = "", params object?[] args)
    {
        logger.Log(LogLevel.Critical, eventId, $"{GetMessage(method, null, message ?? "", businessFilter)}", args);
    }

    private static string GetMessage(string method, Exception? exception, string message = "", string businessFilter = "")
    {
        return new DoveLoggerModule()
        {
            Body = message,
            BusinessFilter = businessFilter,
            Exception = exception,
            Method = method,
        }.Serialize();
    }

    #endregion
}