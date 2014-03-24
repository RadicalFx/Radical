using System;

namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Abstracts the concept of a multi-threading synchronization dispatcher.
	/// </summary>
	public interface IDispatcher
	{
		/// <summary>
		/// Safely invokes the specified delegate.
		/// </summary>
		/// <param name="d">The delegate to invoke.</param>
		/// <param name="args">
		/// The delegate arguments, or null if 
		/// no arguments should be passed to the delegate.
		/// </param>
		void Invoke( Delegate d, params Object[] args );

		/// <summary>
		/// Safely dispatches the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		void Dispatch( Action action );

		/// <summary>
		/// Safely dispatches the given argument to the supplied action.
		/// </summary>
		/// <typeparam name="T">Argument type.</typeparam>
		/// <param name="arg">The argument.</param>
		/// <param name="action">The action to dispatch.</param>
		void Dispatch<T>( T arg, Action<T> action );

		/// <summary>
		/// Safely dispatches the given arguments to the supplied action.
		/// </summary>
		/// <typeparam name="T1">The type of the first argument.</typeparam>
		/// <typeparam name="T2">The type of the second argument.</typeparam>
		/// <param name="arg1">The first argument.</param>
		/// <param name="arg2">The second argument.</param>
		/// <param name="action">The action to dispatch.</param>
		void Dispatch<T1, T2>( T1 arg1, T2 arg2, Action<T1, T2> action );

#if !SILVERLIGHT

		/// <summary>
		/// Safely dispatches the specified Func delegate.
		/// </summary>
		/// <typeparam name="TResult">The type of the Func result.</typeparam>
		/// <param name="func">The Func to dispatch.</param>
		/// <returns>The result of Func invocation.</returns>
		TResult Dispatch<TResult>( Func<TResult> func );

#endif

		/// <summary>
		/// Gets a value indicating whether the caller can safely call a target method without using this dispatcher.
		/// </summary>
		/// <value><c>true</c> if the call is safe; otherwise, <c>false</c>.</value>
		Boolean IsSafe { get; }
	}
}
