namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface IRedisList
    {
        Task LTrimAsync(string redisKey);

        Task<long> LRemoveAsync(string key, string value, long count = 0);

        Task<long> LRemoveAsync<T>(string key, T value, long count = 0);





        Task<IList<string>> GetRangeAsync(string redisKey, long start, long end);

        Task<IList<T>> GetRangeAsync<T>(string redisKey, long start, long end);

        Task<T> GetByIndexAsync<T>(string key, long index);

        Task<string> GetByIndexAsync(string key, long index);

        Task<long> GetLenAsync(string key);

        Task<bool> SetByIndexAsync(string key, long index, string value);

        Task<bool> SetByIndexAsync<T>(string key, long index, T value);





        Task<long> LPushAsync(string redisKey, params string[] values);

        Task<long> LPushAsync<T>(string redisKey, params T[] values);

        Task<long> LPushExistsAsync(string key, string value);

        Task<long> LPushExistsAsync<T>(string key, T value);

        Task<string> LPopAsync(string key);

        Task<T> LPopAsync<T>(string key);





        Task<long> RPushAsync(string redisKey, params string[] values);

        Task<long> RPushAsync<T>(string redisKey, params T[] values);

        Task<long> RPushExistsAsync(string key, string value);

        Task<long> RPushExistsAsync<T>(string key, T value);

        Task<string> RPopAsync(string key);

        Task<T> RPopAsync<T>(string key);




    }
}
