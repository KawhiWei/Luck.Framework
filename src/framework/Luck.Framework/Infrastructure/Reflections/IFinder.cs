using Luck.Framework.Infrastructure.DependencyInjectionModule;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [IgnoreDependency]
    public interface IFinder<out TItem>
    {
        /// <summary>
        /// 根据条件获取
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TItem[] Find(Func<TItem, bool> predicate);

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        TItem[] FindAll();
    }
}