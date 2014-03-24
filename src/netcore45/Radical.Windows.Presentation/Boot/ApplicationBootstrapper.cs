using System;
using System.Windows;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using Windows.UI.Xaml;
using System.Composition;
using System.Linq;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel;
using System.IO;
using Topics.Radical.Windows.Presentation.Messaging;
using Topics.Radical.Windows.Presentation.Navigation.Hosts;
using Windows.ApplicationModel.Search;
using Windows.UI.ApplicationSettings;

namespace Topics.Radical.Windows.Presentation.Boot
{
    /// <summary>
    /// The application bootstrapper. Provides a way to dramatically simplifly the
    /// application boot process.
    /// </summary>
    public abstract class ApplicationBootstrapper : IServiceProvider
    {
        IServiceProvider serviceProvider;
        CompositionHost compositionHost;
        Type homeViewType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBootstrapper"/> class.
        /// </summary>
        protected ApplicationBootstrapper()
        {
            this.BoottimeTypesProvider = () => this.allTypes;

            var view = CoreApplication.GetCurrentView();
            view.Activated += onViewActivated;

            CoreApplication.Suspending += OnSuspending;
            CoreApplication.Resuming += OnResuming;
        }

        async void OnResuming( object sender, object e )
        {
            await this.OnResuming( e );
        }

        protected async virtual Task OnResuming( Object e )
        {

        }

        async void OnSuspending( object sender, SuspendingEventArgs e )
        {
            await this.OnSuspending( e );
        }

        protected async virtual Task OnSuspending( SuspendingEventArgs e )
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            await this.Suspend();

            deferral.Complete();
        }

        public async Task Suspend()
        {
            var storage = this.serviceProvider.GetService<ISuspensionManager>();
            await storage.SuspendAsync();
        }

        async void onViewActivated( CoreApplicationView sender, IActivatedEventArgs e )
        {
            var shouldActivateWindow = true;
            switch ( e.Kind )
            {
                case ActivationKind.Launch:
                    if ( this.isAutoBootEnabled )
                    {
                        await this.InitializeCoreComponents( e );
                        await this.OnBoot( ( ILaunchActivatedEventArgs )e );
                        shouldActivateWindow = false;
                    }
                    break;

                case ActivationKind.Search:
                    await this.InitializeCoreComponents( e );
                    await this.OnSearch( ( ISearchActivatedEventArgs )e );
                    break;

                case ActivationKind.ShareTarget:
                    await this.InitializeCoreComponents( e );
                    await this.OnShare( ( IShareTargetActivatedEventArgs )e );
                    break;

                case ActivationKind.Protocol:
                    await this.InitializeCoreComponents( e );
                    await this.OnProtocol( ( IProtocolActivatedEventArgs )e );
                    break;

                case ActivationKind.CachedFileUpdater:
                case ActivationKind.CameraSettings:
                case ActivationKind.ContactPicker:
                case ActivationKind.Device:
                case ActivationKind.File:
                case ActivationKind.FileOpenPicker:
                case ActivationKind.FileSavePicker:
                case ActivationKind.PrintTaskSettings:
                default:
                    await this.InitializeCoreComponents( e );
                    await this.OnUnhandledActivationKind( e );
                    break;
            }

            if ( shouldActivateWindow && e.PreviousExecutionState != ApplicationExecutionState.Running )
            {
                Window.Current.Activate();
            }
        }

        public void DefineHomeAs<THomeViewType>()
        {
            this.DefineHomeAs( typeof( THomeViewType ) );
        }

        public void DefineHomeAs( Type homeViewType )
        {
            this.homeViewType = homeViewType;
        }

        Func<IDictionary<String, Assembly>, Task> onGetCompositionAssemblies = null;

        public ApplicationBootstrapper OnGetCompositionAssemblies( Func<IDictionary<String, Assembly>, Task> handler )
        {
            this.onGetCompositionAssemblies = handler;

            return this;
        }

