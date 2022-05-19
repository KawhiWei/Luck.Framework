using Luck.Framework.Extensions;
using StackExchange.Redis;

namespace Luck.Redis.StackExchange
{
    public static class RedisValueExtension
    {
        internal static RedisValue[]? ToRedisValue(this string[]? values)
        {
            if (values is null)
                return null;
            var result = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
                result[i] = values[i];
            return result;
        }
        internal static RedisValue[]? ToRedisValue<T>(this T[]? values)
        {
            if (values is  null)
                return null;
            var result = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
                result[i] = RedisValue.Unbox(values[i]);
            return result;
        }

        internal static IDictionary<string, string> ToDictionary(this HashEntry[] values)
        {

            var dic = new Dictionary<string, string>(values.Length);
            foreach (var item in values)
            {
                dic.Add(item.Name, item.Value);
            }

            return dic;
        }

        internal static IDictionary<string, T?> ToDictionary<T>(this HashEntry[] values)
        {

            var dic = new Dictionary<string, T?>(values.Length);
            foreach (var item in values)
            {
                dic.Add(item.Name, item.Value.ToString().Deserialize<T>());
            }
            return dic;
        }

        internal static HashEntry[] DictionaryToHashEntry(this IDictionary<string, string> dictionarys)
        {
            var hashEntries = new HashEntry[dictionarys.Count];
            var index = 0;
            foreach (var item in dictionarys)
            {
                hashEntries[index] = new HashEntry(item.Key, item.Value);
                index++;
            }
            return hashEntries;
        }
        internal static HashEntry[] DictionaryToHashEntry<T>(this IDictionary<string, T> dictionarys)
        {
            var hashEntries = new HashEntry[dictionarys.Count];
            var index = 0;
            foreach (var item in dictionarys)
            {
                hashEntries[index] = new HashEntry(item.Key, item.Serialize());
                index++;
            }
            return hashEntries;
        }
    }
}
