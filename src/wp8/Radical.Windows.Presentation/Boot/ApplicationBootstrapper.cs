using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Phone.Shell.Services;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Messaging;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public abstract class ApplicationBootstrapper : IServiceProvider
    {
        IEnumerable<TypeInfo> allTypes = null;
        IEnumerable<Assembly> catalog;
        IEnumerable<Type> compositionContainer;
        Type homeViewType;
        bool isAutoBootEnabled = true;
        bool isBootCompleted;
        IServiceProvider serviceProvider;

        protected PhoneApplicationFrame RootFrame { get; private set; }

        public ApplicationBootstrapper()
        {
            Application.Current.Exit += ( s, e ) => { };

            Application.Current.Startup += ( s, e ) =>
            {
                if ( this.isAutoBootEnabled )
                {
                    this.OnBoot();
                }
            };

            Application.Current.UnhandledException += ( s, e ) =>
            {
                if ( Debugger.IsAttached )
                {
                    // An unhandled exception has occurred; break into the debugger
                    Debugger.Break();
                }
            };

            var phoneService = new PhoneApplicationService();
            phoneService.Activated += OnActivate;
            phoneService.Deactivated += OnDeactivate;
            phoneService.Launching += OnLaunch;
            phoneService.Closing += OnClose;

            Application.Current.ApplicationLifetimeObjects.Add( phoneService );
        }

        private IEnumerable<Type> CreateCompositionContainer( IEnumerable<Assembly> catalog, IServiceProvider serviceProvider )
        {
            return catalog.SelectMany( a => a.DefinedTypes.Where( t => t.ImplementedInterfaces.Contains( typeof( IPuzzleSetupDescriptor ) ) ) );
        }

        protected virtual PhoneApplicationFrame CreatePhoneApplicationFrame()
        {
            return new PhoneApplicationFrame();
        }

        protected abstract IServiceProvider CreateServiceProvider();

        public void DefineHomeAs<THomeViewType>()
        {
            this.DefineHomeAs( typeof( THomeViewType ) );
        }

        public void DefineHomeAs( Type homeViewType )
        {
            this.homeViewType = homeViewType;
        }

        public ApplicationBootstrapper DisableAutoBoot()
        {
            this.isAutoBootEnabled = false;

            return this;
        }

        protected IEnumerable<Assembly> GetCompositionAssemblies()
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

            var all = Deployment.Current.Parts;
            var allAssemblies = all.Where( p => p.Source.IsLike(pattern) || p.Source.IsLike( radical ) );

            foreach ( var assembly in allAssemblies )
            {
                var name = new AssemblyName( Path.GetFileNameWithoutExtension( assembly.Source ) );

                if ( !temp.ContainsKey( name.Name ) )
                {
                    temp.Add( name.Name, Assembly.Load( name ) );
                }
            }

            if ( this.onGetCompositionAssemblies != null )
            {
                this.onGetCompositionAssemblies( temp );
            }

            this.allTypes = temp.SelectMany( a => a.Value.DefinedTypes );

            return temp.Values;
        }

        public object GetService( Type serviceType )
        {
            return this.serviceProvider.GetService( serviceType );
        }

        protected virtual void OnActivate( object sender, ActivatedEventArgs e )
        {

        }

        Action<Boot.BootstrapConventions> onBeforeInstall;
        
        void OnBoot()
        {
            this.serviceProvider = this.CreateServiceProvider();

            this.catalog = this.GetCompositionAssemblies();
            this.compositionContainer = this.CreateCompositionContainer( catalog, this.serviceProvider );

            if ( this.onBeforeInstall != null )
            {
                var conventions = this.serviceProvider.GetService<Boot.BootstrapConventions>();
                this.onBeforeInstall( conventions );
            }

            this.OnBoot( this.serviceProvider );
        }

        private void OnBoot( IServiceProvider serviceProvider )
        {
            //this.RootFrame = CreatePhoneApplicationFrame();
            //this.RootFrame.Navigated += OnNavigated;
            
            //this.isInitialized = true;
        }

        protected virtual void OnClose( object sender, ClosingEventArgs e )
        {

        }

        protected virtual void OnDeactivate( object sender, DeactivatedEventArgs e )
        {

        }
        
        Func<IDictionary<String, Assembly>, Task> onGetCompositionAssemblies = null;
        
        protected virtual void OnLaunch( object sender, LaunchingEventArgs e )
        {

        }

        void OnNavigated( object sender, NavigationEventArgs e )
        {
            if ( Application.Current.RootVisual != RootFrame )
            {
                Application.Current.RootVisual = RootFrame;
            }
        }

        protected virtual void SetupUICompositionEngine( IServiceProvider serviceProvider )
        {
            //RegionService.Conventions = serviceProvider.GetService<IConventionsHandler>();
        }

        //protected virtual void NavigateHome( Type homeViewType, ILaunchActivatedEventArgs args )
        //{
        //    var ns = this.serviceProvider.GetService<INavigationService>();
        //    ns.Navigate( homeViewType );
        //}
    }
}

