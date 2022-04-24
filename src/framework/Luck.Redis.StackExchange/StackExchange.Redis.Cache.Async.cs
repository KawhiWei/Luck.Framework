using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.Caching;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisCache : ICache
    {
        public async ValueTask<bool> AddAsync<T>(string key, T value, TimeSpan? expiration = null) => await SetStringAsync(key, value.Serialize(), expiration);


        public async Task<T?> GetOrAddAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var exist = await ExistAsync(key);
            if (exist)
            {
                var str = await GetStringAsync(key);
                if (!string.IsNullOrEmpty(str))
                {
                    return str.Deserialize<T>();
                }
            }
            else
            {
                await SetStringAsync(key,value.Serialize(),expiration);
                return value;
            }
            return value;

        }

        public async ValueTask<bool> TryAddAsync<T>(string key, T value, TimeSpan? expiration = null) => await SetStringAsync(key, value.Serialize(), expiration);

        public async ValueTask<bool> ExistAsync(string key) => await ExistAsync(key);

        public async Task<T?> GetAsync<T>(string key)
        {
            var str = await GetStringAsync(key);
            if (!string.IsNullOrEmpty(str))
            {
                return str.Deserialize<T>(); ;
            }
            return default(T);
        }

        public async IAsyncEnumerable<string> GetKeysAsync()
        {
            var endpoint = _connectionMultiplexer.GetEndPoints().First();
            await foreach (var key in _connectionMultiplexer.GetServer(endpoint).KeysAsync())
            {
                yield return key;
            }
        }

        public async Task<bool> RemoveAsync(string key)=>await database.KeyDeleteAsync(key);






        public Task<T> GetOrUpdateAsync<T>(string key, Func<Task<T>> func, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByPrefixAsync(string prefix)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearAllKeysAsync()
        {
            var endpoint = _connectionMultiplexer.GetEndPoints().First();

            var keys = _connectionMultiplexer.GetServer(endpoint).KeysAsync();


            //await foreach (var key in keys)
            //{
            //     await database.KeyDeleteAsync(key);
            //    yield return true;
            //}
            throw new NotImplementedException();
        }
    }
}
