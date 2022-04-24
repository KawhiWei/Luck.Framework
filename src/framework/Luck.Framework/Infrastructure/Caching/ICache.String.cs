namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface ICache
    {
        bool SetString(string key, string cacheValue, TimeSpan? expiration);

        string GetString(string key);

        long GetKeyLeng(string key);

        string SetStringRange(string key, long offest, string value);

        string StringGetRange(string key, long start, long end);
    }
}
