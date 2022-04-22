namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface ICache
    {
        void Add<T>(string key, T value, TimeSpan? expiration = null);

        bool TryAdd<T>(string key, T value, TimeSpan? expiration = null);

        bool Exist(string key);

        void Remove(string key);

        T Get<T>(string key);

        T GetOrAdd<T>(string key, TimeSpan? expiration = null);

        T GetOrUpdate<T>(string key, Func<Task<T>> func, TimeSpan? expiration = null);

        IEnumerable<string> GetKeys();

        void ClearAllKeys();

        void RemoveByPrefix(string prefix);
    }

}
