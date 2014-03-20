namespace Topics.Radical.ComponentModel.ChangeTracking
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// A bookmark is a placeholder to track a specific position in
	/// the change tracking history of a change tracking service.
	/// </summary>
	public interface IBookmark
	{
		/// <summary>
		/// Gets the position this bookmark has
		/// been created to.
		/// </summary>
		/// <value>The position of the bookmark.</value>
		IChange Position { get; }

		/// <summary>
		/// Gets the change tracking service that created 
		/// the bookmark.
		/// </summary>
		/// <value>The owner service.</value>
		IChangeTrackingService Owner { get; }

		/// <summary>
		/// Gets the list of entities that were transient before the creation
		/// of the bookmark.
		/// </summary>
		/// <value>The transient entities.</value>
		IEnumerable<Object> TransientEntities { get; }
	}
}