        IEnumerable<TypeInfo> allTypes = null;
        bool isAutoBootEnabled = true;

        protected virtual async Task<IEnumerable<Assembly>> GetCompositionAssemblies()
        {
            var temp = new Dictionary<String, Assembly>();

            var appAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
            temp.Add( appAssembly.GetName().Name, appAssembly );

            var thisAssembly = this.GetType().GetTypeInfo().Assembly;
            if ( !temp.ContainsKey( thisAssembly.GetName().Name ) )
            {
                temp.Add( thisAssembly.GetName().Name, thisAssembly );
            }

            var pattern = String.Format( "{0}*.dll", appAssembly.GetName().Name );
            var radical = "radical*.dll";

            var all = await Package.Current.InstalledLocation.GetFilesAsync();
            var allAssemblies = all.Where( f => f.Name.IsLike( pattern ) || f.Name.IsLike( radical ) );

            foreach ( var assembly in allAssemblies )
            {
                var name = new AssemblyName( Path.GetFileNameWithoutExtension( assembly.Name ) );
                if ( !temp.ContainsKey( name.Name ) )
                {
                    temp.Add( name.Name, Assembly.Load( name ) );
                }
            }

            if ( this.onGetCompositionAssemblies != null )
            {
                await this.onGetCompositionAssemblies( temp );
            }

            this.allTypes = temp.SelectMany( a => a.Value.DefinedTypes );

            return temp.Values;
        }

        //public Func<IEnumerable<Assembly>> CompositionAssembliesProvider { get; set; }

        public Func<IEnumerable<TypeInfo>> BoottimeTypesProvider { get; set; }

        protected virtual async Task InitializeCoreComponents( IActivatedEventArgs e )
        {
            if ( e.PreviousExecutionState != ApplicationExecutionState.Running )
            {
                /*
                 * L'applicazione viene lanciata e prima non era in esecuzione
                 * dobbiamo fare il setup di tutto il nostro mondo
                 */
                this.serviceProvider = this.CreateServiceProvider();
                this.compositionHost = await this.CreateCompositionHost( this.serviceProvider );

                this.compositionHost.SatisfyImports( this );

                await this.OnCompositionContainerComposed( this.compositionHost, this.serviceProvider );
                this.SetupUICompositionEngine( this.serviceProvider );

                if ( this.SearchSuggestionsRequestHandler != null || this.serviceProvider.TryGetService<ISearchRequestHandler>() != null )
                {
                    //We have a search request handler, we suppose the search extension is declared.
                    SearchPane.GetForCurrentView().SuggestionsRequested += OnSuggestionsRequested;
                }

                SettingsPane.GetForCurrentView().CommandsRequested += ( s, args ) =>
                {
                    var handler = this.serviceProvider.TryGetService<ISettingsCommandsRequestHandler>();
                    if ( handler != null )
                    {
                        handler.Handle( args );
                    }
                };
            }
        }

        private async void OnSuggestionsRequested( SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args )
        {
            await this.OnSearchSuggestionsRequested( sender, args );
        }

        public Func<IProtocolActivatedEventArgs, Task> ProtocolRequestHandler { get; set; }

        protected async virtual Task OnProtocol( IProtocolActivatedEventArgs e )
        {
            var handler = this.serviceProvider.TryGetService<ComponentModel.IProtocolRequestHandler>();
            if ( handler != null )
            {
                await handler.OnProtocolRequest( e );
            }
            else if ( this.ProtocolRequestHandler != null )
            {
                await this.ProtocolRequestHandler( e );
            }
            else
            {
                //TODO: should we throw?
            }
        }

        public Func<SearchPane, SearchPaneSuggestionsRequestedEventArgs, Task> SearchSuggestionsRequestHandler { get; set; }

        public Func<ISearchActivatedEventArgs, Task> SearchRequestHandler { get; set; }

