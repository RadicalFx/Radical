using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Navigation.Hosts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Services
{
    class NavigationService : INavigationService
    {
        readonly NavigationHost navigationHost;
        readonly IViewResolver viewResolver;
        readonly IConventionsHandler conventions;
        readonly IAnalyticsServices analyticsServices;

        Stack<JournalEntry> journal = new Stack<JournalEntry>();

        public NavigationService( NavigationHost navigationHost, IViewResolver viewResolver, IConventionsHandler conventions, IAnalyticsServices analyticsServices )
        {
            this.navigationHost = navigationHost;
            this.viewResolver = viewResolver;
            this.conventions = conventions;
            this.analyticsServices = analyticsServices;
        }

        public Boolean CanGoBack
        {
            get { return this.journal.Count > 1; }
        }

        public void GoBack()
        {
            Ensure.That( this.CanGoBack ).Is( true );

            var source = this.journal.Pop();
            var destination = this.journal.Peek();

            this.HandleNavigation( source, destination, NavigationMode.Back, true );
        }

        public Boolean CanGoForward
        {
            get { return false; }
        }

        public void GoForward()
        {
            throw new NotSupportedException();
        }

        public void Navigate<TView>() where TView : DependencyObject
        {
            this.Navigate( typeof( TView ), null );
        }

        public void Navigate<TView>( Object navigationArguments ) where TView : DependencyObject
        {
            this.Navigate( typeof( TView ), navigationArguments );
        }

        public void Navigate( String uri )
        {
            this.Navigate( uri, null );
        }

        public void Navigate( String uri, Object navigationArguments )
        {
            var segments = uri.Split( new[] { '/' }, StringSplitOptions.RemoveEmptyEntries );

            var v = segments.Last() + "View";
            var ns = "*.Presentation";
            if ( segments.Length > 1 )
            {
                var tmp_ns = String.Join( ".", segments.Take( segments.Length - 1 ).ToArray() );
                ns = ns + "." + tmp_ns;
            }

            var viewType = Application.Current.GetType()
                .GetTypeInfo()
                .Assembly
                .DefinedTypes
                .Where( t => t.Namespace.IsLike( ns ) && t.Name == v )
                .Single()
                .AsType();

            this.Navigate( viewType, navigationArguments );
        }

        public void Navigate( Type viewType )
        {
            this.Navigate( viewType, null );
        }

        public void Navigate( Type viewType, Object navigationArguments )
        {
            //TODO: ensure args are serializable.

            var source = this.journal.Any() ? this.journal.Peek() : null;
            var destination = new JournalEntry()
            {
                ViewType = viewType,
                Arguments = navigationArguments
            };

            this.journal.Push( destination );

            this.HandleNavigation( source, destination, NavigationMode.New, false );
        }

        void EnsureView( JournalEntry entry )
        {
            if ( entry != null && entry.View == null )
            {
                var view = this.viewResolver.GetView( entry.ViewType );
                entry.View = view;
            }
        }

        void HandleNavigation( JournalEntry source, JournalEntry destination, NavigationMode mode, Boolean isCached )
        {
            this.EnsureView( source );
            this.EnsureView( destination );

            var toEventArgs = new NavigationEventArgs( mode, destination.Arguments, destination.Storage, isCached );

            if ( source != null )
            {
                var src = this.conventions.GetViewDataContext( source.View );
                if ( src is IExpectNavigatingAwayCallback )
                {
                    var fromEventArgs = new NavigatingAwayEventArgs( mode, source.Storage );
                    ( ( IExpectNavigatingAwayCallback )src ).OnNavigatingAway( fromEventArgs );

                    if ( fromEventArgs.Cancel )
                    {
                        return;
                    }
                }
            }

            var dest = this.conventions.GetViewDataContext( destination.View );

            if ( dest is IExpectNavigatingToCallback )
            {
                ( ( IExpectNavigatingToCallback )dest ).OnNavigatingTo( toEventArgs );
            }

            this.navigationHost.Content = destination.View;

            if ( source != null )
            {
                var sourceViewModel = this.conventions.GetViewDataContext( source.View );
                if ( sourceViewModel is IExpectNavigatedAwayCallback )
                {
                    ( ( IExpectNavigatedAwayCallback )sourceViewModel ).OnNavigatedAway( mode );
                }
            }

            if ( dest is IExpectNavigatedToCallback )
            {
                ( ( IExpectNavigatedToCallback )dest ).OnNavigatedTo( toEventArgs );
            }
        }

        public void Suspend( ISuspensionManager manager )
        {
            var history = this.journal.ToArray();
            manager.SetValue( "-navigation-history", history, StorageLocation.Local );
        }

        public void Resume( ISuspensionManager manager )
        {
            var history = manager.GetValue<JournalEntry[]>( "-navigation-history" );
            this.journal = new Stack<JournalEntry>( history.Reverse() );

            var destination = this.journal.Peek();
            this.HandleNavigation( null, destination, NavigationMode.Resume, false );
        }
    }

    class NavigationScopedStorage : INavigationScopedStorage
    {
        Dictionary<string, StorageItem> dictionary;

        public NavigationScopedStorage( Dictionary<string, StorageItem> dictionary )
        {
            this.dictionary = dictionary;
        }

        public object GetData( string key )
        {
            return this.dictionary[ key ].Data;
        }

        public T GetData<T>( string key )
        {
            return ( T )this.dictionary[ key ].Data;
        }

        public void SetData( string key, object data, StorageLocation location )
        {
            this.dictionary[ key ] = new StorageItem()
            {
                Location = location,
                Data = data
            };
        }

        public void RemoveData( string key )
        {
            if ( this.dictionary.ContainsKey( key ) )
            {
                this.dictionary.Remove( key );
            }
        }

        public bool Contains( string key )
        {
            return this.dictionary.ContainsKey( key );
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }
    }

    [DataContract]
    class JournalEntry
    {
        [DataMember( Name = "Storage" )]
        Dictionary<String, StorageItem> _storage;
        NavigationScopedStorage _navigationScopedStorage;

        public JournalEntry()
        {
            this.Key = Guid.NewGuid().ToString();

            this._storage = new Dictionary<string, StorageItem>();
            //this.Storage = new NavigationScopedStorage( this._storage );
        }

        [DataMember]
        public String Key { get; set; }

        [DataMember( Name = "ViewType" )]
        String _viewTypeString;
        Type _viewType;

        public Type ViewType
        {
            get
            {
                if ( _viewType == null )
                {
                    _viewType = Type.GetType( _viewTypeString, true );
                }

                return _viewType;
            }
            set
            {
                this._viewType = value;
                this._viewTypeString = value.AssemblyQualifiedName;
            }
        }

        public DependencyObject View { get; set; }

        [DataMember]
        public object Arguments { get; set; }

        public NavigationScopedStorage Storage
        {
            get
            {
                if ( this._navigationScopedStorage == null )
                {
                    this._navigationScopedStorage = new NavigationScopedStorage( this._storage );
                }

                return this._navigationScopedStorage;
            }
        }
    }
}