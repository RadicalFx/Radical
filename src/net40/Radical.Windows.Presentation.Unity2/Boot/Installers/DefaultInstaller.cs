using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Threading;
using System.Configuration;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System;
using Topics.Radical.Windows.Presentation.Extensions;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	public class DefaultInstaller : IUnityInstaller
	{
		public void Install( IUnityContainer container, BootstrapConventions conventions, IEnumerable<Type> allTypes )
		{
			container.RegisterType<TraceSource>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory( c =>
				{
					var name = ConfigurationManager
							.AppSettings[ "radical/windows/presentation/diagnostics/applicationTraceSourceName" ]
							.Return( s => s, "default" );

					return new TraceSource( name );
				} ) );

			container.RegisterType<Application>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory( c =>
				{
					return Application.Current;
				} ) );

			container.RegisterType<Dispatcher>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory( c =>
				{
					return Application.Current.Dispatcher;
				} ) );

			container.RegisterType<IMessageBroker, MessageBroker>( new ContainerControlledLifetimeManager(), new CandidateConstructorSelector(container) );
			container.RegisterType<IDispatcher, WpfDispatcher>( new ContainerControlledLifetimeManager(), new CandidateConstructorSelector( container ) );
		}
	}
}