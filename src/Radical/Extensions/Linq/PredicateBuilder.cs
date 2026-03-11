using System;
using System.Linq;
using System.Linq.Expressions;

namespace Radical.Linq
{
    /// <summary>
    /// Provides methods for dynamically composing strongly-typed LINQ predicate expressions.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate expression that always evaluates to <c>true</c>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
        /// <returns>An expression representing a predicate that always returns <c>true</c>.</returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// Creates a predicate expression that always evaluates to <c>false</c>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
        /// <returns>An expression representing a predicate that always returns <c>false</c>.</returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// Combines two predicate expressions using a logical OR, returning a new predicate that is
        /// <c>true</c> when either predicate is satisfied.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
        /// <param name="expr1">The first predicate expression.</param>
        /// <param name="expr2">The second predicate expression.</param>
        /// <returns>A new predicate expression representing the logical OR of the two predicates.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Combines two predicate expressions using a logical AND, returning a new predicate that is
        /// <c>true</c> only when both predicates are satisfied.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
        /// <param name="expr1">The first predicate expression.</param>
        /// <param name="expr2">The second predicate expression.</param>
        /// <returns>A new predicate expression representing the logical AND of the two predicates.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Conditionally combines two predicate expressions using a logical AND.
        /// The second predicate is only appended when the specified condition evaluates to <c>true</c>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
        /// <param name="expr1">The base predicate expression.</param>
        /// <param name="condition">A factory function that determines whether to apply the second predicate.</param>
        /// <param name="expr2">The predicate expression to AND-combine when the condition is met.</param>
        /// <returns>
        /// A new predicate combining <paramref name="expr1"/> and <paramref name="expr2"/> with AND
        /// if the condition is <c>true</c>; otherwise, <paramref name="expr1"/> unchanged.
        /// </returns>
        public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> expr1, Func<bool> condition, Expression<Func<T, bool>> expr2)
        {
            if (condition())
            {
                var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
                return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
            }
            else
            {
                return expr1;
            }
        }
    }
}
