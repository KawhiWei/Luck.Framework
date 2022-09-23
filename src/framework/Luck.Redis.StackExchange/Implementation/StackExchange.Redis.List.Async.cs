using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.Caching;
using StackExchange.Redis;
using IRedisList = Luck.Framework.Infrastructure.Caching.Interface.IRedisList;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisList : IRedisList
    {

        public async Task<T?> GetByIndexAsync<T>(string key, long index) => (await Database.ListGetByIndexAsync(key, index)).ToString().Deserialize<T>();


        public async Task SetByIndexAsync(string key, long index, string value) => await Database.ListSetByIndexAsync(key, index, value);

        public async Task SetByIndexAsync<T>(string key, long index, T value) => await SetByIndexAsync(key, index, value);

        public async Task<string> GetByIndexAsync(string key, long index) => (await Database.ListGetByIndexAsync(key, index)).ToString();


        public async Task<long> GetLenAsync(string key) => await Database.ListLengthAsync(key);


        public async Task<IList<string>> GetRangeAsync(string key, long start, long end) => (await Database.ListRangeAsync(key, start, end)).Select(x => x.ToString()).ToList();


        public async Task<IList<T?>> GetRangeAsync<T>(string key, long start, long end) => (await GetRangeAsync(key, start, end)).Select(x => x.Deserialize<T>()).ToList();


        public async Task LTrimAsync(string key, long start, long end) => await Database.ListTrimAsync(key, start, end);





        public async Task<string> LPopAsync(string key) => await Database.ListLeftPopAsync(key);


        public async Task<T?> LPopAsync<T>(string key) => (await LPopAsync(key)).Deserialize<T>();


        public async Task<long> LPushAsync(string key, params string[] values) => await Database.ListLeftPushAsync(key, values.Serialize());


        public async Task<long> LPushAsync<T>(string key, params T[] values) => await Database.ListLeftPushAsync(key, values.Serialize());



        public async Task<long> LPushExistsAsync(string key, string value) => await Database.ListLeftPushAsync(key, value, When.Exists);


        public async Task<long> LPushExistsAsync<T>(string key, T value) => await Database.ListLeftPushAsync(key, value.Serialize(), When.Exists);


        public async Task<long> LRemoveAsync(string key, string value, long count = 0) => await Database.ListRemoveAsync(key, value, count);


        public async Task<long> LRemoveAsync<T>(string key, T value, long count = 0) => await Database.ListRemoveAsync(key, value.Serialize(), count);




        public async Task<string> RPopAsync(string key) => await Database.ListRightPopAsync(key);


        public async Task<T?> RPopAsync<T>(string key) => (await RPopAsync(key)).Deserialize<T>();


        public async Task<long> RPushAsync(string key, params string[] values) => await Database.ListRightPushAsync(key, values.ToRedisValue());

        public async Task<long> RPushAsync<T>(string key, params T[] values) => await Database.ListRightPushAsync(key, values.ToRedisValue());

        public async Task<long> RPushExistsAsync(string key, string value) => await Database.ListRightPushAsync(key, value, When.Exists);

        public async Task<long> RPushExistsAsync<T>(string key, T value) => await Database.ListRightPushAsync(key, value.Serialize(), When.Exists);



    }
}
