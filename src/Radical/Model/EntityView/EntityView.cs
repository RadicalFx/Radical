namespace Radical.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Radical.ComponentModel;
    using Radical.Validation;
    using Radical.Reflection;
    using Radical.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// A full custom implementation of IBindingListView
    /// </summary>
    /// <typeparam name="T">The underlying object type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1039:ListsAreStronglyTyped" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface" )]
    public class EntityView<T> :
        IBindingList,
        IBindingListView,
        IEntityView,
        IEntityView<T>,
        IList,
        IRaiseItemChangedEvents,
        ICancelAddNew,
        ITypedList,
        IComponent,
        IDisposable,
        IServiceProvider //,
    //INotifyCollectionChanged
    //where T : class
    {
        private Indexer<T> _indexer;

        /// <summary>
        /// Gets the indexer.
        /// </summary>
        /// <value>The indexer.</value>
        protected Indexer<T> Indexer
        {
            get
            {
                if( _indexer == null )
                {
                    _indexer = new Indexer<T>( this );
                }

                return this._indexer;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is detached.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is detached; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsDetached
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns a item indicationg this instance has an array
        /// as underlying data storage or not
        /// </summary>
        /// <item>
        ///     <collection>true</collection> if this instance is array based; otherwise, <collection>false</collection>.
        /// </item>
        public Boolean IsArrayBased
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the underlying DataSource as IList
        /// </summary>
        /// <item>The DataSource</item>
        public IList DataSource
        {
            get;
            private set;
        }

        EventHandler onEntityItemViewEditBegunHandler = null;
        EventHandler onEntityItemViewEditCanceledHandler = null;
        EventHandler onEntityItemViewEditEndedHandler = null;
        PropertyChangedEventHandler onEntityItemViewPropertyChangedHandler = null;

        #region .ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityView&lt;T&gt;"/> class.
        /// </summary>
        public EntityView()
            : this( ( IList )new EntityCollection<T>() )
        {
            this.IsDetached = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public EntityView( IEntityCollection<T> list )
            : this( ( IList )list )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityView&lt;T&gt;"/> 
        /// class using the supplied array of <typeparamref name="T"/> as the
        /// underlying list of elements
        /// </summary>
        /// <param name="list">The list contining the data to build this view on</param>
        public EntityView( T[] list )
            : this( ( IList )list )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityView&lt;T&gt;"/>
        /// class using the supplied IList&lt;<typeparamref name="T"/>&gt; as the
        /// underlying list of elements
        /// </summary>
        /// <param name="list">The list contining the data to build this view on</param>
        public EntityView( IList<T> list )
            : this( ( IList )list )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        protected EntityView( IList list )
        {
            this.DataSource = list;
            this.IsArrayBased = this.DataSource.GetType().IsArray;

            this.onEntityItemViewEditBegunHandler = new EventHandler( OnEntityItemViewEditBegun );
            this.onEntityItemViewEditCanceledHandler = new EventHandler( OnEntityItemViewEditCanceled );
            this.onEntityItemViewEditEndedHandler = new EventHandler( OnEntityItemViewEditEnded );
            this.onEntityItemViewPropertyChangedHandler = new PropertyChangedEventHandler( OnEntityItemViewPropertyChanged );

            this.OnInit();
            this.Indexer.Rebuild();
            this.OnLoad();
        }

        #endregion

        protected internal virtual IEntityItemView<T> CreateEntityItemView( T sourceItem )
        {
            return new EntityItemView<T>( this, sourceItem );
        }

        /// <summary>
        /// Called just before the first full indexing
        /// </summary>
        protected virtual void OnInit()
        {
            IEntityCollection<T> dataSource = this.DataSource as IEntityCollection<T>;
            if( dataSource != null )
            {
                dataSource.CollectionChanged += OnDataSourceCollectionChanged;
            }
        }

        #region DataSource Collection EventHandlers

        /*
         * Gestisce l'evento che viene scatenato 
         * quando la collection sottostante notifica
         * che ci sono modifiche al suo interno.
         */
        void OnDataSourceCollectionChanged( object sender, CollectionChangedEventArgs<T> e )
        {
            Int32 oldIndex;
            Int32 newIndex;
            RebuildIndexesEventArgs args = new RebuildIndexesEventArgs( e.Index );

            switch( e.ChangeType )
            {
                case CollectionChangeType.ItemAdded:
                    this.OnCollectionChanged( args, e.ChangeType );
                    if( !args.Cancel )
                    {
                        this.Indexer.Rebuild();
                        newIndex = this.Indexer.FindEntityItemViewIndexInView( e.Index );
                    }
                    else
                    {
                        newIndex = e.Index;
                    }

                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemAdded, newIndex ) );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, this.Indexer[ newIndex ], newIndex ) );
                    break;

                //case CollectionChangeType.ItemSaved:
                case CollectionChangeType.ItemChanged:
                    oldIndex = this.Indexer.FindEntityItemViewIndexInView( e.Index );
                    newIndex = oldIndex;

                    this.OnCollectionChanged( args, e.ChangeType );
                    if( !args.Cancel )
                    {
                        this.Indexer.Rebuild();
                        newIndex = this.Indexer.FindEntityItemViewIndexInView( e.Index );
                    }

                    if( newIndex > -1 )
                    {
                        if( newIndex != oldIndex )
                        {
                            /*
                             * Se i due indici, prima e dopo il Rebuild sono diversi
                             * significa che la posizione dell'elemento è cambiata a seguito
                             * della modifica che ha subito, molto presumibilmente questo è
                             * dovuto al fatto che la View è sortata quindi informiamo dello
                             * spostamento
                             */
                            this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemMoved, newIndex, oldIndex ) );
                            //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Move ) );
                        }
                        else
                        {
                            this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemChanged, newIndex ) );
                            //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                        }
                    }
                    else
                    {
                        /*
                         * L'introduzione di ItemDeleted non dovrebbe più portare qui...
                         */
                        Debug.Fail( "...CollectionChangeType.ItemChanged and newIndex = -1..." );
                        this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemDeleted, oldIndex ) );
                        //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, null, oldIndex ) );
                    }
                    break;

                //case CollectionChangeType.ItemDeleted:
                case CollectionChangeType.ItemRemoved:

                    /*
                     * Qui abbiamo una discreta magagna:
                     * 
                     * quando un elemento viene rimosso dalla DataSource la griglia
                     * collegata a questa IBindingList ha bisogno di sapere l'indice
                     * (nella View) dell'elemento rimosso per farlo sparire anche a
                     * video.
                     * 
                     * Questa informazione non c'è... perchè a questo punto l'elemento
                     * è già stato rimosso e l'Indexer non è in grado di dirci dove stava.
                     * 
                     * L'unica soluzione che mi viene in mente è che l'Indexer abbia un indice
                     * che tiene traccia degli indici: Dictionary<Int32, Int32> con 
                     * SourceIndex --> ViewIndex
                     * 
                     * Un'altra possibile soluzione è che tra gli arguments ci sia anche un riferimento
                     * all'elemento rimosso, in questo modo lo potremmo cercare nella View, perderemmo in
                     * performance ma sarebbe più semplice
                     */
                    oldIndex = this.Indexer.IndexOf( ( T )e.Item );
                    this.ClearCustomValuesFor( e.Item );

                    this.OnCollectionChanged( args, e.ChangeType );
                    if( !args.Cancel )
                    {
                        this.Indexer.Rebuild();
                    }

                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemDeleted, oldIndex ) );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, null, oldIndex ) );
                    break;

                case CollectionChangeType.ItemReplaced:
                case CollectionChangeType.ItemMoved:
                case CollectionChangeType.SortChanged:
                case CollectionChangeType.Reset:
                    //case CollectionChangeType.ChangesRejected:

                    if( e.ChangeType == CollectionChangeType.ItemReplaced )
                    {
                        this.ClearCustomValuesFor( e.Item );
                    }
                    else if( e.ChangeType == CollectionChangeType.Reset && this.DataSource.Count == 0 )
                    {
                        //Supponiamo che possa esserci stata una clear...
                        this.customPropertyValues.Clear();
                    }

                    this.OnCollectionChanged( args, e.ChangeType );
                    if( !args.Cancel )
                    {
                        this.Indexer.Rebuild();
                    }
                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                    break;

                //case CollectionChangeType.ChangesAccepted:
                //    /*
                //     * In questo caso visivamente non cambia nulla
                //     * quindi non ribaltiamo niente all'esterno
                //     */
                //    break;
            }
        }

        #endregion

        /// <summary>
        /// This method is called whenever the underlying DataSource fires the CollectionChanged event,
        /// here inheritors have the opportunity to prevent this instance to rebuild indexes after the
        /// changes, the default behavior is to rebuild indexes on every change.
        /// </summary>
        /// <param name="e">The <see cref="Radical.Model.RebuildIndexesEventArgs"/> instance containing the event data.</param>
        /// <param name="changeType">Type of the change.</param>
        protected virtual void OnCollectionChanged( RebuildIndexesEventArgs e, CollectionChangeType changeType )
        {

        }

        /// <summary>
        /// Called when the View is initialized and fully indexed.
        /// </summary>
        protected virtual void OnLoad()
        {

        }

        #region IEntityItemView EventHandlers

        /// <summary>
        /// This method is called whenever a new IEntityItemView is built in order
        /// to allow the View to add event handlers to the new element
        /// </summary>
        /// <param name="item">The IEntityItemView to wire.</param>
        protected virtual internal void OnWireEntityItemView( IEntityItemView<T> item )
        {
            item.EditBegun += this.onEntityItemViewEditBegunHandler;
            item.EditCanceled += this.onEntityItemViewEditCanceledHandler;
            item.EditEnded += this.onEntityItemViewEditEndedHandler;
            item.PropertyChanged += this.onEntityItemViewPropertyChangedHandler;

            //if( this.tIsINotifySaved )
            //{
            //    ( ( INotifySaved )item.EntityItem ).Saved += this.onEntityItemViewSaved;
            //}

            //if( this.tIsINotifyDeleted )
            //{
            //    ( ( INotifyDeleted )item.EntityItem ).Deleted += this.onEntityItemViewDeleted;
            //}
        }

        /// <summary>
        /// This method is called alaways before destroyng an EntityItemView
        /// in order to allow the View to detach event handlers from the element
        /// </summary>
        /// <param name="item">The EntityItemView to unwire.</param>
        protected virtual internal void OnUnwireEntityItemView( IEntityItemView<T> item )
        {
            item.EditBegun -= this.onEntityItemViewEditBegunHandler;
            item.EditCanceled -= this.onEntityItemViewEditCanceledHandler;
            item.EditEnded -= this.onEntityItemViewEditEndedHandler;
            item.PropertyChanged -= this.onEntityItemViewPropertyChangedHandler;

            //if( this.tIsINotifySaved )
            //{
            //    ( ( INotifySaved )item.EntityItem ).Saved -= this.onEntityItemViewSaved;
            //}

            //if( this.tIsINotifyDeleted )
            //{
            //    ( ( INotifyDeleted )item.EntityItem ).Deleted -= this.onEntityItemViewDeleted;
            //}
        }

        //void OnEntityItemViewDeleted( object sender, DeletedEventArgs e )
        //{
        //    /*
        //     * Questo è l'elemento che è stato cancellato
        //     */
        //    T item = ( T )sender;

        //    /*
        //     * Ci assicuriamo che il contesto sia quello giusto,
        //     * non vedo perchè non dovrebbe ma un controllo in più
        //     * non fa male... :-)
        //     */
        //    if( this.IsInAddingNewQueue( item ) )
        //    {
        //        Int32 viewIndex = this.Indexer.IndexOf( item );
        //        this.CancelNew( viewIndex );
        //    }
        //}

        //void OnEntityItemViewSaved( object sender, SavedEventArgs e )
        //{
        //    /*
        //     * Questo è l'elemento che è stato salvato
        //     */
        //    T item = ( T )sender;

        //    /*
        //     * Ci assicuriamo che il contesto sia quello giusto,
        //     * non vedo perchè non dovrebbe ma un controllo in più
        //     * non fa male... :-)
        //     */
        //    if( this.IsInAddingNewQueue( item ) )
        //    {
        //        Int32 viewIndex = this.Indexer.IndexOf( item );
        //        this.EndNew( viewIndex );
        //    }
        //}

        void OnEntityItemViewPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            IEntityItemView<T> obj = ( IEntityItemView<T> )sender;

            Int32 index = this.Indexer.IndexOf( obj );
            if( index > -1 )
            {
                /*
                 * Scateniamo l'evento solo nel caso in cui l'item che sta comunicando una
                 * modifica sia presente nella View, potrebbe non esserci perchè in quel 
                 * momento la View è filtrata. In realtà non è vero perchè se è filtrata non
                 * dovrebbe esistere neanche l'EntityItemView
                 */
                this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemChanged, index ) );
                //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
            }
        }

        /// <summary>
        /// Called when EntityItemView edit begins
        /// </summary>
        /// <param name="item">The item. entered in edit mode</param>
        protected virtual void OnEntityItemViewEditBegun( IEntityItemView<T> item )
        {

        }

        void OnEntityItemViewEditBegun( object sender, EventArgs e )
        {
            IEntityItemView<T> item = ( IEntityItemView<T> )sender;
            this.OnEntityItemViewEditBegun( item );
        }

        /// <summary>
        /// Called when IEntityItemView edit is ended. This method can be overriden in order to decide
        /// if after an Edit operation the Index has to be rebuilt or not. One reason not to rebuild the
        /// Indexes after an edit operation is that the position, or the visibility, of the edited element
        /// could change due to the applied changes.
        /// </summary>
        /// <param name="e">A RebuildIndexesEventArgs wich allow derived classes to change the rebuild indexes behavior</param>
        protected virtual void OnEntityItemViewEditEnded( RebuildIndexesEventArgs e )
        {

        }

        void OnEntityItemViewEditEnded( object sender, EventArgs e )
        {
            /*
             * Al termine dell'edit di un elemento abbiamo un problema che 
             * non siamo in grado di risolvere in autonomia:
             * 
             * Se la View è filtrata o sortata in qualche modo collection'è la possibilità che
             * l'elemento di cui abbiamo appena finito l'editing si sposti dalla sua 
             * posizione, a causa del sort, o addirittura scompaia a causa del filtro:
             * un esempio classico è la classica Person con una proprietà Name, se la View
             * è sortata e cambio il Name la posizione dell'istanza all'interno della view
             * potrebbe dover essere cambiata, stessa cosa per il filtro se sto visualizzando
             * le sole Person il cui nome inizia per "M*" cmabiando il nome di una person
             * non è detto che questa debba restare tra quelle visibili.
             * 
             * Anche se tutto ciò concettualmente è giusto resta il problema che la variazione 
             * di visibilità o posizione potrebbe essere "mal digerita" dall'utente diamo quindi
             * la possibilità alla classe derivata di decidere se riapplicare filtro e sort a seguito
             * di una modifica ad un elemento.
             * 
             * Il comportamento è: riapplicare sempre.
             */
            IEntityItemView<T> item = ( IEntityItemView<T> )sender;

            /*
             * Dobbiamo sapere se l'elemento è visibile 
             * o meno a seconda del filtro corrente, recuperiamo
             * quindi l'indice dell'elemento prima di applicare
             * il filtro
             */
            Int32 preFilterIndex = this.Indexer.IndexOf( item );

            /*
             * Costruiamo un bel RebuildIndexesEventArgs e lo passiamo 
             * all'eventuale classe derivata che avrà la possibilità di
             * bloccare il Rebuild
             */
            RebuildIndexesEventArgs args = new RebuildIndexesEventArgs( preFilterIndex );
            this.OnEntityItemViewEditEnded( args );

            if( !args.Cancel )
            {
                /*
                 * Se nessuno blocca riapplichiamo il filtro
                 */
                this.Indexer.Rebuild();
            }

            /*
             * Recuperiamo l'indice dopo aver applicato il filtro
             * se il valore è -1 significa che è stato escluso dal 
             * filtro corrente, adesso siamo obbligati ad usare EntityItem
             * perchè se gli Indici sono stati ricreati l'istanza (sender)
             * che abbiamo di IEntityItemView non esisterà di certo più
             */
            Int32 postFilterIndex = this.Indexer.IndexOf( item.EntityItem );

            if( postFilterIndex > -1 )
            {
                if( preFilterIndex == postFilterIndex )
                {
                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemChanged, postFilterIndex ) );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                }
                else
                {
                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemMoved, postFilterIndex, preFilterIndex ) );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Move, item, postFilterIndex, preFilterIndex ) );
                }
            }
            else
            {
                this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemDeleted, preFilterIndex ) );
                //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, preFilterIndex ) );
            }


        }

        /// <summary>
        /// Called when IEntityItemView edit is canceled by the user
        /// </summary>
        /// <param name="item">The item which editing has been cancelled</param>
        protected virtual void OnEntityItemViewEditCanceled( IEntityItemView<T> item )
        {

        }

        void OnEntityItemViewEditCanceled( object sender, EventArgs e )
        {
            IEntityItemView<T> item = ( IEntityItemView<T> )sender;
            this.OnEntityItemViewEditCanceled( item );
        }

        #endregion

        /// <summary>
        /// Refreshes this instance, rebuilding internal indexes
        /// </summary>
        public virtual void Refresh()
        {
            this.Indexer.Rebuild();
            this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
            //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
        }

        /// <summary>
        /// Gets a value indicating whether moving elements is allowed in this view.
        /// </summary>
        /// <value><c>true</c> if move is allowed; otherwise, <c>false</c>.</value>
        public Boolean AllowMove
        {
            get { return !this.IsSorted; }
        }

        /// <summary>
        /// Moves the element at the specified source index to the specified new index.
        /// </summary>
        /// <param name="sourceIndex">Index of the source element to move.</param>
        /// <param name="newIndex">The destination index.</param>
        public void Move( Int32 sourceIndex, Int32 newIndex )
        {
            /*
             * Non ha nessun senso "spostare" elementi su un qualcosa 
             * che è sortato perchè dato che dopo la Move gli indici 
             * vengono ricostruiti gli elementi restano, per l'utente,
             * nella stessa identica posizione....
             */
            //TODO: migliorare l'eccezione
            Ensure.That( this )
                .If( view => view.IsSorted || view.IsArrayBased )
                .Then( ( val ) => { throw new NotSupportedException(); } );

            //TODO: migliorare l'eccezione
            Ensure.That( sourceIndex )
                .If( ( val ) => val < 0 || val > this.Count - 1 )
                .Then( ( val ) => { throw new ArgumentOutOfRangeException(); } );

            Ensure.That( newIndex )
                .If( ( val ) => val < 0 || val > this.Count - 1 )
                .Then( ( val ) => { throw new ArgumentOutOfRangeException(); } );

            /*
             * Se la view non è sorted/filterd gli indici coincidono
             * con quelli della data source altrimenti no, quindi in
             * tutti i casi la prima operazione è quella di recuperare
             * gli indici dei rispettivi item nella data source e poi
             * delegare a lei.
             */
            Int32 sourceIndexInDataSource = this.Indexer.FindObjectIndexInDataSource( sourceIndex );
            Int32 destinationIndexInDataSource = this.Indexer.FindObjectIndexInDataSource( newIndex );

            IEntityCollection<T> entityCollection = this.DataSource as IEntityCollection<T>;
            if( entityCollection != null )
            {
                /*
                 * Se è una IEntityCollection<T> usiamo Move così l'eventuale
                 * motore di tracking traccia una singola operazione atomica
                 */
                entityCollection.Move( sourceIndexInDataSource, destinationIndexInDataSource );
            }
            else
            {
                /*
                 * Altrimenti rimuoviamo e reinseriamo
                 * e alla fine indicizziamo
                 */
                T entityItem = this[ sourceIndex ].EntityItem;

                this.DataSource.RemoveAt( sourceIndexInDataSource );
                this.DataSource.Insert( destinationIndexInDataSource, entityItem );

                this.Indexer.Rebuild();
                this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
            }
        }

        /// <summary>
        /// Moves the specified item to the specified new index.
        /// </summary>
        /// <param name="item">The item to move.</param>
        /// <param name="newIndex">The destination index.</param>
        public void Move( IEntityItemView<T> item, Int32 newIndex )
        {
            Int32 sourceIndex = this.IndexOf( item );
            this.Move( sourceIndex, newIndex );
        }

        #region Filtering

        /// <summary>
        /// Gets a item indicating whether this instance is currently filtered.
        /// </summary>
        /// <item>
        ///     <collection>true</collection> if this instance is filtered; otherwise, <collection>false</collection>.
        /// </item>
        public Boolean IsFiltered
        {
            get { return this.Filter != ViewAllEntityItemViewFilter<T>.Instance; }
        }

        /// <summary>
        /// Gets a item indicating whether the data source supports filtering.
        /// </summary>
        /// <item></item>
        /// <returns>true if the data source supports filtering; otherwise, false. </returns>
        Boolean IBindingListView.SupportsFiltering
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the filter to be used to exclude items from the collection of items returned by the data source
        /// </summary>
        /// <item></item>
        /// <returns>The string used to filter items out in the item collection returned by the data source. </returns>
        String IBindingListView.Filter
        {
            get { return this.Filter.ToString(); }
            set
            {
                var msg = String.Format( "Setting '{0}' directly as Filter is not supported.", value ?? "<null>");
                throw new NotSupportedException( msg );
                //TODO: IBindingListView.Instance
                /* Bisogna capire se siamo in grado data 
                 * una stringa di convertirla in un filtro 
                 * valido...
                 */
            }
        }

        IEntityItemViewFilter IEntityView.Filter
        {
            get { return this.Filter; }
            set
            {
                if( value == null )
                {
                    this.Filter = null;
                }
                else
                {
                    IEntityItemViewFilter<T> filter = value as IEntityItemViewFilter<T>;
                    if( filter == null )
                    {
                        //TODO: impostare il messaggio dell'eccezione
                        throw new ArgumentException();
                    }

                    this.Filter = filter;
                }
            }
        }

        public void ApplyFilter( Predicate<T> predicate )
        {
            if( predicate == null )
            {
                this.Filter = null;
            }
            else
            {
                this.Filter = new PredicateEntityItemViewFilter<T>( predicate );
            }
        }

        /*
         * Il filtro correntemente impostato
         */
        private IEntityItemViewFilter<T> _filter;

        /// <summary>
        /// Gets or sets the filter to be used to exclude items from the collection of items returned by the data source
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The string used to filter items out in the item collection returned by the data source.
        /// </returns>
        public IEntityItemViewFilter<T> Filter
        {
            get
            {
                if( this._filter == null )
                {
                    /*
                     * Se il filtro è null impostiamo un bel "ViewAll"
                     */
                    this._filter = ( IEntityItemViewFilter<T> )ViewAllEntityItemViewFilter<T>.Instance;
                }

                return this._filter;
            }
            set
            {
                if( value == null )
                {
                    /*
                     * Se il filtro è null impostiamo un bel "ViewAll"
                     */
                    value = ( IEntityItemViewFilter<T> )ViewAllEntityItemViewFilter<T>.Instance;
                }

                if( this._filter != value )
                {
                    /*
                     * Se il filtro sta per cambiare:
                     *    - impostiamo la nuova istanza
                     *    - ricostruiamo gli indici in modo
                     *      da applicare le modifiche
                     *    - comunichiamo al mondo sia il reset della lista
                     *      che un evento custom che informa che il filtro
                     *      è cambiato
                     */
                    this._filter = value;

                    this.Indexer.Rebuild();

                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                    this.OnFilterChanged();
                    this.OnPropertyChanged( "IsFiltered" );
                }
            }
        }

        ///// <summary>
        ///// Reapply the given filter to the underlying data
        ///// </summary>
        ///// <param name="filter">The filter to (re)apply</param>
        //void IEntityView.ReapplyFilter( IEntityItemViewFilter filter )
        //{
        //    IEntityItemViewFilter<T> value = filter as IEntityItemViewFilter<T>;
        //    if( value == null )
        //    {
        //        //TODO: impostare il messaggio dell'eccezione
        //        throw new ArgumentException();
        //    }

        //    this.ReapplyFilter( value );
        //}


        ///// <summary>
        ///// Reapply the given filter to the underlying data
        ///// </summary>
        ///// <param name="filter">The filter to (re)apply</param>
        //public void ReapplyFilter( IEntityItemViewFilter<T> filter )
        //{
        //    if( filter == null )
        //    {
        //        filter = ( IEntityItemViewFilter<T> )ViewAllEntityItemViewFilter<T>.Instance;
        //    }

        //    Boolean filterIsChanged = false;
        //    if( this.Filter != filter )
        //    {
        //        this._filter = filter;
        //        filterIsChanged = true;
        //    }

        //    this.Indexer.Rebuild();

        //    this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
        //    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );

        //    if( filterIsChanged )
        //    {
        //        this.OnFilterChanged();
        //    }
        //}

        /// <summary>
        /// Removes the current filter applied to the data source.
        /// </summary>
        public void RemoveFilter()
        {
            this.Filter = null;
        }

        private static readonly object filterChangedEventKey = new object();

        /// <summary>
        /// Occurs when filter changes.
        /// </summary>
        public event EventHandler FilterChanged
        {
            add { this.Events.AddHandler( filterChangedEventKey, value ); }
            remove { this.Events.RemoveHandler( filterChangedEventKey, value ); }
        }

        /// <summary>
        /// Fires the FilterChanged event.
        /// </summary>
        protected virtual void OnFilterChanged()
        {
            EventHandler h = this.Events[ filterChangedEventKey ] as EventHandler;
            if( h != null )
            {
                h( this, EventArgs.Empty );
            }
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Called when this list needs to create the sort comparer used for data sorting.
        /// </summary>
        /// <returns>An implementation of the IComparer&lt;IEntityItemView&lt;T&gt;&gt; used to compare items.</returns>
        protected virtual IComparer<IEntityItemView<T>> OnCreateSortComparer()
        {
            return new EntityItemViewSortComparer<T>( this.SortDescriptions );
        }

        Boolean IsCustomComparer = false;

        private IComparer<IEntityItemView<T>> _sortComparer;
        internal IComparer<IEntityItemView<T>> SortComparer
        {
            get
            {
                if( this._sortComparer == null )
                {
                    this._sortComparer = this.OnCreateSortComparer();
                    this.IsCustomComparer = false;
                }

                return this._sortComparer;
            }
        }

        /// <summary>
        /// Gets whether the list supports sorting.
        /// </summary>
        /// <item></item>
        /// <returns>true if the list supports sorting; otherwise, false.</returns>
        Boolean IBindingList.SupportsSorting
        {
            get { return this.AllowSort; }
        }

        /// <summary>
        /// Gets a item indicating whether the data source supports advanced sorting.
        /// </summary>
        /// <item></item>
        /// <returns>true if the data source supports advanced sorting; otherwise, false. </returns>
        Boolean IBindingListView.SupportsAdvancedSorting
        {
            get { return this.AllowSort; }
        }

        /// <summary>
        /// Sorts the list based on a <see cref="T:System.ComponentModel.PropertyDescriptor"/> and a <see cref="T:System.ComponentModel.ListSortDirection"/>.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to sort by.</param>
        /// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values.</param>
        /// <exception cref="T:System.NotSupportedException">
        ///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public void ApplySort( PropertyDescriptor property, ListSortDirection direction )
        {
            ListSortDescription[] sorts = new ListSortDescription[] { new ListSortDescription( property, direction ) };
            this.ApplySort( new ListSortDescriptionCollection( sorts ) );
        }

        /// <summary>
        /// Sorts the data source based on the given <see cref="T:System.ComponentModel.ListSortDescriptionCollection"/>.
        /// </summary>
        /// <param name="sorts">The <see cref="T:System.ComponentModel.ListSortDescriptionCollection"/> containing the sorts to apply to the data source.</param>
        public void ApplySort( ListSortDescriptionCollection sorts )
        {
            //this.SortDescriptions = sorts;

            Boolean wasSorted = this.IsSorted;

            this._sortDescriptions = sorts;

            //TODO: dovremmo verificare se le SortDescriptions sono effettivamente cambiate.

            /*
             * Le SortDescriptions sono cambiate quindi l'eventuale SortComparer attuale
             * non va più bene, lo impostiamo a null in questo modo quando l'Indexer 
             * riapplica il sort e chiede alla View il SortComparer ne viene creato uno nuovo
             */
            this._sortComparer = null;
            this.IsCustomComparer = false;

            if( wasSorted && !this.IsSorted )
            {
                this.Indexer.RemoveSort();
            }
            else
            {
                this.Indexer.ApplySort();
            }

            this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
            //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
            this.OnSortChanged();
            this.OnPropertyChanged( "IsSorted" );
        }

        /// <summary>
        /// Sorts the data source based on the given sort string.
        /// </summary>
        /// <param name="sort">The sort string in T-Sql format</param>
        public void ApplySort( String sort )
        {
            if( String.IsNullOrEmpty( sort ) )
            {
                this.RemoveSort();
            }
            else
            {
                //Cerca gli elementi secondo cui sortare
                var sorts = sort.Split( ',' );

                var col = new ListSortDescription[ sorts.Length ];
                for( var i = 0; i < sorts.Length; i++ )
                {
                    /*
                     * Estrae la proprietà per cui sortare, 
                     * eventualmente seguita dalla direzione
                     * */
                    sort = sorts[ i ].Trim();

                    //L'eventuale spazio che separa la "SortDirection"
                    var pos = sort.IndexOf( ' ' );

                    String name;
                    var direction = ListSortDirection.Ascending;

                    if( pos == -1 )
                    {
                        name = sort;
                    }
                    else
                    {
                        /*
                         * Il nome è tutto ciò che c'è 
                         * prima dello spazio
                         */
                        name = sort.Substring( 0, pos );

                        /*
                         * La direzione tutto quello che viene dopo
                         */
                        var dir = sort.Substring( pos + 1 ).Trim();
                        if( String.Equals( dir, "desc", StringComparison.OrdinalIgnoreCase ) )
                        {
                            direction = ListSortDirection.Descending;
                        }
                    }

                    /*
                     * Cerchiamo di ricavre la proprietà che 
                     * rappresentata dal nome passato
                     */
                    var property = new EntityItemViewPropertyDescriptor<T>( typeof( T ).GetProperty( name ) );

                    /*
                     * Tutto ok, aggiungiamo al ns elenco
                     */
                    col[ i ] = new ListSortDescription( property, direction );
                }

                this.ApplySort( new ListSortDescriptionCollection( col ) );
            }
        }

        public void ApplySort( IComparer<IEntityItemView<T>> comparer )
        {
            //this.SortDescriptions = null;
            if( this.IsSorted )
            {
                this.RemoveSort();
            }

            this._sortComparer = comparer;
            if( comparer == null )
            {
                this.Indexer.RemoveSort();
            }
            else
            {
                this.IsCustomComparer = true;
                this.Indexer.ApplySort();
            }
        }

        /// <summary>
        /// Removes any sort applied using <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        ///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public void RemoveSort()
        {
            this.ApplySort( new ListSortDescriptionCollection() );
        }

        /// <summary>
        /// Gets whether the items in the list are sorted.
        /// </summary>
        /// <item></item>
        /// <returns>true if <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/> has been called and <see cref="M:System.ComponentModel.IBindingList.RemoveSort"/> has not been called; otherwise, false.</returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public Boolean IsSorted
        {
            get { return ( this.SortDescriptions.Count > 0 || this.IsCustomComparer ); }
        }

        /// <summary>
        /// Gets the direction of the sort.
        /// </summary>
        /// <item></item>
        /// <returns>One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values.</returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public ListSortDirection SortDirection
        {
            get
            {
                if( this.IsSorted )
                {
                    return this.SortDescriptions[ 0 ].SortDirection;
                }
                else
                {
                    return ListSortDirection.Ascending;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.
        /// </summary>
        /// <item></item>
        /// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.</returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public PropertyDescriptor SortProperty
        {
            get
            {
                if( this.IsSorted )
                {
                    return this.SortDescriptions[ 0 ].PropertyDescriptor;
                }
                else
                {
                    return null;
                }
            }
        }

        private ListSortDescriptionCollection _sortDescriptions;
        /// <summary>
        /// Gets the collection of sort descriptions currently applied to the data source.
        /// </summary>
        /// <item></item>
        /// <returns>The <see cref="T:System.ComponentModel.ListSortDescriptionCollection"/> currently applied to the data source.</returns>
        public ListSortDescriptionCollection SortDescriptions
        {
            get
            {
                if( this._sortDescriptions == null )
                {
                    this._sortDescriptions = new ListSortDescriptionCollection();
                }

                return this._sortDescriptions;
            }
            //private set
            //{
            //    Boolean wasSorted = this.IsSorted;

            //    this._sortDescriptions = value;

            //    //TODO: dovremmo verificare se le SortDescriptions sono effettivamente cambiate.

            //    /*
            //     * Le SortDescriptions sono cambiate quindi l'eventuale SortComparer attuale
            //     * non va più bene, lo impostiamo a null in questo modo quando l'Indexer 
            //     * riapplica il sort e chiede alla View il SortComparer ne viene creato uno nuovo
            //     */
            //    this._sortComparer = null;
            //    this.IsCustomComparer = false;

            //    if( wasSorted && !this.IsSorted )
            //    {
            //        this.Indexer.RemoveSort();
            //    }
            //    else
            //    {
            //        this.Indexer.ApplySort();
            //    }

            //    this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
            //    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
            //    this.OnSortChanged();
            //    this.OnPropertyChanged( "IsSorted" );
            //}
        }

        private static readonly object sortChangedEventKey = new object();

        /// <summary>
        /// Occurs when sort changes.
        /// </summary>
        public event EventHandler SortChanged
        {
            add { this.Events.AddHandler( sortChangedEventKey, value ); }
            remove { this.Events.RemoveHandler( sortChangedEventKey, value ); }
        }

        /// <summary>
        /// Fires the SortChanged event.
        /// </summary>
        protected virtual void OnSortChanged()
        {
            EventHandler h = this.Events[ sortChangedEventKey ] as EventHandler;
            if( h != null )
            {
                h( this, EventArgs.Empty );
            }
        }

        #endregion

        #region IRaiseItemChangedEvents Members

        /// <summary>
        /// Gets a item indicating whether the <see cref="T:System.ComponentModel.IRaiseItemChangedEvents"/> object raises <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> events.
        /// </summary>
        /// <item></item>
        /// <returns>true if the <see cref="T:System.ComponentModel.IRaiseItemChangedEvents"/> object raises <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> events when one of its property values changes; otherwise, false.</returns>
        public Boolean RaisesItemChangedEvents
        {
            get { return true; }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( ( IEnumerable )this.Indexer ).GetEnumerator();
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="array"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="array"/> is multidimensional.-or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
        /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
        void ICollection.CopyTo( Array array, Int32 index )
        {
            IEntityItemView<T>[] data = new IEntityItemView<T>[ this.Indexer.Count ];
            this.Indexer.CopyTo( data, index );
            data.CopyTo( array, index );
        }

        /// <summary>
        /// Gets a item indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <item></item>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        Boolean ICollection.IsSynchronized
        {
            get { return ( ( ICollection )this.DataSource ).IsSynchronized; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <item></item>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        object ICollection.SyncRoot
        {
            get { return ( ( ICollection )this.DataSource ).SyncRoot; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <item></item>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
        public Int32 Count
        {
            get { return this.Indexer.Count; }
        }

        #endregion

        #region IList Members

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        Int32 IList.Add( object value )
        {
            throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, Resources.Exceptions.CannotAccessEntityViewException, "Add" ) );
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only. </exception>
        void IList.Clear()
        {
            throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, Resources.Exceptions.CannotAccessEntityViewException, "Clear" ) );
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        Boolean IList.Contains( object value )
        {
            //return ( -1 != this.IndexOf( value as IEntityItemView<T> ) );

            //WARN: implementazione di IndexOf e Contains probabilmente errata;
            /*
             * Dobbiamo prima capire come funziona la DataView e scrivere poi dei test
             * L'inghippo è questo, da verificare se con la DataView non si verifica.
             * Utilizzando il sistema per sincronizzare gli elementi selected di una
             * listview wpf con una collection nel ViewModel succede che la sincronia
             * funziona solo dalla listview verso il vm e non viceversa, la cosa è evidente
             * perchè non potendo aggiungere alla view degli elementi selezionati
             * direttamente un EntityItemView l'unica cosa che possiamo fare è agire
             * sulla datasource ma questo comporta che i 2 entityitemview che wrappano
             * la datasource siano diversi, quando aggiungiamo l'elemento alla lista dei selected
             * da codice e sincronizziamocon la listview la listview viene qui e con in
             * mano un'istanza dell'entityitemview ci chiede se lo conteniamo, è evidente
             * che la risposta è no perchè quell'elemento sta nella lista dei selezionati
             * e non nella lista degli itemsSource della lista, quello che invece è shared
             * è la data source. In effetti, se spulciamo con reflector, sembra che la dataView
             * faccia sempre il controllo sull'entity warppata e non sull'itemview...
             */
            var contains = false;

            IEntityItemView<T> v1 = value as IEntityItemView<T>;
            if( v1 != null )
            {
                contains = this.Indexer.Contains( v1 );
            }

            //T v2 = value as T;
            //if( v2 != null )
            //{
            //    contains = this.Indexer.Contains( v2 );
            //}

            if( value is T )
            {
                contains = this.Indexer.Contains( ( T )value );
            }

            return contains;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
        /// </returns>
        Int32 IList.IndexOf( object value )
        {
            //return this.IndexOf( value as IEntityItemView<T> );

            var idx = -1;

            //T v1 = value as T;
            //if( v1 != null )
            //{
            //    idx = this.IndexOf( v1 );
            //}

            if( value is T )
            {
                idx = this.IndexOf( ( T )value );
            }

            IEntityItemView<T> v2 = value as IEntityItemView<T>;
            if( v2 != null )
            {
                idx = this.IndexOf( v2 );
            }

            return idx;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="item">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public Int32 IndexOf( IEntityItemView<T> item )
        {
            if( item != null )
            {
                if( this.IsAddingNew && Object.ReferenceEquals( this.PendingNewItem.EntityItem, item.EntityItem ) )
                {
                    return this.Count - 1;
                }

                var idx = this.Indexer.IndexOf( item );
                return idx;
            }

            return -1;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="item">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public Int32 IndexOf( T item )
        {
            return this.Indexer.IndexOf( item );
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The <see cref="T:System.Object"/> to insert into the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        /// <exception cref="T:System.NullReferenceException">
        ///     <paramref name="item"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        void IList.Insert( Int32 index, object item )
        {
            throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, Resources.Exceptions.CannotAccessEntityViewException, "Insert" ) );
        }

        /// <summary>
        /// Gets a item indicating whether the <see cref="T:System.Collections.IList"/> is read-only.
        /// </summary>
        /// <item></item>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> is read-only; otherwise, false.</returns>
        Boolean IList.IsReadOnly
        {
            get
            {
                if( this.IsArrayBased )
                {
                    return true;
                }
                else
                {
                    return this.DataSource.IsReadOnly;
                }
            }
        }

        /// <summary>
        /// Gets a item indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <item></item>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
        Boolean IList.IsFixedSize
        {
            get { return this.DataSource.IsFixedSize; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="item">The <see cref="T:System.Object"/> to remove from the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        void IList.Remove( object item )
        {
            IEntityItemView<T> eiv = item as IEntityItemView<T>;
            if( eiv != null )
            {
                Int32 eivIndex = this.IndexOf( eiv );
                if( eivIndex != -1 )
                {
                    ( ( IList )this ).RemoveAt( eivIndex );
                }
                else
                {
                    //TODO: migliorare l'eccezione
                    throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, Resources.Exceptions.CannotAccessEntityViewException, "Remove" ) );
            }
        }

        /// <summary>
        /// Override this method in order to prevent this instance to rebuild indexes after 
        /// a remove operation
        /// </summary>
        /// <param name="e">The <see cref="Radical.Model.RebuildIndexesEventArgs"/> instance containing the event data.</param>
        protected virtual void OnRemovedAt( RebuildIndexesEventArgs e )
        {
            /*
             * E' stato rimosso un elemento di default la EntityView
             * (Data) farebbe il rebuild degli indici ma 
             * siccome in questo caso la DataSource supporta le notifiche 
             * il rebuild viene già fatto a seguito degli eventi scatenati
             * dalla DataSource, chiediamo quindi alla EntityView di non
             * fare il rebuild
             */
            e.Cancel = this.DataSource is IEntityCollection<T>;
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.IList"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        void IList.RemoveAt( Int32 index )
        {
            /*
             * L'indice (index) che arriva è l'indice
             * dell'elemento nella Vista
             * Recuperiamo dall'Indexer l'indice
             * dell'elemento originale
             */
            Int32 sourceIndex = this.Indexer.FindObjectIndexInDataSource( index );
            if( sourceIndex > -1 )
            {
                /*
                 * Chiediamo alla lista su cui siamo
                 * montati di rimuovere l'elemento 
                 */
                this.DataSource.RemoveAt( sourceIndex );

                /*
                 * Anche qui abbiamo un problema, se la lista supporta
                 * le notifiche noi non dobbiamo ricostruire gli indici
                 * perchè il meccanismo è implicito, in caso contrario
                 * dobbiamo pensarci noi alla ricostruzione dell'indice
                 * l'unica possibilità che abbiamo è quella di chiedere
                 */
                RebuildIndexesEventArgs args = new RebuildIndexesEventArgs( index );
                this.OnRemovedAt( args );

                if( !args.Cancel )
                {
                    this.Indexer.Rebuild();

                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemDeleted, index ) );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, null, index ) );
                }
            }
            else
            {
                /*
                 * Se il sourceIndex è -1 significa
                 * che l'elemento è nuovo... quindi ci
                 * limitaimo a cancellare l'aggiunta del
                 * nuovo elemento
                 */
                this.CancelNew( index );
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IEntityItemView&lt;T&gt;"/> at the specified index.
        /// </summary>
        /// <item></item>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "index" )]
        public IEntityItemView<T> this[ Int32 index ]
        {
            get { return this.Indexer[ index ]; }
            set { throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, Resources.Exceptions.CannotAccessEntityViewException, "Set Accessor" ), "index" ); }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <item></item>
        object IList.this[ Int32 index ]
        {
            get { return this[ index ]; }
            set { this[ index ] = ( IEntityItemView<T> )value; }
        }

        #endregion

        #region IBindingList Members

        static readonly object listChangedEventKey = new object();

        /// <summary>
        /// Occurs when the list changes or an item in the list changes.
        /// </summary>
        public event ListChangedEventHandler ListChanged
        {
            add { this.Events.AddHandler( listChangedEventKey, value ); }
            remove { this.Events.RemoveHandler( listChangedEventKey, value ); }
        }

        /// <summary>
        /// Raises the <see cref="E:ListChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.ListChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnListChanged( ListChangedEventArgs e )
        {
            if( !this.IsInitializing )
            {
                ListChangedEventHandler h = this.Events[ listChangedEventKey ] as ListChangedEventHandler;
                if( h != null )
                {
                    h( this, e );
                }

                if( e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.Reset )
                {
                    this.OnPropertyChanged( "Count" );
                }
            }
        }

        /// <summary>
        /// Gets whether a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or an item in the list changes.
        /// </summary>
        /// <item></item>
        /// <returns>true if a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or when an item changes; otherwise, false.</returns>
        Boolean IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method.
        /// </summary>
        /// <item></item>
        /// <returns>true if the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method; otherwise, false.</returns>
        Boolean IBindingList.SupportsSearching
        {
            get { return true; }
        }

        /// <summary>
        /// Adds a new item to the list.
        /// </summary>
        /// <returns>The item added to the list.</returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     <see cref="P:System.ComponentModel.IBindingList.AllowNew"/> is false. </exception>
        object IBindingList.AddNew()
        {
            return this.AddNew();
        }

        private Boolean _allowEdit = true;
        /// <summary>
        /// Gets whether you can update items in the list.
        /// </summary>
        /// <item></item>
        /// <returns>true if you can update the items in the list; otherwise, false.</returns>
        public virtual Boolean AllowEdit
        {
            get { return this._allowEdit; }
            set
            {
                if( this._allowEdit != value )
                {
                    this._allowEdit = value;
                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
                    this.OnPropertyChanged( "AllowEdit" );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                }
            }
        }

        private Boolean _allowNew = true;
        /// <summary>
        /// Gets whether you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>.
        /// </summary>
        /// <item></item>
        /// <returns>true if you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>; otherwise, false.</returns>
        public virtual Boolean AllowNew
        {
            get
            {
                if( this.IsArrayBased )
                {
                    return false;
                }
                else
                {
                    return this._allowNew;
                }
            }
            set
            {
                if( this.IsArrayBased )
                {
                    throw new InvalidOperationException( "Cannot change AllowNew item on a ArrayBased EntityView." );
                }
                else if( this._allowNew != value )
                {
                    this._allowNew = value;
                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
                    this.OnPropertyChanged( "AllowNew" );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                }
            }
        }

        private Boolean _allowRemove = true;
        /// <summary>
        /// Gets whether you can remove items from the list, using <see cref="M:System.Collections.IList.Remove(System.Object)"/> or <see cref="M:System.Collections.IList.RemoveAt(System.Int32)"/>.
        /// </summary>
        /// <item></item>
        /// <returns>true if you can remove items from the list; otherwise, false.</returns>
        public virtual Boolean AllowRemove
        {
            get
            {
                if( this.IsArrayBased )
                {
                    return false;
                }
                else
                {
                    return this._allowRemove;
                }
            }
            set
            {
                if( this.IsArrayBased )
                {
                    throw new InvalidOperationException( "Cannot change AllowRemove item on a ArrayBased EntityView" );
                }

                if( this._allowRemove != value )
                {
                    this._allowRemove = value;
                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
                    this.OnPropertyChanged( "AllowRemove" );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                }
            }
        }

        private Boolean _allowSort = true;
        /// <summary>
        /// Gets or sets a value indicating whether this list allow sort.
        /// </summary>
        /// <value><c>true</c> if [allow sort]; otherwise, <c>false</c>.</value>
        public virtual Boolean AllowSort
        {
            get { return this._allowSort; }
            set
            {
                if( this.AllowSort != value )
                {
                    this._allowSort = value;
                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
                    this.OnPropertyChanged( "AllowSort" );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                }
            }
        }

        /// <summary>
        /// Adds the <see cref="T:System.ComponentModel.PropertyDescriptor"/> to the indexes used for searching.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to add to the indexes used for searching.</param>
        public void AddIndex( PropertyDescriptor property )
        {
            this.Indexer.AddIndex( property );
        }

        /// <summary>
        /// Adds the property with the given name to the indexes used for searching.
        /// </summary>
        /// <param name="propertyName">The name of the property to add to the indexes used for searching.</param>
        public void AddIndex( String propertyName )
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties( typeof( T ) ).Find( propertyName, false );
            this.Indexer.AddIndex( property );
        }

        /// <summary>
        /// Removes the <see cref="T:System.ComponentModel.PropertyDescriptor"/> from the indexes used for searching.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to remove from the indexes used for searching.</param>
        public void RemoveIndex( PropertyDescriptor property )
        {
            this.Indexer.RemoveIndex( property );
        }

        /// <summary>
        /// removes the property with the given name from the indexes used for searching.
        /// </summary>
        /// <param name="propertyName">The name of the property to remove from the indexes used for searching.</param>
        public void RemoveIndex( String propertyName )
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties( typeof( T ) ).Find( propertyName, false );
            this.Indexer.RemoveIndex( property );
        }

        #endregion

        public PropertyDescriptor GetProperty( String name )
        {
            var all = ( ( ITypedList )this ).GetItemProperties( null );
            foreach( PropertyDescriptor property in all )
            {
                if( property.Name == name )
                {
                    return property;
                }
            }

            return null;
        }

        #region Custom Property Mappings

        private Boolean _autoGenerateProperties = true;

        /// <summary>
        /// Gets or sets a item indicating whether this instance must auto generate properties.
        /// The default item is <collection>true</collection>
        /// </summary>
        /// <item>
        ///     <collection>true</collection> if this instance must auto generate properties; otherwise, <collection>false</collection>.
        /// </item>
        public Boolean AutoGenerateProperties
        {
            get { return this._autoGenerateProperties; }
            set
            {
                if( value != this.AutoGenerateProperties )
                {
                    this._autoGenerateProperties = value;
                }
            }
        }

        IDictionary<String, EntityItemViewPropertyDescriptor<T>> _customProperties = null;
        
        /// <summary>
        /// Gets the custom properties.
        /// </summary>
        /// <value>The custom properties.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures" )]
        protected IDictionary<String, EntityItemViewPropertyDescriptor<T>> CustomProperties
        {
            get
            {
                if( this._customProperties == null )
                {
                    this._customProperties = new Dictionary<String, EntityItemViewPropertyDescriptor<T>>();
                }

                return this._customProperties;
            }
        }

        /// <summary>
        /// Gets the all the dinamically added custom property mappings.
        /// </summary>
        /// <returns>
        /// A read-only list af duìinamically added property mappings.
        /// </returns>
        public IEnumerable<EntityItemViewPropertyDescriptor<T>> GetCustomProperties()
        {
            return this.CustomProperties.Values.ToArray<EntityItemViewPropertyDescriptor<T>>();
        }

        public Boolean IsPropertyMappingDefined( String propertyName )
        {
            return this.CustomProperties.ContainsKey( propertyName );
        }

        public EntityItemViewPropertyDescriptor<T> GetCustomProperty( String name )
        {
            return this.CustomProperties[ name ];
        }

        readonly IDictionary<T, IDictionary<String, PropertyValue>> customPropertyValues = new Dictionary<T, IDictionary<String, PropertyValue>>();

        public TValue GetCustomPropertyValue<TValue>( String customPropertyName, IEntityItemView<T> owner )
        {
            Ensure.That( customPropertyName )
                .Named( () => customPropertyName )
                .IsNotNullNorEmpty()
                .IsTrue( c => this.CustomProperties.ContainsKey( c ) );

            Ensure.That( owner ).Named( "owner" ).IsNotNull();

            return this.GetCustomPropertyValueCore<TValue>( customPropertyName, owner );
        }

        protected virtual TValue GetCustomPropertyValueCore<TValue>( String customPropertyName, IEntityItemView<T> owner )
        {
            var item = owner.EntityItem;
            if( this.customPropertyValues.ContainsKey( item ) )
            {
                var values = this.customPropertyValues[ item ];
                if( values.ContainsKey( customPropertyName ) )
                {
                    return ( ( PropertyValue<TValue> )values[ customPropertyName ] ).Value;
                }
            }
            else
            {
                var pd = this.CustomProperties[ customPropertyName ]
                    as EntityItemViewCustomPropertyDescriptor<T, TValue>;

                if( pd != null )
                {
                    var value = pd.GetDefaultValue();
                    this.SetCustomPropertyValue( customPropertyName, owner, value );

                    return value;
                }
            }

            return default( TValue );
        }

        public void SetCustomPropertyValue<TValue>( String customPropertyName, IEntityItemView<T> owner, TValue value )
        {
            Ensure.That( customPropertyName )
                .Named( "customPropertyName" )
                .IsNotNullNorEmpty()
                .IsTrue( c => this.CustomProperties.ContainsKey( c ) );

            Ensure.That( owner ).Named( "owner" ).IsNotNull();

            var item = owner.EntityItem;
            if( !this.customPropertyValues.ContainsKey( item ) )
            {
                this.customPropertyValues.Add( item, new Dictionary<String, PropertyValue>() );
            }

            var values = this.customPropertyValues[ item ];
            if( !values.ContainsKey( customPropertyName ) )
            {
                values.Add( customPropertyName, null );
            }

            values[ customPropertyName ] = new PropertyValue<TValue>( value );
        }

        private void ClearCustomValuesFor( T item )
        {
            if( this.customPropertyValues.ContainsKey( item ) )
            {
                this.customPropertyValues.Remove( item );
            }
        }

        /// <summary>
        /// Adds a the given property to the property mappings of this instance.
        /// </summary>
        /// <param name="customProperty">The custom property to add.</param>
        public virtual EntityItemViewPropertyDescriptor<T> AddPropertyMapping( EntityItemViewPropertyDescriptor<T> customProperty )
        {
            this.CustomProperties.Add( customProperty.Name, customProperty );
            this.OnListChanged( new ListChangedEventArgs( ListChangedType.PropertyDescriptorAdded, customProperty ) );

            return customProperty;
        }

        /// <summary>
        /// Adds a property mapping that maps the supplied property.
        /// </summary>
        /// <param name="propertyName">Name of the property to map to.</param>
        /// <returns>
        /// A reference to the dinamically generated property.
        /// </returns>
        public EntityItemViewPropertyDescriptor<T> AddPropertyMapping( String propertyName )
        {
            var pd = new EntityItemViewPropertyDescriptor<T>( propertyName );
            return this.AddPropertyMapping( pd );
        }

        /// <summary>
        /// Adds a property mapping that maps a property using the supplied display name.
        /// </summary>
        /// <param name="propertyName">Name of the property to map to.</param>
        /// <param name="displayName">The display name.</param>
        /// <returns>
        /// A reference to the dinamically generated property.
        /// </returns>
        public EntityItemViewPropertyDescriptor<T> AddPropertyMapping( String propertyName, String displayName )
        {
            var pd = new EntityItemViewPropertyDescriptor<T>( propertyName, displayName );
            return this.AddPropertyMapping( pd );
        }

        /// <summary>
        /// Adds a property mapping using the specified display name, the supplied property type and the supplied getter.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="customPropertyName">Custom property name, must be unique among entity properties.</param>
        /// <param name="getter">A delegate to call in order to get the value of the dinamically generated property.</param>
        /// <returns>
        /// A reference to the dinamically generated property.
        /// </returns>
        /// <remarks>
        /// Using this overload implicitly creates a read-only property because no setter has been supplied.
        /// </remarks>
        public EntityItemViewPropertyDescriptor<T> AddPropertyMapping<TProperty>(
            String customPropertyName,
            EntityItemViewValueGetter<T, TProperty> getter )
        {
            return this.AddPropertyMapping( customPropertyName, getter, null, null );
        }

        public EntityItemViewPropertyDescriptor<T> AddPropertyMapping<TProperty>(
            String customPropertyName,
            EntityItemViewValueGetter<T, TProperty> getter,
            Func<TProperty> defaultValueInterceptor )
        {
            return this.AddPropertyMapping( customPropertyName, getter, null, defaultValueInterceptor );
        }

        /// <summary>
        /// Adds a property mapping using the specified display name, the supplied property type and the supplied getter and setter.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="customPropertyName">Custom property name, must be unique among entity properties.</param>
        /// <param name="getter">A delegate to call in order to get the value of the dinamically generated property.</param>
        /// <param name="setter">A delegate to call in order to set the value of the dinamically generated property.</param>
        /// <returns>
        /// A reference to the dinamically generated property.
        /// </returns>
        public EntityItemViewPropertyDescriptor<T> AddPropertyMapping<TProperty>(
            String customPropertyName,
            EntityItemViewValueGetter<T, TProperty> getter,
            EntityItemViewValueSetter<T, TProperty> setter )
        {
            return this.AddPropertyMapping( customPropertyName, getter, setter, null );
        }

        public EntityItemViewPropertyDescriptor<T> AddPropertyMapping<TProperty>(
            String customPropertyName,
            EntityItemViewValueGetter<T, TProperty> getter,
            EntityItemViewValueSetter<T, TProperty> setter,
            Func<TProperty> defaultValueInterceptor )
        {
            var pd = new EntityItemViewCustomPropertyDescriptor<T, TProperty>(
                customPropertyName,
                getter,
                setter );

            pd.DafaultValueInterceptor = defaultValueInterceptor;

            return this.AddPropertyMapping( pd );
        }

        /// <summary>
        /// Removes the property mapping.
        /// </summary>
        /// <param name="customProperty">The custom property.</param>
        /// <returns><c>True</c> if the operation was successful, otherwise <c>false</c>.</returns>
        public virtual Boolean RemovePropertyMapping( EntityItemViewPropertyDescriptor<T> customProperty )
        {
            return this.RemovePropertyMapping( customProperty.Name );
        }

        /// <summary>
        /// Removes the property mapping.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>True</c> if the operation was successful, otherwise <c>false</c>.</returns>
        public Boolean RemovePropertyMapping( String propertyName )
        {
            EntityItemViewPropertyDescriptor<T> customProperty;
            if( this.CustomProperties.TryGetValue( propertyName, out customProperty ) )
            {
                foreach( var kvp in this.customPropertyValues )
                {
                    if( kvp.Value.ContainsKey( propertyName ) )
                    {
                        kvp.Value.Remove( propertyName );
                    }
                }

                this.CustomProperties.Remove( propertyName );

                this.OnListChanged( new ListChangedEventArgs( ListChangedType.PropertyDescriptorDeleted, customProperty ) );
                //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );

                return true;
            }

            return false;
        }

        #endregion

        #region ITypedList Members

        /// <summary>
        /// Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties on each item used to bind data.
        /// </summary>
        /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor"/> objects to find in the collection as bindable. This can be null.</param>
        /// <returns>
        /// The <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties on each item used to bind data.
        /// </returns>
        PropertyDescriptorCollection ITypedList.GetItemProperties( PropertyDescriptor[] listAccessors )
        {
            return this.OnGetItemProperties( listAccessors );
        }

        /// <summary>
        /// Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties on each item used to bind data.
        /// </summary>
        /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor"/> objects to find in the collection as bindable. This can be null.</param>
        /// <returns>
        /// The <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties on each item used to bind data.
        /// </returns>
        protected virtual PropertyDescriptorCollection OnGetItemProperties( PropertyDescriptor[] listAccessors )
        {
            if( listAccessors == null || listAccessors.Length == 0 )
            {
                List<EntityItemViewPropertyDescriptor<T>> properties = new List<EntityItemViewPropertyDescriptor<T>>();

                if( this.AutoGenerateProperties )
                {
                    /*
                     * Dobbiamo generare automaticamente le colonne, procediamo 
                     * nel più tradizionale dei metodi analizzando via reflection
                     * il tipo T e recuperando tutte le proprietà che non hanno un
                     * attributo "Bindable" impostato a false.
                     * Per ogni PropertyDescriptor trovato generiamo un nuovo
                     * EntityItemViewPropertyDescriptor<T> passandogli un riferimento
                     * alla property (PropertyInfo) su cui mapparsi 
                     */
                    IEnumerable<EntityItemViewPropertyDescriptor<T>> bindableProperties = null;

                    if( typeof( T ).IsInterface )
                    {
                        bindableProperties = this.FlattenInterfaces()
                            .Aggregate( new List<EntityItemViewPropertyDescriptor<T>>(), ( a, t ) =>
                            {
                                var all = this.GetBindablePropertiesForType( t );

                                a.AddRange( all );

                                return a;
                            } );
                    }
                    else
                    {
                        bindableProperties = this.GetBindablePropertiesForType( typeof( T ) );
                    }

                    /*
                     * aggiungiamo i descriptor così trovati alla lista di quelli che
                     * dovranno essere ritornati al chiamante
                     */
                    properties.AddRange( bindableProperties );
                }

                if( this.CustomProperties.Count > 0 )
                {
                    /*
                     * Se ci sono delle proprietà custom le aggiungiamo
                     * alla lista delle proprietà da ritornare, in questo
                     * modo se AutoGenerateProperties è true le proprietà
                     * custom verranno correttamente aggiunte in coda
                     */
                    properties.AddRange( this.GetCustomProperties() );
                }

                return new PropertyDescriptorCollection( properties.ToArray() );
            }

            return new PropertyDescriptorCollection( null );
        }

        IEnumerable<EntityItemViewPropertyDescriptor<T>> GetBindablePropertiesForType( Type t )
        {
            var all = TypeDescriptor.GetProperties( t )
                        .OfType<PropertyDescriptor>()
                        .Where( pd => !pd.Attributes.Matches( BindableAttribute.No ) )
                        .Select( pd => this.OnCreateDescriptor( pd.ComponentType.GetProperty( pd.Name ) ) );

            return all;
        }

        IEnumerable<Type> FlattenInterfaces()
        {
            Ensure.That( typeof( T ) ).IsTrue( t => t.IsInterface );

            yield return typeof( T );

            foreach( var i in typeof( T ).GetInterfaces() )
            {
                yield return i;
            }
        }

        /// <summary>
        /// Called when by this instance in each time this list needs to create a new 
        /// item property descriptor, inheritors can override this method in order to 
        /// provide their own descriptor implementation.
        /// </summary>
        /// <param name="property">The property to build the descriptor for.</param>
        /// <returns>The built descriptor.</returns>
        protected virtual EntityItemViewPropertyDescriptor<T> OnCreateDescriptor( PropertyInfo property )
        {
            return new EntityItemViewPropertyDescriptor<T>( property );
        }

        /// <summary>
        /// Returns the name of the list.
        /// </summary>
        /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor"/> objects, for which the list name is returned. This can be null.</param>
        /// <returns>The name of the list.</returns>
        String ITypedList.GetListName( PropertyDescriptor[] listAccessors )
        {
            return this.OnGetListName( listAccessors );
        }

        /// <summary>
        /// Returns the name of the list.
        /// </summary>
        /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor"/> objects, for which the list name is returned. This can be null.</param>
        /// <returns>The name of the list.</returns>
        protected virtual String OnGetListName( PropertyDescriptor[] listAccessors )
        {
            if( listAccessors == null || listAccessors.Length == 0 )
            {
                return this.GetType().Name;
            }

            return String.Empty;
        }

        #endregion

        #region Searching

        /// <summary>
        /// Returns the index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to search on.</param>
        /// <param name="key">The item of the <paramref name="property"/> parameter to search for.</param>
        /// <returns>
        /// The index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     <see cref="P:System.ComponentModel.IBindingList.SupportsSearching"/> is false. </exception>
        public Int32 Find( PropertyDescriptor property, object key )
        {
            return this.Indexer.Find( property, key );
        }

        /// <summary>
        /// Returns the index of the row that has the given property name set to the given key item.
        /// </summary>
        /// <param name="propertyName">The property name to search on.</param>
        /// <param name="key">The item of the <paramref name="propertyName"/> parameter to search for.</param>
        /// <returns>
        /// The index of the row that has the given property name set to the given key item.
        /// </returns>
        public Int32 Find( String propertyName, object key )
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties( typeof( T ) ).Find( propertyName, false );
            if( pd != null )
            {
                return this.Find( pd, key );
            }
            else
            {
                throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, Resources.Exceptions.PropertyNotFoundException, propertyName ), "propertyName" );
            }
        }

        #endregion

        /// <summary>
        /// Gets a item indicating whether this instance is adding new.
        /// The case is when AddNew as been called but nor CancelNew nor EndNew
        /// has already been called.
        /// </summary>
        /// <item>
        ///     <collection>true</collection> if this instance is adding new; otherwise, <collection>false</collection>.
        /// </item>
        public Boolean IsAddingNew
        {
            get { return this.PendingNewItem != null; }
        }

        /// <summary>
        /// Determines whether the specified item is a pending item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     <c>true</c> if the specified item is a pending new item; otherwise, <c>false</c>.
        /// </returns>
        protected Boolean IsInAddingNewQueue( T item )
        {
            return this.IsAddingNew && this.PendingNewItem.EntityItem.Equals( item );
        }

        private static readonly object addingNewEventKey = new object();

        /// <summary>
        /// Occurs when AddingNew is invoked or the list needs to add a new item.
        /// </summary>
        public event EventHandler<AddingNewEventArgs<T>> AddingNew
        {
            add { this.Events.AddHandler( addingNewEventKey, value ); }
            remove { this.Events.RemoveHandler( addingNewEventKey, value ); }
        }

        /// <summary>
        /// Raises the <see cref="E:AddingNew"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Radical.Model.AddingNewEventArgs&lt;T&gt;"/> instance containing the event data.</param>
        protected virtual void OnAddingNew( AddingNewEventArgs<T> e )
        {
            EventHandler<AddingNewEventArgs<T>> h = this.Events[ addingNewEventKey ] as EventHandler<AddingNewEventArgs<T>>;
            if( h != null )
            {
                h( this, e );
            }

            if( !e.Cancel )
            {
                IEntityCollection<T> dataSource = this.DataSource as IEntityCollection<T>;
                if( e.NewItem == null && dataSource != null && dataSource.AllowNew )
                {
                    /*
                     * il comportamento di default è che se NewItem è null
                     * e abbiamo in mano IEntityCollection<T> usiamo l'implementazione
                     * di default di CreateNew su IEntityCollection<T>
                     * Se invece ci viene fornito un elemento prendiamo quello e
                     * scavalchiamo IEntityCollection<T>
                     */
                    e.NewItem = dataSource.CreateNew();
                }
            }
        }

        private IEntityItemView<T> _pendingNewItem;

        /// <summary>
        /// Gets the pending new item.
        /// </summary>
        /// <value>The pending new item.</value>
        protected IEntityItemView<T> PendingNewItem
        {
            get { return this._pendingNewItem; }
            private set
            {
                if( value != this._pendingNewItem )
                {
                    this._pendingNewItem = value;
                    this.OnPropertyChanged( "IsAddingNew" );
                }
            }
        }

        public IEntityItemView<T> AddNew( Action<AddingNewEventArgs<T>> addNewInterceptor )
        {
            Ensure.That( addNewInterceptor ).Named( "addNewInterceptor" ).IsNotNull();

            if( !this.AllowNew )
            {
                throw new InvalidOperationException( Resources.Exceptions.AllowNewException );
            }

            var args = new AddingNewEventArgs<T>();
            addNewInterceptor( args );
            return this.OnAddNewCompleted( args );
        }

        /// <summary>
        /// Adds, if supported, a new Item to this list.
        /// </summary>
        /// <returns>The newly added item.</returns>
        public IEntityItemView<T> AddNew()
        {
            if( !this.AllowNew )
            {
                throw new InvalidOperationException( Resources.Exceptions.AllowNewException );
            }

            //Informiamo il mondo che stiamo per aggiungere
            var args = new AddingNewEventArgs<T>();
            this.OnAddingNew( args );

            return this.OnAddNewCompleted( args );
        }

        IEntityItemView<T> OnAddNewCompleted( AddingNewEventArgs<T> args )
        {
            if( !args.Cancel )
            {
                T item = args.NewItem;

                /*
                 * qui dovremmo avere in mano per forza un item
                 * perchè o è stato creato nel gestore dell'evento
                 * o ci è stato dato da EntityCollection<T> quindi se
                 * non c'è l'exception è sacrosanta!
                 */
                if( item == null )
                {
                    throw new InvalidOperationException( "New Item cannot be null." );
                }

                if( this.IsAddingNew )
                {
                    /*
                     * C'è un elemento in attesa
                     * di essere 'committed', quindi
                     * prima di aggiungerne uno nuovo
                     * eseguiamo il 'commit'
                     */
                    this.EndNew( this.Indexer.Count - 1 );
                }

                IEntityItemView<T> obj = this.CreateEntityItemView( item );

                this.OnWireEntityItemView( obj );
                this.PendingNewItem = obj;
                this.Indexer.Add( obj );

                if( args.AutoCommit )
                {
                    this.EndNew();
                }
                else
                {
                    Int32 index = this.Indexer.Count - 1;
                    this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemAdded, index ) );
                    //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, obj, index ) );
                }

                return obj;
            }
            else
            {
                //Cancel
                return null;
            }
        }

        /// <summary>
        /// Ends a pending AddNew operation.
        /// </summary>
        public void EndNew()
        {
            if( this.IsAddingNew )
            {
                this.EndNew( this.IndexOf( this.PendingNewItem ) );
            }
        }

        /// <summary>
        /// Called when EndNew].
        /// </summary>
        /// <param name="index">The index.</param>
        protected virtual void OnEndNew( Int32 index )
        {

        }

        /// <summary>
        /// Override this method in order to prevent this instance to rebuild indexes after 
        /// an EndNew operation
        /// </summary>
        /// <param name="e">The <see cref="RebuildIndexesEventArgs"/> instance containing the event data.</param>
        protected virtual void OnEndNewCompleted( RebuildIndexesEventArgs e )
        {
            /*
             * E' stato completato l'editing di un elemento
             * di default la EntityView (Radical.Model) farebbe
             * il rebuild degli indici ma siccome in questo caso
             * la DataSource supporta le notifiche il rebuild viene
             * già fatto a seguito degli eventi scatenati dalla
             * DataSource, chiediamo quindi alla IEntityView di non
             * fare il rebuild se la DataSource è di tipo IEntityCollection
             */
            e.Cancel = this.DataSource is IEntityCollection<T>;
        }

        /// <summary>
        /// Ends a pending AddNew operation.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public void EndNew( Int32 itemIndex )
        {
            if( this.AllowNew && itemIndex > -1 && itemIndex < this.Indexer.Count &&
                this.PendingNewItem != null && this.Indexer[ itemIndex ] == this.PendingNewItem )
            {
                this.OnEndNew( itemIndex );

                /*
                 * Siamo in fase di commit, come da MSDN ci assicuriamo di avere
                 * degli elementi pending (può succedere che questo metodo venga 
                 * richiamato più volte).
                 * 
                 * Sganciamo il gestore degli eventi per l'oggetto pending
                 * 
                 * E' importante notare che nelle implementazioni attuali
                 * della DataGrid e del DataGridView itemIndex corrisponderà
                 * _sempre_ all'indice dell'ultimo elemento semplicemente perchè
                 * i 2 controlli non consentono di avere più elementi 
                 * contemporaneamente in Edit e quindi alla stessa stregua in
                 * PendingAddNew 
                 */
                this.OnUnwireEntityItemView( PendingNewItem );

                /*
                 * Lo rimuoviamo dall'Indice
                 */
                this.Indexer.RemoveAt( itemIndex );

                try
                {
                    /*
                     * Lo aggiungiamo alla Collection sottostante 
                     * e aspettiamo la notifica 
                     * 
                     * sta minchia!!! perchè qui non avviene proprio un bel nulla...
                     * la collection sottostante non ci notifica proprio un cazzo...
                     * 
                     * Il problema è che la DataSource qui non sappiamo cosa sia
                     * quindi non possiamo affidarci al suo motore di notifica
                     * per sapere che un nuovo elemento è stato aggiunto e quindi
                     * ricostruire gli indici
                     */
                    this.DataSource.Add( this.PendingNewItem.EntityItem );

                    RebuildIndexesEventArgs args = new RebuildIndexesEventArgs( itemIndex );
                    this.OnEndNewCompleted( args );

                    if( !args.Cancel )
                    {
                        this.OnWireEntityItemView( this.PendingNewItem );
                        this.Indexer.Add( this.PendingNewItem );
                    }
                }
                finally
                {
                    //TODO: Exception --> Roolback
                    /*
                     * Probabilmente in caso di Exception sarebbe più sensato
                     * eseguire un "Rollback" per non perdere il riferimento
                     * al "PendingNewItem"
                     * 
                     * Rimuoviamo il riferimento all'elemento 
                     * pending
                     */
                    this.PendingNewItem = null;
                }
            }
        }

        /// <summary>
        /// Cancels a pending AddNew operation.
        /// </summary>
        public void CancelNew()
        {
            if( this.IsAddingNew )
            {
                this.CancelNew( this.IndexOf( this.PendingNewItem ) );
            }
        }

        /// <summary>
        /// Called when on CancelNew].
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        protected virtual void OnCancelNew( Int32 itemIndex )
        {

        }

        /// <summary>
        /// Cancels a pending AddNew operation that is occurring
        /// at the specified index.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public void CancelNew( Int32 itemIndex )
        {
            if( this.AllowNew && itemIndex > -1 && itemIndex < this.Indexer.Count &&
                this.PendingNewItem != null && this.Indexer[ itemIndex ] == this.PendingNewItem )
            {
                this.OnCancelNew( itemIndex );

                /*
                 * Per l'itemIndex valgono le stesse considerazione
                 * fatte per il metodo EndNew
                 */
                this.OnUnwireEntityItemView( this.PendingNewItem );

                this.Indexer.RemoveAt( itemIndex );

                this.OnListChanged( new ListChangedEventArgs( ListChangedType.ItemDeleted, itemIndex ) );
                //this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, this.pendingNewItem, itemIndex ) );

                this.PendingNewItem = null;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="EntityView&lt;T&gt;"/> is reclaimed by garbage collection.
        /// </summary>
        ~EntityView()
        {
            this.Dispose( false );
        }

        private Boolean isDisposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><collection>true</collection> to release both managed and unmanaged resources; <collection>false</collection> to release only unmanaged resources.</param>
        protected virtual void Dispose( Boolean disposing )
        {
            if( !this.isDisposed )
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
                    lock( this )
                    {
                        if( this.site != null && this.site.Container != null )
                        {
                            this.site.Container.Remove( this );
                        }

                        if( this._events != null )
                        {
                            EventHandler h = this.Events[ disposedEventKey ] as EventHandler;
                            if( h != null )
                            {
                                h( this, EventArgs.Empty );
                            }
                        }
                    }
                }

                if( this._events != null )
                {
                    this.Events.Dispose();
                    this._events = null;
                }

                //Set isDisposed flag
                this.isDisposed = true;
            }
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

        #region IComponent, IServiceProvider

        private static readonly object disposedEventKey = new object();

        /// <summary>
        /// Represents the method that handles the <see cref="E:System.ComponentModel.IComponent.Disposed"/> event of a component.
        /// </summary>
        public event EventHandler Disposed
        {
            add { this.Events.AddHandler( disposedEventKey, value ); }
            remove { this.Events.RemoveHandler( disposedEventKey, value ); }
        }

        private ISite site;

        /// <summary>
        /// Gets or sets the <see cref="T:System.ComponentModel.ISite"/> associated with the <see cref="T:System.ComponentModel.IComponent"/>.
        /// </summary>
        /// <returns>The <see cref="T:System.ComponentModel.ISite"/> object associated with the component; or null, if the component does not have a site.</returns>
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public virtual ISite Site
        {
            get { return this.site; }
            set { this.site = value; }
        }

        [NonSerialized]
        private EventHandlerList _events;

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        protected EventHandlerList Events
        {
            get
            {
                if( this._events == null )
                {
                    this._events = new EventHandlerList();
                }

                return this._events;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [design mode].
        /// </summary>
        /// <value><c>true</c> if [design mode]; otherwise, <c>false</c>.</value>
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public virtual bool DesignMode
        {
            get
            {
                if( this.site != null )
                {
                    return this.site.DesignMode;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public virtual IContainer Container
        {
            get
            {
                if( this.site != null )
                {
                    return this.site.Container;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        public virtual object GetService( Type serviceType )
        {
            if( this.site != null )
            {
                return this.site.GetService( serviceType );
            }

            return null;
        }

        #endregion

        #region INotifyPropertyChanged Members

        static readonly object propertyChangedEventKey = new object();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this.Events.AddHandler( propertyChangedEventKey, value ); }
            remove { this.Events.RemoveHandler( propertyChangedEventKey, value ); }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        protected void OnPropertyChanged<T>( Expression<Func<T>> property )
        {
            var h = this.Events[ propertyChangedEventKey ] as PropertyChangedEventHandler;
            if( h != null )
            {
                this.OnPropertyChanged( property.GetMemberName() );
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected virtual void OnPropertyChanged( String propertyName )
        {
            Ensure.That( propertyName )
                .IsNotNull()
                .IsNotEmpty();

            var h = this.Events[ propertyChangedEventKey ] as PropertyChangedEventHandler;
            if( h != null )
            {
                h( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        #endregion

        #region IEnumerable<IEntityItemView<T>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<IEntityItemView<T>> IEnumerable<IEntityItemView<T>>.GetEnumerator()
        {
            return ( ( IEnumerable<IEntityItemView<T>> )this.Indexer ).GetEnumerator();
        }

        #endregion

        void ISupportInitialize.BeginInit()
        {
            this.IsInitializing = true;
        }

        protected Boolean IsInitializing { get; private set; }

        void ISupportInitialize.EndInit()
        {
            this.IsInitializing = false;
            this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
        }
    }
}
