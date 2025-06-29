using System;
using System.Linq.Expressions;

public class OrderBy<T, TKey> : IOrderBy
{
    public Expression<Func<T, TKey>> Expression { get; }

    dynamic IOrderBy.Expression => Expression;

    public OrderBy(Expression<Func<T, TKey>> expression)
    {
        Expression = expression;
    }
}