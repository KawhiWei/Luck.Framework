using Luck.Framework.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Redis.StackExchange
{
    public partial class RedisCache : ICache
    {
        public ValueTask AddAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }


        public Task ClearAllKeysAsync()
        {
            throw new NotImplementedException();
        }



        public ValueTask<bool> ExistAsync(string key)
        {
            throw new NotImplementedException();
        }


        public Task<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
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



        public ValueTask TryAddAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }
    }
}
