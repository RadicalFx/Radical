using Radical.ComponentModel.ChangeTracking;
using Radical.Linq;
using Radical.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Radical.ChangeTracking.Specialized
{
    /// <summary>
    /// Identifies an AddRange operation on a collection.
    /// </summary>
    /// <typeparam name="T">The type of the collection items.</typeparam>
    public class AddRangeCollectionChange<T> : CollectionChange<CollectionRangeDescriptor<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddRangeCollectionChange&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="rejectCallback">The reject callback.</param>
        /// <param name="commitCallback">The commit callback.</param>
        /// <param name="description">The description.</param>
        public AddRangeCollectionChange(object owner, CollectionRangeDescriptor<T> descriptor, RejectCallback<CollectionRangeDescriptor<T>> rejectCallback, CommitCallback<CollectionRangeDescriptor<T>> commitCallback, string description)
            : base(owner, descriptor, rejectCallback, commitCallback, description)
        {

        }

        /// <summary>
        /// Gets the changed entities.
        /// </summary>
        /// <returns>A read-only list of the added items.</returns>
        public override IEnumerable<object> GetChangedEntities()
        {
            return Descriptor.Items.OfType<object>().AsReadOnly();
        }

        /// <summary>
        /// Gets the advised action for this IChange.
        /// </summary>
        /// <param name="changedItem">The item to get the advised action for.</param>
        /// <returns>The advised action.</returns>
        public override ProposedActions GetAdvisedAction(object changedItem)
        {
            Ensure.That(changedItem)
                .Named("changedItem")
                .If(o =>!Descriptor.Items.Any(t => Equals(t, o)))
                .Then((o, n) =>
                {
                    throw new ArgumentOutOfRangeException(n);
                });

            return ProposedActions.Create | ProposedActions.Update;
        }

        /// <summary>
        /// Clones this IChange instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public override IChange Clone()
        {
            return new AddRangeCollectionChange<T>(
                Owner,
                Descriptor,
                RejectCallback,
                CommitCallback,
                Description);
        }
    }
}