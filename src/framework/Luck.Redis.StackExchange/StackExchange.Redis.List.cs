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
            Database = _connectionMultiplexer.GetDatabase();
        }

        public IDatabase Database { get; }

        public T? GetByIndex<T>(string key, long index)
        {
            return Database.ListGetByIndex(key, index).ToString().Deserialize<T>();
        }

        public string GetByIndex(string key, long index)
        {
            return Database.ListGetByIndex(key, index).ToString();
        }

        public bool SetByIndex(string key, long index, string value)
        {
            Database.ListSetByIndex(key, index, value);
            return true;
        }

        public bool SetByIndex<T>(string key, long index, T value)
        {
            return SetByIndex(key, index, value.Serialize());
        }

        public long GetLen(string key)
        {
            return Database.ListLength(key);
        }


        public IList<string> GetRange(string key, long start, long end)
        {
            return Database.ListRange(key, start, end).Select(x => x.ToString()).ToList();
        }

        public IList<T?> GetRange<T>(string key, long start, long end)
        {
            var list = Database.ListRange(key, start, end);
            return list.Select(x => x.ToString().Deserialize<T>()).ToList();
        }



        public string LPop(string key)
        {
            return Database.ListLeftPop(key).ToString();
        }

        public T? LPop<T>(string key)
        {
            return LPop(key).Deserialize<T>();
        }


        public long LPush(string key, params string[] values)
        {
            return Database.ListLeftPush(key, values.ToRedisValue());
        }

        public long LPush<T>(string key, params T[] values)
        {
            return Database.ListLeftPush(key, values.ToRedisValue());
        }

        public long LPushExists(string key, string value)
        {
            return Database.ListLeftPush(key, value, When.Exists);
        }

        public long LPushExists<T>(string key, T value)
        {
            return Database.ListLeftPush(key, value.Serialize(), When.Exists);
        }

        public long LRemove(string key, string value, long count = 0)
        {
            return Database.ListRemove(key, value, count);
        }

        public long LRemove<T>(string key, T value, long count = 0)
        {
            return Database.ListRemove(key, value.Serialize(), count);
        }



        public void LTrim(string key, long start, long end)
        {
            Database.ListTrim(key, start, end);
        }


        public string RPop(string key)
        {
            return Database.ListRightPop(key);
        }

        public T? RPop<T>(string key)
        {
            return RPop(key).Deserialize<T>();
        }



        public long RPush(string key, params string[] values)
        {
            return Database.ListRightPush(key, values.ToRedisValue());
        }

        public long RPush<T>(string key, params T[] values)
        {
            return Database.ListRightPush(key, values.ToRedisValue());
        }

        public long RPushExists(string key, string value)
        {
            return Database.ListRightPush(key, value, When.Exists);
        }

        public long RPushExists<T>(string key, T value)
        {
            return Database.ListRightPush(key, value.Serialize());
        }




    }
}
