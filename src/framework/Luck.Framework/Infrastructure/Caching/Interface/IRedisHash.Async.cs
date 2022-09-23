namespace Luck.Framework.Infrastructure.Caching.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IRedisHash
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<long> DeleteAsync(string key, params string[] fields);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        Task<bool> HashExistsAsync(string key, string field);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IDictionary<string,string>> HashGetAllAsync(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IDictionary<string, T?>> HashGetAllAsync<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        Task<string> HashGetAsyncByFieldAsync(string key, string field);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T?> HashGetAsyncByFieldAsync<T>(string key, string field);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        Task<long> HashIncrementAsync(string key, string field, long increment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        Task<double> HashIncrementAsync(string key, string field, double increment);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        Task<long> HashDecrementAsync(string key, string field, long decrement);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        Task<double> HashDecrementAsync(string key, string field, double decrement);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IList<string>> HashGetKeysAsync(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> HashLenAsync(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<IList<string>> HashGetAsync(string key, params string[] fields);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IList<T?>> HashGetAsync<T>(string key,params string[] fields);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> HashSetAsync(string key, string field, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> HashSetAsync<T>(string key, string field, T value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> HashSetNxAsync(string key, string field, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> HashSetNxAsync<T>(string key, string field,T value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dictionarys"></param>
        /// <returns></returns>
        Task<bool> HashMSetAsync(string key, IDictionary<string, string> dictionarys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dictionarys"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> HashMSetAsync<T>(string key, IDictionary<string, T> dictionarys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IList<string>> HashGetValueAsync(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IList<T?>> HashGetValueAsync<T>(string key);


    }
}
