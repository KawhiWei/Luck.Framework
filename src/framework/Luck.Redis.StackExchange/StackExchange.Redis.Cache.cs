using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.Caching;
using StackExchange.Redis;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisCache : ICache
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        public IDatabase database { get; }
        public StackExchangeRedisCache(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            database= _connectionMultiplexer.GetDatabase();
        }

        public void Add<T>(string key, T value, TimeSpan? expiration = null)
        {


            database.SetAdd(key, value.Serialize());
        }

        public void ClearAllKeys()
        {
            throw new NotImplementedException();
        }

        public bool Exist(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetKeys()
        {
            throw new NotImplementedException();
        }

        public T GetOrAdd<T>(string key, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public T GetOrUpdate<T>(string key, Func<Task<T>> func, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveByPrefix(string prefix)
        {
            throw new NotImplementedException();
        }

        public bool TryAdd<T>(string key, T value, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public bool SetString(string key, string cacheValue, TimeSpan? expiration)
        {
            throw new NotImplementedException();
        }

        public string GetString(string key)
        {
            throw new NotImplementedException();
        }

        public long GetKeyLeng(string key)
        {
            throw new NotImplementedException();
        }

        public long SetStringRange(string key, long offest, string value)
        {
            throw new NotImplementedException();
        }

        public string StringGetRange(string key, long start, long end)
        {
            throw new NotImplementedException();
        }


    }
}