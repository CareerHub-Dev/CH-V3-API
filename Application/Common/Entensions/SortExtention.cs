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
        //MemberExpression me = Expression.Property(pe, propertyName);

        Expression ex = pe;
        foreach (var member in propertyName.Split('.'))
        {
            ex = Expression.PropertyOrField(ex, member);
        }

        MethodCallExpression orderByCall = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { query.ElementType, ex.Type }, query.Expression
            , Expression.Quote(Expression.Lambda(ex, pe)));

        return (IQueryable<T>)query.Provider.CreateQuery(orderByCall);
    }
}
