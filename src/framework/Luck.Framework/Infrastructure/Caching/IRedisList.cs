namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface IRedisList
    {
        void LTrim(string redisKey);

        long LRemove(string key, string value, long count = 0);

        long LRemove<T>(string key, T value, long count = 0);





        IList<string> GetRange(string redisKey, long start, long end);

        IList<T> GetRange<T>(string redisKey, long start, long end);

        T? GetByIndex<T>(string key, long index);

        string GetByIndex(string key, long index);

        long GetLen(string key);

        bool SetByIndex(string key, long index, string value);

        bool SetByIndex<T>(string key, long index, T value);





        long LPush(string redisKey, params string[] values);

        long LPush<T>(string redisKey, params T[] values);

        long LPushExists(string key, string value);

        long LPushExists<T>(string key, T value);

        string LPop(string key);

        T LPop<T>(string key);





        long RPush(string redisKey, params string[] values);

        long RPush<T>(string redisKey, params T[] values);

        long RPushExists(string key, string value);

        long RPushExists<T>(string key, T value);

        string RPop(string key);

        T RPop<T>(string key);

    }
}
