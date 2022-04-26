namespace Luck.Framework.Infrastructure.Caching
{
    public partial interface IRedisList
    {
        /// <summary>
        /// 对一个列表进行修剪(trim)，就是说，让列表只保留指定区间内的元素，不在指定区间之内的元素都将被删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void LTrim(string key,long start, long end);

        /// <summary>
        /// <para>根据参数 count 的值，移除列表中与参数 value 相等的元素。count 的值可以是以下几种</para>
        /// <para>count 大于 0 : 从表头开始向表尾搜索，移除与 value 相等的元素，数量为 count</para>
        /// <para>count 小于 0 : 从表尾开始向表头搜索，移除与 value 相等的元素，数量为 count 的绝对值</para>
        /// <para>count = 0 : 移除表中所有与 value 相等的值</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        long LRemove(string key, string value, long count = 0);

        long LRemove<T>(string key, T value, long count = 0);





        IList<string> GetRange(string key, long start, long end);

        IList<T?> GetRange<T>(string key, long start, long end);

        T? GetByIndex<T>(string key, long index);

        string GetByIndex(string key, long index);

        long GetLen(string key);

        bool SetByIndex(string key, long index, string value);

        bool SetByIndex<T>(string key, long index, T value);





        long LPush(string key, params string[] values);

        long LPush<T>(string redisKey, params T[] values);

        long LPushExists(string key, string value);

        long LPushExists<T>(string key, T value);

        string LPop(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T? LPop<T>(string key);




        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        long RPush(string key, params string[] values);

        long RPush<T>(string key, params T[] values);

        long RPushExists(string key, string value);

        long RPushExists<T>(string key, T value);

        string RPop(string key);

        T? RPop<T>(string key);

    }
}
