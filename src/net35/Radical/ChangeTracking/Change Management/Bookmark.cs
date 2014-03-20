namespace Topics.Radical.ChangeTracking
{
	using System;
	using System.Collections.Generic;
	using Topics.Radical.Collections;
	using Topics.Radical.ComponentModel.ChangeTracking;
	using Topics.Radical.Validation;

	/// <summary>
	/// A bookmark is a placeholder to track a specific position in
	/// the change tracking history of a change tracking service.
	/// </summary>
	public class Bookmark : IBookmark
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Bookmark"/> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="position">The position.</param>
		/// <param name="transientEntities">The transient entities.</param>
		public Bookmark( IChangeTrackingService owner, IChange position, IEnumerable<Object> transientEntities )
		{
			Ensure.That( owner ).Named( "owner" ).IsNotNull();

			this.Owner = owner;
			this.Position = position;
			this.TransientEntities = transientEntities ?? new ReadOnlyCollection<Object>( new Object[ 0 ] );
		}

		/// <summary>
		/// Gets the position this bookmark has
		/// been created to.
		/// </summary>
		/// <value>The position of the bookmark.</value>
		public IChange Position
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the change tracking service that created
		/// the bookmark.
		/// </summary>
		/// <value>The owner service.</value>
		public IChangeTrackingService Owner
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the transient entities.
		/// </summary>
		/// <value>The transient entities.</value>
		public IEnumerable<Object> TransientEntities
		{
			get;
			private set;
		}
	}
}
