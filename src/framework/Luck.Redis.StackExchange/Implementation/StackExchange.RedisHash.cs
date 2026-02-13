using Luck.Framework.Extensions;
using StackExchange.Redis;
using System.Net;
using IRedisHash = Luck.Framework.Infrastructure.Caching.Interface.IRedisHash;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisHash : IRedisHash
    {

        private readonly ConnectionMultiplexer _connectionMultiplexer;

        private readonly EndPoint _endPoint;

        private readonly IDatabase _database;

        public StackExchangeRedisHash(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            _endPoint = _connectionMultiplexer.GetEndPoints().First();
        }


        public  long Delete(string key, params string[] fields)
        {
            return  _database.HashDelete(key, fields.ToRedisValue() ?? Array.Empty<RedisValue>());
        }
        public  bool HashExists(string key, string field)
        {
            return  _database.HashExists(key, field);
        }

        public  IDictionary<string, string> HashGetAll(string key)
        {
            return ( _database.HashGetAll(key)).ToDictionary();
        }

        public  IDictionary<string, T?> HashGetAll<T>(string key)
        {
            return ( _database.HashGetAll(key)).ToDictionary<T>();
        }

        public  string HashGetByField(string key, string field)
        {
            return  _database.HashGet(key, field);
        }

        public  T? HashGetByField<T>(string key, string field)
        {
            return ( HashGetByField(key, field)).Deserialize<T>();
        }

        public long HashIncrement(string key, string field, long increment) => _database.HashIncrement(key, field, increment);

        public double HashIncrement(string key, string field, double increment) => _database.HashIncrement(key, field, increment);

        public long HashDecrement(string key, string field, long decrement) =>_database.HashDecrement(key, field, decrement);

        public double HashDecrement(string key, string field, double decrement) => _database.HashDecrement(key, field, decrement);

        public  IList<string> HashGetKeys(string key) => ( _database.HashKeys(key)).Select(x =>x.ToString()).ToList();

        public  IList<string> HashGet(string key, params string[] fields) => ( _database.HashGet(key, fields.ToRedisValue())).Select(x =>x.ToString()).ToList();

        public  IList<T?> HashGet<T>(string key, params string[] fields) => ( HashGet(key, fields)).Select(x => x.Deserialize<T>()).ToList();

        public  IList<string> HashGetValue(string key) => ( _database.HashValues(key)).Select(x => x.ToString()).ToList();

        public  IList<T? >HashGetValue<T>(string key) => ( HashGetValue(key)).Select(x => x.Deserialize<T>()).ToList();

        public  long HashLen(string key) =>(long) _database.Execute("hstrlen", key);





        public  bool HashSet(string key, string field, string value)
        {
             _database.HashSet(key, field, value);
            return true;
        }

        public  bool HashSet<T>(string key, string field, T value)
        {
             _database.HashSet(key, field, value.Serialize());
            return true;
        }

        public  bool HashMSet(string key, IDictionary<string, string> dictionarys)
        {
             _database.HashSet(key, dictionarys.DictionaryToHashEntry());
            return true;
        }

        public  bool HashMSet<T>(string key, IDictionary<string, T> dictionarys)
        {
             _database.HashSet(key, dictionarys.DictionaryToHashEntry());
            return true;
        }

        public bool HashSetNx<T>(string key, string field, T value) => _database.HashSet(key, field, value.Serialize(), When.NotExists);

        public bool HashSetNx(string key, string field, string value) => _database.HashSet(key, field, value, When.NotExists);

    }
}