        protected async virtual Task OnSearchSuggestionsRequested( SearchPane searchPane, SearchPaneSuggestionsRequestedEventArgs e )
        {
            var handler = this.serviceProvider.TryGetService<ComponentModel.ISearchRequestHandler>();
            if ( handler != null )
            {
                await handler.OnSearchSuggestionsRequest( searchPane, e );
            }
            else if ( this.SearchSuggestionsRequestHandler != null )
            {
                await this.SearchSuggestionsRequestHandler( searchPane, e );
            }
        }

        protected async virtual Task OnSearch( ISearchActivatedEventArgs e )
        {
            var handler = this.serviceProvider.TryGetService<ComponentModel.ISearchRequestHandler>();
            if ( handler != null )
            {
                await handler.OnSearchRequest( e );
            }
            else if ( this.SearchRequestHandler != null )
            {
                await this.SearchRequestHandler( e );
            }
            else
            {
                //TODO: should we throw?
            }
        }

        public Func<IShareTargetActivatedEventArgs, Task> ShareRequestHandler { get; set; }

        protected async virtual Task OnShare( IShareTargetActivatedEventArgs e )
        {
            var handler = this.serviceProvider.TryGetService<ComponentModel.IShareRequestHandler>();
            if ( handler != null )
            {
                await handler.OnShareRequest( e );
            }
            else if ( this.ShareRequestHandler != null )
            {
                await this.ShareRequestHandler( e );
            }
            else
            {
                //TODO: should we throw?
            }
        }

        public Func<IActivatedEventArgs, Task> UnhandledActivationKindHandler { get; set; }

        protected async virtual Task OnUnhandledActivationKind( IActivatedEventArgs e )
        {
            if ( this.UnhandledActivationKindHandler != null )
            {
                await this.UnhandledActivationKindHandler( e );
            }
        }

        Func<IServiceProvider, Task> bootHandler;

        public ApplicationBootstrapper OnBoot( Func<IServiceProvider, Task> bootHandler )
        {
            this.bootHandler = bootHandler;

            return this;
        }

        protected virtual async Task OnBoot( ILaunchActivatedEventArgs args )
        {
            await this.OnBoot( this.serviceProvider, args );
            if ( this.bootHandler != null )
            {
                await this.bootHandler( this.serviceProvider );
            }

            var preview = this.GetService<IWantConventionsPreview>();
            if ( preview != null )
            {
                var conventions = this.GetService<IConventionsHandler>();
                preview.PreviewConventions( conventions );
            }

            if ( !String.IsNullOrWhiteSpace( args.Arguments ) )
            {
                /*
                    * se abbiamo degli args da command line ci limitamo
                    * a rimbalzare a chi di dovere.
                    */
                await this.HandleLaunchArguments( args );
            }
            else if ( args.PreviousExecutionState == ApplicationExecutionState.Terminated )
            {
                /*
                    * non abbiamo args sulla command line e lo stato precedente era 
                    * terminated signica che l'applicazione è stata sospesa...resume
                    */
                var service = this.serviceProvider.GetService<ISuspensionManager>();
                await service.ResumeAsync();
            }
            else if ( args.PreviousExecutionState != ApplicationExecutionState.Running )
            {
                /*
                    * nessuno dei precedenti e lo stato precedente 
                    * non era running...andiamo a casa.
                    */
                this.NavigateHome( this.homeViewType, args );
            }

            await this.OnBootCompleted( this.serviceProvider );

            Window.Current.Activate();
        }

        public async Task Boot( ILaunchActivatedEventArgs args )
        {
            if ( !this.isAutoBootEnabled )
            {
                await this.InitializeCoreComponents( args );
                await this.OnBoot( args );
            }
        }

        public Func<ILaunchActivatedEventArgs, Task> LaunchArgumentsHandler { get; set; }

        protected virtual async Task HandleLaunchArguments( ILaunchActivatedEventArgs e )
        {
            var handler = this.serviceProvider.TryGetService<ComponentModel.ILaunchArgumentsHandler>();
            if ( handler != null )
            {
                await handler.OnLaunch( e );
            }
            else if ( this.LaunchArgumentsHandler != null )
            {
                await this.LaunchArgumentsHandler( e );
            }
            else
            {
                //TODO: should we throw?
            }
        }

