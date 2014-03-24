namespace Topics.Radical.Reflection
{
	using System;
	using System.Linq;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Reflection;
	using Topics.Radical.Linq;
	using System.Collections.Generic;

	///// <summary>
	///// Entry point for accessing assembly types.
	///// </summary>
	//public static class GetAssembly
	//{
	//	/// <summary>
	//	/// Returns a reference to the assembly that contains the gievn type T.
	//	/// </summary>
	//	/// <typeparam name="T">The type whose assembly reference should be returned.</typeparam>
	//	/// <returns>An instance of the assembly containing the given type T.</returns>
	//	public static Assembly ThatContains<T>()
	//	{
	//		return typeof( T ).Assembly;
	//	}
	//}

	/// <summary>
	/// Adds behaviors to an assembly class instance.
	/// </summary>
	public static class AssemblyExtensions
	{
		/// <summary>
		/// Determines whether the given attribute is defined on the specified assembly.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="assembly">The assembly.</param>
		/// <returns>
		/// 	<c>true</c> if the attribute is defined on the specified assembly; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsAttributeDefined<T>( this Assembly assembly )
			where T : Attribute
		{
			return assembly.IsDefined( typeof( T ) );
		}

		/// <summary>
		/// Tries to get the attribute at assembly level identified by the 
		/// given attribute type T.
		/// </summary>
		/// <typeparam name="T">The attribute type to look for.</typeparam>
		/// <param name="assembly">The assembly to search in.</param>
		/// <param name="attribute">The attribute used to assign the output refence.</param>
		/// <returns><c>True</c> if an attribute of the given type can be found; otherwise false.</returns>
		public static Boolean TryGetAttribute<T>( this Assembly assembly, out T attribute )
			where T : Attribute
		{
			if( assembly.IsAttributeDefined<T>() )
			{
				attribute = assembly.GetCustomAttribute<T>();
			}
			else
			{
				attribute = null;
			}

			return attribute != null;
		}
	}
}
