using System.Linq.Expressions;

namespace Application.Common.Entensions;

public static class SortExtention
{
    public static IQueryable<T> OrderByExpression<T>(this IQueryable<T> query, string orderByExpression)
    {
        if (string.IsNullOrWhiteSpace(orderByExpression))
            return query;

        string propertyName, orderByMethod;
        string[] strs = orderByExpression.Split(' ');
        propertyName = strs[0];

        if (strs.Length == 1)
            orderByMethod = "OrderBy";
        else
            orderByMethod = strs[1].Equals("DESC", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

        ParameterExpression pe = Expression.Parameter(query.ElementType);
        MemberExpression me = Expression.Property(pe, propertyName);

        MethodCallExpression orderByCall = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { query.ElementType, me.Type }, query.Expression
            , Expression.Quote(Expression.Lambda(me, pe)));

        return (IQueryable<T>)query.Provider.CreateQuery(orderByCall);
    }
}
