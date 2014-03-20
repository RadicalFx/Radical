//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Threading;
//using Topics.Radical;
//using Topics.Radical.ComponentModel;
//using Topics.Radical.Model;
//using Topics.Radical.Validation;
//using Topics.Radical.Observers;
//using System.Collections;
//using System.Collections.ObjectModel;
//using System.Globalization;

//namespace Topics.Radical.Windows.Model
//{
//    public class EntityCollectionView<T> :
//        EntityView<T>,
//        IEntityCollectionView<T>,
//        IItemProperties
//    {
//        private class DeferHelper<T> : IDisposable
//        {
//            EntityCollectionView<T> _view;

//            public DeferHelper( EntityCollectionView<T> view )
//            {
//                this._view = view;
//            }

//            public void Dispose()
//            {
//                if( this._view != null )
//                {
//                    this._view.EndDeferredRefresh();
//                    this._view = null;
//                }

//                GC.SuppressFinalize( this );
//            }
//        }

//        PropertyChangedMonitor<EntityCollectionView<T>> propertyChangedMonitor;

//        protected override void Dispose( bool disposing )
//        {
//            base.Dispose( disposing );

//            if( disposing )
//            {
//                if( this.asyncOperation != null )
//                {
//                    this.asyncOperation.OperationCompleted();
//                }

//                if( this.propertyChangedMonitor != null )
//                {
//                    this.propertyChangedMonitor.StopMonitoring();
//                }
//            }

//            this.propertyChangedMonitor = null;
//            this.asyncOperation = null;
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="EntityCollectionView&lt;T&gt;"/> class.
//        /// </summary>
//        public EntityCollectionView()
//            : base()
//        {

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="EntityCollectionView&lt;T&gt;"/> class.
//        /// </summary>
//        /// <param name="list">The list.</param>
//        public EntityCollectionView( IList<T> list )
//            : base( list )
//        {

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="EntityCollectionView&lt;T&gt;"/> class.
//        /// </summary>
//        /// <param name="list">The list.</param>
//        public EntityCollectionView( T[] list )
//            : base( list )
//        {

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="EntityCollectionView&lt;T&gt;"/> class.
//        /// </summary>
//        /// <param name="list">The list.</param>
//        public EntityCollectionView( IEntityCollectionView<T> list )
//            : base( list )
//        {
//            //list.CollectionChanged += ( s, e ) => 
//            //{
//            //    Dobbiamo capire se cosa fare qui...
//            //    this.OnCollectionChanged( e );
//            //};
//        }

//        private AsyncOperation asyncOperation;
//        private SendOrPostCallback propertyChangedCallback;
//        private SendOrPostCallback addingNewCallback;
//        private SendOrPostCallback filterChangedCallback;
//        private SendOrPostCallback sortChangedCallback;
//        //private SendOrPostCallback listChangedCallback;
//        //private SendOrPostCallback collectionChangedCallback;

//        protected override void OnInit()
//        {
//            this.propertyChangedMonitor = PropertyObserver.For( this )
//                .Observe
//                (
//                    o => o.Count,
//                    ( o, p ) => this.OnPropertyChanged( () => this.IsEmpty )
//                );

//            this.asyncOperation = AsyncOperationManager.CreateOperation( null );

//            this.propertyChangedCallback = new SendOrPostCallback( ( obj ) =>
//            {
//                var propertyName = ( String )obj;
//                base.OnPropertyChanged( propertyName );

//                //if( propertyName.Equals( "Count" ) )
//                //{
//                //    //TODO: cachare il valore per evitare notifiche inutili
//                //    /*
//                //     * Qui sarebbe interessante cachare il valore di IsEmpty
//                //     * al fine di scatenare questa notifica solo ed esclusivamente
//                //     * se IsEmpty è effettivamente cambiato.
//                //     */
//                //    base.OnPropertyChanged( "IsEmpty" );
//                //}
//            } );

//            this.addingNewCallback = new SendOrPostCallback( obj =>
//            {
//                var args = ( AddingNewEventArgs<T> )obj;
//                base.OnAddingNew( args );
//            } );

