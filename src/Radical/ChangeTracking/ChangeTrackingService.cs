namespace Radical.ChangeTracking
{
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Linq;
    using Radical.Validation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides a default implementation of the <see cref="IChangeTrackingService"/>
    /// interface in order to provide change tracking functionalities.
    /// </summary>
    [ToolboxItem(false)]
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
        /// The shared sync lock
        /// </summary>
        protected static readonly object SyncRoot = new object();

        List<IChange> backwardChangesStack = new List<IChange>();
        List<IChange> forwardChangesStack = new List<IChange>();
        Dictionary<object, bool> transientEntities = new Dictionary<object, bool>();
        List<IComponent> iComponentEntities = new List<IComponent>();


        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ChangeTrackingService"/> is reclaimed by garbage collection.
        /// </summary>
        ~ChangeTrackingService()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    /*
                     * Se disposing è 'true' significa che dispose
                     * è stato invocato direttamentente dall'utente
                     * è quindi lecito accedere ai 'field' e ad 
                     * eventuali reference perchè sicuramente Finalize
                     * non è ancora stato chiamato su questi oggetti
                     * 
                     * Staminchia...
                     */
                    //this.RejectChangesCore( false );

                    backwardChangesStack.ForEach(c => OnUnwire(c));
                    forwardChangesStack.ForEach(c => OnUnwire(c));

                    iComponentEntities.ForEach(ic =>
                    {
                        if (ic != null)
                        {
                            ic.Disposed -= onComponentDisposed;
                        }
                    });

                    backwardChangesStack.Clear();
                    forwardChangesStack.Clear();
                    transientEntities.Clear();
                    iComponentEntities.Clear();

                    if (Site != null && Site.Container != null)
                    {
                        Site.Container.Remove(this);
                    }

                    if (_events != null)
                    {
                        Events.Dispose();
                    }
                }

                onChangeCommitted = null;
                onChangeRejected = null;
                onComponentDisposed = null;
                tryUnregisterTransient = null;

                _events = null;
                Site = null;

                backwardChangesStack = null;
                forwardChangesStack = null;
                transientEntities = null;
                iComponentEntities = null;

                IsDisposed = true;
            }

            OnDisposed();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private static readonly object trackingServiceStateChangedEventKey = new object();

        /// <summary>
        /// Occurs when the internal state of the tracking service changes.
        /// </summary>
        public event EventHandler TrackingServiceStateChanged
        {
            add { Events.AddHandler(trackingServiceStateChangedEventKey, value); }
            remove { Events.RemoveHandler(trackingServiceStateChangedEventKey, value); }
        }

        /// <summary>
        /// Raises the <c>TrackingServiceStateChanged</c> event.
        /// </summary>
        protected virtual void OnTrackingServiceStateChanged()
        {
            if (Events != null)
            {
                EventHandler h = Events[trackingServiceStateChangedEventKey] as EventHandler;
                if (h != null)
                {
                    h(this, EventArgs.Empty);
                }
            }
        }

        private static readonly object rejectingChangesEventKey = new object();

        /// <summary>
        /// Occurs when changes are rejected.
        /// </summary>
        public event EventHandler<CancelEventArgs> RejectingChanges
        {
            add { Events.AddHandler(rejectingChangesEventKey, value); }
            remove { Events.RemoveHandler(rejectingChangesEventKey, value); }
        }

        /// <summary>
        /// Raises the <c>ChangesRejected</c> event.
        /// </summary>
        protected virtual void OnRejectingChanges(CancelEventArgs e)
        {
            if (Events != null)
            {
                EventHandler<CancelEventArgs> h = Events[rejectingChangesEventKey] as EventHandler<CancelEventArgs>;
                if (h != null)
                {
                    h(this, e);
                }
            }
        }

        private static readonly object changesRejectedEventKey = new object();

        /// <summary>
        /// Occurs when changes are rejected.
        /// </summary>
        public event EventHandler ChangesRejected
        {
            add { Events.AddHandler(changesRejectedEventKey, value); }
            remove { Events.RemoveHandler(changesRejectedEventKey, value); }
        }

        /// <summary>
        /// Raises the <c>ChangesRejected</c> event.
        /// </summary>
        protected virtual void OnChangesRejected()
        {
            if (Events != null)
            {
                EventHandler h = Events[changesRejectedEventKey] as EventHandler;
                if (h != null)
                {
                    h(this, EventArgs.Empty);
                }
            }
        }

        private static readonly object acceptingChangesEventKey = new object();

        public event EventHandler<CancelEventArgs> AcceptingChanges
        {
            add { Events.AddHandler(acceptingChangesEventKey, value); }
            remove { Events.RemoveHandler(acceptingChangesEventKey, value); }
        }

        protected virtual void OnAcceptingChanges(CancelEventArgs e)
        {
            if (Events != null)
            {
                EventHandler<CancelEventArgs> h = Events[acceptingChangesEventKey] as EventHandler<CancelEventArgs>;
                if (h != null)
                {
                    h(this, e);
                }
            }
        }

        private static readonly object changesAcceptedEventKey = new object();

        /// <summary>
        /// Occurs when are changes accepted.
        /// </summary>
        public event EventHandler ChangesAccepted
        {
            add { Events.AddHandler(changesAcceptedEventKey, value); }
            remove { Events.RemoveHandler(changesAcceptedEventKey, value); }
        }

        /// <summary>
        /// Raises the <c>ChangesRejected</c> event.
        /// </summary>
        protected virtual void OnChangesAccepted()
        {
            if (Events != null)
            {
                EventHandler h = Events[changesAcceptedEventKey] as EventHandler;
                if (h != null)
                {
                    h(this, EventArgs.Empty);
                }
            }
        }

        EventHandler<CommittedEventArgs> onChangeCommitted;
        EventHandler<RejectedEventArgs> onChangeRejected;
        EventHandler onComponentDisposed;
        Action<object, IBookmark> tryUnregisterTransient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTrackingService"/> class.
        /// </summary>
        public ChangeTrackingService()
        {
            onChangeCommitted = (s, e) =>
            {
                IChange change = (IChange)s;
                OnChangeCommitted(change, e.Reason);
            };

            onChangeRejected = (s, e) =>
            {
                IChange change = (IChange)s;
                OnChangeRejected(change, e.Reason);
            };

            onComponentDisposed = (s, e) =>
            {
                var entity = (IMemento)s;
                OnDetach(entity, StopTrackingReason.DisposedEvent);
            };

            tryUnregisterTransient = (entity, bookmark) =>
            {
                if (bookmark == null || !bookmark.TransientEntities.Contains(entity))
                {
                    /*
                     * Se non c'è un bookmark o se l'entity è stata registrata
                     * transient dopo la creazione del bookmark procediamo con
                     * la deregistrazione come transient.
                     */
                    var state = GetEntityState(entity);
                    var isTransient = (state & EntityTrackingStates.IsTransient) == EntityTrackingStates.IsTransient;
                    var isAutoRemove = (state & EntityTrackingStates.AutoRemove) == EntityTrackingStates.AutoRemove;
                    var hasBackwardChanges = (state & EntityTrackingStates.HasBackwardChanges) == EntityTrackingStates.HasBackwardChanges;
                    var hasForwardChanges = (state & EntityTrackingStates.HasForwardChanges) == EntityTrackingStates.HasForwardChanges;

                    if (isTransient && isAutoRemove && !hasBackwardChanges && !hasForwardChanges)
                    {
                        OnUnregisterTransient(entity);
                    }
                }
            };
        }


        /// <summary>
        /// Creates a bookmark useful to save a position
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
            lock (SyncRoot)
            {
                var currentPosition = backwardChangesStack.LastOrDefault();
                var transientEntitiesBeforeBookmarkCreation = transientEntities.Keys.AsReadOnly();

                return new Bookmark(this, currentPosition, transientEntitiesBeforeBookmarkCreation);
            }
        }

        /// <summary>
        /// Reverts the status of this IChangeTrackingService
        /// to the specified bookmark.
        /// </summary>
        /// <param name="bookmark">The bookmark.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified
        /// bookmark has not been created by this service.</exception>
        public void Revert(IBookmark bookmark)
        {
            EnsureNotSuspended();

            Ensure.That(bookmark)
                .Named("bookmark")
                .IsNotNull()
                .If(bmk => !Validate(bmk))
                .Then((bmk, n) => { throw new ArgumentOutOfRangeException(n); });

            if (CanRevertTo(bookmark))
            {
                OnRevert(bookmark);
                OnTrackingServiceStateChanged();
            }
        }

        bool CanRevertTo(IBookmark bookmark)
        {
            /*
             * Siamo in grado di fare la revert:
             * - se il bookmark è valido e...;
             *        * se la posizione del bookmark è diversa dall'ultima modifica nel backwardChangesStack
             *        * oppure se nel bookmark ci sono delle entity registrate transient dopo la creazione del bookmark stesso
             */
            lock (SyncRoot)
            {
                var last = backwardChangesStack.LastOrDefault();
                return /*this.EnsureIsDefined( bookmark ) &&*/ (bookmark.Position != last || transientEntities
                    .Any(kvp => kvp.Value && !bookmark.TransientEntities.Contains(kvp.Key)));
            }
        }

        /// <summary>
        /// Called in order to reverts the status of this <see cref="IChangeTrackingService"/>
        /// to the specified bookmark.
        /// </summary>
        /// <param name="bookmark">The bookmark.</param>
        protected virtual void OnRevert(IBookmark bookmark)
        {
            lock (SyncRoot)
            {
                IChange last = backwardChangesStack.LastOrDefault();
                while (last != bookmark.Position)
                {
                    last.Reject(RejectReason.Revert);
                    last.GetChangedEntities().ForEach(entity => tryUnregisterTransient(entity, bookmark));

                    last = backwardChangesStack.LastOrDefault();
                }
            }

            transientEntities.Keys
                .AsReadOnly()
                .ForEach(entity => tryUnregisterTransient(entity, bookmark));
        }

        /// <summary>
        /// Validates the specified bookmark.
        /// </summary>
        /// <param name="bookmark">The bookmark.</param>
        /// <returns><c>True</c> if the given bookmark is valid; otherwise <c>false</c>.</returns>
        public virtual bool Validate(IBookmark bookmark)
        {
            Ensure.That(bookmark).Named("bookmark").IsNotNull();
            return bookmark.Owner == this && (bookmark.Position == null || backwardChangesStack.Contains(bookmark.Position));
        }

        /// <summary>
        /// Registers the supplied object as a new object.
        /// </summary>
        /// <param name="entity">The object to track as transient.</param>
        /// <exception cref="ArgumentException">If the change tracking service has already registered the object or if has pending changes for the object an ArgumentException is raised.</exception>
        public void RegisterTransient(object entity)
        {
            RegisterTransient(entity, true);
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
        public void RegisterTransient(object entity, bool autoRemove)
        {
            EnsureNotSuspended();
            OnRegisterTransient(entity, autoRemove);
            OnTrackingServiceStateChanged();
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
        protected virtual void OnRegisterTransient(object entity, bool autoRemove)
        {
            EntityTrackingStates state = GetEntityState(entity);
            bool isTransient = (state & EntityTrackingStates.IsTransient) == EntityTrackingStates.IsTransient;

            Ensure.That(entity)
                .Named(() => entity)
                .WithMessage("Object already registered as transient.")
                .If(o => isTransient)
                .ThenThrow(o => new ArgumentException(o.GetFullErrorMessage()));

            if (IsInAtomicOperation)
            {
                AtomicOperation.RegisterTransient(entity, autoRemove);
            }
            else
            {
                OnWire(entity as IComponent);
                transientEntities.Add(entity, autoRemove);
            }
        }

        /// <summary>
        /// Unregisters the supplied entity from the transient objects
        /// marking it as a NonTransient entity.
        /// </summary>
        /// <param name="entity">The entity to unregister.</param>
        /// <exception cref="ArgumentOutOfRangeException">If the supplied entity is not in <c>IsTransient</c> state an ArgumentException is raised.</exception>
        public void UnregisterTransient(object entity)
        {
            EnsureNotSuspended();
            OnUnregisterTransient(entity);
            OnTrackingServiceStateChanged();
        }

        /// <summary>
        /// Unregisters the supplied entity from the transient objects
        /// marking it as a NonTransient entity.
        /// </summary>
        /// <param name="entity">The entity to unregister.</param>
        /// <exception cref="ArgumentOutOfRangeException">If the supplied entity is not in <c>IsTransient</c> state an ArgumentException is raised.</exception>
        protected virtual void OnUnregisterTransient(object entity)
        {
            var state = GetEntityState(entity);
            var isTransient = (state & EntityTrackingStates.IsTransient) == EntityTrackingStates.IsTransient;
            if (!isTransient)
            {
                throw new ArgumentOutOfRangeException("Cannot unregister the given object, object is not in IsTransient state.", "entity");
            }

            transientEntities.Remove(entity);
        }

        /// <summary>
        /// Gets all the entities tracked by this service instance.
        /// </summary>
        /// <returns>A enumerable list of tracked entities.</returns>
        public IEnumerable<object> GetEntities()
        {
            return GetEntities(EntityTrackingStates.None, false);
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="stateFilter">The state filter.</param>
        /// <param name="exactMatch">if set to <c>true</c> [exact match].</param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetEntities(EntityTrackingStates stateFilter, bool exactMatch)
        {
            HashSet<object> all = new HashSet<object>(ObjectReferenceEqualityComparer.Instance);
            transientEntities.Keys.ForEach(te => all.Add(te));
            backwardChangesStack.ForEach(c =>
           {
               /*
                * Recuperiamo le entity modificate incapsulate
                * in ogni IChange e le aggiugiamo all'HashSet che
                * le inserisce "disinct" in base alla reference
                * ergo se una reference è già stata inserita non verrà
                * inserita una seconda volta.
                */
               c.GetChangedEntities().ForEach(entity => all.Add(entity));
           });

            var query = from entity in all
                        let state = GetEntityState(entity)
                        where exactMatch ? state == stateFilter : (state & stateFilter) == stateFilter
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
        public EntityTrackingStates GetEntityState(object entity)
        {
            var state = EntityTrackingStates.None;

            lock (SyncRoot)
            {
                if (transientEntities != null && transientEntities.ContainsKey(entity))
                {
                    state |= EntityTrackingStates.IsTransient;

                    if (transientEntities[entity])
                    {
                        state |= EntityTrackingStates.AutoRemove;
                    }
                }

                if (backwardChangesStack != null)
                {
                    var hasBackwardChanges = backwardChangesStack.Any(c => Object.Equals(c.Owner, entity));
                    if (hasBackwardChanges)
                    {
                        state |= EntityTrackingStates.HasBackwardChanges;
                    }

                    var subSystemsState = backwardChangesStack.OfType<AtomicChange>()
                        .Select(ac =>
                       {
                           var s = ac.GetEntityState(entity);
                           if (EntityTrackingStates.HasForwardChanges == (s & EntityTrackingStates.HasForwardChanges))
                           {
                               s = s ^ EntityTrackingStates.HasForwardChanges;
                           }

                           return s;
                       })
                        .Aggregate(EntityTrackingStates.None, (a, s) => a |= s);

                    state |= subSystemsState;
                }

                if (forwardChangesStack != null)
                {
                    var hasForwardChanges = forwardChangesStack.Any(c => Object.Equals(c.Owner, entity));
                    if (hasForwardChanges)
                    {
                        state |= EntityTrackingStates.HasForwardChanges;
                    }

                    var subSystemsState = forwardChangesStack.OfType<AtomicChange>()
                        .Select(ac =>
                       {
                           var s = ac.GetEntityState(entity);
                           if (EntityTrackingStates.HasBackwardChanges == (s & EntityTrackingStates.HasBackwardChanges))
                           {
                               s = s ^ EntityTrackingStates.HasBackwardChanges;
                           }

                           return s;
                       })
                        .Aggregate(EntityTrackingStates.None, (a, s) => a |= s);

                    state |= subSystemsState;
                }
            }

            return state;
        }

        /// <summary>
        /// Called in order to perform the undo operation.
        /// </summary>
        /// <param name="reason">
        /// The reason of the undo. If the RejectReason is Revert 
        /// the Bookmark cannot be null.
        /// </param>
        protected virtual void OnUndo(RejectReason reason /*, IBookmark bmk */ )
        {
            lock (SyncRoot)
            {
                var last = backwardChangesStack.Last();

                /*
                 * Questo special case mi fa veramente cagare...
                 * dovremmo poter passare alla change il servizio 
                 * di change tracking e lasciare che sia lei a 
                 * decidere cosa fare...
                 */
                if (last is AtomicChange)
                {
                    using (var tmp = BeginAtomicOperation(AddChangeBehavior.UndoRequest))
                    {
                        last.Reject(reason);

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
                    last.Reject(reason);
                    last.GetChangedEntities().ForEach(entity => tryUnregisterTransient(entity, null));
                }
            }
        }

        /// <summary>
        /// Called in order to perform the redo operation.
        /// </summary>
        /// <param name="reason">The reason of the redo request.</param>
        protected virtual void OnRedo(RejectReason reason)
        {
            lock (SyncRoot)
            {
                var last = forwardChangesStack.Last();

                /*
                 * Questo special case mi fa veramente cagare...
                 * dovremmo poter passare alla change il servizio 
                 * di change tracking e lasciare che sia lei a 
                 * decidere cosa fare...
                 */
                if (last is AtomicChange)
                {
                    using (var tmp = BeginAtomicOperation(AddChangeBehavior.RedoRequest))
                    {
                        last.Reject(reason);
                        tmp.Complete();
                    }
                }
                else
                {
                    last.Reject(reason);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can undo the last change.
        /// </summary>
        /// <value><c>true</c> if this instance can undo; otherwise, <c>false</c>.</value>
        public bool CanUndo
        {
            get { return backwardChangesStack.Count > 0; }
        }

        /// <summary>
        /// Undoes the last IChange held by
        /// this instance and removes it from
        /// the cache.
        /// </summary>
        public void Undo()
        {
            EnsureNotSuspended();
            if (CanUndo)
            {
                OnUndo(RejectReason.Undo);
                OnTrackingServiceStateChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has transient entities.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has transient entities; otherwise, <c>false</c>.
        /// </value>
        public bool HasTransientEntities
        {
            get { return transientEntities.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can redo.
        /// </summary>
        /// <value><c>true</c> if this instance can redo; otherwise, <c>false</c>.</value>
        public bool CanRedo
        {
            get { return forwardChangesStack.Count > 0; }
        }

        /// <summary>
        /// Redoes the last undone change.
        /// </summary>
        public void Redo()
        {
            EnsureNotSuspended();
            if (CanRedo)
            {
                OnRedo(RejectReason.Redo);
                OnTrackingServiceStateChanged();
            }
        }

        /// <summary>
        /// Gets all the changes currently held by
        /// this IChangeTrackingService
        /// </summary>
        /// <returns></returns>
        public virtual IChangeSet GetChangeSet()
        {
            return GetChangeSet(IncludeAllChangeSetFilter.Instance);
        }

        /// <summary>
        /// Gets all the changes currently hold by
        /// this IChangeTrackingService filtered by the
        /// supplied IChangeSetFilter.
        /// </summary>
        /// <param name="filter">The IChangeSetFilter.</param>
        /// <returns></returns>
        public virtual IChangeSet GetChangeSet(IChangeSetFilter filter)
        {
            Ensure.That(filter).Named("filter").IsNotNull();

            lock (SyncRoot)
            {
                var included = backwardChangesStack.Where(c => filter.ShouldInclude(c)).ToList();
                return new ChangeSet(included);
            }
        }

        /// <summary>
        /// Adds a new change definition to this IChangeTrackingService.
        /// </summary>
        /// <param name="change">The change to store.</param>
        /// <param name="behavior">The requested behavior.</param>
        public virtual void Add(IChange change, AddChangeBehavior behavior)
        {
            EnsureNotSuspended();

            Ensure.That(change)
                .Named("change")
                .IsNotNull();

            if (IsInAtomicOperation)
            {
                AtomicOperation.Add(change, behavior);
            }
            else
            {
                /*
                 * è necessario agganciare gli eventi
                 * Committed e Rejected su IChange perchè
                 * se viene manualmente chiamato Commit o Reject
                 * su una IChange dobbiamo liberarcene anche noi
                 */
                lock (SyncRoot)
                {
                    switch (behavior)
                    {
                        case AddChangeBehavior.Default:
                            forwardChangesStack.Clear();
                            backwardChangesStack.Add(change);
                            break;

                        case AddChangeBehavior.RedoRequest:
                            backwardChangesStack.Add(change);
                            break;

                        case AddChangeBehavior.UndoRequest:
                            forwardChangesStack.Add(change);
                            break;

                        case AddChangeBehavior.None:
                            throw new ArgumentOutOfRangeException();

                        default:
                            throw new EnumValueOutOfRangeException();
                    }

                    OnWire(change);
                }

                OnTrackingServiceStateChanged();
            }
        }

        /// <summary>
        /// Generates an advisory that contains all the operations that
        /// an hypothetical UnitOfWork must perform in order to persist
        /// all the changes tracked by this ChangeTrackingService.
        /// </summary>
        /// <returns>
        /// A read-only list of <see cref="IAdvisedAction"/>.
        /// </returns>
        public virtual IAdvisory GetAdvisory()
        {
            var ab = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
            var advisory = GetAdvisory(ab);

            return advisory;
        }

        /// <summary>
        /// Generates an advisory that contains all the operations that
        /// an hypothetical UnitOfWork must perform in order to persist
        /// all the changes tracked by this ChangeTrackingService.
        /// The generation is customized using the supplied <see cref="IAdvisoryBuilder"/>.
        /// </summary>
        /// <param name="builder">An instance of a class implementing this <see cref="IAdvisoryBuilder"/>
        /// interface used to control the advisory generation process.</param>
        /// <returns>
        /// A read-only list of <see cref="IAdvisedAction"/>.
        /// </returns>
        public virtual IAdvisory GetAdvisory(IAdvisoryBuilder builder)
        {
            Ensure.That(builder).Named("builder").IsNotNull();

            var cSet = GetChangeSet();
            var advisory = builder.GenerateAdvisory(this, cSet);

            return advisory;
        }

        /// <summary>
        /// Called when the change tracking service needs to add handlers to the <c>IChange</c> supplied as parameter.
        /// </summary>
        /// <param name="change">The change to wire to.</param>
        protected virtual void OnWire(IChange change)
        {
            change.Committed += onChangeCommitted;
            change.Rejected += onChangeRejected;

            OnWire(change.Owner as IComponent);
        }

        /// <summary>
        /// Called when the change tracking service needs to add handlers to the <c>IComponent</c> supplied as parameter.
        /// </summary>
        /// <param name="entity">The entity to wire to.</param>
        protected virtual void OnWire(IComponent entity)
        {
            if (entity != null && !iComponentEntities.Contains(entity))
            {
                entity.Disposed += onComponentDisposed;
                iComponentEntities.Add(entity);
            }
        }

        /// <summary>
        /// Called when the change tracking service needs to remove handlers from the <c>IChange</c> supplied as parameter.
        /// </summary>
        /// <param name="change">The change to unwire from.</param>
        protected virtual void OnUnwire(IChange change)
        {
            change.Committed -= onChangeCommitted;
            change.Rejected -= onChangeRejected;
        }

        /// <summary>
        /// Called when change(s) have been committed from the <c>IChange</c> instance.
        /// </summary>
        /// <param name="change">The committed change.</param>
        /// <param name="reason">The reason of the commit.</param>
        protected virtual void OnChangeCommitted(IChange change, CommitReason reason)
        {
            OnUnwire(change);

            switch (reason)
            {
                case CommitReason.AcceptChanges:

                    lock (SyncRoot)
                    {
                        backwardChangesStack.Remove(change);
                    }

                    break;

                case CommitReason.None:
                    throw new ArgumentOutOfRangeException("reason");

                default:
                    throw new EnumValueOutOfRangeException();
            }
        }

        /// <summary>
        /// Handles the change rejected event.
        /// </summary>
        /// <param name="change">The change that raises the rejected event.</param>
        /// <param name="reason">The reason of the event raise.</param>
        protected virtual void OnChangeRejected(IChange change, RejectReason reason)
        {
            OnUnwire(change);

            switch (reason)
            {
                case RejectReason.Undo:
                case RejectReason.RejectChanges:
                case RejectReason.Revert:

                    lock (SyncRoot)
                    {
                        backwardChangesStack.Remove(change);
                    }

                    break;

                case RejectReason.Redo:

                    lock (SyncRoot)
                    {
                        forwardChangesStack.Remove(change);
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
        protected virtual void OnDetach(IMemento entity, StopTrackingReason reason)
        {
            EntityTrackingStates state = GetEntityState(entity);

            bool isTransient = (state & EntityTrackingStates.IsTransient) == EntityTrackingStates.IsTransient;
            bool hasBackwardChanges = (state & EntityTrackingStates.HasBackwardChanges) == EntityTrackingStates.HasBackwardChanges;
            bool hasForwardChanges = (state & EntityTrackingStates.HasForwardChanges) == EntityTrackingStates.HasForwardChanges;

            /*
             * questo handler potrebbe venire invocato in maniera un po' strana 
             * perchè il programmatore che ci usa non fa buon uso delle Dispose
             * e/o del costrutto using, come ad esempio simuliamo negli unit 
             * test(s), quindi le Dispose vengono chiamate in ordine casuale dal 
             * GC...
             */
            IComponent cmp = entity as IComponent;
            if (cmp != null)
            {
                cmp.Disposed -= onComponentDisposed;
                if (iComponentEntities != null && iComponentEntities.Contains(cmp))
                {
                    iComponentEntities.Remove(cmp);
                }
            }

            if (reason == StopTrackingReason.UserRequest)
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

            if (isTransient)
            {
                OnUnregisterTransient(entity);
            }

            if (hasBackwardChanges && backwardChangesStack != null)
            {
                lock (SyncRoot)
                {
                    backwardChangesStack
                        .Where(c => Object.Equals(c.Owner, entity))
                        .AsReadOnly()
                        .ForEach(c => backwardChangesStack.Remove(c));
                }
            }

            if (hasForwardChanges && forwardChangesStack != null)
            {
                lock (SyncRoot)
                {
                    forwardChangesStack
                        .Where(c => Object.Equals(c.Owner, entity))
                        .AsReadOnly()
                        .ForEach(c => forwardChangesStack.Remove(c));
                }
            }

            if ((isTransient || hasBackwardChanges || hasForwardChanges))
            {
                OnTrackingServiceStateChanged();
            }
        }

        /// <summary>
        /// Ensures the this service instance is not suspended.
        /// </summary>
        /// <exception cref="SuspendedChangeTrackingServiceException">A 
        /// <c>SuspendedChangeTrackingServiceException</c> is raised if this instance is in a suspended state.</exception>
        protected void EnsureNotSuspended()
        {
            if (IsSuspended)
            {
                throw new SuspendedChangeTrackingServiceException();
            }
        }

        /// <summary>
        /// Suspends all the tracking operation of this service.
        /// </summary>
        public virtual void Suspend()
        {
            IsSuspended = true;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is suspended.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is suspended; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuspended
        {
            get;
            private set;
        }

        /// <summary>
        /// Resumes all the tracking operation of this service.
        /// </summary>
        public virtual void Resume()
        {
            IsSuspended = false;
        }

        /// <summary>
        /// Stops tracking the supplied entities removing any changes linked to the entity
        /// and removing it, if necessary, from the transient entities.
        /// </summary>
        /// <param name="entity">The entity to stop tracking.</param>
        public void Detach(IMemento entity)
        {
            EnsureNotSuspended();

            OnDetach(entity, StopTrackingReason.UserRequest);
        }

        /// <summary>
        /// Attaches the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Attach(IMemento item)
        {
            Ensure.That(item)
                .Named(() => item)
                .IsNotNull()
                .If(obj => obj.Memento != null && obj.Memento != this)
                .ThenThrow(obj =>
               {
                   var msg = obj.GetFullErrorMessage("The supplied memento is already attached to another Change Tracking Service, before attaching to this service the memento object must be detached from the other tracking service.");
                   throw new NotSupportedException(msg);
               });

            OnAttach(item);
        }

        /// <summary>
        /// Called in order to attach the supplied entity to this tracking service.
        /// </summary>
        /// <param name="item">The item to attach.</param>
        protected virtual void OnAttach(IMemento item)
        {
            item.Memento = this;
        }



        void RejectChangesCore(bool shouldNotify)
        {
            if (shouldNotify)
            {
                var args = new CancelEventArgs(false);
                OnRejectingChanges(args);
                if (args.Cancel)
                {
                    return;
                }
            }

            while (IsChanged)
            {
                OnUndo(RejectReason.RejectChanges);
            }

            //TODO: should it clear even the "forwardChangesStack"?

            if (HasTransientEntities)
            {
                transientEntities.Clear();
            }

            if (shouldNotify)
            {
                OnTrackingServiceStateChanged();
                OnChangesRejected();
            }
        }

        /// <summary>
        /// Resets the object’s state to unchanged by rejecting the modifications.
        /// </summary>
        public virtual void RejectChanges()
        {
            EnsureNotSuspended();
            RejectChangesCore(IsChanged || HasTransientEntities);
        }



        /// <summary>
        /// Resets the object’s state to unchanged by accepting the modifications.
        /// </summary>
        public virtual void AcceptChanges()
        {
            EnsureNotSuspended();

            var shouldNotify = IsChanged || HasTransientEntities;

            if (shouldNotify)
            {
                var args = new CancelEventArgs(false);
                OnAcceptingChanges(args);
                if (args.Cancel)
                {
                    return;
                }
            }

            if (IsChanged)
            {
                /*
                 * In caso di Commit vengono confermate
                 * nell'ordine in cui sono state fatte
                 */
                backwardChangesStack
                    .Where(change => change.IsCommitSupported)
                    .AsReadOnly()
                    .ForEach(change => change.Commit(CommitReason.AcceptChanges));

                backwardChangesStack.Clear();

                //TODO: should it clear even the "forwardChangesStack"?
            }

            if (HasTransientEntities)
            {
                transientEntities.Clear();
            }

            if (shouldNotify)
            {
                OnTrackingServiceStateChanged();
                OnChangesAccepted();
            }
        }

        /// <summary>
        /// Gets the object's changed status.
        /// </summary>
        /// <value></value>
        /// <returns>true if the object’s content has changed since the last call to <see cref="M:System.ComponentModel.IChangeTracking.AcceptChanges"/>; otherwise, false.</returns>
        public virtual bool IsChanged
        {
            get { return backwardChangesStack.Count > 0; }
        }



        /// <summary>
        /// Event raised when the Dispose of this instance has completed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Raises the Disposed event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            if (Disposed != null)
            {
                Disposed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.ComponentModel.ISite"/> associated with the <see cref="T:System.ComponentModel.IComponent"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.ComponentModel.ISite"/> object associated with the component; or null, if the component does not have a site.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site
        {
            get;
            set;
        }

        [NonSerialized]
        private EventHandlerList _events = new EventHandlerList();

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        protected EventHandlerList Events
        {
            get { return _events; }
        }


        bool IsInAtomicOperation
        {
            get { return AtomicOperation != null; }
        }

        AtomicOperation AtomicOperation;

        IAtomicOperation BeginAtomicOperation(AddChangeBehavior behavior)
        {
            Ensure.That(IsInAtomicOperation)
                .WithMessage("Only one single atomic operation can be created at a time.")
                .Is(false);

            Action<AtomicChange> completed = c =>
            {
                AtomicOperation = null;
                Add(c, behavior);

                /*
                 * Qui potrebbe aver senso recuperare dalla IChange
                 * tutte le transient entities e travasarle nelle transient entities
                 * locali.
                 */
                c.MergeTransientEntities(transientEntities);
            };

            Action disposed = () =>
            {
                AtomicOperation = null;
            };

            AtomicOperation = new AtomicOperation(completed, disposed);

            return AtomicOperation;
        }

        /// <summary>
        /// Begins a new atomic operation. An atomic operation is useful to
        /// treat a set of subsequent changes as a single change.
        /// </summary>
        /// <returns>The newly created atomic operation.</returns>
        /// <exception cref="ArgumentException">An <c>ArgumentException</c> is raised if there
        /// is another active atomic operation.</exception>
        public IAtomicOperation BeginAtomicOperation()
        {
            return BeginAtomicOperation(AddChangeBehavior.Default);
        }

        /// <summary>
        /// Whether this component is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets the state of the given entity property.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="property">The property to inspect.</param>
        /// <returns>
        /// The actual property state.
        /// </returns>
        public EntityPropertyStates GetEntityPropertyState<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
        {
            return GetEntityPropertyState<TEntity, TProperty>(entity, property.GetMemberName());
        }

        /// <summary>
        /// Gets the state of the given entity property.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The actual property state.
        /// </returns>
        public EntityPropertyStates GetEntityPropertyState<TEntity, TProperty>(TEntity entity, string propertyName)
        {
            var property = entity.GetType().GetProperty(propertyName);

            Ensure.That(property)
                .WithMessage("Cannot find property: {0}", propertyName)
                .IsNotNull();

            var state = EntityPropertyStates.None;

            TProperty originalValue;
            if (TryGetOriginalValue(propertyName, out originalValue))
            {
                state |= EntityPropertyStates.Changed;

                TProperty actualValue;
                var e = entity as Model.Entity;
                if (e != null)
                {
                    actualValue = e.GetPropertyValue<TProperty>(propertyName);
                }
                else
                {
                    actualValue = (TProperty)property.GetValue(entity, null);
                }

                if (!object.Equals(originalValue, actualValue))
                {
                    state |= EntityPropertyStates.ValueChanged;
                }
            }

            return state;
        }

        bool TryGetOriginalValue<T>(string propertyName, out T value)
        {
            var valueChange = GetChangeSet()
                .OfType<Specialized.PropertyValueChange<T>>()
                .FirstOrDefault(x => x.PropertyName == propertyName);

            if (valueChange != null)
            {
                value = valueChange.CachedValue;
                return true;
            };

            value = default(T);
            return false;
        }
    }
}
