namespace Luck.Logging.Serilog;

/// <summary>Canonical Serilog templates shared by Luck hosts.</summary>
public static class LuckLogTemplates
{
    /// <summary>Renders a fixed set of operational fields for each event.</summary>
    public const string Output =
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}][{Level:u3}][{Module}][{Category}][{Subcategory}][{RequestTraceId}][{Filter1}][{Filter2}][{Message:lj}{Exception}]\n";

    /// <summary>The message emitted when an HTTP request completes.</summary>
    public const string HttpRequestCompleted =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
}