        protected virtual void NavigateHome( Type homeViewType, ILaunchActivatedEventArgs args )
        {
            var ns = this.serviceProvider.GetService<INavigationService>();
            ns.Navigate( homeViewType );
        }

        public abstract ApplicationBootstrapper UsingAsNavigationHost( NavigationHost host );

        /// <summary>
        /// Creates the IoC service provider.
        /// </summary>
        /// <returns>The IoC service provider.</returns>
        protected abstract IServiceProvider CreateServiceProvider();

        /// <summary>
        /// Creates the composition container.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>
        /// The composition container.
        /// </returns>
        protected virtual async Task<CompositionHost> CreateCompositionHost( IServiceProvider serviceProvider )
        {
            var all = await this.GetCompositionAssemblies();
            var config = new ContainerConfiguration()
                .WithAssemblies( all );

            var host = config.CreateContainer();

            return host;
        }

        /// <summary>
        /// Setups the UI composition engine.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        protected virtual void SetupUICompositionEngine( IServiceProvider serviceProvider )
        {
            //RegionService.CurrentService = serviceProvider.GetService<IRegionService>();
            //RegionService.Conventions = serviceProvider.GetService<IConventionsHandler>();
        }

        /// <summary>
        /// Called when the composition container has been composed.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceProvider">The service provider.</param>
        protected virtual async Task OnCompositionContainerComposed( CompositionHost container, IServiceProvider serviceProvider )
        {
            if ( this.onBeforeInstall != null )
            {
                var conventions = serviceProvider.GetService<Boot.BootstrapConventions>();
                this.onBeforeInstall( conventions );
            }
        }

        Action<Boot.BootstrapConventions> onBeforeInstall;

        public ApplicationBootstrapper OnBeforeInstall( Action<Boot.BootstrapConventions> onBeforeInstall )
        {
            this.onBeforeInstall = onBeforeInstall;

            return this;
        }

        /// <summary>
        /// Called in order to execute the boot process.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        protected virtual async Task OnBoot( IServiceProvider serviceProvider, ILaunchActivatedEventArgs e )
        {

        }

        /// <summary>
        /// Called when the boot process has been completed.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        protected virtual async Task OnBootCompleted( IServiceProvider serviceProvider )
        {
            if ( this.onBootCompletedHandler != null ) 
            {
                await this.onBootCompletedHandler( serviceProvider );
            }

            var broker = serviceProvider.TryGetService<IMessageBroker>();
            if ( broker != null )
            {
                await broker.DispatchAsync( this, new ApplicationBootCompleted() );
            }
        }

        Func<IServiceProvider, Task> onBootCompletedHandler;

        public ApplicationBootstrapper OnBootCompleted( Func<IServiceProvider, Task> onBootCompletedHandler ) 
        {
            this.onBootCompletedHandler = onBootCompletedHandler;

            return this;
        }

        //void OnShutdownCore()
        //{
        //	//this.catalog.Dispose();
        //	this.compositionContainer.Dispose();
        //	if( this.serviceProvider is IDisposable )
        //	{
        //		( ( IDisposable )this.serviceProvider ).Dispose();
        //	}

        //	//this.catalog = null;
        //	this.compositionContainer = null;
        //	this.serviceProvider = null;

        //	//RegionService.CurrentService = null;
        //	//RegionService.Conventions = null;
        //}

        ///// <summary>
        ///// Called when the application shutdowns.
        ///// </summary>
        ///// <param name="e">The <see cref="System.Windows.ExitEventArgs"/> instance containing the event data.</param>
        //protected virtual void OnShutdown()
        //{
        //	this.OnShutdownCore();
        //}

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        public object GetService( Type serviceType )
        {
            return this.serviceProvider.GetService( serviceType );
        }

        public void DisableAutoBoot()
        {
            this.isAutoBootEnabled = false;
        }
    }
}
