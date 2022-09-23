using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sources"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static IQueryable<TSource> ToPage<TSource>(this IQueryable<TSource> sources, int pageIndex,
        int pageSize)
    {
        var list = sources.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return list;
    }

    /// <summary>
    /// IQueryable 多条件查询
    /// </summary>
    /// <param name="sources"></param>
    /// <param name="predicate"></param>
    /// <param name="condition"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> sources, Expression<Func<TSource, bool>> predicate, bool condition)
    {
        return condition ? sources.Where(predicate) : sources;
    }


    
}