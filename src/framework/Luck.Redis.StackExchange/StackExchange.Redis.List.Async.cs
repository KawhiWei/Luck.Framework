using Luck.Framework.Infrastructure.Caching;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisList : IRedisList
    {

        public Task<T> GetByIndexAsync<T>(string key, long index)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetByIndexAsync(string key, long index)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetLenAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRangeAsync(string redisKey, long start, long end)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetRangeAsync<T>(string redisKey, long start, long end)
        {
            throw new NotImplementedException();
        }

        public Task LTrimAsync(string redisKey)
        {
            throw new NotImplementedException();
        }




        public Task<string> LPopAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<T> LPopAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushAsync(string redisKey, params string[] values)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushAsync<T>(string redisKey, params T[] values)
        {
            throw new NotImplementedException();
        }


        public Task<long> LPushExistsAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushExistsAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task<long> LRemoveAsync(string key, string value, long count = 0)
        {
            throw new NotImplementedException();
        }

        public Task<long> LRemoveAsync<T>(string key, T value, long count = 0)
        {
            throw new NotImplementedException();
        }



        public Task<string> RPopAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<T> RPopAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushAsync(string redisKey, params string[] values)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushAsync<T>(string redisKey, params T[] values)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushExistsAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushExistsAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetByIndexAsync(string key, long index, string value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetByIndexAsync<T>(string key, long index, T value)
        {
            throw new NotImplementedException();
        }

    }
}
