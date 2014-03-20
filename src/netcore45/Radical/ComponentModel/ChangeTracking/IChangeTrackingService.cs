namespace Topics.Radical.ComponentModel.ChangeTracking
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	/// Provides change tracking functionalities.
	/// </summary>
	public interface IChangeTrackingService : IRevertibleChangeTracking, IDisposable
	{
		/// <summary>
		/// Occurs when the internal state of the tracking service changes.
		/// </summary>
		event EventHandler TrackingServiceStateChanged;

		/// <summary>
		/// Occurs when are changes accepted.
		/// </summary>
		event EventHandler ChangesAccepted;

		/// <summary>
		/// Occurs when changes are rejected.
		/// </summary>
		event EventHandler ChangesRejected;

		/// <summary>
		/// Creates a bookmark usefull to save a position
		/// in this IChangeTrackingService.
		/// </summary>
		/// <remarks>
		/// A bookmark is always created also if there are no changes currently registered by
		/// the change tracking service, in this case reverting to the created bookmark equals
		/// to perform a full change reject.
		/// </remarks>
		/// <returns>An <c>IBookmark</c> instance.</returns>
		IBookmark CreateBookmark();

		/// <summary>
		/// Reverts the status of this IChangeTrackingService
		/// to the specified bookmark.
		/// </summary>
		/// <param name="bookmark">The bookmark.</param>
		/// <exception cref="ArgumentOutOfRangeException">The specified 
		/// bookmark has not been created by this service.</exception>
		void Revert( IBookmark bookmark );

		/// <summary>
		/// Validates the specified bookmark.
		/// </summary>
		/// <param name="bookmark">The bookmark.</param>
		/// <returns><c>True</c> if the given bookmark is valid; otherwise <c>false</c>.</returns>
		Boolean Validate( IBookmark bookmark );

		/// <summary>
		/// Registers the supplied object as a new object.
		/// </summary>
		/// <param name="entity">The object to track as transient.</param>
		/// <exception cref="ArgumentException">If thew change tracking service has already registered the object or if hhas pending changes for the object an ArgumentException is raised.</exception>
		void RegisterTransient( Object entity );

		/// <summary>
		/// Registers the supplied entity as a new object.
		/// </summary>
		/// <param name="entity">The object to track as transient.</param>
		/// <param name="autoRemove">if set to <c>true</c> the object is automatically removed from the list 
		/// of registered objects in case of Undo and RejectChanges.</param>
		/// <remarks>if <c>autoRemove</c> is set to true (the default value) and RejectChanges, 
		/// or an Undo that removes the last IChange of the object, is called the object then is automatically 
		/// removed from the list of the new objects.</remarks>
		/// <exception cref="ArgumentException">If the change tracking service has already registered the object or if has pending changes for the object an ArgumentException is raised.</exception>
		void RegisterTransient( Object entity, Boolean autoRemove );

		/// <summary>
		/// Unregisters the supplied entity from the transient objects 
		/// marking it as a NonTransient entity.
		/// </summary>
		/// <param name="entity">The entity to unregister.</param>
		/// <exception cref="ArgumentOutOfRangeException">If the supplied entity is not in <c>IsTransient</c> state an ArgumentException is raised.</exception>
		void UnregisterTransient( Object entity );

		/// <summary>
		/// Gets a value indicating whether this instance has transient entities.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has transient entities; otherwise, <c>false</c>.
		/// </value>
		Boolean HasTransientEntities { get; }

		/// <summary>
		/// Gets the state of the entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>A set of values from the <see cref="EntityTrackingStates"/> enumeration.</returns>
		EntityTrackingStates GetEntityState( Object entity );

		/// <summary>
		/// Gets all the entities tracked by this service instance.
		/// </summary>
		/// <returns>A enumerable list of tracked entities.</returns>
		IEnumerable<Object> GetEntities();

		/// <summary>
		/// Gets a list of entities based on a filter.
		/// </summary>
		/// <param name="sateFilter">The sate filter to use to search entities.</param>
		/// <param name="exactMatch">if set to <c>true</c> the search is performed using an exact match behavior.</param>
		/// <returns>An enumerable list of entities that matches the filter.</returns>
		IEnumerable<Object> GetEntities( EntityTrackingStates sateFilter, Boolean exactMatch );

		/// <summary>
		/// Gets a value indicating whether this instance can undo the last change.
		/// </summary>
		/// <value><c>true</c> if this instance can undo; otherwise, <c>false</c>.</value>
		Boolean CanUndo { get; }

		/// <summary>
		/// Undoes the last IChange holded by 
		/// this instance and removes it from
		/// the cache.
		/// </summary>
		void Undo();

		/// <summary>
		/// Gets a value indicating whether this instance can redo.
		/// </summary>
		/// <value><c>true</c> if this instance can redo; otherwise, <c>false</c>.</value>
		Boolean CanRedo { get; }

		/// <summary>
		/// Redoes the last undoed change.
		/// </summary>
		void Redo();

		/// <summary>
		/// Gets all the changes currently holded by
		/// this IChangeTrackingService
		/// </summary>
		/// <returns></returns>
		IChangeSet GetChangeSet();

		/// <summary>
		/// Gets all the changes currently holded by
		/// this IChangeTrackingService filtered by the
		/// supplied IChangeSetBuilder.
		/// </summary>
		/// <param name="builder">The IChangeSetBuilder.</param>
		/// <returns></returns>
		IChangeSet GetChangeSet( IChangeSetFilter builder );

		/// <summary>
		/// Adds a new change definition to this IChangeTrackingService.
		/// </summary>
		/// <param name="change">The change to store.</param>
		/// <param name="behavior">The requested behavior.</param>
		void Add( IChange change, AddChangeBehavior behavior );

		/// <summary>
		/// Generates an advisory that contains all the operations that
		/// an ipothetical UnitOfWork must perform in order to persist
		/// all the changes tracked by this ChangeTrackingService.
		/// </summary>
		/// <returns>A readonly list of <see cref="IAdvisedAction"/>.</returns>
		IAdvisory GetAdvisory();

		/// <summary>
		/// Generates an advisory that contains all the operations that
		/// an ipothetical UnitOfWork must perform in order to persist
		/// all the changes tracked by this ChangeTrackingService.
		/// The generation is customized using the supplied <see cref="IAdvisoryBuilder"/>.
		/// </summary>
		/// <param name="builder">An instance of a class implementing this <see cref="IAdvisoryBuilder"/> 
		/// interface used to control the advisory generation process.</param>
		/// <returns>A readonly list of <see cref="IAdvisedAction"/>.</returns>
		IAdvisory GetAdvisory( IAdvisoryBuilder builder );

		/// <summary>
		/// Suspends all the tracking operation of this service.
		/// </summary>
		void Suspend();

		/// <summary>
		/// Gets a value indicating whether this instance is suspended.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is suspended; otherwise, <c>false</c>.
		/// </value>
		Boolean IsSuspended { get; }

		/// <summary>
		/// Resumes all the tracking operation of this service.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Resume" )]
		void Resume();

		/// <summary>
		/// Stops tracking the supplied entities removing any changes linked to the entity 
		/// and removing it, if necessary, from the transient entities.
		/// </summary>
		/// <param name="entity">The entity to stop tracking.</param>
		void Detach( IMemento entity );

		/// <summary>
		/// Attaches the specified item.
		/// </summary>
		/// <param name="item">The item to attach.</param>
		void Attach( IMemento item );

		/// <summary>
		/// Begins a new atomic operation. An atomic operation is usefull to
		/// treat a set of subsequent changes as a single change.
		/// </summary>
		/// <exception cref="ArgumentException">An <c>ArgumentException</c> is raised if there
		/// is another active atomic operation.</exception>
		/// <returns>The newly created atomic operation.</returns>
		IAtomicOperation BeginAtomicOperation();
	}
}