//namespace Topics.Radical.Windows.Presentation.Boot
//{
//    public abstract  class ApplicationBootstrapper : IServiceProvider
//    {
//        bool isInitialized;
//        //protected Application Application { get; set; }
//        protected PhoneApplicationFrame RootFrame { get; private set; }



//        bool isAutoBootEnabled = true;

//        IServiceProvider serviceProvider;
//        Type homeViewType;

//        Action<Boot.BootstrapConventions> onBeforeInstall;

//        IEnumerable<TypeInfo> allTypes = null;

//        protected ApplicationBootstrapper()
//        {
//            //this.BoottimeTypesProvider = () => this.allTypes;

//            //var view = CoreApplication.GetCurrentView();
//            //view.Activated += onViewActivated;

//            //CoreApplication.Suspending += OnSuspending;
//            //CoreApplication.Resuming += OnResuming;

//            this.PrepareApplication();
//        }

//        protected virtual void PrepareApplication()
//        {
//            Application.Current.Exit += ( s, e ) =>
//            {

//            };

//            Application.Current.Startup += ( s, e ) =>
//            {
//                if ( this.isAutoBootEnabled )
//                {
//                    this.OnBoot();
//                }
//            };

//            Application.Current.UnhandledException += ( s, e ) =>
//            {
//                if ( System.Diagnostics.Debugger.IsAttached )
//                {
//                    // An unhandled exception has occurred; break into the debugger
//                    System.Diagnostics.Debugger.Break();
//                }
//            };

//            var phoneService = new PhoneApplicationService();
//            phoneService.Activated += OnActivate;
//            phoneService.Deactivated += OnDeactivate;
//            phoneService.Launching += OnLaunch;
//            phoneService.Closing += OnClose;

//            Application.Current.ApplicationLifetimeObjects.Add( phoneService );

//            if ( this.isInitialized )
//            {
//                return;
//            }

//            this.RootFrame = CreatePhoneApplicationFrame();
//            this.RootFrame.Navigated += OnNavigated;

//            this.isInitialized = true;
//        }

//        protected virtual PhoneApplicationFrame CreatePhoneApplicationFrame()
//        {
//            return new PhoneApplicationFrame();
//        }

//        void OnNavigated( object sender, NavigationEventArgs e )
//        {
//            if ( Application.Current.RootVisual != RootFrame )
//            {
//                Application.Current.RootVisual = RootFrame;
//            }
//        }

//        protected virtual void OnClose( object sender, ClosingEventArgs e )
//        {

//        }

//        protected virtual void OnLaunch( object sender, LaunchingEventArgs e )
//        {

//        }

//        protected virtual void OnDeactivate( object sender, DeactivatedEventArgs e )
//        {

//        }

//        protected virtual void OnActivate( object sender, ActivatedEventArgs e )
//        {

//        }
        
//        public void Boot()
//        {
//            if (!this.isAutoBootEnabled)
//            {
//                //this.InitializeCoreComponents(args);
//                //this.OnBoot(args);
//                this.InitializePhoneApplication();
//                this.OnBoot();
//            }
//        }

//        protected virtual async Task<IEnumerable<Assembly>> GetCompositionAssemblies()
//        {
//            var temp = new Dictionary<String, Assembly>();

//            var appAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
//            temp.Add( appAssembly.GetName().Name, appAssembly );

//            var thisAssembly = this.GetType().GetTypeInfo().Assembly;
//            if ( !temp.ContainsKey( thisAssembly.GetName().Name ) )
//            {
//                temp.Add( thisAssembly.GetName().Name, thisAssembly );
//            }

//            var pattern = String.Format( "{0}*.dll", appAssembly.GetName().Name );
//            var radical = "radical*.dll";

//            var all = await Package.Current.InstalledLocation.GetFilesAsync();
//            var allAssemblies = all.Where( f => f.Name.IsLike( pattern ) || f.Name.IsLike( radical ) );

//            foreach ( var assembly in allAssemblies )
//            {
//                var name = new AssemblyName( Path.GetFileNameWithoutExtension( assembly.Name ) );
//                if ( !temp.ContainsKey( name.Name ) )
//                {
//                    temp.Add( name.Name, Assembly.Load( name ) );
//                }
//            }

//            //if ( this.onGetCompositionAssemblies != null )
//            //{
//            //    await this.onGetCompositionAssemblies( temp );
//            //}

