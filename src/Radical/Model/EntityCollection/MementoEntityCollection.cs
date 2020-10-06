using Radical.ChangeTracking.Specialized;
using Radical.ComponentModel.ChangeTracking;
using Radical.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Radical.Model
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly")]
    [Serializable]
    public class MementoEntityCollection<T> : EntityCollection<T>, IMemento
    {

        protected virtual void OnMementoChanged(IChangeTrackingService newMemento, IChangeTrackingService oldMemento)
        {
            EnsureNotDisposed();

            this.ForEach(e =>
           {
               IMemento memento = e as IMemento;
               if (memento != null)
               {
                   memento.Memento = newMemento;
               }
           });
        }

        IChangeTrackingService _memento = null;
        public IChangeTrackingService Memento
        {
            get
            {
                //this.EnsureNotDisposed();
                return _memento;
            }
            set
            {
                if (value != Memento)
                {
                    //this.EnsureNotDisposed();
                    //Ensure.That( value )
                    //    .Named( "value" )
                    //    .If( v => ( v != null && !( v is TMemento ) ) )
                    //    .Then( v => { throw new ArgumentException(); } );

                    var old = Memento;
                    _memento = value;

                    OnMementoChanged(value, old);
                }
            }
        }

        /// <summary>
        /// Gets a item indicating whether there is an active change tracking service.
        /// </summary>
        /// <value>
        ///     <c>true</c> if there is an active change tracking service; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool IsTracking
        {
            get
            {
                EnsureNotDisposed();

                return Memento != null && !Memento.IsSuspended;
            }
        }


        protected void SuspendCaching()
        {
            EnsureNotDisposed();
            _isCachingSuspended = true;
        }

        bool _isCachingSuspended;
        /// <summary>
        /// Gets a value indicating whether caching is suspended.
        /// </summary>
        /// <value>
        ///     <c>true</c> if caching is suspended; otherwise, <c>false</c>.
        /// </value>
        protected bool IsCachingSuspended
        {
            get
            {
                EnsureNotDisposed();
                return _isCachingSuspended;
            }
        }

        protected void ResumeCaching()
        {
            EnsureNotDisposed();
            _isCachingSuspended = false;
        }

        /// <summary>
        /// Begins a data load session.
        /// During a data load session both the notification engine
        /// and the change tracking system are suspended.
        /// </summary>
        public override void BeginInit()
        {
            base.BeginInit();
            SuspendCaching();
        }

        /// <summary>
        /// Ends a previously begun data load session optionally
        /// notifying to the outside world the changes occurred.
        /// </summary>
        /// <param name="notify">if set to <c>true</c> raises the CollectionChanged event.</param>
        public override void EndInit(bool notify)
        {
            ResumeCaching();
            base.EndInit(notify);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
        /// </summary>
        public MementoEntityCollection()
            : base()
        {
            _init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The initial capacity.</param>
        public MementoEntityCollection(int capacity)
            : base(capacity)
        {
            _init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="collection">The readonly list to use as source.</param>
        public MementoEntityCollection(IEnumerable<T> collection)
            : base(collection)
        {
            _init();
        }

        public MementoEntityCollection(IList<T> storage)
            : base(storage)
        {
            _init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected MementoEntityCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _init();
        }


        [NonSerialized]
        RejectCallback<ItemChangedDescriptor<T>> itemAddedRejectCallback = null;

        [NonSerialized]
        RejectCallback<ItemMovedDescriptor<T>> itemMovedRejectCallback = null;

        [NonSerialized]
        RejectCallback<ItemChangedDescriptor<T>> itemRemovedRejectCallback = null;

        [NonSerialized]
        RejectCallback<ItemChangedDescriptor<T>> itemInsertedRejectCallback = null;

        [NonSerialized]
        RejectCallback<ItemReplacedDescriptor<T>> itemReplacedRejectCallback = null;

        [NonSerialized]
        RejectCallback<CollectionRangeDescriptor<T>> collectionClearedRejectCallback = null;

        [NonSerialized]
        RejectCallback<CollectionRangeDescriptor<T>> collectionAddRangeRejectCallback = null;

        private bool _initInvoked;
        void _init()
        {
            if (_initInvoked)
            {
                return;
            }

            _initInvoked = true;
            
            itemAddedRejectCallback = args =>
            {
                SuspendCaching();

                switch (args.Reason)
                {
                    case RejectReason.Undo:
                        /*
                         * Stiamo facendo l'Undo di un elemento
                         * che è stato aggiunto:
                         *    - lo rimuoviamo;
                         *    - lo aggiungiamo alla coda delle Redo
                         */
                        Remove(args.CachedValue.Item);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.UndoRequest);
                        break;

                    case RejectReason.Redo:
                        /*
                         * Stiamo facendo la Redo di un elemento che è 
                         * stato aggiunto:
                         *    - dobbiamo riaggiungerlo;
                         *    - lo aggiungiamo alla coda delle Undo
                         */
                        Add(args.CachedValue.Item);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.RedoRequest);
                        break;

                    case RejectReason.RejectChanges:
                    case RejectReason.Revert:
                        /*
                         * Stiamo resettando lo stato di un elemento
                         * che è stato aggiunto:
                         *    - ci limitiamo a rimuoverlo
                         */
                        Remove(args.CachedValue.Item);
                        break;
                }

                ResumeCaching();
            };



            itemMovedRejectCallback = args =>
            {
                SuspendCaching();

                switch (args.Reason)
                {
                    case RejectReason.Undo:
                        /*
                         * Stiamo facendo l'Undo di un elemento
                         * che è stato spostato:
                         *    - lo rimettiamo al suo posto;
                         *    - lo aggiungiamo alla coda delle Redo;
                         */
                        Move(args.CachedValue.NewIndex, args.CachedValue.OldIndex);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.UndoRequest);
                        break;

                    case RejectReason.Redo:
                        /*
                         * Stiamo facendo la Redo di un elemento che è 
                         * stato spostato:
                         *    - dobbiamo rispostarlo;
                         *    - lo aggiungiamo alla coda delle Undo
                         */
                        Move(args.CachedValue.OldIndex, args.CachedValue.NewIndex);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.RedoRequest);
                        break;

                    case RejectReason.RejectChanges:
                    case RejectReason.Revert:
                        /*
                         * Stiamo resettando lo stato di un elemento
                         * che è stato spostato:
                         *    - ci limitiamo a rimetterlo al posto originario
                         */
                        Move(args.CachedValue.NewIndex, args.CachedValue.OldIndex);
                        break;
                }

                ResumeCaching();
            };



            itemRemovedRejectCallback = args =>
            {
                SuspendCaching();

                switch (args.Reason)
                {
                    case RejectReason.Undo:
                        /*
                         * Stiamo facendo l'Undo di un elemento
                         * che è stato rimosso:
                         *    - lo rimettiamo al suo posto;
                         *    - lo aggiungiamo alla coda delle Redo;
                         */
                        Insert(args.CachedValue.Index, args.CachedValue.Item);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.UndoRequest);
                        break;

                    case RejectReason.Redo:
                        /*
                         * Stiamo facendo la Redo di un elemento che è 
                         * stato rimosso:
                         *    - dobbiamo rimuoverlo nuovamente;
                         *    - lo aggiungiamo alla coda delle Undo
                         */
                        Remove(args.CachedValue.Item);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.RedoRequest);
                        break;

                    case RejectReason.RejectChanges:
                    case RejectReason.Revert:
                        /*
                         * Stiamo resettando lo stato di un elemento
                         * che è stato rimosso:
                         *    - ci limitiamo a rimetterlo definitivamente al posto originario
                         */
                        Insert(args.CachedValue.Index, args.CachedValue.Item);
                        break;
                }

                ResumeCaching();
            };



            itemInsertedRejectCallback = args =>
            {
                SuspendCaching();

                switch (args.Reason)
                {
                    case RejectReason.Undo:
                        /*
                         * Stiamo facendo l'Undo di un elemento
                         * che è stato inserito:
                         *    - lo togliamo;
                         *    - lo aggiungiamo alla coda delle Redo;
                         */
                        RemoveAt(args.CachedValue.Index);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.UndoRequest);
                        break;

                    case RejectReason.Redo:
                        /*
                         * Stiamo facendo la Redo di un elemento che è 
                         * stato inserito:
                         *    - dobbiamo reinserirlo;
                         *    - lo aggiungiamo alla coda delle Undo
                         */
                        Insert(args.CachedValue.Index, args.CachedValue.Item);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.RedoRequest);
                        break;

                    case RejectReason.RejectChanges:
                    case RejectReason.Revert:
                        /*
                         * Stiamo resettando lo stato di un elemento
                         * che è stato inserito:
                         *    - ci limitiamo a rimoverlo definitivamente
                         */
                        RemoveAt(args.CachedValue.Index);
                        break;
                }

                ResumeCaching();
            };



            itemReplacedRejectCallback = args =>
            {
                SuspendCaching();

                switch (args.Reason)
                {
                    case RejectReason.Undo:
                        /*
                         * Stiamo facendo l'Undo di un elemento
                         * che è stato sostituito:
                         *    - lo togliamo e rimettiamo al suo posto quello vecchio;
                         *    - lo aggiungiamo alla coda delle Redo;
                         */
                        this[args.CachedValue.Index] = args.CachedValue.ReplacedItem;
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.UndoRequest);
                        break;

                    case RejectReason.Redo:
                        /*
                         * Stiamo facendo la Redo di un elemento che è 
                         * stato sostituito:
                         *    - dobbiamo rimmetere quello nuovo;
                         *    - lo aggiungiamo alla coda delle Undo
                         */
                        this[args.CachedValue.Index] = args.CachedValue.NewItem;
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.RedoRequest);
                        break;

                    case RejectReason.RejectChanges:
                    case RejectReason.Revert:
                        /*
                         * Stiamo resettando lo stato di un elemento
                         * che è stato sostituito:
                         *    - ci limitiamo a rimettere posto quello vecchio;
                         */
                        this[args.CachedValue.Index] = args.CachedValue.ReplacedItem;
                        break;
                }

                ResumeCaching();
            };



            collectionClearedRejectCallback = args =>
            {
                SuspendCaching();

                switch (args.Reason)
                {
                    case RejectReason.Undo:
                        /*
                         * Stiamo facendo l'Undo della clear:
                         *    - rimettiamo a posto tutti gli elementi;
                         *    - aggiungiamo alla coda delle Redo;
                         */
                        AddRange(args.CachedValue.Items);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.UndoRequest);
                        break;

                    case RejectReason.Redo:
                        /*
                         * Stiamo facendo la Redo della Clear:
                         *    - rifacciamo la Clear;
                         *    - lo aggiungiamo alla coda delle Undo
                         */
                        Clear();
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.RedoRequest);
                        break;

                    case RejectReason.RejectChanges:
                    case RejectReason.Revert:
                        /*
                         * Stiamo resettando lo stato da una clear:
                         *    - ci limitiamo a rimettere a posto tutti gli elementi;
                         */
                        AddRange(args.CachedValue.Items);
                        break;
                }

                ResumeCaching();
            };



            collectionAddRangeRejectCallback = args =>
            {
                SuspendCaching();

                switch (args.Reason)
                {
                    case RejectReason.Undo:
                        /*
                         * Stiamo facendo l'Undo della AddRange:
                         *    - dobbiamo rimuovere tutti gli elementi "added";
                         *    - aggiungiamo alla coda delle Redo;
                         */
                        foreach (var addedItem in args.CachedValue.Items)
                        {
                            Remove(addedItem);
                        }

                        Memento.Add(args.Source.Clone(), AddChangeBehavior.UndoRequest);
                        break;

                    case RejectReason.Redo:
                        /*
                         * Stiamo facendo la Redo della AddRange:
                         *    - rifacciamo la AddRange;
                         *    - lo aggiungiamo alla coda delle Undo
                         */
                        AddRange(args.CachedValue.Items);
                        Memento.Add(args.Source.Clone(), AddChangeBehavior.RedoRequest);
                        break;

                    case RejectReason.RejectChanges:
                    case RejectReason.Revert:
                        /*
                         * Stiamo resettando lo stato di una AddRange:
                         *    - Rimuoviamo tutti gli elementi added;
                         */
                        foreach (var addedItem in args.CachedValue.Items)
                        {
                            Remove(addedItem);
                        }
                        break;
                }

                ResumeCaching();
            };

        }

        /// <summary>
        /// Called every time this list needs to wire events of the given items,
        /// tipically this method is called every time an item is added to the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        protected override void WireListItem(T item)
        {
            base.WireListItem(item);

            var memento = item as IMemento;
            if (memento != null)
            {
                memento.Memento = Memento;
            }
        }

        /// <summary>
        /// Called just after SetValueAt
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="newValue">The new item.</param>
        /// <param name="overwrittenValue">The overwritten item.</param>
        protected override void OnSetValueAtCompleted(int index, T newValue, T overwrittenValue)
        {
            base.OnSetValueAtCompleted(index, newValue, overwrittenValue);

            EnsureNotDisposed();
            if (!IsCachingSuspended && IsTracking)
            {
                var descriptor = new ItemReplacedDescriptor<T>(newValue, overwrittenValue, index);
                var change = new ItemReplacedCollectionChange<T>(this, descriptor, itemReplacedRejectCallback, null, string.Empty);
                Memento.Add(change, AddChangeBehavior.Default);
            }
        }

        /// <summary>
        /// Called just after Add
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The item.</param>
        protected override void OnAddCompleted(int index, T value)
        {
            base.OnAddCompleted(index, value);

            EnsureNotDisposed();
            if (!IsCachingSuspended && IsTracking)
            {
                var descriptor = new ItemChangedDescriptor<T>(value, index);
                var change = new ItemChangedCollectionChange<T>(this, descriptor, itemAddedRejectCallback, null, string.Empty);
                Memento.Add(change, AddChangeBehavior.Default);
            }
        }

        protected override void OnAddRangeCompleted(IEnumerable<T> addedRange)
        {
            base.OnAddRangeCompleted(addedRange);

            EnsureNotDisposed();
            if (!IsCachingSuspended && IsTracking)
            {
                var descriptor = new CollectionRangeDescriptor<T>(addedRange);
                var change = new AddRangeCollectionChange<T>(this, descriptor, collectionAddRangeRejectCallback, null, string.Empty);
                Memento.Add(change, AddChangeBehavior.Default);
            }
        }

        /// <summary>
        /// Called after the move operation has been completed.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="value">The value.</param>
        protected override void OnMoveCompleted(int oldIndex, int newIndex, T value)
        {
            base.OnMoveCompleted(oldIndex, newIndex, value);

            EnsureNotDisposed();
            if (!IsCachingSuspended && IsTracking)
            {
                var descriptor = new ItemMovedDescriptor<T>(value, newIndex, oldIndex);
                var change = new ItemMovedCollectionChange<T>(this, descriptor, itemMovedRejectCallback, null, string.Empty);
                Memento.Add(change, AddChangeBehavior.Default);
            }
        }

        /// <summary>
        /// Called when clear is completed.
        /// </summary>
        protected override void OnClearCompleted(IEnumerable<T> clearedItems)
        {
            base.OnClearCompleted(clearedItems);

            EnsureNotDisposed();

            if (clearedItems.Any() && !IsCachingSuspended && IsTracking)
            {
                var descriptor = new CollectionRangeDescriptor<T>(clearedItems);
                var change = new CollectionClearedChange<T>(this, descriptor, collectionClearedRejectCallback, null, string.Empty);
                Memento.Add(change, AddChangeBehavior.Default);
            }
        }

        /// <summary>
        /// Called just after Insert
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The item.</param>
        protected override void OnInsertCompleted(int index, T value)
        {
            base.OnInsertCompleted(index, value);
            EnsureNotDisposed();

            if (!IsCachingSuspended && IsTracking)
            {
                var descriptor = new ItemChangedDescriptor<T>(value, index);
                var change = new ItemChangedCollectionChange<T>(this, descriptor, itemInsertedRejectCallback, null, string.Empty);
                Memento.Add(change, AddChangeBehavior.Default);
            }
        }

        /// <summary>
        /// Called just after Remove
        /// </summary>
        /// <param name="value">The item.</param>
        /// <param name="index">The index.</param>
        protected override void OnRemoveCompleted(T value, int index)
        {
            base.OnRemoveCompleted(value, index);
            EnsureNotDisposed();

            if (!IsCachingSuspended && IsTracking)
            {
                var descriptor = new ItemChangedDescriptor<T>(value, index);
                var change = new ItemRemovedCollectionChange<T>(this, descriptor, itemRemovedRejectCallback, null, string.Empty);
                Memento.Add(change, AddChangeBehavior.Default);
            }
        }

        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// </summary>
        /// <param name="info">The serailization info.</param>
        /// <param name="context">The serailization context.</param>
        protected override void OnDeserialization(SerializationInfo info, StreamingContext context)
        {
            base.OnDeserialization(info, context);
            SuspendCaching();
        }

        /// <summary>
        /// Called when the deserialization process has been completed.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected override void OnDeserializationCompleted(SerializationInfo info, StreamingContext context)
        {
            ResumeCaching();
            base.OnDeserializationCompleted(info, context);
        }

        //WARN: non siamo in grado di cachare un eventuale cambiamento di Sort o il Reverse
    }
}