//            this.filterChangedCallback = new SendOrPostCallback( obj => base.OnFilterChanged() );
//            this.sortChangedCallback = new SendOrPostCallback( obj => base.OnSortChanged() );
//            //this.listChangedCallback = new SendOrPostCallback( SafeOnListChanged );
//            //this.collectionChangedCallback = new SendOrPostCallback( SafeOnCollectionChanged );

//            base.OnInit();
//        }

//        protected override void OnLoad()
//        {
//            base.OnLoad();

//            var current = this.FirstOrDefault();
//            this.SetCurrentItem( current, true, current != null ? 0 : -1 );
//        }

//        //protected override IEntityItemView<T> CreateEntityItemView( T sourceItem )
//        //{
//        //    return new EntityItemView<T>( this.asyncOperation, this, sourceItem );
//        //}

//        /// <summary>
//        /// Returns a value that indicates whether the resulting view is empty.
//        /// </summary>
//        /// <value></value>
//        /// <returns>true if the resulting view is empty; otherwise, false.
//        /// </returns>
//        public Boolean IsEmpty
//        {
//            get { return this.Count == 0; }
//        }

//        protected override void OnPropertyChanged( string propertyName )
//        {
//            this.asyncOperation.Post( this.propertyChangedCallback, propertyName );
//        }

//        protected override void OnAddingNew( AddingNewEventArgs<T> e )
//        {
//            this.asyncOperation.Post( this.addingNewCallback, e );
//        }

//        protected override void OnFilterChanged()
//        {
//            this.asyncOperation.Post( this.filterChangedCallback, null );
//        }

//        protected override void OnSortChanged()
//        {
//            this.asyncOperation.Post( this.sortChangedCallback, null );
//        }

//        /// <summary>
//        /// Gets a value that indicates whether this view supports filtering via the <see cref="P:System.ComponentModel.ICollectionView.Filter"/> property.
//        /// </summary>
//        /// <value></value>
//        /// <returns>true if this view support filtering; otherwise, false.
//        /// </returns>
//        public virtual Boolean CanFilter
//        {
//            get { return true; }
//        }

//        /// <summary>
//        /// Gets a value that indicates whether this view supports grouping via the <see cref="P:System.ComponentModel.ICollectionView.GroupDescriptions"/> property.
//        /// </summary>
//        /// <value></value>
//        /// <returns>true if this view supports grouping; otherwise, false.
//        /// </returns>
//        public virtual Boolean CanGroup
//        {
//            get { return false; }
//        }

//        /// <summary>
//        /// Gets a value that indicates whether this view supports sorting via the <see cref="P:System.ComponentModel.ICollectionView.SortDescriptions"/> property.
//        /// </summary>
//        /// <value></value>
//        /// <returns>true if this view supports sorting; otherwise, false.
//        /// </returns>
//        public Boolean CanSort
//        {
//            get { return base.AllowSort; }
//        }

//        /// <summary>
//        /// Returns a value that indicates whether a given item belongs to this collection view.
//        /// </summary>
//        /// <param name="item">The object to check.</param>
//        /// <returns>
//        /// true if the item belongs to this collection view; otherwise, false.
//        /// </returns>
//        Boolean ICollectionView.Contains( object item )
//        {
//            return ( ( IList )this ).Contains( item );
//        }

//        //public bool Contains( object item )
//        //{
//        //    return ( ( IList )base ).Contains( item );
//        //}

//        public CultureInfo Culture
//        {
//            get;
//            set;
//        }

//        public event EventHandler CurrentChanged;

//        protected virtual void OnCurrentChanged()
//        {
//            var h = this.CurrentChanged;
//            this.asyncOperation.Post( o =>
//            {
//                var handler = ( EventHandler )o;
//                if( handler != null )
//                {
//                    handler( this, EventArgs.Empty );
//                }

//            }, h );
//        }

//        public event CurrentChangingEventHandler CurrentChanging;

