using System;

namespace Topics.Radical.Threading
{
    /// <summary>
    /// Defines a configurable async worker that has both input arguments 
    /// and an output result.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IInputOutputWorker<T, TResult>
    {
        /// <summary>
        /// Configures the current worker.
        /// </summary>
        /// <param name="cfg">The configuration handler.</param>
        /// <returns>A configured worker ready to be executed.</returns>
        IConfiguredInputOutputWorker<T, TResult> Configure( Action<IInputOutputWorkerConfiguration<T, TResult>> cfg );

        /// <summary>
        /// Executes the specified async action.
        /// </summary>
        /// <param name="asyncAction">The async action.</param>
        /// <returns>An <see cref="IWorker"/> instance useful to track worker state.</returns>
        IWorker Execute( Action<IAsyncArgs<T, TResult>> asyncAction );

        /// <summary>
        /// Stores the specified async action and return an <see cref="IExecutableWorker"/> that can be run when required.
        /// </summary>
        /// <param name="asyncAction">The async action.</param>
        /// <returns>
        /// An <see cref="IExecutableWorker"/> instance that can be run when required and is useful to track worker state.
        /// </returns>
        IExecutableWorker OnExecute( Action<IAsyncArgs<T, TResult>> asyncAction );
    }

    /// <summary>
    /// Defines a configured async worker that has both input arguments 
    /// and an output result ready to be executed.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IConfiguredInputOutputWorker<T, TResult>
    {
        /// <summary>
        /// Executes the specified async action.
        /// </summary>
        /// <param name="asyncAction">The async action.</param>
        /// <returns>An <see cref="IWorker"/> instance useful to track worker state.</returns>
        IWorker Execute( Action<IAsyncArgs<T, TResult>> asyncAction );

        /// <summary>
        /// Stores the specified async action and return an <see cref="IExecutableWorker"/> that can be run when required.
        /// </summary>
        /// <param name="asyncAction">The async action.</param>
        /// <returns>
        /// An <see cref="IExecutableWorker"/> instance that can be run when required and is useful to track worker state.
        /// </returns>
        IExecutableWorker OnExecute( Action<IAsyncArgs<T, TResult>> asyncAction );
    }
}
