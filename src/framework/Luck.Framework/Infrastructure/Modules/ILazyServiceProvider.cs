namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILazyServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T LazyGetRequiredService<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        object LazyGetRequiredService(Type serviceType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T LazyGetService<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        object LazyGetService(Type service);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T LazyGetService<T>(T defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        object LazyGetService(Type service, object? defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        object LazyGetService(Type service, Func<IServiceProvider, object> factory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T LazyGetService<T>(Func<IServiceProvider, object> factory);
    }
}
