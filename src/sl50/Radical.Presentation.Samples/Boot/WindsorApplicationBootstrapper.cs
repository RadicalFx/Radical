using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Topics.Radical.Windows.Presentation.Boot;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using System.Xml;
using System.Windows;
using System.Windows.Resources;
using System.Net;
using Castle.Facilities;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical;

namespace Radical.Presentation.Samples.Boot
{
	public class WindsorApplicationBootstrapper<TShellView> : ApplicationBootstrapper 
		where TShellView : UIElement
	{
		IWindsorContainer windsor;

		[ImportMany]
		public IEnumerable<IWindsorInstaller> Installers { get; set; }

		protected override IServiceProvider CreateServiceProvider()
		{
			this.windsor = new WindsorContainer();
			var wrapper = new WindsorServiceProviderWrapper( this.windsor );

			//this.windsor.Kernel.AddSubSystem( SubSystemConstants.NamingKey, nss );
			this.windsor.Kernel.Resolver.AddSubResolver( new ArrayResolver( this.windsor.Kernel, true ) );

			this.windsor.Register( Component.For<IWindsorContainer>().Instance( this.windsor ) );
			this.windsor.Register( Component.For<IServiceProvider>().Instance( wrapper ) );
			this.windsor.Register( Component.For<BootstrapConventions>() );

			this.windsor.AddFacility<SubscribeToMessageFacility>();
			this.windsor.AddFacility<InjectViewInRegionFacility>();

			return wrapper;
		}

		protected override void OnCompositionContainerComposed( CompositionContainer container, IServiceProvider serviceProvider )
		{
			base.OnCompositionContainerComposed( container, serviceProvider );

			var toInstall = this.Installers.ToArray();

			this.windsor.Install( toInstall );
		}

		protected override void OnBootCompleted( IServiceProvider serviceProvider )
		{
			var root = serviceProvider.GetService<IViewResolver>().GetView<TShellView>();
			App.Current.RootVisual = root;

			base.OnBootCompleted( serviceProvider );
		}
	}
}
