using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System;
using Microsoft.Practices.Unity;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Presentation.Extensions;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	public class PresentationInstaller : IUnityInstaller
	{
		public void Install( IUnityContainer container, BootstrapConventions conventions, IEnumerable<Type> allTypes )
		{
			allTypes
				.Where( t => conventions.IsViewModel( t ) && !conventions.IsExcluded( t ) )
				.Select( type => new
				{
					TypeFrom = conventions.SelectViewModelContracts( type ).Single(),
					TypeTo = type
				} )
				.ForEach( r =>
				{
					if ( conventions.IsShellViewModel( new[] { r.TypeFrom }, r.TypeTo ) )
					{
						container.RegisterType( r.TypeFrom, r.TypeTo, new ContainerControlledLifetimeManager(), new CandidateConstructorSelector( container ) );
					}
					else
					{
						container.RegisterType( r.TypeFrom, r.TypeTo, new TransientLifetimeManager(), new CandidateConstructorSelector( container ) );
					}
				} );

			allTypes
				.Where( t => conventions.IsView( t ) && !conventions.IsExcluded( t ) )
				.Select( type => new
				{
					TypeFrom = conventions.SelectViewContracts( type ).Single(),
					TypeTo = type
				} )
				.ForEach( r =>
				{
					if ( conventions.IsShellView( new[] { r.TypeFrom }, r.TypeTo ) )
					{
						container.RegisterType( r.TypeFrom, r.TypeTo, new ContainerControlledLifetimeManager(), new CandidateConstructorSelector( container ) );
					}
					else
					{
						container.RegisterType( r.TypeFrom, r.TypeTo, new TransientLifetimeManager(), new CandidateConstructorSelector( container ) );
					}
				} );
		}
	}
}