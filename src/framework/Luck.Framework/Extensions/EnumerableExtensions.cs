namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// IEnumerable 多条件查询
    /// </summary>
    /// <param name="sources"></param>
    /// <param name="predicate"></param>
    /// <param name="condition"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> sources, Func<TSource, bool> predicate, bool condition)
    {
        return condition ? sources.Where(predicate) : sources;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sources"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TSource> ToPage<TSource>(this IEnumerable<TSource> sources, int pageIndex,
        int pageSize)
    {
        var list = sources.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return list;
    }
}