//        protected virtual void OnCurrentChanging( CurrentChangingEventArgs e )
//        {
//            var h = this.CurrentChanging;
//            this.asyncOperation.Post( o =>
//            {
//                var data = ( Object[] )o;
//                var handler = ( CurrentChangingEventHandler )data[ 0 ];
//                var args = ( CurrentChangingEventArgs )data[ 1 ];
//                if( handler != null )
//                {
//                    handler( this, args );
//                }

//            }, new Object[] { h, e } );
//        }

//        void SetCurrentItem( Object item, Boolean notify, Int32 position )
//        {
//            var args = new CurrentChangingEventArgs( true );
//            if( notify )
//            {
//                this.OnCurrentChanging( args );
//            }

//            if( !args.Cancel )
//            {
//                this._currentItem = item;
//                this.CurrentPosition = position;

//                if( notify ) 
//                {
//                    this.OnCurrentChanged();
//                }
//            }
//        }

//        Object _currentItem;
//        Object ICollectionView.CurrentItem
//        {
//            get { return this._currentItem; }
//        }

//        public int CurrentPosition
//        {
//            get;
//            private set;
//        }

//        //public override void Refresh()
//        //{
//        //    base.Refresh();
//        //}

//        private int deferLevel;

//        public IDisposable DeferRefresh()
//        {
//            var view = this as IEditableCollectionView;
//            if( this.IsAddingNew || ( view != null && view.IsEditingItem ) )
//            {
//                throw new InvalidOperationException( "Cannot defer refresh." );
//            }

//            this.deferLevel++;
//            return new DeferHelper<T>( this );
//        }

//        void EndDeferredRefresh()
//        {
//            this.deferLevel--;
//            if( ( this.deferLevel == 0 ) )// && this.CheckFlag( CollectionViewFlags.NeedsRefresh ) )
//            {
//                this.Refresh();
//            }
//        }

//        Predicate<Object> ICollectionView.Filter
//        {
//            get { return ( o ) => base.Filter.ShouldInclude( o ); }
//            set
//            {
//                if( value == null )
//                {
//                    base.Filter = null;
//                }
//                else
//                {
//                    base.Filter = new PredicateEntityItemViewFilter<T>( t => value( t ) );
//                }
//            }
//        }

//        public virtual ObservableCollection<GroupDescription> GroupDescriptions
//        {
//            get { return null; }
//        }

//        public virtual ReadOnlyObservableCollection<Object> Groups
//        {
//            get { return null; }
//        }

//        public bool IsCurrentAfterLast
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public bool IsCurrentBeforeFirst
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public bool MoveCurrentTo( object item )
//        {
//            throw new NotImplementedException();
//        }

//        public bool MoveCurrentToFirst()
//        {
//            throw new NotImplementedException();
//        }

//        public bool MoveCurrentToLast()
//        {
//            throw new NotImplementedException();
//        }

//        public bool MoveCurrentToNext()
//        {
//            throw new NotImplementedException();
//        }

//        public bool MoveCurrentToPosition( int position )
//        {
//            throw new NotImplementedException();
//        }

//        public bool MoveCurrentToPrevious()
//        {
//            throw new NotImplementedException();
//        }

//        SortDescriptionCollection _sortDescriptions;
//        SortDescriptionCollection ICollectionView.SortDescriptions
//        {
//            get
//            {
//                if( this._sortDescriptions != null )
//                {
//                    ( ( INotifyCollectionChanged )this._sortDescriptions ).CollectionChanged -= OnSortDescriptionCollectionChanged;
//                }

//                this._sortDescriptions = base.SortDescriptions.Convert();
//                ( ( INotifyCollectionChanged )this._sortDescriptions ).CollectionChanged += OnSortDescriptionCollectionChanged;

//                return this._sortDescriptions;
//            }
//        }

//        void OnSortDescriptionCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
//        {
//            //Reapply/Apply Sort
//            var sortDescriptions = ( ( ICollectionView )this ).SortDescriptions.Convert<T>();
//            base.ApplySort( sortDescriptions );
//        }

