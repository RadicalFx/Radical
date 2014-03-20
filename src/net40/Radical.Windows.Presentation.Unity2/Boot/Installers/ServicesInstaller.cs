using System;
using System.Collections.Generic;
using System.Linq;
using o2 = Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Presentation.Extensions;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	public class ServicesInstaller : IUnityInstaller
	{
		public void Install( IUnityContainer container, BootstrapConventions conventions, IEnumerable<Type> allTypes )
		{
			allTypes
				.Where( t => conventions.IsService( t ) && !conventions.IsExcluded( t ) )
				.Select( type =>
				{
					return new
					{
						Contracts = conventions.SelectServiceContracts( type ),
						TypeTo = type
					};
				} )
				.ForEach( r =>
				{
					/*
					 * questa è una magagna perchè Unity non ci consente la registrazione dei forwarder
					 * ed è un macello perché anche se riuscissimo ad iniettare una policy che "conosce"
					 * i forwarder ci scontriamo con il fatto che le build-key, in apparenza, infilate nella
					 * ContainerRegistration vengono risolte immediatamente all'avvio e non on-demend, ergo
					 * non abbiamo come in Castle il concetto di "attesa" per le dipendenze.
					 */
					container.RegisterType( r.TypeTo, r.TypeTo,
						new ContainerControlledLifetimeManager(),
						new CandidateConstructorSelector( container ) );

					foreach ( var contract in r.Contracts )
					{
						container.RegisterType( contract,
							new InjectionFactory( c =>
							{
								return c.Resolve( r.TypeTo );
							} ) );
					}
				} );
		}
	}
}