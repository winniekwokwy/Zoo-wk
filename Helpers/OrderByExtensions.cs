namespace Zoo.Helpers;
public static class OrderByExtensions
{
    public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> source, OrderBy<T, TKey> orderBy)
    {
        return Queryable.OrderBy(source, orderBy.Expression);
    }

    public static IOrderedQueryable<T> OrderByDescending<T, TKey>(this IQueryable<T> source, OrderBy<T, TKey> orderBy)
    {
        return Queryable.OrderByDescending(source, orderBy.Expression);
    }

    public static IOrderedQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> source, OrderBy<T, TKey> orderBy)
    {
        return Queryable.ThenBy(source, orderBy.Expression);
    }

    public static IOrderedQueryable<T> ThenByDescending<T, TKey>(this IOrderedQueryable<T> source, OrderBy<T, TKey> orderBy)
    {
        return Queryable.ThenByDescending(source, orderBy.Expression);
    }
}