namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface ICache
    {

        Task<bool> SetStringAsync(string key, string cacheValue, TimeSpan? expiration);

        Task<string> GetStringAsync(string key);

        Task<long> GetKeyLengAsync(string key);

        Task<long> SetStringRangeAsync(string key, long offest, string value);

        Task<string> StringGetRangeAsync(string key, long start, long end);

    }
}
