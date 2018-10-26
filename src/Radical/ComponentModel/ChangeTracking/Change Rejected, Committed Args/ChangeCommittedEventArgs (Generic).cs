namespace Radical.ComponentModel.ChangeTracking
{
    using System;

    /// <summary>
    /// ChangeCommittedArgs describes the change commit request and transport
    /// data containg detailed infos about the committed change.
    /// </summary>
    /// <typeparam name="T">The type of the cached value.</typeparam>
    public class ChangeCommittedEventArgs<T> : ChangeEventArgs<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeCommittedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cachedValue">The cached value.</param>
        /// <param name="source">The source.</param>
        /// <param name="reason">The reason.</param>
        public ChangeCommittedEventArgs( Object entity, T cachedValue, IChange source, CommitReason reason )
            : base( entity, cachedValue, source )
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
