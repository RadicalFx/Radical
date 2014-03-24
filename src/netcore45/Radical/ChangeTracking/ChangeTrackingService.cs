namespace Topics.Radical.ChangeTracking
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using Topics.Radical.ComponentModel.ChangeTracking;
	using Topics.Radical.Linq;
	using Topics.Radical.Validation;

	/// <summary>
	/// Provides a default implementation of the <see cref="IChangeTrackingService"/>
	/// interface in order to provide change tracking functionalities.
	/// </summary>
	public class ChangeTrackingService : IChangeTrackingService
	{
		/// <summary>
		/// Provides information about the reason why 
		/// the stopTracking method has been called
		/// </summary>
		protected enum StopTrackingReason
		{
			/// <summary>
			/// The StopTracking request is due to an outside request, a user request.
			/// </summary>
			UserRequest,

			/// <summary>
			/// The StopTracking request is due to the handling of the entity disposed event.
			/// </summary>
			DisposedEvent
		}

		/// <summary>
		/// The shared synch lock
		/// </summary>
		protected static readonly Object SyncRoot = new Object();

		List<IChange> backwardChangesStack = new List<IChange>();
		List<IChange> forwardChangesStack = new List<IChange>();
		Dictionary<Object, Boolean> transientEntities = new Dictionary<Object, Boolean>();
		//List<IComponent> iComponentEntities = new List<IComponent>();

		#region IDisposable Members

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="ChangeTrackingService"/> is reclaimed by garbage collection.
		/// </summary>
		~ChangeTrackingService()
		{
			this.Dispose( false );
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( Boolean disposing )
		{
			if( disposing )
			{
				/*
				 * Se disposing è 'true' significa che dispose
				 * è stato invocato direttamentente dall'utente
				 * è quindi lecito accedere ai 'field' e ad 
				 * eventuali reference perchè sicuramente Finalize
				 * non è ancora stato chiamato su questi oggetti
				 */
				this.RejectChangesCore( false );

				this.backwardChangesStack.ForEach( c => this.OnUnwire( c ) );
				//this.iComponentEntities.ForEach( ic =>
				//{
				//	if( ic != null )
				//	{
				//		ic.Disposed -= this.onComponentDisposed;
				//	}
				//} );

				this.backwardChangesStack.Clear();
				this.forwardChangesStack.Clear();
				this.transientEntities.Clear();
				//this.iComponentEntities.Clear();

				if( this._events != null )
				{
					this.Events.Dispose();
				}
			}

			this.onChangeCommitted = null;
			this.onChangeRejected = null;
			this.onComponentDisposed = null;
			this.tryUnregisterTransient = null;

			this._events = null;
			
			this.backwardChangesStack = null;
			this.forwardChangesStack = null;
			this.transientEntities = null;
			//this.iComponentEntities = null;

			this.OnDisposed();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		#endregion

		private static readonly Object trackingServiceStateChangedEventKey = new Object();

		/// <summary>
		/// Occurs when the internal state of the tracking service changes.
		/// </summary>
		public event EventHandler TrackingServiceStateChanged
		{
			add { this.Events.AddHandler( trackingServiceStateChangedEventKey, value ); }
			remove { this.Events.RemoveHandler( trackingServiceStateChangedEventKey, value ); }
		}

		/// <summary>
		/// Raises the <c>TrackingServiceStateChanged</c> event.
		/// </summary>
		protected virtual void OnTrackingServiceStateChanged()
		{
			if( this.Events != null )
			{
				EventHandler h = this.Events[ trackingServiceStateChangedEventKey ] as EventHandler;
				if( h != null )
				{
					h( this, EventArgs.Empty );
				}
			}
		}

		private static readonly Object changesRejectedEventKey = new Object();

		/// <summary>
		/// Occurs when changes are rejected.
		/// </summary>
		public event EventHandler ChangesRejected
		{
			add { this.Events.AddHandler( changesRejectedEventKey, value ); }
			remove { this.Events.RemoveHandler( changesRejectedEventKey, value ); }
		}

		/// <summary>
		/// Raises the <c>ChangesRejected</c> event.
		/// </summary>
		protected virtual void OnChangesRejected()
		{
			if( this.Events != null )
			{
				EventHandler h = this.Events[ changesRejectedEventKey ] as EventHandler;
				if( h != null )
				{
					h( this, EventArgs.Empty );
				}
			}
		}

		private static readonly Object changesAcceptedEventKey = new Object();

		/// <summary>
		/// Occurs when are changes accepted.
		/// </summary>
		public event EventHandler ChangesAccepted
		{
			add { this.Events.AddHandler( changesAcceptedEventKey, value ); }
			remove { this.Events.RemoveHandler( changesAcceptedEventKey, value ); }
		}

		/// <summary>
		/// Raises the <c>ChangesRejected</c> event.
		/// </summary>
		protected virtual void OnChangesAccepted()
		{
			if( this.Events != null )
			{
				EventHandler h = this.Events[ changesAcceptedEventKey ] as EventHandler;
				if( h != null )
				{
					h( this, EventArgs.Empty );
				}
			}
		}

		EventHandler<CommittedEventArgs> onChangeCommitted;
		EventHandler<RejectedEventArgs> onChangeRejected;
		EventHandler onComponentDisposed;
		Action<Object, IBookmark> tryUnregisterTransient;

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangeTrackingService"/> class.
		/// </summary>
		public ChangeTrackingService()
		{
			this.onChangeCommitted = ( s, e ) =>
			{
				IChange change = ( IChange )s;
				this.OnChangeCommitted( change, e.Reason );
			};

			this.onChangeRejected = ( s, e ) =>
			{
				IChange change = ( IChange )s;
				this.OnChangeRejected( change, e.Reason );
			};

			this.onComponentDisposed = ( s, e ) =>
			{
				var entity = ( IMemento )s;
				this.OnDetach( entity, StopTrackingReason.DisposedEvent );
			};

			this.tryUnregisterTransient = ( entity, bookmark ) =>
			{
				if( bookmark == null || !bookmark.TransientEntities.Contains( entity ) )
				{
					/*
					 * Se non c'è un bookmark o se l'entity è stata registrata
					 * transient dopo la creazione del bookmark procediamo con
					 * la deregistrazione come transient.
					 */
					var state = this.GetEntityState( entity );
					var isTransient = ( state & EntityTrackingStates.IsTransient ) == EntityTrackingStates.IsTransient;
					var isAutoRemove = ( state & EntityTrackingStates.AutoRemove ) == EntityTrackingStates.AutoRemove;
					var hasBackwardChanges = ( state & EntityTrackingStates.HasBackwardChanges ) == EntityTrackingStates.HasBackwardChanges;
					var hasForwardChanges = ( state & EntityTrackingStates.HasForwardChanges ) == EntityTrackingStates.HasForwardChanges;

					if( isTransient && isAutoRemove && !hasBackwardChanges && !hasForwardChanges )
					{
						this.OnUnregisterTransient( entity );
					}
				}
			};
		}

		#region IChangeTrackingService Members

		/// <summary>
		/// Creates a bookmark usefull to save a position
		/// in this IChangeTrackingService.
		/// </summary>
		/// <returns>An <c>IBookmark</c> instance.</returns>
		/// <remarks>
		/// A bookmark is always created also if there are no changes currently registered by
		/// the change tracking service, in this case reverting to the created bookmark equals
		/// to perform a full change reject.
		/// </remarks>
		public virtual IBookmark CreateBookmark()
		{
			lock( SyncRoot )
			{
				var currentPosition = this.backwardChangesStack.LastOrDefault();
				var transientEntitiesBeforeBookmarkCreation = this.transientEntities.Keys.AsReadOnly();

				return new Bookmark( this, currentPosition, transientEntitiesBeforeBookmarkCreation );
			}
		}

		/// <summary>
		/// Reverts the status of this IChangeTrackingService
		/// to the specified bookmark.
		/// </summary>
		/// <param name="bookmark">The bookmark.</param>
		/// <exception cref="ArgumentOutOfRangeException">The specified
		/// bookmark has not been created by this service.</exception>
		public void Revert( IBookmark bookmark )
		{
			this.EnsureNotSuspended();

			Ensure.That( bookmark )
				.Named( "bookmark" )
				.IsNotNull()
				.If( bmk => !this.Validate( bmk ) )
				.Then( ( bmk, n ) => { throw new ArgumentOutOfRangeException( n ); } );

			if( this.CanRevertTo( bookmark ) )
			{
				this.OnRevert( bookmark );
				this.OnTrackingServiceStateChanged();
			}
		}

		Boolean CanRevertTo( IBookmark bookmark )
		{
			/*
			 * Siamo in grado di fare la revert:
			 * - se il bookmark è valido e...;
			 *		* se la posizione del bookmark è diversa dall'ultima modifica nel backwardChangesStack
			 *		* oppure se nel bookmark ci sono delle entity registrate transient dopo la creazione del bookmark stesso
			 */
			lock( SyncRoot )
			{
				var last = this.backwardChangesStack.LastOrDefault();
				return /*this.EnsureIsDefined( bookmark ) &&*/ ( bookmark.Position != last || this.transientEntities
					.Where( kvp => kvp.Value && !bookmark.TransientEntities.Contains( kvp.Key ) )
					.Any() );
			}
		}

		/// <summary>
		/// Called in order to reverts the status of this <see cref="IChangeTrackingService"/>
		/// to the specified bookmark.
		/// </summary>
		/// <param name="bookmark">The bookmark.</param>
		protected virtual void OnRevert( IBookmark bookmark )
		{
			lock( SyncRoot )
			{
				IChange last = this.backwardChangesStack.LastOrDefault();
				while( last != bookmark.Position )
				{
					last.Reject( RejectReason.Revert );
					last.GetChangedEntities().ForEach( entity => tryUnregisterTransient( entity, bookmark ) );

					last = this.backwardChangesStack.LastOrDefault();
				}
			}

			this.transientEntities.Keys
				.AsReadOnly()
				.ForEach( entity => tryUnregisterTransient( entity, bookmark ) );
		}

		/// <summary>
		/// Validates the specified bookmark.
		/// </summary>
		/// <param name="bookmark">The bookmark.</param>
		/// <returns><c>True</c> if the given bookmark is valid; otherwise <c>false</c>.</returns>
		public virtual Boolean Validate( IBookmark bookmark )
		{
			Ensure.That( bookmark ).Named( "bookmark" ).IsNotNull();
			return bookmark.Owner == this && ( bookmark.Position == null || this.backwardChangesStack.Contains( bookmark.Position ) );
		}

		/// <summary>
		/// Registers the supplied object as a new object.
		/// </summary>
		/// <param name="entity">The object to track as transient.</param>
		/// <exception cref="ArgumentException">If the change tracking service has already registered the object or if hhas pending changes for the object an ArgumentException is raised.</exception>
		public void RegisterTransient( Object entity )
		{
			this.RegisterTransient( entity, true );
		}

		/// <summary>
		/// Registers the supplied object as a new object.
		/// </summary>
		/// <param name="entity">The object to track as transient.</param>
		/// <param name="autoRemove">
		/// if set to <c>true</c> the object is automatically removed from the list of registered objects in 
		/// case of Undo and RejectChanges.
		/// </param>
		/// <remarks>
		/// if <c>autoRemove</c> is set to true (the default value) and RejectChnages, or an Undo that removes 
		/// the last IChange of the object, is called the object then is automatically removed from the list of 
		/// the new objects. An object marked as auto remove is not included in any advisory if it has no pending 
		/// changes.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// If the change tracking service has already registered the object or if has pending 
		/// changes for the object an ArgumentException is raised.
		/// </exception>
		public void RegisterTransient( Object entity, Boolean autoRemove )
		{
			this.EnsureNotSuspended();
			this.OnRegisterTransient( entity, autoRemove );
			this.OnTrackingServiceStateChanged();
		}

		/// <summary>
		/// Registers the supplied object as a new object.
		/// </summary>
		/// <param name="entity">The object to track as transient.</param>
		/// <param name="autoRemove">
		/// if set to <c>true</c> the object is automatically removed from the list of registered objects in 
		/// case of Undo and RejectChanges.
		/// </param>
		/// <remarks>
		/// if <c>autoRemove</c> is set to true (the default value) and RejectChnages, or an Undo that removes 
		/// the last IChange of the object, is called the object then is automatically removed from the list of 
		/// the new objects.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// If the change tracking service has already registered the object or if has pending 
		/// changes for the object an ArgumentException is raised.
		/// </exception>
		protected virtual void OnRegisterTransient( Object entity, Boolean autoRemove )
		{
			EntityTrackingStates state = this.GetEntityState( entity );
			Boolean isTransient = ( state & EntityTrackingStates.IsTransient ) == EntityTrackingStates.IsTransient;

			Ensure.That( entity )
				.Named( () => entity )
				.WithMessage( "Object already registered as transient." )
				.If( o => isTransient )
				.ThenThrow( o => new ArgumentException( o.GetFullErrorMessage() ) );

			if( this.IsInAtomicOperation )
			{
				this.AtomicOperation.RegisterTransient( entity, autoRemove );
			}
			else
			{
				//this.OnWire( entity as IComponent );
				this.transientEntities.Add( entity, autoRemove );
			}
		}

		/// <summary>
		/// Unregisters the supplied entity from the transient objects
		/// marking it as a NonTransient entity.
		/// </summary>
		/// <param name="entity">The entity to unregister.</param>
		/// <exception cref="ArgumentOutOfRangeException">If the supplied entity is not in <c>IsTransient</c> state an ArgumentException is raised.</exception>
		public void UnregisterTransient( Object entity )
		{
			this.EnsureNotSuspended();
			this.OnUnregisterTransient( entity );
			this.OnTrackingServiceStateChanged();
		}

		/// <summary>
		/// Unregisters the supplied entity from the transient objects
		/// marking it as a NonTransient entity.
		/// </summary>
		/// <param name="entity">The entity to unregister.</param>
		/// <exception cref="ArgumentOutOfRangeException">If the supplied entity is not in <c>IsTransient</c> state an ArgumentException is raised.</exception>
		protected virtual void OnUnregisterTransient( object entity )
		{
			var state = this.GetEntityState( entity );
			var isTransient = ( state & EntityTrackingStates.IsTransient ) == EntityTrackingStates.IsTransient;
			if( !isTransient )
			{
				throw new ArgumentOutOfRangeException( "Cannot unregister the given object, object is not in IsTransient state.", "entity" );
			}

			this.transientEntities.Remove( entity );
		}

		/// <summary>
		/// Gets all the entities tracked by this service instance.
		/// </summary>
		/// <returns>A enumerable list of tracked entities.</returns>
		public IEnumerable<Object> GetEntities()
		{
			return this.GetEntities( EntityTrackingStates.None, false );
		}

		/// <summary>
		/// Gets the entities.
		/// </summary>
		/// <param name="stateFilter">The state filter.</param>
		/// <param name="exactMatch">if set to <c>true</c> [exact match].</param>
		/// <returns></returns>
		public virtual IEnumerable<Object> GetEntities( EntityTrackingStates stateFilter, Boolean exactMatch )
		{
			HashSet<Object> all = new HashSet<Object>( ObjectReferenceEqualityComparer.Instance );
			transientEntities.Keys.ForEach( te => all.Add( te ) );
			this.backwardChangesStack.ForEach( c =>
			{
				/*
				 * Recuperiamo le entity modificate incapsulate
				 * in ogni IChange e le aggiugiamo all'HashSet che
				 * le inserisce "disinct" in base alla reference
				 * ergo se una reference è già stata inserita non verrà
				 * inserita una seconda volta.
				 */
				c.GetChangedEntities().ForEach( entity => all.Add( entity ) );
			} );

			var query = from entity in all
						let state = this.GetEntityState( entity )
						where exactMatch ? state == stateFilter : ( state & stateFilter ) == stateFilter
						select entity;

			return query.AsReadOnly();
		}

		/// <summary>
		/// Gets the state of the entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>
		/// A set of values from the <see cref="EntityTrackingStates"/> enumeration.
		/// </returns>
		public EntityTrackingStates GetEntityState( Object entity )
		{
			var state = EntityTrackingStates.None;

			lock( SyncRoot )
			{
				if( this.transientEntities != null && this.transientEntities.ContainsKey( entity ) )
				{
					state |= EntityTrackingStates.IsTransient;

					if( this.transientEntities[ entity ] )
					{
						state |= EntityTrackingStates.AutoRemove;
					}
				}

				if( this.backwardChangesStack != null )
				{
					var hasBackwardChanges = this.backwardChangesStack.Any( c => Object.Equals( c.Owner, entity ) );
					if( hasBackwardChanges )
					{
						state |= EntityTrackingStates.HasBackwardChanges;
					}

					var subSystemsState = this.backwardChangesStack.OfType<AtomicChange>()
						.Select( ac =>
						{
							var s = ac.GetEntityState( entity );
							if( EntityTrackingStates.HasForwardChanges == ( s & EntityTrackingStates.HasForwardChanges ) )
							{
								s = s ^ EntityTrackingStates.HasForwardChanges;
							}

							return s;
						} )
						.Aggregate( EntityTrackingStates.None, ( a, s ) => a |= s );

					state |= subSystemsState;
				}

				if( this.forwardChangesStack != null )
				{
					var hasForwardChanges = this.forwardChangesStack.Any( c => Object.Equals( c.Owner, entity ) );
					if( hasForwardChanges )
					{
						state |= EntityTrackingStates.HasForwardChanges;
					}

					var subSystemsState = this.forwardChangesStack.OfType<AtomicChange>()
						.Select( ac =>
						{
							var s = ac.GetEntityState( entity );
							if( EntityTrackingStates.HasBackwardChanges == ( s & EntityTrackingStates.HasBackwardChanges ) )
							{
								s = s ^ EntityTrackingStates.HasBackwardChanges;
							}

							return s;
						} )
						.Aggregate( EntityTrackingStates.None, ( a, s ) => a |= s );

					state |= subSystemsState;
				}
			}

			return state;
		}

		/// <summary>
		/// Called in order to perform the undo operation.
		/// </summary>
		/// <param name="reason">The reason of the undo.</param>
		/// If the RejectReason is Revert the Bookmark cannot be null.</param>
		protected virtual void OnUndo( RejectReason reason /*, IBookmark bmk */ )
		{
			lock( SyncRoot )
			{
				var last = this.backwardChangesStack.Last();

				/*
				 * Questo special case mi fa veramente cagare...
				 * dovremmo poter passare alla change il servizio 
				 * di change tracking e lasciare che sia lei a 
				 * decidere cosa fare...
				 */
				if( last is AtomicChange )
				{
					using( var tmp = this.BeginAtomicOperation( AddChangeBehavior.UndoRequest ) )
					{
						last.Reject( reason );

						/*
						 * Qui dovrebbe anche cercare di deregistrare
						 * le eventuali entità "transient" presenti 
						 * nella atomic change
						 * 
						 * Dovremmo valutare se ha senso che le transient
						 * entities vengano registrate nella AtomicChange o no
						 * a quanto pare non c'è nessuna logica legata al HasTransientEntities
						 * che abbia qualcosa a che spartire con le Atomic Operation. Dobbiamo
						 * testare un po' di scenari limite.
						 */

						tmp.Complete();
					}
				}
				else
				{
					last.Reject( reason );
					last.GetChangedEntities().ForEach( entity => tryUnregisterTransient( entity, null ) );
				}
			}
		}

		/// <summary>
		/// Called in order to perform the redo operation.
		/// </summary>
		/// <param name="reason">The reason of the redo request.</param>
		protected virtual void OnRedo( RejectReason reason )
		{
			lock( SyncRoot )
			{
				var last = this.forwardChangesStack.Last();

				/*
				 * Questo special case mi fa veramente cagare...
				 * dovremmo poter passare alla change il servizio 
				 * di change tracking e lasciare che sia lei a 
				 * decidere cosa fare...
				 */
				if( last is AtomicChange )
				{
					using( var tmp = this.BeginAtomicOperation( AddChangeBehavior.RedoRequest ) )
					{
						last.Reject( reason );
						tmp.Complete();
					}
				}
				else
				{
					last.Reject( reason );
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance can undo the last change.
		/// </summary>
		/// <value><c>true</c> if this instance can undo; otherwise, <c>false</c>.</value>
		public Boolean CanUndo
		{
			get { return this.backwardChangesStack.Count > 0; }
		}

		/// <summary>
		/// Undoes the last IChange holded by
		/// this instance and removes it from
		/// the cache.
		/// </summary>
		public void Undo()
		{
			this.EnsureNotSuspended();
			if( this.CanUndo )
			{
				this.OnUndo( RejectReason.Undo );
				this.OnTrackingServiceStateChanged();
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance has transient entities.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has transient entities; otherwise, <c>false</c>.
		/// </value>
		public Boolean HasTransientEntities
		{
			get { return this.transientEntities.Count > 0; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance can redo.
		/// </summary>
		/// <value><c>true</c> if this instance can redo; otherwise, <c>false</c>.</value>
		public Boolean CanRedo
		{
			get { return this.forwardChangesStack.Count > 0; }
		}

		/// <summary>
		/// Redoes the last undoed change.
		/// </summary>
		public void Redo()
		{
			this.EnsureNotSuspended();
			if( this.CanRedo )
			{
				this.OnRedo( RejectReason.Redo );
				this.OnTrackingServiceStateChanged();
			}
		}

		/// <summary>
		/// Gets all the changes currently holded by
		/// this IChangeTrackingService
		/// </summary>
		/// <returns></returns>
		public virtual IChangeSet GetChangeSet()
		{
			return this.GetChangeSet( IncludeAllChangeSetFilter.Instance );
		}

		/// <summary>
		/// Gets all the changes currently holded by
		/// this IChangeTrackingService filtered by the
		/// supplied IChangeSetFilter.
		/// </summary>
		/// <param name="filter">The IChangeSetFilter.</param>
		/// <returns></returns>
		public virtual IChangeSet GetChangeSet( IChangeSetFilter filter )
		{
			Ensure.That( filter ).Named( "filter" ).IsNotNull();

			lock( SyncRoot )
			{
				var included = this.backwardChangesStack.Where( c => filter.ShouldInclude( c ) );
				return new ChangeSet( included );
			}
		}

		/// <summary>
		/// Adds a new change definition to this IChangeTrackingService.
		/// </summary>
		/// <param name="change">The change to store.</param>
		/// <param name="behavior">The requested behavior.</param>
		public virtual void Add( IChange change, AddChangeBehavior behavior )
		{
			this.EnsureNotSuspended();

			Ensure.That( change )
				.Named( "change" )
				.IsNotNull();

			if( this.IsInAtomicOperation )
			{
				this.AtomicOperation.Add( change, behavior );
			}
			else
			{
				/*
				 * è necessario agganciare gli eventi
				 * Committed e Rejected su IChange perchè
				 * se viene manualmente chiamato Commit o Reject
				 * su una IChange dobbiamo liberarcene anche noi
				 */
				lock( SyncRoot )
				{
					switch( behavior )
					{
						case AddChangeBehavior.Default:
							this.forwardChangesStack.Clear();
							this.backwardChangesStack.Add( change );
							break;

						case AddChangeBehavior.RedoRequest:
							this.backwardChangesStack.Add( change );
							break;

						case AddChangeBehavior.UndoRequest:
							this.forwardChangesStack.Add( change );
							break;

						case AddChangeBehavior.None:
							throw new ArgumentOutOfRangeException();

						default:
							throw new EnumValueOutOfRangeException();
					}

					this.OnWire( change );
				}

				this.OnTrackingServiceStateChanged();
			}
		}

		/// <summary>
		/// Generates an advisory that contains all the operations that
		/// an ipothetical UnitOfWork must perform in order to persist
		/// all the changes tracked by this ChangeTrackingService.
		/// </summary>
		/// <returns>
		/// A readonly list of <see cref="IAdvisedAction"/>.
		/// </returns>
		public virtual IAdvisory GetAdvisory()
		{
			var ab = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
			var advisory = this.GetAdvisory( ab );

			return advisory;
		}

		/// <summary>
		/// Generates an advisory that contains all the operations that
		/// an ipothetical UnitOfWork must perform in order to persist
		/// all the changes tracked by this ChangeTrackingService.
		/// The generation is customized using the supplied <see cref="IAdvisoryBuilder"/>.
		/// </summary>
		/// <param name="builder">An instance of a class implementing this <see cref="IAdvisoryBuilder"/>
		/// interface used to control the advisory generation process.</param>
		/// <returns>
		/// A readonly list of <see cref="IAdvisedAction"/>.
		/// </returns>
		public virtual IAdvisory GetAdvisory( IAdvisoryBuilder builder )
		{
			Ensure.That( builder ).Named( "builder" ).IsNotNull();

			var cSet = this.GetChangeSet();
			var advisory = builder.GenerateAdvisory( this, cSet );

			return advisory;
		}

		/// <summary>
		/// Called when the change tracking service needs to add handlers to the <c>IChange</c> supplied as parameter.
		/// </summary>
		/// <param name="change">The change to wire to.</param>
		protected virtual void OnWire( IChange change )
		{
			change.Committed += this.onChangeCommitted;
			change.Rejected += this.onChangeRejected;

			//this.OnWire( change.Owner as IComponent );
		}

		///// <summary>
		///// Called when the change tracking service needs to add handlers to the <c>IComponent</c> supplied as parameter.
		///// </summary>
		///// <param name="entity">The entity to wire to.</param>
		//protected virtual void OnWire( IComponent entity )
		//{
		//	if( entity != null )
		//	{
		//		if( !this.iComponentEntities.Contains( entity ) )
		//		{
		//			entity.Disposed += this.onComponentDisposed;
		//			this.iComponentEntities.Add( entity );
		//		}
		//	}
		//}

		/// <summary>
		/// Called when the change tracking service needs to remove handlers from the <c>IChange</c> supplied as parameter.
		/// </summary>
		/// <param name="change">The change to unwire from.</param>
		protected virtual void OnUnwire( IChange change )
		{
			change.Committed -= this.onChangeCommitted;
			change.Rejected -= this.onChangeRejected;
		}

		/// <summary>
		/// Called when change(s) have been committed from the <c>IChange</c> instance.
		/// </summary>
		/// <param name="change">The committed change.</param>
		/// <param name="reason">The reason of the commit.</param>
		protected virtual void OnChangeCommitted( IChange change, CommitReason reason )
		{
			this.OnUnwire( change );

			switch( reason )
			{
				case CommitReason.AcceptChanges:

					lock( SyncRoot )
					{
						this.backwardChangesStack.Remove( change );
					}

					break;

				case CommitReason.None:
					throw new ArgumentOutOfRangeException( "reason" );

				default:
					throw new EnumValueOutOfRangeException();
			}
		}

		/// <summary>
		/// Handles the change rejected event.
		/// </summary>
		/// <param name="change">The change that raises the rejected event.</param>
		/// <param name="reason">The reason of the event raise.</param>
		protected virtual void OnChangeRejected( IChange change, RejectReason reason )
		{
			this.OnUnwire( change );

			switch( reason )
			{
				case RejectReason.Undo:
				case RejectReason.RejectChanges:
				case RejectReason.Revert:

					lock( SyncRoot )
					{
						this.backwardChangesStack.Remove( change );
					}

					break;

				case RejectReason.Redo:

					lock( SyncRoot )
					{
						this.forwardChangesStack.Remove( change );
					}

					break;

				case RejectReason.None:
					throw new ArgumentOutOfRangeException();

				default:
					throw new EnumValueOutOfRangeException();
			}
		}

		/// <summary>
		/// Stops tracking the supplied entities removing any changes linked to the entity
		/// and removing it, if necessary, from the transient entities.
		/// </summary>
		/// <param name="entity">The entity to stop tracking.</param>
		/// <param name="reason">The reason why OnStopTracking has been called.</param>
		protected virtual void OnDetach( IMemento entity, StopTrackingReason reason )
		{
			EntityTrackingStates state = this.GetEntityState( entity );

			Boolean isTransient = ( state & EntityTrackingStates.IsTransient ) == EntityTrackingStates.IsTransient;
			Boolean hasBackwardChanges = ( state & EntityTrackingStates.HasBackwardChanges ) == EntityTrackingStates.HasBackwardChanges;
			Boolean hasForwardChanges = ( state & EntityTrackingStates.HasForwardChanges ) == EntityTrackingStates.HasForwardChanges;

			/*
			 * questo handler potrebbe venire invocato in maniera un po' strana 
			 * perchè il programmatore che ci usa non fa buon uso delle Dispose
			 * e/o del costrutto using, come ad esempio simuliamo negli unit 
			 * test(s), quindi le Dispose vengono chiamate in ordine casuale dal 
			 * GC...
			 */
			//IComponent cmp = entity as IComponent;
			//if( cmp != null )
			//{
			//	cmp.Disposed -= this.onComponentDisposed;
			//	if( this.iComponentEntities != null && this.iComponentEntities.Contains( cmp ) )
			//	{
			//		this.iComponentEntities.Remove( cmp );
			//	}
			//}

			if( reason == StopTrackingReason.UserRequest )
			{
				/*
				 * Se arriviamo qui a seguito del Disposed event non
				 * possiamo accedere ai membri della Entity perchè 
				 * rischiamo, giustamente, una ObjectDisposedException 
				 * quidi lo facciamo solo ed esclusivamente se la richiesta
				 * di StopTracking è una richiesta esplicita dell'utente.
				 */
				entity.Memento = null;
			}

			if( isTransient )
			{
				this.OnUnregisterTransient( entity );
			}

			if( hasBackwardChanges && this.backwardChangesStack != null )
			{
				lock( SyncRoot )
				{
					this.backwardChangesStack
						.Where( c => Object.Equals( c.Owner, entity ) )
						.AsReadOnly()
						.ForEach( c => this.backwardChangesStack.Remove( c ) );
				}
			}

			if( hasForwardChanges && this.forwardChangesStack != null )
			{
				lock( SyncRoot )
				{
					this.forwardChangesStack
						.Where( c => Object.Equals( c.Owner, entity ) )
						.AsReadOnly()
						.ForEach( c => this.forwardChangesStack.Remove( c ) );
				}
			}

			if( ( isTransient || hasBackwardChanges || hasForwardChanges ) )
			{
				this.OnTrackingServiceStateChanged();
			}
		}

		/// <summary>
		/// Ensures the this service instance is not suspended.
		/// </summary>
		/// <exception cref="SuspendedChangeTrackingServiceException">A 
		/// <c>SuspendedChangeTrackingServiceException</c> is raised if thsi instance is in a suspended state.</exception>
		protected void EnsureNotSuspended()
		{
			if( this.IsSuspended )
			{
				throw new SuspendedChangeTrackingServiceException();
			}
		}

		/// <summary>
		/// Suspends all the tracking operation of this service.
		/// </summary>
		public virtual void Suspend()
		{
			this.IsSuspended = true;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is suspended.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is suspended; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsSuspended
		{
			get;
			private set;
		}

		/// <summary>
		/// Resumes all the tracking operation of this service.
		/// </summary>
		public virtual void Resume()
		{
			this.IsSuspended = false;
		}

		/// <summary>
		/// Stops tracking the supplied entities removing any changes linked to the entity
		/// and removing it, if necessary, from the transient entities.
		/// </summary>
		/// <param name="entity">The entity to stop tracking.</param>
		public void Detach( IMemento entity )
		{
			this.EnsureNotSuspended();

			this.OnDetach( entity, StopTrackingReason.UserRequest );
		}

		/// <summary>
		/// Attaches the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Attach( IMemento item )
		{
			Ensure.That( item )
				.Named( () => item )
				.IsNotNull()
				.If( obj => obj.Memento != null && obj.Memento != this )
				.ThenThrow( obj =>
				{
					var msg = obj.GetFullErrorMessage( "The supplied memento is already attached to another Change Tracking Service, before attaching to this service the memento object must be detached from the other tracking service." );
					throw new NotSupportedException( msg );
				} );

			this.OnAttach( item );
		}

		/// <summary>
		/// Called in order to attach the supplied entity to this tracking service.
		/// </summary>
		/// <param name="item">The item to attach.</param>
		protected virtual void OnAttach( IMemento item )
		{
			item.Memento = this;
		}

		#endregion

		#region IRevertibleChangeTracking Members

		void RejectChangesCore( Boolean shouldNotify )
		{
			while( this.IsChanged )
			{
				this.OnUndo( RejectReason.RejectChanges );
			}

			if( this.HasTransientEntities )
			{
				this.transientEntities.Clear();
			}

			if( shouldNotify )
			{
				this.OnTrackingServiceStateChanged();
				this.OnChangesRejected();
			}
		}

		/// <summary>
		/// Resets the object’s state to unchanged by rejecting the modifications.
		/// </summary>
		public virtual void RejectChanges()
		{
			this.EnsureNotSuspended();
			this.RejectChangesCore( this.IsChanged || this.HasTransientEntities );
		}

		#endregion

		#region IChangeTracking Members

		/// <summary>
		/// Resets the object’s state to unchanged by accepting the modifications.
		/// </summary>
		public virtual void AcceptChanges()
		{
			this.EnsureNotSuspended();

			var shouldNotify = this.IsChanged || this.HasTransientEntities;
			if( this.IsChanged )
			{
				/*
				 * In caso di Commit vengono confermate
				 * nell'ordine in cui sono state fatte
				 */
				this.backwardChangesStack
					.Where( change => change.IsCommitSupported )
					.AsReadOnly()
					.ForEach( change => change.Commit( CommitReason.AcceptChanges ) );

				this.backwardChangesStack.Clear();
			}

			if( this.HasTransientEntities )
			{
				this.transientEntities.Clear();
			}

			if( shouldNotify )
			{
				this.OnTrackingServiceStateChanged();
				this.OnChangesAccepted();
			}
		}

		/// <summary>
		/// Gets the object's changed status.
		/// </summary>
		/// <value></value>
		/// <returns>true if the object’s content has changed since the last call to <see cref="M:System.ComponentModel.IChangeTracking.AcceptChanges"/>; otherwise, false.</returns>
		public virtual bool IsChanged
		{
			get { return this.backwardChangesStack.Count > 0; }
		}

		#endregion

		#region IComponent, ISite

		/// <summary>
		/// Event raised when the Dispose of this instance has completed.
		/// </summary>
		public event EventHandler Disposed;

		/// <summary>
		/// Raises the Disposed event.
		/// </summary>
		protected virtual void OnDisposed()
		{
			if( this.Disposed != null )
			{
				this.Disposed( this, EventArgs.Empty );
			}
		}

		private EventHandlerList _events = new EventHandlerList();

		/// <summary>
		/// Gets the events.
		/// </summary>
		/// <value>The events.</value>
		protected EventHandlerList Events
		{
			get { return this._events; }
		}

		#endregion

		Boolean IsInAtomicOperation
		{
			get { return this.AtomicOperation != null; }
		}

		AtomicOperation AtomicOperation;

		IAtomicOperation BeginAtomicOperation( AddChangeBehavior behavior )
		{
			Ensure.That( this.IsInAtomicOperation )
				.WithMessage( "Only one single atomic operation can be created at a time." )
				.Is( false );

			Action<AtomicChange> completed = c =>
			{
				this.AtomicOperation = null;
				this.Add( c, behavior );

				/*
				 * Qui potrebbe aver senso recuperare dalla IChange
				 * tutte le transient entities e travasarle nelle transient entities
				 * locali.
				 */
				c.MergeTransientEntities( this.transientEntities );
			};

			Action disposed = () =>
			{
				this.AtomicOperation = null;
			};

			this.AtomicOperation = new AtomicOperation( completed, disposed );

			return this.AtomicOperation;
		}

		/// <summary>
		/// Begins a new atomic operation. An atomic operation is usefull to
		/// treat a set of subsequent changes as a single change.
		/// </summary>
		/// <returns>The newly created atomic operation.</returns>
		/// <exception cref="ArgumentException">An <c>ArgumentException</c> is raised if there
		/// is another active atomic operation.</exception>
		public IAtomicOperation BeginAtomicOperation()
		{
			return this.BeginAtomicOperation( AddChangeBehavior.Default );
		}
	}
}