//            this.allTypes = temp.SelectMany( a => a.Value.DefinedTypes );

//            return temp.Values;
//        }

//        protected abstract IServiceProvider CreateServiceProvider();

//        public void DefineHomeAs<THomeViewType>()
//        {
//            this.DefineHomeAs( typeof( THomeViewType ) );
//        }

//        public void DefineHomeAs(Type homeViewType)
//        {
//            this.homeViewType = homeViewType;
//        }

//        public void DisableAutoBoot()
//        {
//            this.isAutoBootEnabled = false;
//        }

//        public ApplicationBootstrapper OnBeforeInstall(Action<Boot.BootstrapConventions> onBeforeInstall)
//        {
//            this.onBeforeInstall = onBeforeInstall;

//            return this;
//        }

//        void OnBoot()
//        {
//            this.serviceProvider = this.CreateServiceProvider();

//            if (this.onBeforeInstall != null)
//            {
//                var conventions = this.serviceProvider.GetService<Boot.BootstrapConventions>();
//                this.onBeforeInstall(conventions);
//            }

//            this.GetCompositionAssemblies();

//            //this.catalog = this.CreateAggregateCatalog(this.serviceProvider);
//            //this.compositionContainer = this.CreateCompositionContainer(this.catalog, this.serviceProvider);

//            //this.compositionContainer.ComposeParts(this);

//            //this.OnCompositionContainerComposed(this.compositionContainer, this.serviceProvider);
//            //this.SetupUICompositionEngine(this.serviceProvider);

//            //this.InitializeCurrentPrincipal();
//            //this.InitializeCultures();

//            //this.OnBoot(this.serviceProvider);

//            //if (!this.IsShuttingDown)
//            //{
//            //	this.OnBootCompleted(this.serviceProvider);
//            //	this.isBootCompleted = true;
//            //}
//        }

//        #region IServiceProvider

//        public object GetService(Type serviceType)
//        {
//            return this.serviceProvider.GetService(serviceType);
//        }

//        #endregion

//        // Do not add any additional code to this method
//        protected virtual void InitializePhoneApplication()
//        {
//            //if ( phoneApplicationInitialized )
//            //{
//            //    return;
//            //}

//            // Create the frame but don't set it as RootVisual yet; this allows the splash
//            // screen to remain active until the application is ready to render.
//            this.RootFrame = new PhoneApplicationFrame();
//            this.RootFrame.Navigated += CompleteInitializePhoneApplication;

//            // Handle navigation failures
//            this.RootFrame.NavigationFailed += RootFrame_NavigationFailed;

//            // Handle reset requests for clearing the backstack
//            this.RootFrame.Navigated += CheckForResetNavigation;

//            //// Ensure we don't initialize again
//            //phoneApplicationInitialized = true;
//        }

//        private void CompleteInitializePhoneApplication( object sender, System.Windows.Navigation.NavigationEventArgs e )
//        {
//            // Set the root visual to allow the application to render
//            if ( System.Windows.Application.Current.RootVisual != this.RootFrame )
//            {
//                System.Windows.Application.Current.RootVisual = this.RootFrame;
//            }

//            // Remove this handler since it is no longer needed
//            this.RootFrame.Navigated -= CompleteInitializePhoneApplication;
//        }

//        private void RootFrame_NavigationFailed( object sender, System.Windows.Navigation.NavigationFailedEventArgs e )
//        {
//            if ( System.Diagnostics.Debugger.IsAttached )
//            {
//                // A navigation has failed; break into the debugger
//                System.Diagnostics.Debugger.Break();
//            }
//        }

//        private void CheckForResetNavigation( object sender, System.Windows.Navigation.NavigationEventArgs e )
//        {
//            // If the app has received a 'reset' navigation, then we need to check
//            // on the next navigation to see if the page stack should be reset
//            if ( e.NavigationMode == System.Windows.Navigation.NavigationMode.Reset )
//            {
//                this.RootFrame.Navigated += ClearBackStackAfterReset;
//            }
//        }

//        private void ClearBackStackAfterReset( object sender, System.Windows.Navigation.NavigationEventArgs e )
//        {
//            // Unregister the event so it doesn't get called again
//            RootFrame.Navigated -= ClearBackStackAfterReset;

//            // Only clear the stack for 'new' (forward) and 'refresh' navigations
//            if ( e.NavigationMode != System.Windows.Navigation.NavigationMode.New && e.NavigationMode != System.Windows.Navigation.NavigationMode.Refresh )
//            {
//                return;
//            }

//            // For UI consistency, clear the entire page stack
//            while ( RootFrame.RemoveBackEntry() != null )
//            {
//                ; // do nothing
//            }
//        }
//    }
//}
