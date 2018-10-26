using System;

namespace Radical.Threading
{
    /// <summary>
    /// AsyncWorker creation and configuration entry point.
    /// </summary>
    [Obsolete( "Use the Task Parallel Library (TPL) or the new async/await keywords of C# 5.0.", false )]
    public static class AsyncWorker
    {
        //public static IAnonymousWorker Create() 
        //{

        //}

        /// <summary>
        /// Create an AsyncWorker passing in the supplied argument.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="argument">The argument value.</param>
        /// <returns>An instance of the created worker.</returns>
        public static IInputWorker<T> Using<T>( T argument )
        {
            var wArgs = new WorkerArgs<T, Object>( argument );
            var wConfig = new InputWorkerConfiguration<T>( wArgs );

            return new Worker<T, Object>( wConfig );
        }

        /// <summary>
        /// Create an AsyncWorker that has no input arguments but 
        /// procudes the given result type.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns>An instance of the created worker.</returns>
        public static IOuputWorker<TResult> Expecting<TResult>()
        {
            return AsyncWorker.Expecting<TResult>( default( TResult ) );
        }

        /// <summary>
        /// Create an AsyncWorker that has no input arguments but 
        /// procudes a result whose type definition is equal to the
        /// supplied one.
        /// </summary>
        /// <remarks>
        /// This overload is usefull when the worker must return more then
        /// one result and the caller does not want to define a specific tupla
        /// to handle multiple results, this overload allows the caller to pass
        /// in an anonymous type that the compiler will use as type reference to
        /// check that the returned type matches the supllied one.
        /// </remarks>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="sample">The result sample.</param>
        /// <returns>An instance of the created worker.</returns>
        public static IOuputWorker<TResult> Expecting<TResult>( TResult sample )
        {
            var wArgs = new WorkerArgs<Object, TResult>( null );
            var wConfig = new OutputWorkerConfiguration<TResult>( wArgs );

            return new Worker<Object, TResult>( wConfig );
        }
    }
}
