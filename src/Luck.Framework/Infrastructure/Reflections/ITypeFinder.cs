namespace Luck.Framework.Infrastructure
{
    public interface ITypeFinder
    {
        /// <summary>
        /// 获取Type
        /// </summary>
        /// <param name="predicate">传入方法</param>
        /// <returns></returns>
        Type[] Find(Func<Type, bool> predicate);

        Type[] FindAll();
    }
}