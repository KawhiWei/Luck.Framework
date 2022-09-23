using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.Caching;
using StackExchange.Redis;
using IRedisHash = Luck.Framework.Infrastructure.Caching.Interface.IRedisHash;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisHash : IRedisHash
    {

        public async Task<long> DeleteAsync(string key, params string[] fields)
        {
            return await _database.HashDeleteAsync(key, fields.ToRedisValue());
        }
        public async Task<bool> HashExistsAsync(string key, string field)
        {
            return await _database.HashExistsAsync(key, field);
        }

        public async Task<IDictionary<string, string>> HashGetAllAsync(string key)
        {
            return (await _database.HashGetAllAsync(key)).ToDictionary();
        }

        public async Task<IDictionary<string, T?>> HashGetAllAsync<T>(string key)
        {
            return (await _database.HashGetAllAsync(key)).ToDictionary<T>();
        }

        public async Task<string> HashGetAsyncByFieldAsync(string key, string field)
        {
            return await _database.HashGetAsync(key, field);
        }

        public async Task<T?> HashGetAsyncByFieldAsync<T>(string key, string field)
        {
            return (await HashGetAsyncByFieldAsync(key, field)).Deserialize<T>();
        }

        public Task<long> HashIncrementAsync(string key, string field, long increment) => _database.HashIncrementAsync(key, field, increment);

        public Task<double> HashIncrementAsync(string key, string field, double increment) => _database.HashIncrementAsync(key, field, increment);

        public Task<long> HashDecrementAsync(string key, string field, long decrement) => _database.HashDecrementAsync(key, field, decrement);

        public Task<double> HashDecrementAsync(string key, string field, double decrement) => _database.HashDecrementAsync(key, field, decrement);

        public async Task<IList<string>> HashGetKeysAsync(string key) => (await _database.HashKeysAsync(key)).Select(x => x.ToString()).ToList();

        public async Task<IList<string>> HashGetAsync(string key, params string[] fields) => (await _database.HashGetAsync(key, fields.ToRedisValue())).Select(x => x.ToString()).ToList();

        public async Task<IList<T?>> HashGetAsync<T>(string key, params string[] fields) => (await HashGetAsync(key, fields)).Select(x => x.Deserialize<T>()).ToList();

        public async Task<IList<string>> HashGetValueAsync(string key) => (await _database.HashValuesAsync(key)).Select(x => x.ToString()).ToList();

        public async Task<IList<T?>> HashGetValueAsync<T>(string key) => (await HashGetValueAsync(key)).Select(x => x.Deserialize<T>()).ToList();

        public async Task<long> HashLenAsync(string key) => (long)await _database.ExecuteAsync("hstrlen", key);





        public async Task<bool> HashSetAsync(string key, string field, string value)
        {
            await _database.HashSetAsync(key, field, value);
            return true;
        }

        public async Task<bool> HashSetAsync<T>(string key, string field, T value)
        {
            await _database.HashSetAsync(key, field, value.Serialize());
            return true;
        }

        public async Task<bool> HashMSetAsync(string key, IDictionary<string, string> dictionarys)
        {
            await _database.HashSetAsync(key, dictionarys.DictionaryToHashEntry());
            return true;
        }

        public async Task<bool> HashMSetAsync<T>(string key, IDictionary<string, T> dictionarys)
        {
            await _database.HashSetAsync(key, dictionarys.DictionaryToHashEntry());
            return true;
        }

        public  Task<bool> HashSetNxAsync<T>(string key, string field, T value)=> _database.HashSetAsync(key, field, value.Serialize(),When.NotExists);
         
        public Task<bool> HashSetNxAsync(string key, string field, string value)=> _database.HashSetAsync(key, field, value, When.NotExists);
    }
}
