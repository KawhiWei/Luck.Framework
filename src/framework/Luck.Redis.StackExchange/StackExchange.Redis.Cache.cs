using Luck.Framework.Infrastructure.Caching;

namespace Luck.Redis.StackExchange
{
    public partial class RedisCache : ICache
    {

        public void Add<T>(string key, T value, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
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

        
    }
}