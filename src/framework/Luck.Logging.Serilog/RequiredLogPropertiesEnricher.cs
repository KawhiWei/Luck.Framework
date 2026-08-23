using Serilog.Core;
using Serilog.Events;

namespace Luck.Logging.Serilog;

/// <summary>Ensures all fields required by the shared output template have values.</summary>
internal sealed class RequiredLogPropertiesEnricher : ILogEventEnricher
{
    private readonly string _module;

    public RequiredLogPropertiesEnricher(string module)
    {
        _module = module;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        AddIfMissing(logEvent, propertyFactory, LuckLogPropertyNames.Module, _module);
        AddIfMissing(logEvent, propertyFactory, LuckLogPropertyNames.Category, "-");
        AddSubcategoryIfMissing(logEvent, propertyFactory);
        AddIfMissing(logEvent, propertyFactory, LuckLogPropertyNames.TraceId, "-");
        AddIfMissing(logEvent, propertyFactory, LuckLogPropertyNames.Filter1, "-");
        AddIfMissing(logEvent, propertyFactory, LuckLogPropertyNames.Filter2, "-");
    }

    private static void AddSubcategoryIfMissing(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        LogEventPropertyValue? existing;
        if (logEvent.Properties.TryGetValue(LuckLogPropertyNames.Subcategory, out existing) && !IsEmpty(existing))
            return;

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(
            LuckLogPropertyNames.Subcategory,
            ExtractClassName(logEvent)));
    }

    private static string ExtractClassName(LogEvent logEvent)
    {
        LogEventPropertyValue? sourceContext;
        if (!logEvent.Properties.TryGetValue(LuckLogPropertyNames.SourceContext, out sourceContext)
            || !(sourceContext is ScalarValue scalar)
            || !(scalar.Value is string source)
            || string.IsNullOrWhiteSpace(source))
            return "-";

        var className = source.Split('.', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        return string.IsNullOrWhiteSpace(className) ? "-" : className;
    }

    private static void AddIfMissing(
        LogEvent logEvent,
        ILogEventPropertyFactory propertyFactory,
        string name,
        object value)
    {
        LogEventPropertyValue? existing;
        if (!logEvent.Properties.TryGetValue(name, out existing) || IsEmpty(existing))
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(name, value));
    }

    private static bool IsEmpty(LogEventPropertyValue value)
    {
        var scalar = value as ScalarValue;
        if (scalar != null)
            return scalar.Value == null || (scalar.Value is string text && string.IsNullOrWhiteSpace(text));

        var sequence = value as SequenceValue;
        if (sequence != null)
            return sequence.Elements.Count == 0;

        var dictionary = value as DictionaryValue;
        if (dictionary != null)
            return dictionary.Elements.Count == 0;

        var structure = value as StructureValue;
        return structure != null && structure.Properties.Count == 0;
    }
}
