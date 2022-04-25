using StackExchange.Redis;

namespace Luck.Redis.StackExchange
{
    public static class RedisValueExtension
    {
        internal static RedisValue[] ToRedisValue(this string[] values)
        {
            if (values == null)
                return null;
            var result = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
                result[i] = values[i];
            return result;
        }
        internal static RedisValue[] ToRedisValue<T>(this T[] values)
        {
            if (values == null)
                return null;
            var result = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
                result[i] = RedisValue.Unbox(values[i]);
            return result;
        }
    }
}
