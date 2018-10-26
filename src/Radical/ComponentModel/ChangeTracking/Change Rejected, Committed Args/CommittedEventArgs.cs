namespace Radical.ComponentModel.ChangeTracking
{
    using System;

    /// <summary>
    /// Contains event data describing the change commit.
    /// </summary>
    public class CommittedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommittedEventArgs"/> class.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public CommittedEventArgs( CommitReason reason )
        {
            this.Reason = reason;
        }

        /// <summary>
        /// Gets the reason of the commit request.
        /// </summary>
        /// <value>The reason.</value>
        public CommitReason Reason
        {
            get;
            private set;
        }
    }
}
