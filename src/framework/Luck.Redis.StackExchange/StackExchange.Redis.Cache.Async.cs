using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisCache : ICache
    {
        public async ValueTask<bool> AddAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            return await database.SetAddAsync(key, value.Serialize());
        }

        public Task<bool> ClearAllKeysAsync()
        {
            throw new NotImplementedException();
        }

        public async ValueTask<bool> ExistAsync(string key)
        {
            return await database.KeyExistsAsync(key);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var str = await database.StringGetAsync(key);
            if (str.HasValue)
            {
                return str.ToString().Deserialize<T>(); ;
            }
            return default(T);
        }

        public Task<IEnumerable<string>> GetKeysAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOrAddAsync<T>(string key, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOrUpdateAsync<T>(string key, Func<Task<T>> func, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByPrefixAsync(string prefix)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> TryAddAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }
    }
}
