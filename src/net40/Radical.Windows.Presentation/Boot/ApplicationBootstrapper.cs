using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Messaging;
using Topics.Radical.Windows.Presentation.Regions;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Windows.Threading;
using System.Threading;
using System.Security.Principal;
using System.Globalization;
using System.Windows.Markup;
using Topics.Radical.Helpers;

#if !SILVERLIGHT
using System.Diagnostics;
using Topics.Radical.Diagnostics;
#endif

namespace Topics.Radical.Windows.Presentation.Boot
{
	/// <summary>
	/// The application bootstrapper. Provides a way to dramatically simplifly the
	/// application boot process.
	/// </summary>
	public abstract class ApplicationBootstrapper : IServiceProvider
	{

#if !SILVERLIGHT
		static readonly TraceSource logger = new TraceSource( typeof( ApplicationBootstrapper ).Name );
#endif

		IServiceProvider serviceProvider;
		AggregateCatalog catalog;
		CompositionContainer compositionContainer;

		Boolean isAutoBootEnabled = true;
		Boolean isBootCompleted;

		private Action<IServiceProvider> bootCompletedHandler;
		private Action<ApplicationShutdownArgs> shutdownHandler;
		private Action<IServiceProvider> bootHandler;
		private Func<IEnumerable<ComposablePartCatalog>> catalogDefinitionHandler;

#if !SILVERLIGHT
		ShutdownMode? mode = null;
		Mutex mutex;
		String key;
		SingletonApplicationScope singleton = SingletonApplicationScope.NotSupported;
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationBootstrapper"/> class.
		/// </summary>
		protected ApplicationBootstrapper()
		{
			var commandLine = CommandLine.GetCurrent();

#if !SILVERLIGHT
			if ( commandLine.Contains( "radical-wait-for-debugger" ) && !Debugger.IsAttached )
			{
				logger.Warning( "Application is waiting for the debugger..." );

				Int32 waitCycle = 0;
				while ( !Debugger.IsAttached && waitCycle <= 100 )
				{
					Thread.Sleep( 600 );
					waitCycle++;
				}

				if ( !Debugger.IsAttached )
				{
					logger.Warning( "Waiting for the debugger overlapped the maximum wait time of 1 minute, application will start now." );
				}
			}
			else if ( commandLine.Contains( "radical-debugger-break" ) && !Debugger.IsAttached )
			{
				Debugger.Break();
			}
#endif

			this.DefineCatalogs = () =>
			{
				var tmp = new List<ComposablePartCatalog>();

				if ( this.catalogDefinitionHandler != null )
				{
					tmp.AddRange( this.catalogDefinitionHandler() );
				}

#if SILVERLIGHT
				tmp.Add( new DeploymentCatalog() );
#else
				var entry = Assembly.GetEntryAssembly();

				var currentDirectory = Helpers.EnvironmentHelper.GetCurrentDirectory();
				var conventions = this.serviceProvider.GetService<BootstrapConventions>();
				var patterns = conventions.AssemblyFileScanPatterns(entry);

				foreach ( var p in patterns ) 
				{
					tmp.Add( new DirectoryCatalog( currentDirectory, p ) );
				}

				//var name = entry.GetName().Name;

				//var dllPattern = String.Format( "{0}*.dll", name );

				//tmp.Add( new DirectoryCatalog( currentDirectory, "Radical.*.dll" ) );
				//tmp.Add( new DirectoryCatalog( currentDirectory, dllPattern ) );
				tmp.Add( new AssemblyCatalog( entry ) );
#endif

				return tmp;
			};

			Application.Current.Startup += ( s, e ) =>
			{
				if ( this.isAutoBootEnabled )
				{
					this.OnBoot();
				}
			};

#if !SILVERLIGHT

			Application.Current.SessionEnding += ( s, e ) =>
			{
				this.IsSessionEnding = true;
			};

			Application.Current.DispatcherUnhandledException += ( s, e ) =>
			{
				var ex = e.Exception;
				this.OnUnhandledException( ex );
			};

			AppDomain.CurrentDomain.UnhandledException += ( s, e ) =>
			{
				var ex = e.ExceptionObject as Exception;
				this.OnUnhandledException( ex );
			};

#endif

			Application.Current.Exit += ( s, e ) =>
			{
				if ( !this.IsShuttingDown )
				{
#if !SILVERLIGHT
					var reason = this.IsSessionEnding ? ApplicationShutdownReason.SessionEnding : ApplicationShutdownReason.ApplicationRequest;
					this.OnShutdownCore( reason );
#else
					this.OnShutdownCore( ApplicationShutdownReason.ApplicationRequest );
#endif
				}
			};
		}

