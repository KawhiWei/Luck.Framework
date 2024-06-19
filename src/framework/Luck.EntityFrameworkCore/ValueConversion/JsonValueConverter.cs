using Luck.Framework.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Luck.EntityFrameworkCore.ValueConversion;

public class JsonValueConverter<T>(ConverterMappingHints? hints = default)
    : ValueConverter<T, string>(v => v.Serialize(), v => v.Deserialize<T>()!, hints)
    where T : class;
    
    