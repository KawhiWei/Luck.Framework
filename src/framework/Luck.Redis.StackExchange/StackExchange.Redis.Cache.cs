using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.Caching;
using StackExchange.Redis;
using System.Net;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisCache : ICache
    {

        private readonly ConnectionMultiplexer _connectionMultiplexer;

        private readonly EndPoint _endPoint;

        public IDatabase database { get; }

        public StackExchangeRedisCache(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            database = _connectionMultiplexer.GetDatabase();
            _endPoint = _connectionMultiplexer.GetEndPoints().First();
        }

        public void Add<T>(string key, T value, TimeSpan? expiration = null) => SetString(key, value.Serialize(), expiration);
        
        public bool Exist(string key) => database.KeyExists(key);

        public T? Get<T>(string key)
        {
            var str = GetString(key);
            if (!string.IsNullOrEmpty(str))
            {
                return str.Deserialize<T>(); ;
            }
            return default(T);
        }

        public IEnumerable<string> GetKeys() => _connectionMultiplexer.GetServer(_endPoint).Keys().Select(x => x.ToString());

        public bool TryAdd<T>(string key, T value, TimeSpan? expiration = null) => SetString(key, value.Serialize(), expiration);

        public T? GetOrAdd<T>(string key, T value, TimeSpan? expiration = null)
        {
            var exist = Exist(key);
            if (exist)
            {
                var str = GetString(key);
                if (!string.IsNullOrEmpty(str))
                {
                    return str.Deserialize<T>();
                }
            }
            else
            {
                SetString(key, value.Serialize(), expiration);
            }
            return value;
        }

        public void Remove(string key) => database.KeyDelete(key);

        public void ClearAllKeys()
        {
            var keys = _connectionMultiplexer.GetServer(_endPoint).Keys();

            foreach (var key in keys)
            {
                database.KeyDeleteAsync(key);
            }
        }


        public T GetOrUpdate<T>(string key, Func<T> func, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveByPrefix(string prefix)
        {
            throw new NotImplementedException();
        }




    }
}