		/// <summary>
		/// Disables the auto boot.
		/// </summary>
		public ApplicationBootstrapper DisableAutoBoot()
		{
			this.isAutoBootEnabled = false;

			return this;
		}

		/// <summary>
		/// Creates the IoC service provider.
		/// </summary>
		/// <returns>The IoC service provider.</returns>
		protected abstract IServiceProvider CreateServiceProvider();

		/// <summary>
		/// Gets or sets the on create aggregate catalog func.
		/// </summary>
		/// <value>
		/// The on create aggregate catalog.
		/// </value>
		public Func<IEnumerable<ComposablePartCatalog>> DefineCatalogs { get; set; }

		/// <summary>
		/// Called in order to inject custom catalogs.
		/// </summary>
		/// <param name="catalogDefinitionHandler">The catalog definition handler.</param>
		/// <returns></returns>
		public ApplicationBootstrapper OnCatalogDefinition( Func<IEnumerable<ComposablePartCatalog>> catalogDefinitionHandler )
		{
			this.catalogDefinitionHandler = catalogDefinitionHandler;

			return this;
		}

		/// <summary>
		/// Creates the aggregate catalog.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		/// <returns>
		/// The aggregate catalog.
		/// </returns>
		protected virtual AggregateCatalog CreateAggregateCatalog( IServiceProvider serviceProvider )
		{
			var catalogs = this.DefineCatalogs().ToArray();

			return new AggregateCatalog( catalogs );
		}

		/// <summary>
		/// Creates the composition container.
		/// </summary>
		/// <param name="catalog">The catalog.</param>
		/// <param name="serviceProvider">The service provider.</param>
		/// <returns>
		/// The composition container.
		/// </returns>
		protected virtual CompositionContainer CreateCompositionContainer( AggregateCatalog catalog, IServiceProvider serviceProvider )
		{
			return new CompositionContainer( catalog );
		}

		/// <summary>
		/// Setups the UI composition engine.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		protected virtual void SetupUICompositionEngine( IServiceProvider serviceProvider )
		{
#if !WINDOWS_PHONE_8
			RegionService.CurrentService = serviceProvider.GetService<IRegionService>();
#endif
			RegionService.Conventions = serviceProvider.GetService<IConventionsHandler>();
		}

		/// <summary>
		/// Called when the composition container has been composed.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="serviceProvider">The service provider.</param>
		protected virtual void OnCompositionContainerComposed( CompositionContainer container, IServiceProvider serviceProvider )
		{

		}

		Action<Boot.BootstrapConventions> onBeforeInstall;

		/// <summary>
		/// Called before the install and boot process begins, right after the service provider creation.
		/// </summary>
		/// <param name="onBeforeInstall">The on before install.</param>
		/// <returns></returns>
		public ApplicationBootstrapper OnBeforeInstall( Action<Boot.BootstrapConventions> onBeforeInstall )
		{
			this.onBeforeInstall = onBeforeInstall;

			return this;
		}

