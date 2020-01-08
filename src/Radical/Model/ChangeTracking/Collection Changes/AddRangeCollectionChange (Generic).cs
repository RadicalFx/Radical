namespace Radical.ChangeTracking.Specialized
{
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Linq;
    using Radical.Validation;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Identifies an AddRange operation on a collection.
    /// </summary>
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
        /// <returns></returns>
        public override IEnumerable<object> GetChangedEntities()
        {
            return Descriptor.Items.OfType<object>().AsReadOnly();
        }

        /// <summary>
        /// Gets the advised action for this IChange.
        /// </summary>
        /// <param name="changedItem"></param>
        /// <returns></returns>
        /// <value>The advised action.</value>
        public override ProposedActions GetAdvisedAction(object changedItem)
        {
            Ensure.That(changedItem)
                .Named("changedItem")
                .If(o =>
               {
                   return !Descriptor.Items.Where(t => Equals(t, o)).Any();
               })
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