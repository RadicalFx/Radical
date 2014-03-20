namespace Topics.Radical.ChangeTracking.Specialized
{
	using System;
	using System.Collections.Generic;
	using Topics.Radical.Collections;
	using Topics.Radical.ComponentModel.ChangeTracking;
	using Topics.Radical.Validation;

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
		public ItemReplacedCollectionChange( Object owner, ItemReplacedDescriptor<T> descriptor, RejectCallback<ItemReplacedDescriptor<T>> rejectCallback, CommitCallback<ItemReplacedDescriptor<T>> commitCallback, String description )
			: base( owner, descriptor, rejectCallback, commitCallback, description )
		{

		}

		/// <summary>
		/// Gets the changed entities.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<Object> GetChangedEntities()
		{
			return new ReadOnlyCollection<Object>( new Object[] { this.Descriptor.NewItem, this.Descriptor.ReplacedItem } );
		}

		/// <summary>
		/// Gets the advised action for this IChange.
		/// </summary>
		/// <param name="changedItem"></param>
		/// <returns></returns>
		/// <value>The advised action.</value>
		public override ProposedActions GetAdvisedAction( object changedItem )
		{
			Ensure.That( changedItem )
				.If( o =>
				{
					return !Object.Equals( o, this.Descriptor.NewItem ) &&
						!Object.Equals( o, this.Descriptor.ReplacedItem );
				} )
				.Then( o => { throw new ArgumentOutOfRangeException(); } );

			if( Object.Equals( changedItem, this.Descriptor.NewItem ) )
			{
				return ProposedActions.Update | ProposedActions.Create;
			}
			else if( Object.Equals( changedItem, this.Descriptor.ReplacedItem ) )
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
				this.Owner,
				this.Descriptor,
				this.RejectCallback,
				this.CommitCallback,
				this.Description );
		}
	}
}