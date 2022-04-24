using System.Collections.Concurrent;

namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface ICache
    {
        ValueTask<bool> AddAsync<T>(string key, T value, TimeSpan? expiration = null);

        ValueTask<bool> TryAddAsync<T>(string key, T value, TimeSpan? expiration = null);

        ValueTask<bool> ExistAsync(string key);

        Task<bool> RemoveAsync(string key);

        Task<T?> GetAsync<T>(string key);

        Task<T> GetOrAddAsync<T>(string key, T value, TimeSpan? expiration = null);

        Task<T> GetOrUpdateAsync<T>(string key, Func<Task<T>> func, TimeSpan? expiration = null);

        IAsyncEnumerable<string> GetKeysAsync();

        Task<bool> ClearAllKeysAsync();

        Task RemoveByPrefixAsync(string prefix);
    }
}
