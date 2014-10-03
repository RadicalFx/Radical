using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Threading;
using System.Configuration;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	/// <summary>
	/// A Windsor installer.
	/// </summary>
	[Export( typeof( IWindsorInstaller ) )]
	public class DefaultInstaller : IWindsorInstaller
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
                Component.For<TraceSource>()
                    .UsingFactoryMethod( () =>
                    {
                        var name = ConfigurationManager
                            .AppSettings[ "radical/windows/presentation/diagnostics/applicationTraceSourceName" ]
                            .Return( s => s, "default" );
                        
                        return new TraceSource( name );
                    } )
                    .LifeStyle.Is( LifestyleType.Singleton )
                    .PropertiesIgnore( conventions.IgnorePropertyInjection )
            );

			container.Register(
				Component.For<Application>()
					.UsingFactoryMethod( () => Application.Current )
                    .LifeStyle.Is( LifestyleType.Singleton )
                    .PropertiesIgnore( conventions.IgnorePropertyInjection ),

				Component.For<Dispatcher>()
					.UsingFactoryMethod( () => Application.Current.Dispatcher )
					.LifeStyle.Is( LifestyleType.Singleton )
                    .PropertiesIgnore( conventions.IgnorePropertyInjection )
			);

            container.Register(
                Component.For<IDispatcher>()
                    .ImplementedBy<WpfDispatcher>()
                    .LifeStyle.Is( LifestyleType.Singleton )
                    .Overridable(),

                Component.For<IMessageBroker>()
                    .ImplementedBy<MessageBroker>()
                    .LifeStyle.Is( LifestyleType.Singleton )
                    .Overridable()
                    .PropertiesIgnore( conventions.IgnorePropertyInjection )
            );
		}
	}
}