using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using Microsoft.Practices.Unity;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.Boot;
using Topics.Radical.Windows.Presentation.Extensions;
using Topics.Radical.Windows.Presentation.Messaging;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class UnityApplicationBootstrapper : ApplicationBootstrapper
	{
		IUnityContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnityApplicationBootstrapper"/> class.
		/// </summary>
		public UnityApplicationBootstrapper()
		{

		}

		protected override IServiceProvider CreateServiceProvider()
		{
			//var nss = new DelegateNamingSubSystem()
			//{
			//	SubSystemHandler = ( s, hs ) =>
			//	{
			//		if ( hs.Any( h => h.ComponentModel.IsOverridable() ) )
			//		{
			//			var nonOverridable = hs.Except( hs.Where( h => h.ComponentModel.IsOverridable() ) );
			//			if ( nonOverridable.Any() )
			//			{
			//				return nonOverridable.Single();
			//			}
			//		}

			//		return null;
			//	}
			//};

			this.container = new UnityContainer();

			//this.container.Kernel.AddSubSystem( SubSystemConstants.NamingKey, nss );

			//in teoria Unity lo fa per i fatti suoi
			//this.container.Kernel.Resolver.AddSubResolver( new ArrayResolver( this.container.Kernel, true ) );

			var wrapper = new ServiceProviderWrapper( this.container );

			this.container.RegisterInstance<IServiceProvider>( wrapper );
			this.container.RegisterInstance<IUnityContainer>( this.container );
			this.container.RegisterType<BootstrapConventions>( new ContainerControlledLifetimeManager() );

			this.container.AddNewExtension<SubscribeToMessageExtension>();
			this.container.AddNewExtension<InjectViewInRegionExtension>();

			return wrapper;
		}

		[ImportMany]
		IEnumerable<IUnityInstaller> Installers { get; set; }

		protected override void OnCompositionContainerComposed( CompositionContainer container, IServiceProvider serviceProvider )
		{
			base.OnCompositionContainerComposed( container, serviceProvider );

			var toInstall = this.Installers.Where( i => this.ShouldInstall( i ) ).ToArray();

			var conventions = this.container.Resolve<Boot.BootstrapConventions>();

			var allTypes = new HashSet<Type>( Assembly.GetEntryAssembly().GetTypes() );
			foreach ( var dll in Directory.EnumerateFiles( Helpers.EnvironmentHelper.GetCurrentDirectory(), "*.dll" ) )
			{
				var name = Path.GetFileNameWithoutExtension( dll );
				var a = Assembly.Load( name );
				if ( conventions.IncludeAssemblyInContainerScan( a ) )
				{
					var ts = a.GetTypes();
					foreach ( var t in ts )
					{
						allTypes.Add( t );
					}
				}
			}

			foreach ( var installer in toInstall )
			{
				installer.Install( this.container, conventions, allTypes );
			}
		}

		protected virtual Boolean ShouldInstall( IUnityInstaller installer )
		{
			return true;
		}

        protected override IEnumerable<T> ResolveAll<T>()
        {
            return this.container.ResolveAll<T>();
        }
	}
}
