using System.Collections.Concurrent;

namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface IRedisHash
    {
        Task<long> DeleteAsync(string key, params string[] fields);

        Task<bool> HashExistsAsync(string key, string field);

        Task<IDictionary<string,string>> HashGetAllAsync(string key);

        Task<IDictionary<string, T?>> HashGetAllAsync<T>(string key);

        Task<string> HashGetAsyncByFieldAsync(string key, string field);

        Task<T?> HashGetAsyncByFieldAsync<T>(string key, string field);

        Task<long> HashIncrementAsync(string key, string field, long increment);

        Task<double> HashIncrementAsync(string key, string field, double increment);


        Task<long> HashDecrementAsync(string key, string field, long decrement);

        Task<double> HashDecrementAsync(string key, string field, double decrement);

        Task<IList<string>> HashGetKeysAsync(string key);

        
        Task<long> HashLenAsync(string key);

        Task<IList<string>> HashGetAsync(string key, params string[] fields);

        Task<IList<T?>> HashGetAsync<T>(string key,params string[] fields);


        Task<bool> HashSetAsync(string key, string field, string value);

        Task<bool> HashSetAsync<T>(string key, string field, T value);

        Task<bool> HashSetNxAsync(string key, string field, string value);

        Task<bool> HashSetNxAsync<T>(string key, string field,T value);

        Task<bool> HashMSetAsync(string key, IDictionary<string, string> dictionarys);

        Task<bool> HashMSetAsync<T>(string key, IDictionary<string, T> dictionarys);

        Task<IList<string>> HashGetValueAsync(string key);

        Task<IList<T?>> HashGetValueAsync<T>(string key);


    }
}
