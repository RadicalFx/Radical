namespace Topics.Radical.Linq
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    
    public static class PredicateBuilder
    {
        public static Expression<Func<T, Boolean>> True<T>() { return f => true; }
        public static Expression<Func<T, Boolean>> False<T>() { return f => false; }

        public static Expression<Func<T, Boolean>> Or<T>( this Expression<Func<T, Boolean>> expr1, Expression<Func<T, Boolean>> expr2 )
        {
            var invokedExpr = Expression.Invoke( expr2, expr1.Parameters.Cast<Expression>() );
            return Expression.Lambda<Func<T, Boolean>>( Expression.OrElse( expr1.Body, invokedExpr ), expr1.Parameters );
        }

        public static Expression<Func<T, Boolean>> And<T>( this Expression<Func<T, Boolean>> expr1, Expression<Func<T, Boolean>> expr2 )
        {
            var invokedExpr = Expression.Invoke( expr2, expr1.Parameters.Cast<Expression>() );
            return Expression.Lambda<Func<T, Boolean>>( Expression.AndAlso( expr1.Body, invokedExpr ), expr1.Parameters );
        }

        public static Expression<Func<T, Boolean>> AndIf<T>( this Expression<Func<T, Boolean>> expr1, Func<Boolean> condition, Expression<Func<T, Boolean>> expr2 )
        {
            if( condition() )
            {
                var invokedExpr = Expression.Invoke( expr2, expr1.Parameters.Cast<Expression>() );
                return Expression.Lambda<Func<T, Boolean>>( Expression.AndAlso( expr1.Body, invokedExpr ), expr1.Parameters );
            }
            else
            {
                return expr1;
            }
        }
    }
}
