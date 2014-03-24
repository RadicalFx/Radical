using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Topics.Radical.ChangeTracking.Specialized;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.Linq;
using Topics.Radical.Validation;

namespace Topics.Radical.Model
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2240:ImplementISerializableCorrectly" )]
#if !SILVERLIGHT && !NETFX_CORE
	[Serializable]
#endif
	public class MementoEntityCollection<T> : EntityCollection<T>, IMemento
	{
		#region IMemento<TMemento> members

		protected virtual void OnMementoChanged( IChangeTrackingService newMemento, IChangeTrackingService oldMemento )
		{
			this.EnsureNotDisposed();

			this.ForEach( e =>
			{
				IMemento memento = e as IMemento;
				if( memento != null )
				{
					memento.Memento = newMemento;
				}
			} );
		}

		IChangeTrackingService _memento = null;
		public IChangeTrackingService Memento
		{
			get
			{
				//this.EnsureNotDisposed();
				return this._memento;
			}
			set
			{
				if( value != this.Memento )
				{
					//this.EnsureNotDisposed();
					//Ensure.That( value )
					//    .Named( "value" )
					//    .If( v => ( v != null && !( v is TMemento ) ) )
					//    .Then( v => { throw new ArgumentException(); } );

					var old = this.Memento;
					this._memento = value;

					this.OnMementoChanged( value, old );
				}
			}
		}

		/// <summary>
		/// Gets a item indicating whether there is an active change tracking service.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if there is an active change tracking service; otherwise, <c>false</c>.
		/// </value>
		protected virtual Boolean IsTracking
		{
			get
			{
				this.EnsureNotDisposed();

				return this.Memento != null && !this.Memento.IsSuspended;
			}
		}

		#endregion

		protected void SuspendCaching()
		{
			this.EnsureNotDisposed();
			this._isCachingSuspended = true;
		}

		Boolean _isCachingSuspended;
		/// <summary>
		/// Gets a value indicating whether caching is suspended.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if caching is suspended; otherwise, <c>false</c>.
		/// </value>
		protected Boolean IsCachingSuspended
		{
			get
			{
				this.EnsureNotDisposed();
				return this._isCachingSuspended;
			}
		}

		protected void ResumeCaching()
		{
			this.EnsureNotDisposed();
			this._isCachingSuspended = false;
		}

		/// <summary>
		/// Begins a data load session.
		/// During a data load session both the notification engine
		/// and the change tracking system are suspended.
		/// </summary>
		public override void BeginInit()
		{
			base.BeginInit();
			this.SuspendCaching();
		}

		/// <summary>
		/// Ends a previously begun data load session optionally
		/// notifying to the outside world the changes occurred.
		/// </summary>
		/// <param name="notify">if set to <c>true</c> raises the CollectionChanged event.</param>
		public override void EndInit( Boolean notify )
		{
			this.ResumeCaching();
			base.EndInit( notify );
		}

		#region .ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
		/// </summary>
		public MementoEntityCollection()
			: base()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The initial capacity.</param>
		public MementoEntityCollection( Int32 capacity )
			: base( capacity )
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The readonly list to use as source.</param>
		public MementoEntityCollection( IEnumerable<T> collection )
			: base( collection )
		{

		}

		public MementoEntityCollection( IList<T> storage )
			: base( storage )
		{

		}

#if !SILVERLIGHT && !NETFX_CORE

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		protected MementoEntityCollection( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{

		}

#endif

		#endregion
#if !SILVERLIGHT && !NETFX_CORE
		[NonSerialized]
#endif
		RejectCallback<ItemChangedDescriptor<T>> itemAddedRejectCallback = null;

#if !SILVERLIGHT && !NETFX_CORE
		[NonSerialized]
#endif
		RejectCallback<ItemMovedDescriptor<T>> itemMovedRejectCallback = null;

#if !SILVERLIGHT && !NETFX_CORE
		[NonSerialized]
#endif
		RejectCallback<ItemChangedDescriptor<T>> itemRemovedRejectCallback = null;
#if !SILVERLIGHT && !NETFX_CORE
		[NonSerialized]
#endif
		RejectCallback<ItemChangedDescriptor<T>> itemInsertedRejectCallback = null;
#if !SILVERLIGHT && !NETFX_CORE
		[NonSerialized]
#endif
		RejectCallback<ItemReplacedDescriptor<T>> itemReplacedRejectCallback = null;

#if !SILVERLIGHT && !NETFX_CORE
		[NonSerialized]
#endif
		RejectCallback<CollectionRangeDescriptor<T>> collectionClearedRejectCallback = null;
#if !SILVERLIGHT && !NETFX_CORE
		[NonSerialized]
#endif
		RejectCallback<CollectionRangeDescriptor<T>> collectionAddRangeRejectCallback = null;

		/// <summary>
		/// Called when during the inizialization process,
		/// override this method to be notified when the default
		/// ctor has been called.
		/// </summary>
		protected override void OnInitialize()
		{
			base.OnInitialize();

			#region itemAddedRejectCallback

			itemAddedRejectCallback = args =>
			{
				this.SuspendCaching();

				switch( args.Reason )
				{
					case RejectReason.Undo:
						/*
						 * Stiamo facendo l'Undo di un elemento
						 * che è stato aggiunto:
						 *	- lo rimuoviamo;
						 *	- lo aggiungiamo alla coda delle Redo
						 */
						this.Remove( args.CachedValue.Item );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.UndoRequest );
						break;

					case RejectReason.Redo:
						/*
						 * Stiamo facendo la Redo di un elemento che è 
						 * stato aggiunto:
						 *	- dobbiamo riaggiungerlo;
						 *	- lo aggiungiamo alla coda delle Undo
						 */
						this.Add( args.CachedValue.Item );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.RedoRequest );
						break;

					case RejectReason.RejectChanges:
					case RejectReason.Revert:
						/*
						 * Stiamo resettando lo stato di un elemento
						 * che è stato aggiunto:
						 *	- ci limitiamo a rimuoverlo
						 */
						this.Remove( args.CachedValue.Item );
						break;
				}

				this.ResumeCaching();
			};

			#endregion

			#region itemMovedRejectCallback

			this.itemMovedRejectCallback = args =>
			{
				this.SuspendCaching();

				switch( args.Reason )
				{
					case RejectReason.Undo:
						/*
						 * Stiamo facendo l'Undo di un elemento
						 * che è stato spostato:
						 *	- lo rimettiamo al suo posto;
						 *	- lo aggiungiamo alla coda delle Redo;
						 */
						this.Move( args.CachedValue.NewIndex, args.CachedValue.OldIndex );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.UndoRequest );
						break;

					case RejectReason.Redo:
						/*
						 * Stiamo facendo la Redo di un elemento che è 
						 * stato spostato:
						 *	- dobbiamo rispostarlo;
						 *	- lo aggiungiamo alla coda delle Undo
						 */
						this.Move( args.CachedValue.OldIndex, args.CachedValue.NewIndex );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.RedoRequest );
						break;

					case RejectReason.RejectChanges:
					case RejectReason.Revert:
						/*
						 * Stiamo resettando lo stato di un elemento
						 * che è stato spostato:
						 *	- ci limitiamo a rimetterlo al posto originario
						 */
						this.Move( args.CachedValue.NewIndex, args.CachedValue.OldIndex );
						break;
				}

				this.ResumeCaching();
			};

			#endregion

			#region itemRemovedRejectCallback

			this.itemRemovedRejectCallback = args =>
			{
				this.SuspendCaching();

				switch( args.Reason )
				{
					case RejectReason.Undo:
						/*
						 * Stiamo facendo l'Undo di un elemento
						 * che è stato rimosso:
						 *	- lo rimettiamo al suo posto;
						 *	- lo aggiungiamo alla coda delle Redo;
						 */
						this.Insert( args.CachedValue.Index, args.CachedValue.Item );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.UndoRequest );
						break;

					case RejectReason.Redo:
						/*
						 * Stiamo facendo la Redo di un elemento che è 
						 * stato rimosso:
						 *	- dobbiamo rimuoverlo nuovamente;
						 *	- lo aggiungiamo alla coda delle Undo
						 */
						this.Remove( args.CachedValue.Item );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.RedoRequest );
						break;

					case RejectReason.RejectChanges:
					case RejectReason.Revert:
						/*
						 * Stiamo resettando lo stato di un elemento
						 * che è stato rimosso:
						 *	- ci limitiamo a rimetterlo definitivamente al posto originario
						 */
						this.Insert( args.CachedValue.Index, args.CachedValue.Item );
						break;
				}

				this.ResumeCaching();
			};

			#endregion

			#region itemInsertedRejectCallback

			this.itemInsertedRejectCallback = args =>
			{
				this.SuspendCaching();

				switch( args.Reason )
				{
					case RejectReason.Undo:
						/*
						 * Stiamo facendo l'Undo di un elemento
						 * che è stato inserito:
						 *	- lo togliamo;
						 *	- lo aggiungiamo alla coda delle Redo;
						 */
						this.RemoveAt( args.CachedValue.Index );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.UndoRequest );
						break;

					case RejectReason.Redo:
						/*
						 * Stiamo facendo la Redo di un elemento che è 
						 * stato inserito:
						 *	- dobbiamo reinserirlo;
						 *	- lo aggiungiamo alla coda delle Undo
						 */
						this.Insert( args.CachedValue.Index, args.CachedValue.Item );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.RedoRequest );
						break;

					case RejectReason.RejectChanges:
					case RejectReason.Revert:
						/*
						 * Stiamo resettando lo stato di un elemento
						 * che è stato inserito:
						 *	- ci limitiamo a rimoverlo definitivamente
						 */
						this.RemoveAt( args.CachedValue.Index );
						break;
				}

				this.ResumeCaching();
			};

			#endregion

			#region itemReplacedRejectCallback

			this.itemReplacedRejectCallback = args =>
			{
				this.SuspendCaching();

				switch( args.Reason )
				{
					case RejectReason.Undo:
						/*
						 * Stiamo facendo l'Undo di un elemento
						 * che è stato sostituito:
						 *	- lo togliamo e rimettiamo al suo posto quello vecchio;
						 *	- lo aggiungiamo alla coda delle Redo;
						 */
						this[ args.CachedValue.Index ] = args.CachedValue.ReplacedItem;
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.UndoRequest );
						break;

					case RejectReason.Redo:
						/*
						 * Stiamo facendo la Redo di un elemento che è 
						 * stato sostituito:
						 *	- dobbiamo rimmetere quello nuovo;
						 *	- lo aggiungiamo alla coda delle Undo
						 */
						this[ args.CachedValue.Index ] = args.CachedValue.NewItem;
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.RedoRequest );
						break;

					case RejectReason.RejectChanges:
					case RejectReason.Revert:
						/*
						 * Stiamo resettando lo stato di un elemento
						 * che è stato sostituito:
						 *	- ci limitiamo a rimettere posto quello vecchio;
						 */
						this[ args.CachedValue.Index ] = args.CachedValue.ReplacedItem;
						break;
				}

				this.ResumeCaching();
			};

			#endregion

			#region collectionClearedRejectCallback

			this.collectionClearedRejectCallback = args =>
			{
				this.SuspendCaching();

				switch( args.Reason )
				{
					case RejectReason.Undo:
						/*
						 * Stiamo facendo l'Undo della clear:
						 *	- rimettiamo a posto tutti gli elementi;
						 *	- aggiungiamo alla coda delle Redo;
						 */
						this.AddRange( args.CachedValue.Items );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.UndoRequest );
						break;

					case RejectReason.Redo:
						/*
						 * Stiamo facendo la Redo della Clear:
						 *	- rifacciamo la Clear;
						 *	- lo aggiungiamo alla coda delle Undo
						 */
						this.Clear();
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.RedoRequest );
						break;

					case RejectReason.RejectChanges:
					case RejectReason.Revert:
						/*
						 * Stiamo resettando lo stato da una clear:
						 *	- ci limitiamo a rimettere a posto tutti gli elementi;
						 */
						this.AddRange( args.CachedValue.Items );
						break;
				}

				this.ResumeCaching();
			};

			#endregion

			#region collectionAddRangeRejectCallback

			this.collectionAddRangeRejectCallback = args =>
			{
				this.SuspendCaching();

				switch( args.Reason )
				{
					case RejectReason.Undo:
						/*
						 * Stiamo facendo l'Undo della AddRange:
						 *	- dobbiamo rimuovere tutti gli elementi "added";
						 *	- aggiungiamo alla coda delle Redo;
						 */
						foreach( var addedItem in args.CachedValue.Items )
						{
							this.Remove( addedItem );
						}

						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.UndoRequest );
						break;

					case RejectReason.Redo:
						/*
						 * Stiamo facendo la Redo della AddRange:
						 *	- rifacciamo la AddRange;
						 *	- lo aggiungiamo alla coda delle Undo
						 */
						this.AddRange( args.CachedValue.Items );
						this.Memento.Add( args.Source.Clone(), AddChangeBehavior.RedoRequest );
						break;

					case RejectReason.RejectChanges:
					case RejectReason.Revert:
						/*
						 * Stiamo resettando lo stato di una AddRange:
						 *	- Rimuoviamo tutti gli elementi added;
						 */
						foreach( var addedItem in args.CachedValue.Items )
						{
							this.Remove( addedItem );
						}
						break;
				}

				this.ResumeCaching();
			};

			#endregion
		}

		/// <summary>
		/// Called every time this list needs to wire events of the given items,
		/// tipically this method is called every time an item is added to the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		protected override void WireListItem( T item )
		{
			base.WireListItem( item );

			var memento = item as IMemento;
			if( memento != null )
			{
				memento.Memento = this.Memento;
			}
		}

		/// <summary>
		/// Called just after SetValueAt
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="newValue">The new item.</param>
		/// <param name="overwrittenValue">The overwritten item.</param>
		protected override void OnSetValueAtCompleted( Int32 index, T newValue, T overwrittenValue )
		{
			base.OnSetValueAtCompleted( index, newValue, overwrittenValue );

			this.EnsureNotDisposed();
			if( !this.IsCachingSuspended && this.IsTracking )
			{
				var descriptor = new ItemReplacedDescriptor<T>( newValue, overwrittenValue, index );
				var change = new ItemReplacedCollectionChange<T>( this, descriptor, this.itemReplacedRejectCallback, null, String.Empty );
				this.Memento.Add( change, AddChangeBehavior.Default );
			}
		}

		/// <summary>
		/// Called just after Add
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected override void OnAddCompleted( Int32 index, T value )
		{
			base.OnAddCompleted( index, value );

			this.EnsureNotDisposed();
			if( !this.IsCachingSuspended && this.IsTracking )
			{
				var descriptor = new ItemChangedDescriptor<T>( value, index );
				var change = new ItemChangedCollectionChange<T>( this, descriptor, itemAddedRejectCallback, null, String.Empty );
				this.Memento.Add( change, AddChangeBehavior.Default );
			}
		}

		protected override void OnAddRangeCompleted( IEnumerable<T> addedRange )
		{
			base.OnAddRangeCompleted( addedRange );

			this.EnsureNotDisposed();
			if( !this.IsCachingSuspended && this.IsTracking )
			{
				var descriptor = new CollectionRangeDescriptor<T>( addedRange );
				var change = new AddRangeCollectionChange<T>( this, descriptor, collectionAddRangeRejectCallback, null, String.Empty );
				this.Memento.Add( change, AddChangeBehavior.Default );
			}
		}

		/// <summary>
		/// Called after the move operation has been completed.
		/// </summary>
		/// <param name="oldIndex">The old index.</param>
		/// <param name="newIndex">The new index.</param>
		/// <param name="value">The value.</param>
		protected override void OnMoveCompleted( Int32 oldIndex, Int32 newIndex, T value )
		{
			base.OnMoveCompleted( oldIndex, newIndex, value );

			this.EnsureNotDisposed();
			if( !this.IsCachingSuspended && this.IsTracking )
			{
				var descriptor = new ItemMovedDescriptor<T>( value, newIndex, oldIndex );
				var change = new ItemMovedCollectionChange<T>( this, descriptor, itemMovedRejectCallback, null, String.Empty );
				this.Memento.Add( change, AddChangeBehavior.Default );
			}
		}

		/// <summary>
		/// Called when clear is completed.
		/// </summary>
		protected override void OnClearCompleted( IEnumerable<T> clearedItems )
		{
			base.OnClearCompleted( clearedItems );

			this.EnsureNotDisposed();

			if( clearedItems.Any() && !this.IsCachingSuspended && this.IsTracking )
			{
				var descriptor = new CollectionRangeDescriptor<T>( clearedItems );
				var change = new CollectionClearedChange<T>( this, descriptor, collectionClearedRejectCallback, null, String.Empty );
				this.Memento.Add( change, AddChangeBehavior.Default );
			}
		}

		/// <summary>
		/// Called just after Insert
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected override void OnInsertCompleted( Int32 index, T value )
		{
			base.OnInsertCompleted( index, value );
			this.EnsureNotDisposed();

			if( !this.IsCachingSuspended && this.IsTracking )
			{
				var descriptor = new ItemChangedDescriptor<T>( value, index );
				var change = new ItemChangedCollectionChange<T>( this, descriptor, itemInsertedRejectCallback, null, String.Empty );
				this.Memento.Add( change, AddChangeBehavior.Default );
			}
		}

		/// <summary>
		/// Called just after Remove
		/// </summary>
		/// <param name="value">The item.</param>
		/// <param name="index">The index.</param>
		protected override void OnRemoveCompleted( T value, Int32 index )
		{
			base.OnRemoveCompleted( value, index );
			this.EnsureNotDisposed();

			if( !this.IsCachingSuspended && this.IsTracking )
			{
				var descriptor = new ItemChangedDescriptor<T>( value, index );
				var change = new ItemRemovedCollectionChange<T>( this, descriptor, itemRemovedRejectCallback, null, String.Empty );
				this.Memento.Add( change, AddChangeBehavior.Default );
			}
		}

		//WARN: non siamo in grado di cachare un eventuale cambiamento di Sort o il Reverse
	}
}
