using System.Collections.Concurrent;

namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface ICache
    {
        ValueTask AddAsync<T>(string key, T value, TimeSpan? expiration = null);

        ValueTask TryAddAsync<T>(string key, T value, TimeSpan? expiration = null);

        ValueTask<bool> ExistAsync(string key);

        Task<string> RemoveAsync(string key);

        Task<T> GetAsync<T>(string key);

        Task<T> GetOrAddAsync<T>(string key, TimeSpan? expiration = null);

        Task<T> GetOrUpdateAsync<T>(string key, Func<Task<T>> func, TimeSpan? expiration = null);

        Task<IEnumerable<string>> GetKeysAsync();

        Task ClearAllKeysAsync();

        Task RemoveByPrefixAsync(string prefix);
    }
}
