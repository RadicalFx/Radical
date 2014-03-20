using System.Linq;
using System.ComponentModel.Composition;
using Castle.MicroKernel.Registration;
using Topics.Radical.ComponentModel.Messaging;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	/// <summary>
	/// A Windsor installer.
	/// </summary>
	[Export( typeof( IWindsorInstaller ) )]
	public class MessageBrokerInstaller : IWindsorInstaller
	{
		/// <summary>
		/// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="store">The configuration store.</param>
		public void Install( Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store )
		{
			var conventions = container.Resolve<BootstrapConventions>();

			container.Register
			(
				AllTypes.From( conventions.EntryXapKnownTypes().ToArray() )
					.Where( t => conventions.IsMessageHandler( t ) )
					.WithService.Select( ( type, baseTypes ) => conventions.SelectMessageHandlerContracts( type ) )
			);
		}
	}
}