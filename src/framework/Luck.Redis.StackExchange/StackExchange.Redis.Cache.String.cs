using Luck.Framework.Infrastructure.Caching;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisCache : ICache
    {

        public bool SetString(string key, string cacheValue, TimeSpan? expiration) => database.StringSet(key, cacheValue, expiration);

        public string GetString(string key) => GetString(key);

        public long GetKeyLeng(string key) => database.StringLength(key);

        public string SetStringRange(string key, long offest, string value) => database.StringSetRange(key, offset: offest, value: value);

        public string StringGetRange(string key, long start, long end) => database.StringGetRange(key, start, end);
    }
}