		void OnBoot()
		{
			this.serviceProvider = this.CreateServiceProvider();

			if ( this.onBeforeInstall != null ) 
			{
				var conventions = this.serviceProvider.GetService<Boot.BootstrapConventions>();
				this.onBeforeInstall( conventions );
			}

			this.catalog = this.CreateAggregateCatalog( this.serviceProvider );
			this.compositionContainer = this.CreateCompositionContainer( this.catalog, this.serviceProvider );

			this.compositionContainer.ComposeParts( this );

			this.OnCompositionContainerComposed( this.compositionContainer, this.serviceProvider );
			this.SetupUICompositionEngine( this.serviceProvider );

#if !SILVERLIGHT

			if ( this.mode != null && this.mode.HasValue )
			{
				Application.Current.ShutdownMode = this.mode.Value;
			}

#endif

			this.InitializeCurrentPrincipal();
			this.InitializeCultures();

			this.OnBoot( this.serviceProvider );

			if ( !this.IsShuttingDown )
			{
				this.OnBootCompleted( this.serviceProvider );
				this.isBootCompleted = true;
			}
		}

		Func<CultureInfo> currentCultureHandler = () => CultureInfo.CurrentCulture;

		/// <summary>
		/// Usings as current culture.
		/// </summary>
		/// <param name="currentCultureHandler">The current culture handler.</param>
		/// <returns></returns>
		public ApplicationBootstrapper UsingAsCurrentCulture( Func<CultureInfo> currentCultureHandler )
		{
			this.currentCultureHandler = currentCultureHandler;

			return this;
		}

		Func<CultureInfo> currentUICultureHandler = () => CultureInfo.CurrentUICulture;

		/// <summary>
		/// Usings as current UI culture.
		/// </summary>
		/// <param name="currentUICultureHandler">The current UI culture handler.</param>
		/// <returns></returns>
		public ApplicationBootstrapper UsingAsCurrentUICulture( Func<CultureInfo> currentUICultureHandler )
		{
			this.currentUICultureHandler = currentUICultureHandler;

			return this;
		}

		/// <summary>
		/// Initializes the current principal.
		/// </summary>
		protected virtual void InitializeCurrentPrincipal()
		{
#if !SILVERLIGHT
			Thread.CurrentPrincipal = new WindowsPrincipal( WindowsIdentity.GetCurrent() );
#endif
		}

		/// <summary>
		/// Initializes the cultures.
		/// </summary>
		protected virtual void InitializeCultures()
		{
			var currentCulture = this.currentCultureHandler();
			var currentUICulture = this.currentUICultureHandler();

			Thread.CurrentThread.CurrentCulture = currentCulture;
			Thread.CurrentThread.CurrentUICulture = currentUICulture;

#if !SILVERLIGHT
			var xmlLang = XmlLanguage.GetLanguage( currentCulture.IetfLanguageTag );
			FrameworkElement.LanguageProperty.OverrideMetadata
			(
				forType: typeof( FrameworkElement ),
				typeMetadata: new FrameworkPropertyMetadata( xmlLang )
			);

			var fd = currentUICulture.TextInfo.IsRightToLeft ?
				FlowDirection.RightToLeft :
				FlowDirection.LeftToRight;

			FrameworkElement.FlowDirectionProperty.OverrideMetadata
			(
				forType: typeof( FrameworkElement ),
				typeMetadata: new FrameworkPropertyMetadata( fd )
			);
#endif
		}

#if !SILVERLIGHT

		/// <summary>
		/// Handles the singleton application scope.
		/// </summary>
		/// <param name="args">The args.</param>
		protected virtual void HandleSingletonApplicationStartup( SingletonApplicationStartupArgs args )
		{
			if ( args.Scope != SingletonApplicationScope.NotSupported )
			{
				String mutexName = this.key;
				switch ( args.Scope )
				{
					case SingletonApplicationScope.Local:
						mutexName = @"Local\" + mutexName;
						break;

					case SingletonApplicationScope.Global:
						mutexName = @"Global\" + mutexName;
						break;
				}

				this.mutex = new Mutex( false, mutexName );
				args.AllowStartup = this.mutex.WaitOne( TimeSpan.Zero, false );

				if ( this.onSingletonApplicationStartup != null )
				{
					this.onSingletonApplicationStartup( args );
				}
			}
		}

