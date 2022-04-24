using Luck.Framework.Infrastructure.Caching;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisCache : ICache
    {

        public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiration = null) => await SetStringAsync(key, value, expiration);

        public async Task<string> GetStringAsync(string key) => await GetStringAsync(key);

        public async Task<long> GetKeyLengAsync(string key) => await database.StringLengthAsync(key);

        public async Task<string> SetStringRangeAsync(string key, long offest, string value) => await database.StringSetRangeAsync(key, offest, value);

        public async Task<string> StringGetRangeAsync(string key, long start, long end) => await database.StringGetRangeAsync(key, start, end);


    }
}
