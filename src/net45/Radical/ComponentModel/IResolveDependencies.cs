//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Topics.Radical.ComponentModel
//{
//	/// <summary>
//	/// Represents a facade to abstract the interface of a dependency container.
//	/// </summary>
//	[Contract]
//	public interface IResolveDependencies
//	{
//		/// <summary>
//		/// Resolves all the dependencies identified by the given type.
//		/// </summary>
//		/// <typeparam name="T">The type of the dependency.</typeparam>
//		/// <returns></returns>
//		IEnumerable<T> ResolveAll<T>();

//		/// <summary>
//		/// Resolves the dependency identified by the given type.
//		/// </summary>
//		/// <typeparam name="T">The type of the dependency.</typeparam>
//		/// <returns></returns>
//		T Resolve<T>();

//		/// <summary>
//		/// Resolves all the dependencies identified by the given type.
//		/// </summary>
//		/// <param name="t">The type of the dependency.</param>
//		/// <returns></returns>
//		IEnumerable<Object> ResolveAll( Type t );

//		/// <summary>
//		/// Resolves the dependency identified by the given type.
//		/// </summary>
//		/// <param name="t">The type of the dependency.</param>
//		/// <returns></returns>
//		Object Resolve( Type t );
//	}
//}
