using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Topics.Radical.ComponentModel;
using System.Windows.Threading;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Threading
{
	/// <summary>
	/// Abstracts the concept of a multi-threading synchronization dispatcher.
	/// </summary>
	public class SilverlightDispatcher : IDispatcher
	{
		readonly Dispatcher dispatcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="SilverlightDispatcher"/> class.
		/// </summary>
		/// <param name="dispatcher">The dispatcher.</param>
		public SilverlightDispatcher( Dispatcher dispatcher )
		{
			Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

			this.dispatcher = dispatcher;
		}

		/// <summary>
		/// Safely invokes the specified delegate.
		/// </summary>
		/// <param name="d">The delegate to invoke.</param>
		/// <param name="args">The delegate arguments, or null if
		/// no arguments should be passed to the delegate.</param>
		public void Invoke( Delegate d, params object[] args )
		{
			this.dispatcher.BeginInvoke( d, args );
		}

		/// <summary>
		/// Safely dispatches the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		public void Dispatch( Action action )
		{
			this.dispatcher.BeginInvoke( action );
		}

		/// <summary>
		/// Dispatches the specified arg.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg">The arg.</param>
		/// <param name="action">The action.</param>
		public void Dispatch<T>( T arg, Action<T> action )
		{
			this.dispatcher.BeginInvoke( action, new Object[] { arg } );
		}

		/// <summary>
		/// Dispatches the specified arg1.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <typeparam name="T2">The type of the 2.</typeparam>
		/// <param name="arg1">The arg1.</param>
		/// <param name="arg2">The arg2.</param>
		/// <param name="action">The action.</param>
		public void Dispatch<T1, T2>( T1 arg1, T2 arg2, Action<T1, T2> action )
		{
			this.dispatcher.BeginInvoke( action, new Object[] { arg1, arg2 } );
		}

		/// <summary>
		/// Gets a value indicating whether the caller can safely call a target method without using this dispatcher.
		/// </summary>
		/// <value>
		///   <c>true</c> if the call is safe; otherwise, <c>false</c>.
		/// </value>
		public bool IsSafe
		{
			get { return this.dispatcher.CheckAccess(); }
		}
	}
}
