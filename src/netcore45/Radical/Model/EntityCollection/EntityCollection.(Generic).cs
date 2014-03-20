using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using System.Linq.Expressions;

namespace Topics.Radical.Model
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2240:ImplementISerializableCorrectly" )]
	public partial class EntityCollection<T> :
		IEntityCollection<T>,
		IList,
		ICollection,
		IDisposable
	{
		private EventHandlerList _events;

		/// <summary>
		/// Gets the events.
		/// </summary>
		/// <item>The events.</item>
		protected EventHandlerList Events
		{
			get
			{
				this.EnsureNotDisposed();
				if( this._events == null )
				{
					this._events = new EventHandlerList();
				}

				return this._events;
			}
		}

		private IList<T> _storage = null;

		/// <summary>
		/// Gets the internal storage.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1002:DoNotExposeGenericLists" )]
		protected IList<T> Storage
		{
			get
			{
				this.EnsureNotDisposed();
				if( this._storage == null )
				{
					this._storage = new List<T>();
				}

				return this._storage;
			}
		}

		/// <summary>
		/// Begins a data load session.
		/// During a data load session both the notification engine
		/// and the change tracking system are suspended.
		/// </summary>
		public virtual void BeginInit()
		{
			this.EnsureNotDisposed();
			this.IsInitializing = true;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is loading data.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is loading data; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsInitializing
		{
			get;
			private set;
		}

		/// <summary>
		/// Ripristina la generazione degli eventi che la Collection
		/// scatena alla modifica (aggiunta/rimozione), ripristina
		/// anche il meccanismo di caching.
		/// </summary>
		public void EndInit()
		{
			this.EndInit( true );
		}

		/// <summary>
		/// Ends a previously begun data load session optionally
		/// notifying to the outside world the changes occurred.
		/// </summary>
		/// <param name="notify">if set to <c>true</c> raises the <see cref="CollectionChanged"/> event.</param>
		public virtual void EndInit( Boolean notify )
		{
			this.EnsureNotDisposed();

			if( this.IsInitializing )
			{
				this.IsInitializing = false;

				if( notify )
				{
					//Notifichiamo che la collection è cambiata
					this.OnCollectionChanged( new CollectionChangedEventArgs<T>( CollectionChangeType.Reset, -1 ) );
				}
			}
		}

		static readonly object collectionChangedEventKey = new object();
		/// <summary>
		/// Occurs when this collection changes.
		/// </summary>
		public event EventHandler<CollectionChangedEventArgs<T>> CollectionChanged
		{
			add { this.Events.AddHandler( collectionChangedEventKey, value ); }
			remove { this.Events.RemoveHandler( collectionChangedEventKey, value ); }
		}

		/// <summary>
		/// Raises the <see cref="E:CollectionChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="Topics.Radical.ComponentModel.CollectionChangedEventArgs&lt;T&gt;"/> instance containing the event data.</param>
		protected virtual void OnCollectionChanged( CollectionChangedEventArgs<T> e )
		{
			this.EnsureNotDisposed();
			if( !this.IsInitializing )
			{
				EventHandler<CollectionChangedEventArgs<T>> handler = this.Events[ collectionChangedEventKey ] as EventHandler<CollectionChangedEventArgs<T>>;
				if( handler != null )
				{
					handler( this, e );
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
		/// </summary>
		public EntityCollection()
		{
			this.OnInitialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The initial capacity.</param>
		public EntityCollection( Int32 capacity )
			: this()
		{
			this._storage = new List<T>( capacity );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The readonly list to use as source.</param>
		public EntityCollection( IEnumerable<T> collection )
			: this()
		{
			this.AddRange( collection );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="storage">The storage.</param>
		public EntityCollection( IList<T> storage )
			: this()
		{
			this._storage = storage;
		}

		/// <summary>
		/// Called when during the inizialization process,
		/// override this method to be notified when the default
		/// ctor has been called.
		/// </summary>
		protected virtual void OnInitialize()
		{
			//this._serializationKey = String.Format( CultureInfo.InvariantCulture, "{0}_Data_Array", typeof( T ).Name );
		}

		/// <summary>
		/// Called every time this list needs to wire events of the given items, 
		/// tipically this method is called every time an item is added to the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		protected virtual void WireListItem( T item )
		{
			this.EnsureNotDisposed();
			INotifyPropertyChanged inpc = item as INotifyPropertyChanged;
			if( inpc != null )
			{
				inpc.PropertyChanged += new PropertyChangedEventHandler( OnListItemPropertyChanged );
			}
		}

		void OnListItemPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			this.EnsureNotDisposed();
			T item = ( T )sender;
			Int32 index = this.IndexOf( item );

			this.OnCollectionChanged( new CollectionChangedEventArgs<T>( CollectionChangeType.ItemChanged, index, -1, item ) );
		}

		/// <summary>
		/// Called every time this list needs to unwire events of the given items, 
		/// tipically this method is called every time an item is removed to the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		protected virtual void UnwireListItem( T item )
		{
			this.EnsureNotDisposed();
			INotifyPropertyChanged inpc = item as INotifyPropertyChanged;
			if( inpc != null )
			{
				inpc.PropertyChanged -= new PropertyChangedEventHandler( OnListItemPropertyChanged );
			}
		}

		/// <summary>
		/// Called just before the set operation.
		/// </summary>
		/// <param name="e">The <see cref="Topics.Radical.Model.SetValueAtEventArgs&lt;T&gt;"/> instance containing the event data.</param>
		protected virtual void OnSetValueAt( SetValueAtEventArgs<T> e )
		{
			this.EnsureNotDisposed();
		}

		/// <summary>
		/// Sets the item at the given index
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected void SetValueAt( Int32 index, T value )
		{
			this.EnsureNotDisposed();
			//Recuperiamo un riferimento al valore che verrà sovrascritto
			T oldValue = this.GetValueAt( index );

			SetValueAtEventArgs<T> args = new SetValueAtEventArgs<T>( index, value, oldValue );
			this.OnSetValueAt( args );

			if( !args.Cancel )
			{
				//Ci sganciamo da quello attualmente presente nella collection
				this.UnwireListItem( oldValue );
				if( !this.Contains( value ) )
				{
					//Ci leghiamo a quello in arrivo
					this.WireListItem( value );
				}

				//Impostiamo il valore
				this.Storage[ index ] = value;

				this.OnSetValueAtCompleted( index, value, oldValue );
				this.OnCollectionChanged( new CollectionChangedEventArgs<T>( CollectionChangeType.ItemReplaced, index, index, oldValue ) );
			}
		}

		/// <summary>
		/// Called just after SetValueAt
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="newValue">The new item.</param>
		/// <param name="overwrittenValue">The overwritten item.</param>
		protected virtual void OnSetValueAtCompleted( Int32 index, T newValue, T overwrittenValue )
		{

		}

		/// <summary>
		/// Gets the item at the given index
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected virtual T GetValueAt( Int32 index )
		{
			this.EnsureNotDisposed();
			return ( T )this.Storage[ index ];
		}

		/// <summary>
		/// Gets or sets the <see cref="T:T"/> at the specified index.
		/// </summary>
		/// <item></item>
		public T this[ Int32 index ]
		{
			get
			{
				this.EnsureNotDisposed();
				return this.GetValueAt( index );
			}
			set
			{
				this.EnsureNotDisposed();
				this.SetValueAt( index, value );
			}
		}

		/// <summary>
		/// Called just after Add
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected virtual void OnAddCompleted( Int32 index, T value )
		{

		}

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add( T item )
		{
			this.EnsureNotDisposed();
			//Aggiungiamo allo storage
			this.Storage.Add( item );
			Int32 index = this.Count - 1;

			this.WireListItem( item );

			this.OnAddCompleted( index, item );
			this.OnCollectionChanged( new CollectionChangedEventArgs<T>( CollectionChangeType.ItemAdded, index, -1, item ) );
		}

		/// <summary>
		/// Moves the specified item to a new position, identified by the <paramref name="newIndex"/>
		/// parameter, inside the collection.
		/// </summary>
		/// <param name="item">The item to move.</param>
		/// <param name="newIndex">The new index.</param>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="item"/> to move does not belong to this collection.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The supplied index (<paramref name="newIndex"/>)
		/// is outside the bounds of the collection.</exception>
		/// <exception cref="ArgumentNullException">The supplied <paramref name="item"/> is a null reference.</exception>
		public void Move( T item, Int32 newIndex )
		{
			this.EnsureNotDisposed();
			if( item == null )
			{
				throw new ArgumentNullException( "item" );
			}

			Int32 index = this.IndexOf( item );
			if( index == -1 )
			{
				throw new ArgumentOutOfRangeException( "item" );
			}

			this.Move( index, newIndex );
		}

		/// <summary>
		/// Moves the specified item, identified by the <paramref name="oldIndex"/> parameter,
		/// to a new position, identified by the <paramref name="newIndex"/> parameter, inside
		/// the collection.
		/// </summary>
		/// <param name="oldIndex">The old index.</param>
		/// <param name="newIndex">The new index.</param>
		/// <exception cref="ArgumentOutOfRangeException">The supplied index (<paramref name="oldIndex"/> or <paramref name="newIndex"/>)
		/// is outside the bounds of the collection.</exception>
		public void Move( Int32 oldIndex, Int32 newIndex )
		{
			this.EnsureNotDisposed();
			if( oldIndex < 0 || oldIndex >= this.Count )
			{
				throw new ArgumentOutOfRangeException( "oldIndex" );
			}

			if( newIndex < 0 || newIndex >= this.Count )
			{
				throw new ArgumentOutOfRangeException( "newIndex" );
			}


			T item = this[ oldIndex ];
			this.Storage.RemoveAt( oldIndex );
			this.Storage.Insert( newIndex, item );

			this.OnMoveCompleted( oldIndex, newIndex, item );
			this.OnCollectionChanged( new CollectionChangedEventArgs<T>( CollectionChangeType.ItemMoved, newIndex, oldIndex, item ) );
		}

		/// <summary>
		/// Called after the move operation has been completed.
		/// </summary>
		/// <param name="oldIndex">The old index.</param>
		/// <param name="newIndex">The new index.</param>
		/// <param name="value">The value.</param>
		protected virtual void OnMoveCompleted( Int32 oldIndex, Int32 newIndex, T value )
		{

		}

		/// <summary>
		/// Called when clear is completed.
		/// </summary>
		protected virtual void OnClearCompleted( IEnumerable<T> clearedItems )
		{

		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.IList"></see>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only. </exception>
		public void Clear()
		{
			this.EnsureNotDisposed();
			IEnumerable<T> list = this.Storage.ToArray().AsReadOnly();

			//Svuotiamo lo storage
			this.Storage.Clear();

			this.OnClearCompleted( list );
			this.OnCollectionChanged( new CollectionChangedEventArgs<T>( CollectionChangeType.Reset ) );
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific item.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		public Boolean Contains( T item )
		{
			this.EnsureNotDisposed();
			return this.Storage.Contains( item );
		}

		/// <summary>
		/// Returns the index of the supplied item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public Int32 IndexOf( T item )
		{
			this.EnsureNotDisposed();
			return this.Storage.IndexOf( item );
		}

		///// <summary>
		///// Returns the index of the supplied item, starting search at the specified index.
		///// </summary>
		///// <param name="value">The item.</param>
		///// <param name="startIndex">The start index.</param>
		///// <returns></returns>
		//public Int32 IndexOf( T value, Int32 startIndex )
		//{
		//    this.EnsureNotDisposed();
		//    return this.Storage.IndexOf( value, startIndex );
		//}

		/// <summary>
		/// Called just before the Insert
		/// </summary>
		/// <param name="e">The <see cref="Topics.Radical.Model.InsertEventArgs&lt;T&gt;"/> instance containing additional data.</param>
		protected virtual void OnInsert( InsertEventArgs<T> e )
		{
			this.EnsureNotDisposed();
		}

		/// <summary>
		/// Inserts the given item at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void Insert( Int32 index, T item )
		{
			this.EnsureNotDisposed();
			InsertEventArgs<T> args = new InsertEventArgs<T>( index, item );
			this.OnInsert( args );
			if( !args.Cancel )
			{
				//Inseriamo il nuovo elemento
				this.Storage.Insert( index, item );
				this.WireListItem( item );

				this.OnInsertCompleted( index, item );
				this.OnCollectionChanged( new CollectionChangedEventArgs<T>( CollectionChangeType.ItemAdded, index, -1, item ) );
			}
		}

		/// <summary>
		/// Called just after Insert
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected virtual void OnInsertCompleted( Int32 index, T value )
		{

		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		protected virtual Boolean IsReadOnly
		{
			get
			{
				this.EnsureNotDisposed();
				return false;
			}
		}

		/// <summary>
		/// Called just after Remove
		/// </summary>
		/// <param name="value">The item.</param>
		/// <param name="index">The index.</param>
		protected virtual void OnRemoveCompleted( T value, Int32 index )
		{

		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		public Boolean Remove( T item )
		{
			this.EnsureNotDisposed();
			return this.Remove( item, true );
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <param name="notify">if set to <collection>true</collection> raises the CollectionChanged event.</param>
		/// <returns>true if the operation has been completed successfully; otherwise false.</returns>
		protected Boolean Remove( T item, Boolean notify )
		{
			this.EnsureNotDisposed();
			//Recuperiamo l'indice da rimuovere
			Int32 index = this.Storage.IndexOf( item );

			//Rimuoviamo
			if( index > -1 )
			{
				this.UnwireListItem( item );

				//Rimuoviamo
				Boolean retVal = this.Storage.Remove( item );

				this.OnRemoveCompleted( item, index );

				if( notify )
				{
					this.OnCollectionChanged( new CollectionChangedEventArgs<T>( CollectionChangeType.ItemRemoved, -1, index, item ) );
				}

				return retVal;
			}

			return false;
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.IList"></see> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
		public void RemoveAt( Int32 index )
		{
			this.EnsureNotDisposed();
			this.RemoveAt( index, true );
		}

		/// <summary>
		/// Removes at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="notify">if set to <collection>true</collection> raises the CollectionChanged event.</param>
		protected void RemoveAt( Int32 index, Boolean notify )
		{
			this.EnsureNotDisposed();
			T value = this.GetValueAt( index );
			if( value != null )
			{
				this.Remove( value, notify );
			}
			else
			{
				throw new ArgumentOutOfRangeException( "index" );
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public Int32 Count
		{
			get
			{
				this.EnsureNotDisposed();
				return this.Storage.Count;
			}
		}

		/// <summary>
		/// Copies the elements of this collection to a T[].
		/// </summary>
		/// <param name="array">The one-dimensional T[] that is the destination of the elements copied from collection. The T[] must have zero-based indexing.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array"/> is null. </exception>
		///   
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index"/> is less than zero. </exception>
		///   
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array"/> is multidimensional.-or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
		///   
		/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
		public void CopyTo( T[] array )
		{
			this.EnsureNotDisposed();
			this.Storage.CopyTo( array, 0 );
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo( T[] array, Int32 arrayIndex )
		{
			this.EnsureNotDisposed();
			this.Storage.CopyTo( array, arrayIndex );
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			this.EnsureNotDisposed();
			return this.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public virtual IEnumerator<T> GetEnumerator()
		{
			this.EnsureNotDisposed();
			return this.Storage.GetEnumerator();
		}

		/// <summary>
		/// Reverses this instance.
		/// </summary>
		public void Reverse()
		{
			this.EnsureNotDisposed();
			this.Storage.Reverse();
		}

		/// <summary>
		/// Creates a new instance of the type managed by this collection.
		/// </summary>
		/// <returns>
		/// An instance of the type managed by this collection.
		/// </returns>
		protected virtual T OnCreatingNew()
		{
			this.EnsureNotDisposed();

			if( this.HasDefaultCtor() )
			{
				var returnValue = Activator.CreateInstance<T>(); // ( T )tConstructor.Invoke( null );
				return returnValue;
			}

			return default( T );
		}

		Boolean hasDefaultCtor;
		Boolean defaultCtorSearched;
		Boolean HasDefaultCtor()
		{
			if( !defaultCtorSearched )
			{
				var tType = typeof( T );
				hasDefaultCtor = tType.GetTypeInfo().DeclaredConstructors.Any( c => c.GetParameters().None() );

				defaultCtorSearched = true;
			}

			return hasDefaultCtor;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is capable of creating a new instance of the managed type T.
		/// </summary>
		/// <item>
		/// 	<collection>true</collection> if this.instace allo the creation of new items; otherwise, <collection>false</collection>.
		/// </item>
		public virtual Boolean AllowNew
		{
			get
			{
				this.EnsureNotDisposed();
				return this.HasDefaultCtor();
			}
		}

		/// <summary>
		/// Creates a new instance of the type managed by this collection.
		/// </summary>
		/// <returns>
		/// An instance of the type managed by this collection.
		/// </returns>
		/// <exception cref="NotSupportedException">The operation is not supported, <see cref="AllowNew"/> property is set to <collection>false</collection>.</exception>
		/// <exception cref="NullReferenceException">The creation process returned a null reference.</exception>
		public T CreateNew()
		{
			this.EnsureNotDisposed();
			if( !this.AllowNew )
			{
				throw new NotSupportedException( "missing::CreateNewNotSupportedException" );
			}

			T obj = this.OnCreatingNew();

			if( obj == null )
			{
				throw new NotSupportedException( "missing::OnCreateNewReturnNullException" );
			}

			return obj;
		}

		/// <summary>
		/// Called before the AddRange begins.
		/// </summary>
		/// <param name="rangeToAdd">The range to add.</param>
		protected virtual void OnAddRange( IEnumerable<T> rangeToAdd )
		{

		}

		/// <summary>
		/// Adds a range of items.
		/// </summary>
		/// <param name="list">The range of items ot add.</param>
		public void AddRange( IEnumerable<T> list )
		{
			this.EnsureNotDisposed();

			Ensure.That( list ).Named( "list" ).IsNotNull();

			this.OnAddRange( list );

			foreach( T obj in list )
			{
				this.Add( ( T )obj );
			}

			this.OnAddRangeCompleted( list );
		}

		/// <summary>
		/// Called when the AddRange is completed.
		/// </summary>
		/// <param name="addedRange">The added range.</param>
		protected virtual void OnAddRangeCompleted( IEnumerable<T> addedRange )
		{

		}

		/// <summary>
		/// Copies the elements of the EntityCollection to a new array.
		/// </summary>
		/// <returns>An array containing copies of the elements of the EntityCollection.</returns>
		public T[] ToArray()
		{
			this.EnsureNotDisposed();
			return this.Storage.ToArray();
		}
	}
}
