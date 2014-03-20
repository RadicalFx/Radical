namespace Topics.Radical.Reflection
{
	using System;
	using System.Linq;
	using System.Globalization;
	using Topics.Radical.Validation;
	using System.Text;
	using System.Collections.Generic;
	using System.Reflection;

	/// <summary>
	/// Defines static methods to manipulates System.Type types.
	/// All methods are also defined as .NET extension methods.
	/// </summary>
	public static class TypeExtensions
	{
		//		/// <summary>
		//		/// Returns a <see cref="System.String"/> that represents this instance.
		//		/// Use 's' for ShortString, 'sn' for ShortNameString.
		//		/// </summary>
		//		/// <param name="type">The type.</param>
		//		/// <param name="format">The format.</param>
		//		/// <returns>
		//		/// A <see cref="System.String"/> that represents this instance.
		//		/// </returns>
		//		public static String ToString( this Type type, String format )
		//		{
		//			switch( format.ToLower() )
		//			{
		//				case "sn":
		//					return type.ToShortNameString();

		//				case "s":
		//					return type.ToShortString();

		//				default:
		//					throw new NotSupportedException();
		//			}
		//		}

		//		/// <summary>
		//		/// Returns a string rapresentation of the given System.Type composed 
		//		/// only by the type name (including the namespace) and the assembly name.
		//		/// The built string is compatible with a .net fully qualified type name
		//		/// </summary>
		//		/// <param name="type">The type to build the string for.</param>
		//		/// <returns>A string representing the given type.</returns>
		//		public static String ToShortString( this Type type )
		//		{
		//			Ensure.That( type ).Named( "type" ).IsNotNull();

		//			String tName = type.FullName;

		//#if SILVERLIGHT
		//			var assemblyName = new System.Reflection.AssemblyName( type.Assembly.FullName );
		//			String aName = assemblyName.Name;
		//#else
		//			String aName = type.Assembly.GetName().Name;
		//#endif


		//			if( String.Equals( aName, "mscorlib", StringComparison.OrdinalIgnoreCase ) )
		//			{
		//				return String.Format( CultureInfo.InvariantCulture, "{0}", tName );
		//			}
		//			else
		//			{
		//				return String.Format( CultureInfo.InvariantCulture, "{0}, {1}", tName, aName );
		//			}
		//		}

		//		/// <summary>
		//		/// Returns a string rapresentating the name of the given
		//		/// System.Type (without namespaces).
		//		/// </summary>
		//		/// <param name="type">The type to build the string for.</param>
		//		/// <returns>A string representing the name of the given type.</returns>
		//		public static String ToShortNameString( this Type type )
		//		{
		//			if( type.IsGenericType )
		//			{
		//				var name = type.Name.Substring( 0, type.Name.Length - 2 );
		//				var arguments = type.GetGenericArguments();
		//				var argumentsName = arguments.Aggregate( new StringBuilder(), ( r, s ) =>
		//				{
		//					r.AppendFormat( "{0}, ", s.ToShortNameString() );

		//					return r;
		//				} )
		//				.ToString()
		//				.TrimEnd( ',', ' ' );

		//				return String.Format( "{0}<{1}>", name, argumentsName );
		//			}

		//			return type.Name;
		//		}

		/// <summary>
		/// Determines whether the specified type is a descendant of the given T type.
		/// This generic method is a shortcut for the Type.IsAssignableFrom( Type )
		/// method.
		/// </summary>
		/// <typeparam name="T">The supposed base type.</typeparam>
		/// <param name="type">The type to inspect.</param>
		/// <returns>
		/// 	<c>true</c> if the specified type inherits from the T type; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean Is<T>( this Type type )
		{
			return type.Is( typeof( T ) );
		}

		public static Boolean Is<T>( this TypeInfo type )
		{
			return type.Is( typeof( T ).GetTypeInfo() );
		}

		/// <summary>
		/// Determines whether the specified type is a descendant of the given other type.
		/// This generic method is a shortcut for the Type.IsAssignableFrom( Type )
		/// method.
		/// </summary>
		/// <param name="type">The type to inspect.</param>
		/// <param name="otherType">Type of the other.</param>
		/// <returns>
		/// 	<c>true</c> if the specified type inherits from the other type; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean Is( this Type type, Type otherType )
		{
			return Is( type.GetTypeInfo(), otherType.GetTypeInfo() );
		}

		public static Boolean Is( this TypeInfo type, TypeInfo otherType )
		{
			return type != null &&
				otherType != null &&
				otherType.IsAssignableFrom( type );
		}

		///// <summary>
		///// Gets the inheritance chain of the given type.
		///// </summary>
		///// <param name="type">The type to search the inheritance chain.</param>
		///// <returns>The inheritance chain of the given type.</returns>
		//public static IEnumerable<Type> GetInheritanceChain( this Type type )
		//{
		//	return type.GetInheritanceChain( t => false );
		//}

		///// <summary>
		///// Gets the inheritance chain of the given type.
		///// </summary>
		///// <param name="type">The type to search the inheritance chain.</param>
		///// <param name="breakIf">A delegate that determinse whento stop the base type lookup.</param>
		///// <returns>The inheritance chain of the given type.</returns>
		//public static IEnumerable<Type> GetInheritanceChain( this Type type, Func<Type, Boolean> breakIf )
		//{
		//	for( var current = type; current != null; current = current.BaseType )
		//	{
		//		if( breakIf( current ) )
		//		{
		//			yield break;
		//		}

		//		yield return current;
		//	}
		//}

		///// <summary>
		///// Gets all the type that inherits from the given type and are defined in the same assembly.
		///// </summary>
		///// <param name="type">The type.</param>
		///// <returns>A list of descendant types.</returns>
		//public static IEnumerable<Type> GetDescendants( Type type )
		//{
		//	var all = type.Assembly.GetTypes().Where( t => t.Is( type ) );
		//	foreach( var t in all )
		//	{
		//		yield return t;
		//	}
		//}
	}
}
