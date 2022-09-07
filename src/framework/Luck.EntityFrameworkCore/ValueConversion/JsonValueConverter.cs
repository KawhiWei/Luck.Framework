using Luck.Framework.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Luck.EntityFrameworkCore.ValueConversion;

public class JsonValueConverter<T> : ValueConverter<T, string> where T : class
{
    public JsonValueConverter(ConverterMappingHints? hints = default) :
        base(v => v.Serialize(), v => v.Deserialize<T>(), hints)
    { }
}