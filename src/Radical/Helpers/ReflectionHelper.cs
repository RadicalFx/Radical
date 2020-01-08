using Radical.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Radical.Helpers
{
    /// <summary>
    /// Helper class for reflection related stuff.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// A reflection helper specifically bound to a known type.
        /// </summary>
        /// <typeparam name="T">The type to bind to.</typeparam>
        public class BoundReflectionHelper<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BoundReflectionHelper&lt;T&gt;"/> class.
            /// </summary>
            internal BoundReflectionHelper()
            {

            }

            /// <summary>
            /// Gets the names the of the given property expressed as Lambda Expression.
            /// </summary>
            /// <param name="propertyRef">The property ref.</param>
            /// <returns></returns>
            public string NameOf(Expression<Func<T, object>> propertyRef)
            {
                return propertyRef.GetMemberName();
            }

            /// <summary>
            /// Gets property info for the property represented by the given expression.
            /// </summary>
            /// <param name="propertyRef">The property expression.</param>
            /// <returns>The property info.</returns>
            public PropertyInfo GetProperty(Expression<Func<T, object>> propertyRef)
            {
                return typeof(T).GetProperty(propertyRef.GetMemberName());
            }

            /// <summary>
            /// Gets property info for the property represented by the given expression.
            /// </summary>
            /// <param name="propertyExpression">The property expression.</param>
            /// <returns>
            /// The list of property info.
            /// </returns>
            public IEnumerable<PropertyInfo> GetProperties(Expression<Func<T, object>> propertyExpression)
            {
                return GetProperties(propertyExpression.Body);
            }

            private IEnumerable<PropertyInfo> GetProperties(Expression expression)
            {
                var memberExpression = expression as MemberExpression;
                if (memberExpression == null)
                {
                    yield break;
                }

                var property = memberExpression.Member as PropertyInfo;
                if (property == null)
                {
                    throw new NotSupportedException("Expression is not a property accessor");
                }

                foreach (var propertyInfo in GetProperties(memberExpression.Expression))
                {
                    yield return propertyInfo;
                }

                yield return property;
            }
        }

        /// <summary>
        /// Create an instance of the BoundReflectionHelper class bound to the 
        /// specified system type.
        /// </summary>
        /// <typeparam name="T">The type to bind to.</typeparam>
        /// <returns>An instance of the BoundReflectionHelper.</returns>
        public static BoundReflectionHelper<T> BoundTo<T>()
        {
            return new BoundReflectionHelper<T>();
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The type of the instance holding the property.</typeparam>
        /// <param name="propertyRef">The property expressed as a Lambda Expression Function on the given instance type.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> propertyRef)
        {
            return propertyRef.GetMemberName();
        }
    }
}
