using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Threading;
using System.Configuration;
using Autofac;
using System.Reflection;
using System.Collections.Generic;

namespace Topics.Radical.Windows.Presentation.Boot.Modules
{
	public class DefaultModule : IAutofacBuiltinModule
	{
		public void Configure( ContainerBuilder builder, BootstrapConventions conventions, IEnumerable<Assembly> assemblies )
		{
			builder.Register( c =>
			{
				var name = ConfigurationManager
							.AppSettings[ "radical/windows/presentation/diagnostics/applicationTraceSourceName" ]
							.Return( s => s, "default" );

				return new TraceSource( name );
			} )
			.As<TraceSource>().SingleInstance();

			builder.Register( c => Application.Current ).As<Application>().SingleInstance();
			builder.Register( c => Application.Current.Dispatcher ).As<Dispatcher>().SingleInstance();
			builder.RegisterType<WpfDispatcher>().As<IDispatcher>().SingleInstance();
			builder.RegisterType<MessageBroker>().As<IMessageBroker>().SingleInstance();
		}
	}
}