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

        private IDatabase Database { get; }

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

        public long GetLen(string key)=>Database.ListLength(key);
        


        public IList<string> GetRange(string key, long start, long end)=>Database.ListRange(key, start, end).Select(x => x.ToString()).ToList();
        

        public IList<T?> GetRange<T>(string key, long start, long end)
        {
            var list = Database.ListRange(key, start, end);
            return list.Select(x => x.ToString().Deserialize<T>()).ToList();
        }



        public string LPop(string key)=>Database.ListLeftPop(key).ToString();
        

        public T? LPop<T>(string key)=>LPop(key).Deserialize<T>();
        


        public long LPush(string key, params string[] values)=>Database.ListLeftPush(key, values.Serialize());
        

        public long LPush<T>(string key, params T[] values)=>Database.ListLeftPush(key, values.Serialize());
        

        public long LPushExists(string key, string value)=>Database.ListLeftPush(key, value, When.Exists);
        

        public long LPushExists<T>(string key, T value)=>Database.ListLeftPush(key, value.Serialize(), When.Exists);
        

        public long LRemove(string key, string value, long count = 0)=>Database.ListRemove(key, value, count);
        

        public long LRemove<T>(string key, T value, long count = 0)=> Database.ListRemove(key, value.Serialize(), count);
        



        public void LTrim(string key, long start, long end)=> Database.ListTrim(key, start, end);
        


        public string RPop(string key)=>Database.ListRightPop(key);
        

        public T? RPop<T>(string key)=>RPop(key).Deserialize<T>();
        



        public long RPush(string key, params string[] values)=>Database.ListRightPush(key, values.Serialize());
        

        public long RPush<T>(string key, params T[] values)=>Database.ListRightPush(key, values.Serialize());
        

        public long RPushExists(string key, string value)=>Database.ListRightPush(key, value, When.Exists);
        

        public long RPushExists<T>(string key, T value)=>Database.ListRightPush(key, value.Serialize());
        




    }
}
