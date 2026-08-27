using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;

namespace Luck.Logging.Serilog;

/// <summary>Applies endpoint-specific structured fields to all logs for an HTTP request.</summary>
public sealed class LuckRequestLogContextMiddleware
{
    private const string HttpCategory = "HTTP";
    private readonly RequestDelegate _next;
    private readonly ILogger<LuckRequestLogContextMiddleware> _logger;

    public LuckRequestLogContextMiddleware(
        RequestDelegate next,
        ILogger<LuckRequestLogContextMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(logger);

        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var stopwatch = Stopwatch.StartNew();
        var (module, category) = ResolveEndpointContext(context);
        using var requestScope = _logger.BeginLuckLogScope(
            module: module,
            category: category,
            requestTraceId: ResolveTraceId(context),
            filter1: Guid.NewGuid().ToString("N"),
            filter2: ResolveUserId(context.User) ?? Guid.NewGuid().ToString("N"));

        try
        {
            await _next(context);
            stopwatch.Stop();
            _logger.LogInformation(
                "Request completed. StatusCode={StatusCode} ElapsedMs={ElapsedMs} Method={Method} Path={Path}",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                context.Request.Method,
                context.Request.Path);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();
            _logger.LogError(
                exception,
                "Request failed. StatusCode={StatusCode} ElapsedMs={ElapsedMs} Method={Method} Path={Path}",
                context.Response.StatusCode >= StatusCodes.Status400BadRequest
                    ? context.Response.StatusCode
                    : StatusCodes.Status500InternalServerError,
                stopwatch.ElapsedMilliseconds,
                context.Request.Method,
                context.Request.Path);
            throw;
        }
    }

    private static (string? Module, string? Category) ResolveEndpointContext(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var action = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (action is not null)
        {
            return (action.ControllerName, action.ActionName);
        }

        return (HttpCategory, endpoint?.DisplayName);
    }

    private static string? ResolveTraceId(HttpContext context)
    {
        var activity = Activity.Current;
        if (activity is not null && activity.TraceId != default)
            return activity.TraceId.ToString();

        return context.TraceIdentifier;
    }

    private static string? ResolveUserId(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");

        return string.IsNullOrWhiteSpace(userId) ? null : userId;
    }
}
