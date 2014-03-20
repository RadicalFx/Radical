using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Linq;
using Autofac;
using Topics.Radical.Linq;

namespace Topics.Radical.Windows.Presentation.Boot.Modules
{
	public class MessageBrokerModule : IAutofacBuiltinModule
	{
		public void Configure( ContainerBuilder builder, BootstrapConventions conventions, IEnumerable<Assembly> assemblies )
		{
			assemblies.SelectMany( a => a.GetTypes() )
				.Where( t => conventions.IsMessageHandler( t ) && !conventions.IsExcluded( t ) )
				.Select( t => new
				{
					Contracts = conventions.SelectMessageHandlerContracts( t ),
					Implementation = t
				} )
				.ForEach( r =>
				{
					builder.RegisterType( r.Implementation ).As( r.Contracts.ToArray() ).SingleInstance();
				} );
		}
	}
}