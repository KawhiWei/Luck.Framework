using Microsoft.AspNetCore.Builder;

namespace Luck.Logging.Serilog;

/// <summary>Configures HTTP request logging context for Luck Serilog hosts.</summary>
public static class LuckRequestLogContextExtensions
{
    /// <summary>
    /// Adds a request scope containing the controller, action, trace identifier, and request filters.
    /// </summary>
    /// <remarks>
    /// Place this before middleware and endpoint handlers whose logs should carry the request fields.
    /// </remarks>
    public static IApplicationBuilder UseLuckRequestLogContext(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UseMiddleware<LuckRequestLogContextMiddleware>();
    }
}
