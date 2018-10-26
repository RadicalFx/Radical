using System;
using System.Threading;

namespace Radical.Threading
{
    /// <summary>
    /// Defines the basic contract to handle the status of an async worker.
    /// </summary>
    public interface IWorkerStatus
    {
        /// <summary>
        /// Gets a value indicating whether this worker is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        Boolean IsBusy { get; }

        /// <summary>
        /// Occurs when the async worker has completed its work.
        /// </summary>
        event EventHandler<WorkCompletedEventArgs> Completed;

        /// <summary>
        /// Occurs when the async worker has completed its work but there are unhandled async errors.
        /// </summary>
        event EventHandler<AsyncErrorEventArgs> AsyncError;

        /// <summary>
        /// Gets a value indicating whether this instance has completed its async work.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has completed; otherwise, <c>false</c>.
        /// </value>
        Boolean HasCompleted { get; }

        /// <summary>
        /// Gets the async wait handle that the calling thread can use to wait for the async work completition.
        /// </summary>
        /// <value>The async wait handle.</value>
        WaitHandle AsyncWaitHandle { get; }

        ///// <summary>
        ///// Gets a value indicating whether this worker supports cancellation.
        ///// </summary>
        ///// <value><c>true</c> if cancellation is supported; otherwise, <c>false</c>.</value>
        //Boolean SupportCancellation { get; }
    }
}
