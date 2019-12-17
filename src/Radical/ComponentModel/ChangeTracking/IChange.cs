namespace Radical.ComponentModel.ChangeTracking
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a change occurred to an object.
    /// </summary>
    public interface IChange
    {
        /// <summary>
        /// Gets the owner of this change, typically the changed object.
        /// </summary>
        /// <value>The owner.</value>
        Object Owner { get; }

        /// <summary>
        /// Gets the changed entities hold by this IChange instance.
        /// </summary>
        /// <returns>A list of changed entities.</returns>
        IEnumerable<Object> GetChangedEntities();

        /// <summary>
        /// Commits this change.
        /// </summary>
        /// <param name="reason">The reason of the commit.</param>
        void Commit(CommitReason reason);

        /// <summary>
        /// Occurs when this IChange has been committed.
        /// </summary>
        event EventHandler<CommittedEventArgs> Committed;

        /// <summary>
        /// Rejects this change.
        /// </summary>
        /// <param name="reason">The reason of the reject.</param>
        void Reject(RejectReason reason);

        /// <summary>
        /// Occurs when this IChange has been rejected.
        /// </summary>
        event EventHandler<RejectedEventArgs> Rejected;

        /// <summary>
        /// Gets a value indicating whether this instance supports commit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance supports commit; otherwise, <c>false</c>.
        /// </value>
        bool IsCommitSupported { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }

        /// <summary>
        /// Gets the advised action for this IChange.
        /// </summary>
        /// <value>The advised action.</value>
        ProposedActions GetAdvisedAction(Object changedItem);

        /// <summary>
        /// Clones this IChange instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        IChange Clone();
    }
}