using Luck.EntityFrameworkCore.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luck.EntityFrameworkCore.Extensions;

public static class EfCoreConversionExtensions
{
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class
    {
        propertyBuilder.HasConversion(new JsonValueConverter<T>())
            .Metadata.SetValueComparer(new JsonValueComparer<T>());

        return propertyBuilder;
    }
}