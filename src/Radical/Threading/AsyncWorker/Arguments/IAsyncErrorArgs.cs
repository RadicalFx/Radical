using System;

namespace Radical.Threading
{
    /// <summary>
    /// Defines arguments returned to the caller if an 
    /// unhandled exception occurs during the async operation.
    /// </summary>
    public interface IAsyncErrorArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the caller has handled the async error.
        /// If handled is set to false the exception 
        /// is automatically rethrown by the worker.
        /// </summary>
        /// <value><c>true</c> if handled; otherwise, <c>false</c>.</value>
        Boolean Handled { get; set; }

        /// <summary>
        /// Gets the error occured during the async execution.
        /// </summary>
        /// <value>The async error.</value>
        Exception Error { get; }
    }
}
