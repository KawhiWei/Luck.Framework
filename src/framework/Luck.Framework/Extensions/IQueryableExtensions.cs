namespace Microsoft.EntityFrameworkCore;

public static class QueryableExtensions
{
    public static IQueryable<TSource> ToPage<TSource>(this IQueryable<TSource> sources, int pageIndex,
        int pageSize)
    {
        var list = sources.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return list;
    }
}