using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.Navigation.Hosts;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class PuzzleApplicationBootstrapper : ApplicationBootstrapper
	{
		IPuzzleContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindsorApplicationBootstrapper"/> class.
		/// </summary>
		protected PuzzleApplicationBootstrapper()
		{
            this.UsingAsNavigationHost( new WindowNavigationHost( Window.Current ) );
		}

		protected override IServiceProvider CreateServiceProvider()
		{
			this.container = new PuzzleContainer();
			var facade = new PuzzleContainerServiceProviderFacade( this.container );

			this.container.Register( EntryBuilder.For<ApplicationBootstrapper>()
				.UsingInstance( this ) );
			this.container.Register( EntryBuilder.For<IPuzzleContainer>()
				.UsingInstance( this.container ) );
			this.container.Register( EntryBuilder.For<IServiceProvider>()
				.UsingInstance( facade ) );
			this.container.Register( EntryBuilder.For<Boot.BootstrapConventions>()
				.UsingInstance( new Boot.BootstrapConventions() ) );

			var view = CoreApplication.GetCurrentView();
			var dispatcher = view.CoreWindow.Dispatcher;

			this.container.Register(
					EntryBuilder.For<CoreDispatcher>()
						.UsingInstance( dispatcher)
			);

			this.container.AddFacility<SubscribeToMessageFacility>();

			return facade;
		}

        NavigationHost host;

        public override ApplicationBootstrapper UsingAsNavigationHost( NavigationHost host )
        {
            this.host = host;

            return this;
        }

		[ImportMany]
		public IEnumerable<IPuzzleSetupDescriptor> Installers { get; set; }

		protected override async Task OnCompositionContainerComposed( CompositionHost container, IServiceProvider serviceProvider )
		{
			await base.OnCompositionContainerComposed( container, serviceProvider );

			var toInstall = this.Installers.Where( i => this.ShouldInstall( i ) ).ToArray();

			await this.container.SetupWith( this.BoottimeTypesProvider, toInstall );

            if ( !this.container.IsRegistered<NavigationHost>() && this.host != null ) 
            {
                this.container.Register(
                    EntryBuilder.For<NavigationHost>()
                        .UsingInstance( this.host )
                        .WithLifestyle( Lifestyle.Singleton )
                );
            }
		}

		protected virtual Boolean ShouldInstall( IPuzzleSetupDescriptor installer )
		{
			return true;
		}

		//protected override void OnBoot( IServiceProvider serviceProvider, global::Windows.ApplicationModel.Activation.LaunchActivatedEventArgs e )
		//{
		//	base.OnBoot( serviceProvider, e );

		//	//Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

		//	//Thread.CurrentPrincipal = new WindowsPrincipal( WindowsIdentity.GetCurrent() );

		//	//var broker = serviceProvider.GetService<IMessageBroker>();
		//	//broker.Subscribe<ApplicationShutdownRequest>( this, InvocationModel.Safe, m => Application.Current.Shutdown() );
		//}
	}
}
