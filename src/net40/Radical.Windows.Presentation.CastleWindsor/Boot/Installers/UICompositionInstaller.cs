using System.ComponentModel.Composition;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Regions;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	/// <summary>
	/// A Windsor installer.
	/// </summary>
	[Export( typeof( IWindsorInstaller ) )]
	public class UICompositionInstaller : IWindsorInstaller
	{
		/// <summary>
		/// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="store">The configuration store.</param>
		public void Install( Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store )
		{
            var conventions = container.Resolve<BootstrapConventions>();

			container.Register( 
				Component.For<IRegionManagerFactory>()
					.ImplementedBy<RegionManagerFactory>()
					.Overridable()
                    .PropertiesIgnore( conventions.IgnorePropertyInjection ),

				Component.For<IRegionService>()
					.ImplementedBy<RegionService>()
					.Overridable()
                    .PropertiesIgnore( conventions.IgnorePropertyInjection ),
				
				Component.For<IRegionManager>()
					.ImplementedBy<RegionManager>()
					.LifeStyle.Is( LifestyleType.Transient )
					.Overridable()
                    .PropertiesIgnore( conventions.IgnorePropertyInjection )
			);
		}
	}
}