using System;
using System.Threading;

namespace Radical.Threading
{
    /// <summary>
    /// Defines the contract for an async worker that has not yet started.
    /// </summary>
    public interface IExecutableWorker : IWorkerStatus
    {
        /// <summary>
        /// Executes the async action incapsulatewd by this worker.
        /// </summary>
        /// <returns>
        /// An <see cref="IWorker"/> instance useful to track worker state.
        /// </returns>
        IWorker Execute();

        /// <summary>
        /// Adds the trigger.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <returns>This worker instance.</returns>
        IExecutableWorker AddTrigger( ComponentModel.IMonitor trigger );
    }
}
