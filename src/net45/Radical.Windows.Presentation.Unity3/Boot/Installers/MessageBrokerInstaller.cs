using System;
using System.Linq;
using Topics.Radical.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.Unity;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.Extensions;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	public class MessageBrokerInstaller : IUnityInstaller
	{
		public void Install( IUnityContainer container, BootstrapConventions conventions, IEnumerable<Type> allTypes )
		{
			allTypes.Where( type => conventions.IsMessageHandler( type ) && !conventions.IsExcluded( type ) )
				//.Select( type => new
				//{
				//	TypeFrom = conventions.SelectMessageHandlerContracts( type ).Single(),
				//	TypeTo = type
				//} )
				//.ForEach( r =>
				//{
				//	container.RegisterType( r.TypeFrom, r.TypeTo, new ContainerControlledLifetimeManager(), new CandidateConstructorSelector(container) );
				//} );


				.Select( type =>
				{
					return new
					{
						Contracts = conventions.SelectMessageHandlerContracts( type ),
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