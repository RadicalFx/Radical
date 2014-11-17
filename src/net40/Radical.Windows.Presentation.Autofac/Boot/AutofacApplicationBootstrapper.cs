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
using Autofac;
using Autofac.Core;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.Messaging;
using Topics.Radical.Windows.Presentation.Modules;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class AutofacApplicationBootstrapper : ApplicationBootstrapper
	{
		IContainer container;
		IServiceProvider wrapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindsorApplicationBootstrapper"/> class.
		/// </summary>
		public AutofacApplicationBootstrapper()
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

			var conventions = new BootstrapConventions();

			var builder = new ContainerBuilder();
			builder.RegisterInstance( conventions ).AsSelf().SingleInstance();
			builder.Register( c => this.wrapper ).As<IServiceProvider>().SingleInstance();
			builder.Register( c => this.container ).As<IContainer>().SingleInstance();

			this.container = builder.Build();
			this.wrapper = new ServiceProviderWrapper( this.container );

			//this.container.Kernel.AddSubSystem( SubSystemConstants.NamingKey, nss );
			//this.container.Kernel.Resolver.AddSubResolver( new ArrayResolver( this.container.Kernel, true ) );


			return this.wrapper;
		}

		[ImportMany]
		IEnumerable<IAutofacModule> Modules { get; set; }

		protected override void OnCompositionContainerComposed( CompositionContainer container, IServiceProvider serviceProvider )
		{
			base.OnCompositionContainerComposed( container, serviceProvider );

			var conventions = this.container.Resolve<BootstrapConventions>();

			var allAssemblies = new HashSet<Assembly>();
			allAssemblies.Add( Assembly.GetEntryAssembly() );
			foreach ( var dll in Directory.EnumerateFiles( Helpers.EnvironmentHelper.GetCurrentDirectory(), "*.dll" ) )
			{
				var name = Path.GetFileNameWithoutExtension( dll );
				var a = Assembly.Load( name );
				if ( conventions.IncludeAssemblyInContainerScan( a ) )
				{
					allAssemblies.Add( a );
				}
			}

			var comparer = new DelegateComparer<IAutofacModule>( ( x, y ) =>
			{
				var __x = x as IAutofacBuiltinModule;
				var __y = y as IAutofacBuiltinModule;

				if ( __x == null && __y != null ) return 1;
				if ( __x != null && __y == null ) return -1;

				return 0;
			} );

			var toInstall = this.Modules.Where( i => this.ShouldInstall( i ) )
				.OrderBy( m => m, comparer )
				.ToArray();

			var injectViewInRegion = new InjectViewInRegionModule( conventions );
			var subscribeToMessageModule = new SubscribeToMessageModule( conventions );
			
			var updater = new ContainerBuilder();
			updater.RegisterModule( injectViewInRegion );
			updater.RegisterModule( subscribeToMessageModule );

			foreach ( var item in toInstall )
			{
				item.Configure( updater, conventions, allAssemblies );
			}

			updater.Update( this.container );

			injectViewInRegion.Commit( this.container );
			subscribeToMessageModule.Commit( this.container );
		}

		protected virtual Boolean ShouldInstall( IAutofacModule module )
		{
			return true;
		}

        protected override IEnumerable<T> ResolveAll<T>()
        {
            //http://stackoverflow.com/questions/1406148/autofac-resolve-all-instances-of-a-type
            return this.container.Resolve<IEnumerable<T>>();
        }
	}
}
