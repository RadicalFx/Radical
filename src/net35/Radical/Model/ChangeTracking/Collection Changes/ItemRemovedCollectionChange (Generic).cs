namespace Topics.Radical.ChangeTracking.Specialized
{
	using System;
	using System.Collections.Generic;
	using Topics.Radical.Collections;
	using Topics.Radical.ComponentModel.ChangeTracking;
	using Topics.Radical.Validation;

	/// <summary>
	/// Identifies that an item in a collection has been removed.
	/// </summary>
	public class ItemRemovedCollectionChange<T> : CollectionChange<ItemChangedDescriptor<T>, T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ItemRemovedCollectionChange&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="descriptor">The descriptor.</param>
		/// <param name="rejectCallback">The reject callback.</param>
		/// <param name="commitCallback">The commit callback.</param>
		/// <param name="description">The description.</param>
		public ItemRemovedCollectionChange( Object owner, ItemChangedDescriptor<T> descriptor, RejectCallback<ItemChangedDescriptor<T>> rejectCallback, CommitCallback<ItemChangedDescriptor<T>> commitCallback, String description )
			: base( owner, descriptor, rejectCallback, commitCallback, description )
		{

		}

		/// <summary>
		/// Gets the changed entities.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<Object> GetChangedEntities()
		{
			return new ReadOnlyCollection<Object>( new Object[] { this.Descriptor.Item } );
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
				.If( o => !Object.Equals( o, this.Descriptor.Item ) )
				.Then( o => { throw new ArgumentOutOfRangeException(); } );

			return ProposedActions.Delete | ProposedActions.Dispose;
		}

		/// <summary>
		/// Clones this IChange instance.
		/// </summary>
		/// <returns>A clone of this instance.</returns>
		public override IChange Clone()
		{
			return new ItemRemovedCollectionChange<T>(
				this.Owner,
				this.Descriptor,
				this.RejectCallback,
				this.CommitCallback,
				this.Description );
		}
	}
}