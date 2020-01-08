using Radical.ComponentModel.ChangeTracking;
using Radical.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Radical.ChangeTracking.Specialized
{
    /// <summary>
    /// Identifies that an item in a collection has been replaced.
    /// </summary>
    public class ItemReplacedCollectionChange<T> : CollectionChange<ItemReplacedDescriptor<T>, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemReplacedCollectionChange&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="rejectCallback">The reject callback.</param>
        /// <param name="commitCallback">The commit callback.</param>
        /// <param name="description">The description.</param>
        public ItemReplacedCollectionChange(object owner, ItemReplacedDescriptor<T> descriptor, RejectCallback<ItemReplacedDescriptor<T>> rejectCallback, CommitCallback<ItemReplacedDescriptor<T>> commitCallback, string description)
            : base(owner, descriptor, rejectCallback, commitCallback, description)
        {

        }

        /// <summary>
        /// Gets the changed entities.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<object> GetChangedEntities()
        {
            return new ReadOnlyCollection<object>(new List<object> { Descriptor.NewItem, Descriptor.ReplacedItem });
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
                .If(o =>
               {
                   return !Equals(o, Descriptor.NewItem) &&
                       !Equals(o, Descriptor.ReplacedItem);
               })
                .Then(o => { throw new ArgumentOutOfRangeException(); });

            if (Equals(changedItem, Descriptor.NewItem))
            {
                return ProposedActions.Update | ProposedActions.Create;
            }
            else if (Equals(changedItem, Descriptor.ReplacedItem))
            {
                return ProposedActions.Delete | ProposedActions.Dispose;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Clones this IChange instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public override IChange Clone()
        {
            return new ItemReplacedCollectionChange<T>(
                Owner,
                Descriptor,
                RejectCallback,
                CommitCallback,
                Description);
        }
    }
}