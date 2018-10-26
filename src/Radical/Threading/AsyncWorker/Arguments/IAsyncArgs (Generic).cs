
namespace Radical.Threading
{
    /// <summary>
    /// Defines async arguments given to the AsyncWorker by the caller.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    public interface IAsyncArgs<T>
    {
        /// <summary>
        /// Gets the argument given to the AsyncWorker by the caller.
        /// </summary>
        /// <value>The argument.</value>
        T Argument { get; }

        /// <summary>
        /// Gets a value indicating whether there is pending cancellation request.
        /// </summary>
        /// <value>
        ///     <c>true</c> if there is pending cancellation request; otherwise, <c>false</c>.
        /// </value>
        bool IsCancellationPending { get; }
    }

    /// <summary>
    /// Defines async arguments given to the AsyncWorker by the caller.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IAsyncArgs<T, TResult> : IAsyncArgs<T>
    {
        /// <summary>
        /// Gets or sets the result of the async operation.
        /// </summary>
        /// <value>The result.</value>
        TResult Result { get; set; }
    }

    /// <summary>
    /// Defines async arguments returned by AsyncWorker to the caller.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IOutputAsyncArgs<TResult>
    {
        /// <summary>
        /// Gets or sets the result of the async operation.
        /// </summary>
        /// <value>The result.</value>
        TResult Result { get; set; }
    }
}
