using System;
using Topics.Radical.ComponentModel;
using Wpf = System.Windows.Threading;

namespace Topics.Radical.Windows.Threading
{
	/// <summary>
	/// Wraps the Wpf multi-threading synchronization 
	/// Dispatcher (System.Windows.Threading).
	/// </summary>
	public sealed class WpfDispatcher : IDispatcher
	{
		readonly Wpf.Dispatcher dispatcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="WpfDispatcher"/> class.
		/// </summary>
		/// <param name="dispatcher">The wrapped Wpf dispatcher.</param>
		public WpfDispatcher( Wpf.Dispatcher dispatcher )
		{
			this.dispatcher = dispatcher;
		}

		/// <summary>
		/// Safely dispatches the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		public void Dispatch( Action action )
		{
			if( this.dispatcher.CheckAccess() )
			{
				action();
			}
			else
			{
				this.dispatcher.Invoke( action );
			}
		}

		/// <summary>
		/// Safely dispatches the given argument to the supplied action.
		/// </summary>
		/// <typeparam name="T">Argument type.</typeparam>
		/// <param name="arg">The argument.</param>
		/// <param name="action">The action to dispatch.</param>
		public void Dispatch<T>( T arg, Action<T> action )
		{
			if( this.dispatcher.CheckAccess() )
			{
				action( arg );
			}
			else
			{
				this.dispatcher.Invoke( action, arg );
			}
		}

		/// <summary>
		/// Safely dispatches the given arguments to the supplied action.
		/// </summary>
		/// <typeparam name="T1">The type of the first argument.</typeparam>
		/// <typeparam name="T2">The type of the second argument.</typeparam>
		/// <param name="arg1">The first argument.</param>
		/// <param name="arg2">The second argument.</param>
		/// <param name="action">The action to dispatch.</param>
		public void Dispatch<T1, T2>( T1 arg1, T2 arg2, Action<T1, T2> action )
		{
			if( this.dispatcher.CheckAccess() )
			{
				action( arg1, arg2 );
			}
			else
			{
				this.dispatcher.Invoke( action, arg1, arg2 );
			}
		}

		/// <summary>
		/// Safely dispatches the specified Func delegate.
		/// </summary>
		/// <typeparam name="TResult">The type of the Func result.</typeparam>
		/// <param name="func">The Func to dispatch.</param>
		/// <returns>The result of Func invocation.</returns>
		public TResult Dispatch<TResult>( Func<TResult> func )
		{
			if( this.dispatcher.CheckAccess() )
			{
				return func();
			}
			else
			{
				return ( TResult )this.dispatcher.Invoke( func );
			}
		}

		/// <summary>
		/// Safely invokes the specified delegate.
		/// </summary>
		/// <param name="d">The delegate to invoke.</param>
		/// <param name="args">The delegate arguments, or null if no arguments shuold passed to the delegate.</param>
		public void Invoke( Delegate d, params Object[] args )
		{
			this.dispatcher.Invoke( d, args );
		}


		/// <summary>
		/// Gets a value indicating whether the caller can safely call a target method without using this dispatcher.
		/// </summary>
		/// <value><c>true</c> if the call is safe; otherwise, <c>false</c>.</value>
		public bool IsSafe
		{
			get { return this.dispatcher.CheckAccess(); }
		}
	}
}
