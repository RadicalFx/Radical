using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Radical.Reflection
{
    /// <summary>
    /// Represents a fast delegate to a void method call.
    /// </summary>
    public delegate void LateBoundVoidMethod( object target, object[] arguments );
    
    /// <summary>
    /// Represents a fast delegate to a method call.
    /// </summary>
    public delegate object LateBoundMethod( object target, object[] arguments );

    /// <summary>
    /// Adds behaviors to the <see cref="MethodInfo"/> class.
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Creates a late bound fast delegate using lambda expression for a method
        /// with a return type.
        /// </summary>
        /// <param name="method">The method to create the fast delegate for.</param>
        /// <returns>A reference to the created fast delegate.</returns>
        public static LateBoundMethod CreateDelegate( this MethodInfo method )
        {
            var instanceParameter = Expression.Parameter( typeof( object ), "target" );
            var argumentsParameter = Expression.Parameter( typeof( object[] ), "arguments" );

            var call = MethodInfoExtensions.CreateMethodCallExpression
            ( 
                method, 
                instanceParameter, 
                argumentsParameter 
            );
            
            var lambda = Expression.Lambda<LateBoundMethod>
            (
                Expression.Convert( call, typeof( object ) ),
                instanceParameter,
                argumentsParameter 
            );

            return lambda.Compile();
        }

        /// <summary>
        /// Creates a late bound fast delegate using lambda expression for a method
        /// with a 'void' return type.
        /// </summary>
        /// <param name="method">The method to create the fast delegate for.</param>
        /// <returns>A reference to the created fast delegate.</returns>
        public static LateBoundVoidMethod CreateVoidDelegate( this MethodInfo method )
        {
            var instanceParameter = Expression.Parameter( typeof( object ), "target" );
            var argumentsParameter = Expression.Parameter( typeof( object[] ), "arguments" );

            var call = MethodInfoExtensions.CreateMethodCallExpression
            ( 
                method, 
                instanceParameter, 
                argumentsParameter 
            );
            
            var lambda = Expression.Lambda<LateBoundVoidMethod>
            (
                call,
                instanceParameter,
                argumentsParameter 
            );

            return lambda.Compile();
        }

        private static MethodCallExpression CreateMethodCallExpression( MethodInfo method, ParameterExpression instanceParameter, ParameterExpression argumentsParameter )
        {
            var call = Expression.Call
            (
                Expression.Convert( instanceParameter, method.DeclaringType ),
                method,
                MethodInfoExtensions.CreateParameterExpressions
                ( 
                    method, 
                    argumentsParameter 
                ) 
            );

            return call;
        }

        private static Expression[] CreateParameterExpressions( MethodInfo method, Expression argumentsParameter )
        {
            return method.GetParameters().Select
            ( 
                ( p, i ) => Expression.Convert
                            (
                                Expression.ArrayIndex
                                ( 
                                    argumentsParameter, 
                                    Expression.Constant( i ) 
                                ),
                                p.ParameterType 
                            ) 
            ).ToArray();
        }
    }
}
