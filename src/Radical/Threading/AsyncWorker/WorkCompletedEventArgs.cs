using System;

namespace Radical.Threading
{
    /// <summary>
    /// Defines the arguments of the WorkCompleted event exposed by an IWorker.
    /// </summary>
    public class WorkCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="cancelled">if set to <c>true</c> [cancelled].</param>
        public WorkCompletedEventArgs( Boolean cancelled )
        {
            this.Cancelled = cancelled;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the async work has been cancelled by the user.
        /// </summary>
        /// <value><c>true</c> if the async work has been cancelled; otherwise, <c>false</c>.</value>
        public Boolean Cancelled { get; private set; }
    }
}
