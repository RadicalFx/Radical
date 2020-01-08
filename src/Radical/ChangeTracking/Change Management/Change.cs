using Radical.ComponentModel.ChangeTracking;
using Radical.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Radical.ChangeTracking
{
    /// <summary>
    /// Represents a change occurred to an object.
    /// </summary>
    public abstract class Change<T> : IChange<T>
    {
        /// <summary>
        /// The callback to invoke in order to 
        /// reject the cached value.
        /// </summary>
        protected RejectCallback<T> RejectCallback { get; }

        /// <summary>
        /// The callback to invoke in order to
        /// commit the cached value.
        /// </summary>
        protected CommitCallback<T> CommitCallback { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Change&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="valueToCache">The value to cache.</param>
        /// <param name="rejectCallback">The reject callback.</param>
        /// <param name="commitCallback">The commit callback.</param>
        /// <param name="description">The description.</param>
        protected Change(object owner, T valueToCache, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, string description)
        {
            Ensure.That(owner).Named("owner").IsNotNull();
            Ensure.That(rejectCallback).Named("rejectCallback").IsNotNull();

            Owner = owner;
            CachedValue = valueToCache;
            RejectCallback = rejectCallback;
            CommitCallback = commitCallback;
            Description = description;
        }

        #region IChange Members

        /// <summary>
        /// Gets the owner of this change.
        /// </summary>
        /// <value>The owner.</value>
        public object Owner
        {
            get;
            private set;
        }

        /// <summary>
        /// Commits this change.
        /// </summary>
        public void Commit(CommitReason reason)
        {
            reason.EnsureIsDefined();
            Ensure.That(reason)
                .Named("reason")
                .If(v => v == CommitReason.None)
                .Then((v, n) =>
               {
                   throw new ArgumentException("Unsupported CommitReason value.", n);
               });

            OnCommit(reason);
            OnCommitted(new CommittedEventArgs(reason));
        }

        /// <summary>
        /// Called in order to commit this change.
        /// </summary>
        protected virtual void OnCommit(CommitReason reason)
        {
            if (CommitCallback != null)
            {
                ChangeCommittedEventArgs<T> args = new ChangeCommittedEventArgs<T>(Owner, CachedValue, this, reason);
                CommitCallback(args);
            }
        }

        /// <summary>
        /// Occurs when this IChange has been committed.
        /// </summary>
        public event EventHandler<CommittedEventArgs> Committed;

        /// <summary>
        /// Called when Committed event.
        /// </summary>
        protected virtual void OnCommitted(CommittedEventArgs args)
        {
            var h = Committed;
            if (h != null)
            {
                h(this, args);
            }
        }

        /// <summary>
        /// Rejects this change.
        /// </summary>
        public void Reject(RejectReason reason)
        {
            reason.EnsureIsDefined();
            Ensure.That(reason)
                .Named("reason")
                .If(v => v == RejectReason.None)
                .Then((v, n) => { throw new ArgumentException("Unsupported RejectReason value.", n); });

            OnReject(reason);
            OnRejected(new RejectedEventArgs(reason));
        }

        /// <summary>
        /// Called in order to reject this change.
        /// </summary>
        protected virtual void OnReject(RejectReason reason)
        {
            ChangeRejectedEventArgs<T> args = new ChangeRejectedEventArgs<T>(Owner, CachedValue, this, reason);
            RejectCallback(args);
        }

        /// <summary>
        /// Occurs when this IChange has been rejected.
        /// </summary>
        public event EventHandler<RejectedEventArgs> Rejected;

        /// <summary>
        /// Raises the Rejected event.
        /// </summary>
        protected virtual void OnRejected(RejectedEventArgs args)
        {
            var h = Rejected;
            if (h != null)
            {
                h(this, args);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance supports commit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance supports commit; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsCommitSupported
        {
            get { return CommitCallback != null; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the advised action for this IChange.
        /// </summary>
        /// <value>The advised action.</value>
        public abstract ProposedActions GetAdvisedAction(object changedItem);

        /// <summary>
        /// Gets the changed entities.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<object> GetChangedEntities()
        {
            return new ReadOnlyCollection<object>(new List<object> { Owner });
        }

        /// <summary>
        /// Clones this IChange instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public abstract IChange Clone();

        #endregion

        #region IChange<T> Members

        /// <summary>
        /// Gets the cached value.
        /// </summary>
        /// <value>The cached value.</value>
        public T CachedValue
        {
            get;
            private set;
        }

        #endregion
    }
}
