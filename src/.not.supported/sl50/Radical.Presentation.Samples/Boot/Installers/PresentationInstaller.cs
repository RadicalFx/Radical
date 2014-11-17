using System.ComponentModel.Composition;
using Castle.Core;
using Castle.MicroKernel.Registration;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	/// <summary>
	/// A Windsor installer.
	/// </summary>
	[Export( typeof( IWindsorInstaller ) )]
	public class PresentationInstaller : IWindsorInstaller
	{
		/// <summary>
		/// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="store">The configuration store.</param>
		public void Install( Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store )
		{
			var conventions = container.Resolve<BootstrapConventions>();
			var allTypes = conventions.EntryXapKnownTypes();

			container.Register
			(
				AllTypes.From( allTypes )
					.Where( t => conventions.IsViewModel( t ) )
					.WithService.Select( ( type, baseTypes ) => conventions.SelectViewModelContracts( type ) )
					.Configure( r =>
					{
						r.Properties( pi => false );
						if( conventions.IsShellViewModel( r ) )
						{
							r.LifeStyle.Is( LifestyleType.Singleton );
						}
						else
						{
							r.LifeStyle.Is( LifestyleType.Transient );
						}
					} )
			);

			container.Register
			(
				AllTypes.From( allTypes )
					.Where( t => conventions.IsView( t ) )
					.WithService.Select( ( type, baseTypes ) => conventions.SelectViewContracts( type ) )
					.Configure( r =>
					{
						r.Properties( pi => false );
						if( conventions.IsShellView( r ) )
						{
							r.LifeStyle.Is( LifestyleType.Singleton );
						}
						else
						{
							r.LifeStyle.Is( LifestyleType.Transient );
						}
					} )

			);
		}
	}
}