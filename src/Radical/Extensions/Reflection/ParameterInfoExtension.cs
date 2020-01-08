using System;
using System.Linq;
using System.Reflection;

namespace Radical.Reflection
{
    /// <summary>
    /// Defines static methods to manipulates ParameterInfo types.
    /// All methods are also defined as .NET extension methods.
    /// </summary>
    public static class ParameterInfoExtension
    {
        /// <summary>
        /// Determines whether an attribute is defined on the specified type.
        /// </summary>
        /// <typeparam name="T">The type (System.Type) of the attribute to search for.</typeparam>
        /// <param name="memberInfo">The ParameterInfo to invastigate.</param>
        /// <returns>
        ///     <c>true</c> if the attribute is defined; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool IsAttributeDefined<T>(this ParameterInfo memberInfo) where T : Attribute
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            return memberInfo.IsDefined(typeof(T), false);
        }

        /// <summary>
        /// Extracts the attributes from the specified System.Type.
        /// </summary>
        /// <typeparam name="T">The attribute to search for</typeparam>
        /// <param name="memberInfo">The ParameterInfo to search on.</param>
        /// <returns>An array of the found attributes.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T[] GetAttributes<T>(this ParameterInfo memberInfo) where T : Attribute
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            return memberInfo.GetCustomAttributes(typeof(T), true)
                .OfType<T>()
                .ToArray();
        }

        /// <summary>
        /// Extracts the first attribute applied to the specified System.Type.
        /// </summary>
        /// <typeparam name="T">The attribute to search for</typeparam>
        /// <param name="memberInfo">The ParameterInfo to search on.</param>
        /// <returns>
        /// An instance of the found attribute, if one, otherwise null.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T GetAttribute<T>(this ParameterInfo memberInfo) where T : Attribute
        {
            T returnValue = null;

            if (IsAttributeDefined<T>(memberInfo))
            {
                returnValue = (T)memberInfo.GetCustomAttributes(typeof(T), false)[0];
            }

            return returnValue;
        }
    }
}
