using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System;
using Autofac;
using System.Reflection;
using Topics.Radical.Linq;

namespace Topics.Radical.Windows.Presentation.Boot.Modules
{
	public class PresentationModule : IAutofacBuiltinModule
	{
		public void Configure( ContainerBuilder builder, BootstrapConventions conventions, IEnumerable<Assembly> assemblies )
		{
			var types = assemblies.SelectMany( a => a.GetTypes() ).ToArray();

			types.Where( t => conventions.IsViewModel( t ) && !conventions.IsExcluded( t ) )
				.Select( t => new
				{
					Contracts = conventions.SelectViewModelContracts( t ),
					Implementation = t
				} )
				.ForEach( r =>
				{
					if ( conventions.IsShellViewModel( r.Contracts, r.Implementation ) )
					{
						builder.RegisterType( r.Implementation ).As( r.Contracts.ToArray() ).SingleInstance();
					}
					else
					{
						builder.RegisterType( r.Implementation ).As( r.Contracts.ToArray() ).InstancePerDependency();
					}
				} );

			types.Where( t => conventions.IsView( t ) && !conventions.IsExcluded( t ) )
				.Select( t => new
				{
					Contracts = conventions.SelectViewContracts( t ),
					Implementation = t
				} )
				.ForEach( r =>
				{
					if ( conventions.IsShellView( r.Contracts, r.Implementation ) )
					{
						builder.RegisterType( r.Implementation ).As( r.Contracts.ToArray() ).SingleInstance();
					}
					else
					{
						builder.RegisterType( r.Implementation ).As( r.Contracts.ToArray() ).InstancePerDependency();
					}
				} );
		}
	}
}