namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface ICache
    {

        Task<bool> SetStringAsync(string key, string  value, TimeSpan? expiration=null);

        Task<string> GetStringAsync(string key);

        Task<long> GetKeyLengAsync(string key);

        Task<string> SetStringRangeAsync(string key, long offest, string value);

        Task<string> StringGetRangeAsync(string key, long start, long end);

    }
}
