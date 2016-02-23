namespace Topics.Radical.Reflection
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Topics.Radical.Validation;

    /// <summary>
    /// Defines static methods to manipulates MemberInfo types.
    /// All methods are also defined as .NET extension methods.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Determines whether an attribute is defined on the specified type.
        /// </summary>
        /// <typeparam name="T">The type (System.Type) of the attribute to search for.</typeparam>
        /// <param name="memberInfo">The MemberInfo to invastigate.</param>
        /// <returns>
        ///     <c>true</c> if the attribute is defined; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter" )]
        public static Boolean IsAttributeDefined<T>( this MemberInfo memberInfo ) where T : Attribute
        {
            return memberInfo.IsAttributeDefined<T>( true );
        }

        /// <summary>
        /// Determines whether an attribute is defined on the specified type.
        /// </summary>
        /// <typeparam name="T">The type (System.Type) of the attribute to search for.</typeparam>
        /// <param name="memberInfo">The MemberInfo to invastigate.</param>
        /// <param name="inherit">if set to <c>true</c> the search is propagated to the inheritance chain, otherwise the attribute is searched only in the given type.</param>
        /// <returns>
        ///     <c>true</c> if the attribute is defined; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter" )]
        public static Boolean IsAttributeDefined<T>( this MemberInfo memberInfo, Boolean inherit ) where T : Attribute
        {
            Ensure.That( memberInfo ).Named( "memberInfo" ).IsNotNull();

            return memberInfo.IsDefined( typeof( T ), inherit );
        }

        /// <summary>
        /// Extracts the attributes applied to the specified System.Type.
        /// </summary>
        /// <typeparam name="T">The attribute to search for</typeparam>
        /// <param name="memberInfo">The MemberInfo to search on.</param>
        /// <returns>An array of the found attributes.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter" )]
        public static T[] GetAttributes<T>( this MemberInfo memberInfo ) where T : Attribute
        {
            Ensure.That( memberInfo ).Named( "memberInfo" ).IsNotNull();

            return MemberInfoExtensions.GetAttributes<T>( memberInfo, true );
        }

        /// <summary>
        /// Extracts the attributes from the specified System.Type.
        /// </summary>
        /// <typeparam name="T">The attribute to search for</typeparam>
        /// <param name="memberInfo">The MemberInfo to search on.</param>
        /// <param name="inherit"><c>true</c> to search the inheritance chain.</param>
        /// <returns>An array of the found attributes.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter" )]
        public static T[] GetAttributes<T>( this MemberInfo memberInfo, Boolean inherit ) where T : Attribute
        {
            Ensure.That( memberInfo ).Named( "memberInfo" ).IsNotNull();

            T[] returnValue = null;

            if( MemberInfoExtensions.IsAttributeDefined<T>( memberInfo, inherit ) )
            {
                Object[] attributes = memberInfo.GetCustomAttributes( typeof( T ), inherit );
                returnValue = attributes.Cast<T>().ToArray<T>();
            }
            else
            {
                returnValue = new T[ 0 ];
            }

            return returnValue;
        }

        /// <summary>
        /// Extracts the first attribute applied to the specified System.Type.
        /// </summary>
        /// <typeparam name="T">The attribute to search for</typeparam>
        /// <param name="memberInfo">The MemberInfo to search on.</param>
        /// <returns>
        /// An instance of the found attribute, if one, otherwise null.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter" )]
        public static T GetAttribute<T>( this MemberInfo memberInfo ) where T : Attribute
        {
            Ensure.That( memberInfo ).Named( "memberInfo" ).IsNotNull();

            return memberInfo.GetAttribute<T>( true );
        }

        /// <summary>
        /// Extracts the first attribute applied to the specified <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="T">The attribute to search for</typeparam>
        /// <param name="memberInfo">The MemberInfo to search on.</param>
        /// <param name="inherit"><c>true</c> to search the inheritance chain.</param>
        /// <returns>
        /// An instance of the found attribute, if one, otherwise null.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter" )]
        public static T GetAttribute<T>( this MemberInfo memberInfo, Boolean inherit ) where T : Attribute
        {
            Ensure.That( memberInfo ).Named( "memberInfo" ).IsNotNull();

            T returnValue = null;

            if( memberInfo.IsAttributeDefined<T>( inherit ) )
            {
                returnValue = ( T )memberInfo.GetCustomAttributes( typeof( T ), inherit )[ 0 ];
            }

            return returnValue;
        }

        /// <summary>
        /// Tries to extracts the first attribute applied to the specified <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="T">The attribute to search for</typeparam>
        /// <param name="memberInfo">The MemberInfo to search on.</param>
        /// <param name="attribute">An instance of the found attribute, if one, otherwise null.</param>
        /// <returns>
        /// <c>True</c> if an attribute of the given type can be found; otherwise <c>false</c>.
        /// </returns>
        public static Boolean TryGetAttribute<T>( this MemberInfo memberInfo, out T attribute ) where T : Attribute
        {
            Ensure.That( memberInfo ).Named( "memberInfo" ).IsNotNull();

            if( memberInfo.IsAttributeDefined<T>() )
            {
                attribute = memberInfo.GetAttribute<T>();
                return true;
            }

            attribute = null;
            return false;
        }
    }
}
