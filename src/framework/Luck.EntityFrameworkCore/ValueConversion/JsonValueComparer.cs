using Luck.Framework.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Luck.EntityFrameworkCore.ValueConversion;

public class JsonValueComparer<T> : ValueComparer<T>
{
    public JsonValueComparer() : base((t1, t2) => DoEquals(t1, t2), t => DoGetHashCode(t), t => DoGetSnapshot(t))
    {
    }

    private static T DoGetSnapshot(T instance)
    {
        if (instance is ICloneable cloneable)
            return (T)cloneable.Clone();
        return instance.Serialize().Deserialize<T>();
    }

    private static int DoGetHashCode(T instance)
    {
        if (instance is IEquatable<T>)
            return instance.GetHashCode();

        return instance.Serialize().GetHashCode();
    }

    private static bool DoEquals(T? left, T? right)
    {
        if (left is IEquatable<T> equatable)
            return equatable.Equals(right);

        var result = left.Serialize().Equals(right.Serialize());
        return result;
    }
}