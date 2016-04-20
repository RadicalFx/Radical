using System;

namespace Topics.Radical.Threading
{
    /// <summary>
    /// Defines a configurable async worker that has input arguments.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    public interface IInputWorker<T>
    {
        /// <summary>
        /// Configures the current worker.
        /// </summary>
        /// <param name="cfg">The configuration handler.</param>
        /// <returns>A configured worker ready to be executed.</returns>
        IConfiguredInputWorker<T> Configure( Action<IInputWorkerConfiguration<T>> cfg );

        /// <summary>
        /// Executes the specified async action.
        /// </summary>
        /// <param name="asyncHandler">The async handler.</param>
        /// <returns>
        /// An <see cref="IWorker"/> instance useful to track worker state.
        /// </returns>
        IWorker Execute( Action<IAsyncArgs<T>> asyncHandler );

        /// <summary>
        /// Stores the specified async action and return an <see cref="IExecutableWorker"/> that can be run when required.
        /// </summary>
        /// <param name="asyncHandler">The async handler.</param>
        /// <returns>
        /// An <see cref="IExecutableWorker"/> instance that can be run when required and is useful to track worker state.
        /// </returns>
        IExecutableWorker OnExecute( Action<IAsyncArgs<T>> asyncHandler );

        /// <summary>
        /// Defines the expected result type.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns>The current worker.</returns>
        IInputOutputWorker<T, TResult> AndExpecting<TResult>();

        /// <summary>
        /// Defines the expected result type.
        /// </summary>
        /// <remarks>
        /// This overload is really useful to define result type 
        /// based on anonymous types.
        /// </remarks>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="sample">The sample result type.</param>
        /// <returns>The current worker.</returns>
        IInputOutputWorker<T, TResult> AndExpecting<TResult>( TResult sample );
    }

    /// <summary>
    /// Defines a configured async worker that has input arguments.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    public interface IConfiguredInputWorker<T>
    {
        /// <summary>
        /// Executes the specified async action.
        /// </summary>
        /// <param name="asyncHandler">The async handler.</param>
        /// <returns>
        /// An <see cref="IWorker"/> instance useful to track worker state.
        /// </returns>
        IWorker Execute( Action<IAsyncArgs<T>> asyncHandler );

        /// <summary>
        /// Stores the specified async action and return an <see cref="IExecutableWorker"/> that can be run when required.
        /// </summary>
        /// <param name="asyncHandler">The async handler.</param>
        /// <returns>
        /// An <see cref="IExecutableWorker"/> instance that can be run when required and is useful to track worker state.
        /// </returns>
        IExecutableWorker OnExecute( Action<IAsyncArgs<T>> asyncHandler );
    }
}
