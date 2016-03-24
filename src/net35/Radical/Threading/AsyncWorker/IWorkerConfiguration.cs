using System;

namespace Topics.Radical.Threading
{
    /// <summary>
    /// Defines the basic configuration for an async worker.
    /// </summary>
    public interface IWorkerConfiguration
    {
        /// <summary>
        /// Gets or sets the handler to be invoked if an async error occurs.
        /// </summary>
        /// <remarks>If no error handler is injected and an async exception is 
        /// raised during async execution the async exception is automatically 
        /// rethrown at the end of the async work.</remarks>
        /// <value>The async error handler.</value>
        Action<IAsyncErrorArgs> Error { get; set; }

        /// <summary>
        /// Sets the warning threshold. If the async operation lasts more then the 
        /// given timeout the supplied handler is invoked.
        /// </summary>
        WarningThreshold WarningThreshold { get; set; }
    }
}
