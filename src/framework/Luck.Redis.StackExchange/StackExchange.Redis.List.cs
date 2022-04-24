using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.Caching;
using StackExchange.Redis;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisList : IRedisList
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public StackExchangeRedisList(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            database = _connectionMultiplexer.GetDatabase();
        }

        public IDatabase database { get; }

        public T? GetByIndex<T>(string key, long index)
        {
            return database.ListGetByIndex(key, index).ToString().Deserialize<T>();
        }

        public string GetByIndex(string key, long index)
        {
            return database.ListGetByIndex(key, index).ToString();
        }


        public long GetLen(string key)
        {
            return database.ListLength(key);
        }


        public IList<string> GetRange(string key, long start, long end)
        {
            return database.ListRange(key, start, end).Select(x => x.ToString()).ToList();
        }
            
        public IList<T> GetRange<T>(string key, long start, long end)
        {
            var list = database.ListRange(key, start, end);

            //list.Select(x => ).ToList();
            return new List<T>();
        }



        public string LPop(string key)
        {
            throw new NotImplementedException();
        }

        public T LPop<T>(string key)
        {
            throw new NotImplementedException();
        }


        public long LPush(string redisKey, params string[] values)
        {
            throw new NotImplementedException();
        }

        public long LPush<T>(string redisKey, params T[] values)
        {
            throw new NotImplementedException();
        }



        public long LPushExists(string key, string value)
        {
            throw new NotImplementedException();
        }

        public long LPushExists<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public long LRemove(string key, string value, long count = 0)
        {
            throw new NotImplementedException();
        }

        public long LRemove<T>(string key, T value, long count = 0)
        {
            throw new NotImplementedException();
        }



        public void LTrim(string redisKey)
        {
            throw new NotImplementedException();
        }


        public string RPop(string key)
        {
            throw new NotImplementedException();
        }

        public T RPop<T>(string key)
        {
            throw new NotImplementedException();
        }



        public long RPush(string redisKey, params string[] values)
        {
            throw new NotImplementedException();
        }

        public long RPush<T>(string redisKey, params T[] values)
        {
            throw new NotImplementedException();
        }

        public long RPushExists(string key, string value)
        {
            throw new NotImplementedException();
        }

        public long RPushExists<T>(string key, T value)
        {
            throw new NotImplementedException();
        }


        public bool SetByIndex(string key, long index, string value)
        {
            throw new NotImplementedException();
        }

        public bool SetByIndex<T>(string key, long index, T value)
        {
            throw new NotImplementedException();
        }

    }
}
