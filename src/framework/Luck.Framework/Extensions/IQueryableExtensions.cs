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
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> where) => condition ? source.Where(where) : source;

    /// <summary>
    /// IQueryable 多条件查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Func<bool> condition, Expression<Func<T, bool>> where) => condition() ? source.Where(where) : source;

    
}