		Action<SingletonApplicationStartupArgs> onSingletonApplicationStartup;

		/// <summary>
		/// Called when a singleton application startup.
		/// </summary>
		/// <param name="onSingletonApplicationStartup">The singleton application startup handler.</param>
		/// <returns></returns>
		public ApplicationBootstrapper OnSingletonApplicationStartup( Action<SingletonApplicationStartupArgs> onSingletonApplicationStartup )
		{
			this.onSingletonApplicationStartup = onSingletonApplicationStartup;

			return this;
		}

#endif

		/// <summary>
		/// Called in order to execute the boot process.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		protected virtual void OnBoot( IServiceProvider serviceProvider )
		{
#if !SILVERLIGHT

			var broker = serviceProvider.GetService<IMessageBroker>();
			broker.Subscribe<ApplicationShutdownRequest>( this, InvocationModel.Safe, ( s, m ) =>
			{
				this.OnShutdownCore( ApplicationShutdownReason.UserRequest );
			} );

			var args = new SingletonApplicationStartupArgs( this.singleton );
			this.HandleSingletonApplicationStartup( args );

			if ( args.AllowStartup )
			{
				if ( this.bootHandler != null )
				{
					this.bootHandler( serviceProvider );
				}
			}
			else
			{
				this.OnShutdownCore( ApplicationShutdownReason.MultipleInstanceNotAllowed );
			}
#else
			if( this.bootHandler != null )
			{
				this.bootHandler( serviceProvider );
			}
#endif
		}

		/// <summary>
		/// Boots this instance.
		/// </summary>
		public void Boot()
		{
			if ( !this.isAutoBootEnabled && !this.isBootCompleted )
			{
				this.OnBoot();
			}
		}

#if !SILVERLIGHT
		/// <summary>
		/// Shutdowns this application.
		/// </summary>
		public void Shutdown()
		{
			this.OnShutdownCore( ApplicationShutdownReason.UserRequest );
		}
#endif

		/// <summary>
		/// Called when the boot process has been completed.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		protected virtual void OnBootCompleted( IServiceProvider serviceProvider )
		{
			var broker = serviceProvider.TryGetService<IMessageBroker>();
			if ( broker != null )
			{
				broker.Broadcast( this, new ApplicationBootCompleted( this ) );
			}

			if ( this.bootCompletedHandler != null )
			{
				this.bootCompletedHandler( serviceProvider );
			}
		}

		void OnShutdownCore( ApplicationShutdownReason reason )
		{
#if !SILVERLIGHT
			var canceled = false;
#endif

			try
			{
#if !SILVERLIGHT
				if ( reason == ApplicationShutdownReason.UserRequest && this.isBootCompleted )
				{
					//messaggio per notificare ed eventualmente cancellare
					var msg = new ApplicationShutdownRequested( this, reason );

					var broker = this.GetService<IMessageBroker>();
					broker.Dispatch( this, msg );

					canceled = msg.Cancel;

					if ( canceled )
					{
						broker.Broadcast( this, new ApplicationShutdownCanceled( this, reason ) );
						return;
					}
				}
#endif

				this.IsShuttingDown = true;

				if ( this.isBootCompleted )
				{
					this.GetService<IMessageBroker>().Broadcast( this, new ApplicationShutdown( this, reason ) );
				}

				var args = new ApplicationShutdownArgs()
				{
					Reason = reason,
					IsBootCompleted = this.isBootCompleted
				};

				this.OnShutdown( args );

				if ( this.shutdownHandler != null )
				{
					this.shutdownHandler( args );
				}

				if ( this.isBootCompleted )
				{
					this.catalog.Dispose();
					this.compositionContainer.Dispose();
					if ( this.serviceProvider is IDisposable )
					{
						( ( IDisposable )this.serviceProvider ).Dispose();
					}
				}

#if !SILVERLIGHT

				if ( this.mutex != null )
				{
					this.mutex.Dispose();
					this.mutex = null;
				}
#endif

			}
			finally
			{
#if !SILVERLIGHT
				if ( !canceled && reason != ApplicationShutdownReason.ApplicationRequest )
				{
					Application.Current.Shutdown();
				}
#endif

				this.catalog = null;
				this.compositionContainer = null;
				this.serviceProvider = null;

				RegionService.CurrentService = null;
				RegionService.Conventions = null;
			}
		}

