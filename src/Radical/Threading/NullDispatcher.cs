using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radical.ComponentModel;
using Radical.Validation;

namespace Radical.Threading
{
    /// <summary>
    /// Implements the <seealso cref="IDispatcher"/> interface
    /// without doing nothing, the NullDispatcher is usefull in
    /// scenarios, where the MessageBroker is required, but where
    /// there are no cross threading issues.
    /// </summary>
    public sealed class NullDispatcher : IDispatcher
    {
        /// <summary>
        /// Safely invokes the specified delegate.
        /// </summary>
        /// <param name="d">The delegate to invoke.</param>
        /// <param name="args">The delegate arguments, or null if no arguments shuold passed to the delegate.</param>
        public void Invoke( Delegate d, params object[] args )
        {
            Ensure.That( d ).Named( () => d ).IsNotNull();
            Ensure.That( d.Target ).Named( () => d.Target ).IsNotNull();

            d.Method.Invoke( d.Target, args );
        }

        /// <summary>
        /// Safely dispatches the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Dispatch( Action action )
        {
            Ensure.That( action ).Named( () => action ).IsNotNull();

            action();
        }

        /// <summary>
        /// Safely dispatches the given argument to the supplied action.
        /// </summary>
        /// <typeparam name="T">Argument type.</typeparam>
        /// <param name="arg">The argument.</param>
        /// <param name="action">The action to dispatch.</param>
        public void Dispatch<T>( T arg, Action<T> action )
        {
            Ensure.That( action ).Named( () => action ).IsNotNull();

            action( arg );
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
            Ensure.That( action ).Named( () => action ).IsNotNull();

            action( arg1, arg2 );
        }

        /// <summary>
        /// Safely dispatches the specified Func delegate.
        /// </summary>
        /// <typeparam name="TResult">The type of the Func result.</typeparam>
        /// <param name="func">The Func to dispatch.</param>
        /// <returns>The result of Func invocation.</returns>
        public TResult Dispatch<TResult>( Func<TResult> func )
        {
            Ensure.That( func ).Named( () => func ).IsNotNull();

            return func();
        }

        /// <summary>
        /// Gets a value indicating whether the caller can safely call a target method without using this dispatcher.
        /// </summary>
        /// <value><c>true</c> if the call is safe; otherwise, <c>false</c>.</value>
        public bool IsSafe
        {
            get { return true; }
        }
    }
}