//        IEnumerable ICollectionView.SourceCollection
//        {
//            get { return this.DataSource; }
//        }

//        void OnListChangedSafe( ListChangedEventArgs e )
//        {
//            //var e = ( ListChangedEventArgs )obj;

//            base.OnListChanged( e );

//            e.ListChangedType.EnsureIsDefined();
//            NotifyCollectionChangedEventArgs args = null;

//            switch( e.ListChangedType )
//            {
//                case ListChangedType.ItemAdded:
//                    Int32 addedIndex = e.NewIndex;
//                    IEntityItemView<T> addedItem = this[ addedIndex ];
//                    NotifyCollectionChangedAction addAction = NotifyCollectionChangedAction.Add;
//                    args = new NotifyCollectionChangedEventArgs( addAction, addedItem, addedIndex );
//                    break;

//                case ListChangedType.ItemDeleted:
//                    var removedFrom = e.OldIndex;
//                    args = new NotifyCollectionChangedEventArgs(
//                        NotifyCollectionChangedAction.Remove,
//                        null,
//                        removedFrom );
//                    break;

//                case ListChangedType.ItemMoved:
//                    var movedFrom = e.OldIndex;
//                    var movedTo = e.NewIndex;
//                    var movedItem = this[ movedTo ];
//                    args = new NotifyCollectionChangedEventArgs(
//                        NotifyCollectionChangedAction.Move,
//                        movedItem,
//                        movedTo,
//                        movedFrom );
//                    break;

//                case ListChangedType.PropertyDescriptorAdded:
//                case ListChangedType.PropertyDescriptorChanged:
//                case ListChangedType.PropertyDescriptorDeleted:
//                case ListChangedType.Reset:
//                case ListChangedType.ItemChanged:
//                    args = new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset );
//                    break;
//            }

//            Ensure.That( args ).IsNotNull();
//            this.OnCollectionChangedSafe( args );
//        }

//        protected override void OnListChanged( ListChangedEventArgs e )
//        {
//            this.asyncOperation.Post( obj =>
//            {
//                var args = ( ListChangedEventArgs )obj;
//                this.OnListChangedSafe( args );
//            }, e );
//        }

//        protected static readonly object collectionChangedEventKey = new object();
//        public event NotifyCollectionChangedEventHandler CollectionChanged
//        {
//            add { this.Events.AddHandler( collectionChangedEventKey, value ); }
//            remove { this.Events.RemoveHandler( collectionChangedEventKey, value ); }
//        }

//        void OnCollectionChangedSafe( NotifyCollectionChangedEventArgs e )
//        {
//            var handler = this.Events[ collectionChangedEventKey ] as NotifyCollectionChangedEventHandler;
//            if( handler != null )
//            {
//                handler( this, e );
//            }
//        }

//        protected virtual void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
//        {
//            this.asyncOperation.Post( obj =>
//            {
//                var args = ( NotifyCollectionChangedEventArgs )obj;
//                this.OnCollectionChangedSafe( args );
//            }, e );
//        }

//        ReadOnlyCollection<ItemPropertyInfo> GetItemProperties()
//        {
//            var itl = ( ITypedList )this;
//            var itlProps = itl.GetItemProperties( null );

//            var properties = new List<ItemPropertyInfo>();
//            foreach( PropertyDescriptor pd in itlProps )
//            {
//                properties.Add
//                (
//                    new ItemPropertyInfo( pd.Name, pd.PropertyType, pd )
//                );
//            }

//            return new ReadOnlyCollection<ItemPropertyInfo>( properties );
//        }

//        /// <summary>
//        /// Gets a collection that contains information about the properties that are available on the items in a collection.
//        /// </summary>
//        /// <value></value>
//        /// <returns>
//        /// A collection that contains information about the properties that are available on the items in a collection.
//        /// </returns>
//        ReadOnlyCollection<ItemPropertyInfo> IItemProperties.ItemProperties
//        {
//            get { return this.GetItemProperties(); }
//        }
//    }
//}