		/// <summary>
		/// Called when the application shutdowns.
		/// </summary>
		protected virtual void OnShutdown( ApplicationShutdownArgs e )
		{

		}

#if !SILVERLIGHT

		/// <summary>
		/// Registers this application as singleton.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public ApplicationBootstrapper RegisterAsSingleton( String key )
		{
			return this.RegisterAsSingleton( key, SingletonApplicationScope.Local );
		}

		/// <summary>
		/// Registers this application as singleton.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="scope">The scope.</param>
		/// <returns></returns>
		public ApplicationBootstrapper RegisterAsSingleton( String key, SingletonApplicationScope scope )
		{
			this.key = key;
			this.singleton = scope;

			return this;
		}

		/// <summary>
		/// Overrides the shutdown mode.
		/// </summary>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		public ApplicationBootstrapper OverrideShutdownMode( ShutdownMode mode )
		{
			this.mode = mode;

			return this;
		}

#endif

		/// <summary>
		/// Called when the application is booting.
		/// </summary>
		/// <param name="bootHandler">The boot handler.</param>
		/// <returns></returns>
		public ApplicationBootstrapper OnBoot( Action<IServiceProvider> bootHandler )
		{
			this.bootHandler = bootHandler;
			return this;
		}

#if !SILVERLIGHT

		/// <summary>
		/// Gets a value indicating whether the operating system session is ending.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the operating system session is ending; otherwise, <c>false</c>.
		/// </value>
		protected Boolean IsSessionEnding
		{
			get;
			private set;
		}

		private Action<Exception> unhandledExceptionHandler;

		/// <summary>
		/// Called when an unhandled exception occurs.
		/// </summary>
		/// <param name="exception">The exception.</param>
		protected virtual void OnUnhandledException( Exception exception )
		{
			if ( this.unhandledExceptionHandler != null )
			{
				this.unhandledExceptionHandler( exception );
			}
		}

		/// <summary>
		/// Allows to inject an handler for unhandled exception(s).
		/// </summary>
		/// <param name="unhandledExceptionHandler">The unhandled exception handler.</param>
		/// <returns></returns>
		public ApplicationBootstrapper OnUnhandledException( Action<Exception> unhandledExceptionHandler )
		{
			this.unhandledExceptionHandler = unhandledExceptionHandler;
			return this;
		}

#endif

		/// <summary>
		/// Gets a value indicating whether this application is shutting down.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this application is shutting down; otherwise, <c>false</c>.
		/// </value>
		protected Boolean IsShuttingDown
		{
			get;
			private set;
		}

		/// <summary>
		/// Called when the boot process is completed.
		/// </summary>
		/// <param name="bootCompletedHandler">The boot completed handler.</param>
		/// <returns></returns>
		public ApplicationBootstrapper OnBootCompleted( Action<IServiceProvider> bootCompletedHandler )
		{
			this.bootCompletedHandler = bootCompletedHandler;
			return this;
		}

		/// <summary>
		/// Called when the application is shuting down.
		/// </summary>
		/// <param name="shutdownHandler">The shutdown handler.</param>
		/// <returns></returns>
		public ApplicationBootstrapper OnShutdown( Action<ApplicationShutdownArgs> shutdownHandler )
		{
			this.shutdownHandler = shutdownHandler;
			return this;
		}

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
	}
}
