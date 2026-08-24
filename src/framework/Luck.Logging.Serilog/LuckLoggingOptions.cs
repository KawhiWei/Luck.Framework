using Microsoft.Extensions.Configuration;
using Serilog.Events;
using System.Reflection;

namespace Luck.Logging.Serilog;

/// <summary>File logging options shared by Luck hosts.</summary>
public sealed class LuckLoggingOptions
{
    public const string SectionName = "LuckLogging";
    public const string AppKeyEnvironmentVariableName = "AppKey";

    public string Module { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Information;
    public IReadOnlyDictionary<string, LogEventLevel> MinimumLevelOverrides { get; set; }
        = new Dictionary<string, LogEventLevel>(StringComparer.Ordinal);
    public long FileSizeLimitBytes { get; set; } = 100 * 1024 * 1024;
    public int RetainedFileCountLimit { get; set; } = 30;
    public bool RollOnFileSizeLimit { get; set; } = true;
    public bool Shared { get; set; } = true;
    public int FlushIntervalSeconds { get; set; } = 1;

    public static LuckLoggingOptions FromConfiguration(
        IConfiguration configuration,
        string contentRootPath)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        if (string.IsNullOrWhiteSpace(contentRootPath))
            throw new ArgumentException("A content root path is required.", nameof(contentRootPath));

        var section = configuration.GetSection(SectionName);
        var module = Environment.GetEnvironmentVariable(AppKeyEnvironmentVariableName);
        if (string.IsNullOrWhiteSpace(module))
            module = section["Module"];
        if (string.IsNullOrWhiteSpace(module))
            module = Assembly.GetEntryAssembly()?.GetName().Name;
        if (string.IsNullOrWhiteSpace(module))
            module = "unknown";

        var configuredFilePath = section["FilePath"];

        return new LuckLoggingOptions
        {
            Module = module,
            FilePath = string.IsNullOrWhiteSpace(configuredFilePath)
                ? GetDefaultFilePath(module, contentRootPath)
                : configuredFilePath,
            MinimumLevel = ParseLevel(section["MinimumLevel"]),
            MinimumLevelOverrides = ParseLevelOverrides(section.GetSection("MinimumLevelOverrides")),
            FileSizeLimitBytes = ParsePositiveLong(section["FileSizeLimitBytes"], 100 * 1024 * 1024),
            RetainedFileCountLimit = ParsePositiveInt(section["RetainedFileCountLimit"], 30),
            RollOnFileSizeLimit = ParseBool(section["RollOnFileSizeLimit"], true),
            Shared = ParseBool(section["Shared"], true),
            FlushIntervalSeconds = ParsePositiveInt(section["FlushIntervalSeconds"], 1),
        };
    }

    private static string GetDefaultFilePath(string module, string contentRootPath)
    {
        var fileName = $"{module}-.log";
        return Path.Combine(contentRootPath, "logs", fileName);
    }

    private static IReadOnlyDictionary<string, LogEventLevel> ParseLevelOverrides(IConfigurationSection section)
    {
        var overrides = new Dictionary<string, LogEventLevel>(StringComparer.Ordinal);
        foreach (var child in section.GetChildren())
        {
            LogEventLevel level;
            if (Enum.TryParse(child.Value, true, out level))
                overrides[child.Key] = level;
        }

        return overrides;
    }

    private static LogEventLevel ParseLevel(string? value)
    {
        LogEventLevel level;
        return Enum.TryParse(value, true, out level) ? level : LogEventLevel.Information;
    }

    private static bool ParseBool(string? value, bool defaultValue)
    {
        bool result;
        return bool.TryParse(value, out result) ? result : defaultValue;
    }

    private static int ParsePositiveInt(string? value, int defaultValue)
    {
        int result;
        return int.TryParse(value, out result) && result > 0 ? result : defaultValue;
    }

    private static long ParsePositiveLong(string? value, long defaultValue)
    {
        long result;
        return long.TryParse(value, out result) && result > 0 ? result : defaultValue;
    }
}
