using System;
using System.Linq;
using System.Collections.Generic;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Reflection;
using Topics.Radical;
using Topics.Radical.Linq;

namespace Radical.Presentation.Samples.Boot
{
	public sealed class SubscribeToMessageFacility : AbstractFacility
	{
		IList<Tuple<String, IHandler>> buffer = new List<Tuple<String, IHandler>>();
		Boolean isMessageBrokerRegistered = false;

		void Attach( String key, IHandler h )
		{
			var broker = this.Kernel.Resolve<IMessageBroker>();

			var invocationModel = h.ComponentModel.Implementation.Is<INeedSafeSubscription>() ? InvocationModel.Safe : InvocationModel.Default;
			h.ComponentModel.Implementation.GetAttributes<SubscribeToMessageAttribute>()
				.Return( attributes =>
				{
					return attributes.Select( a => typeof( IMessageHandler<> ).MakeGenericType( a.MessageType ) )
					.Where( gh => this.IsInterestingHandler( h, gh ) );
				}, () =>
				{
					return h.ComponentModel.Implementation.GetInterfaces()
										.Where( i => i.Is<IMessageHandler>() && i.IsGenericType );
				}, attributes =>
				{
					return !attributes.Any();
				} )
				.ForEach( t => this.Subscribe( broker, key, h, t, invocationModel ) );
		}

		void Subscribe( IMessageBroker broker, String key, IHandler h, Type genericHandler, InvocationModel invocationModel )
		{
			/*
			 * Qui abbiamo un problema di questo tipo: quando in Castle viene
			 * registrato un componente per più di un servizio, ergo per più 
			 * interfacce vogliamo venga risolto lo stesso tipo, abbiamo l'inghippo
			 * che Castle registra n componenti che risolvono verso lo stesso tipo
			 * tante quante sono le interfacce. Quindi l'evento qui registrato viene
			 * scatenato n volte e siccome il primo test che noi facciamo è verificare
			 * che il servizio sia IMessageHandler se un componente gestisce n messaggi
			 * arriviamo qui n volte. Se il componente inoltre richiede la Subscribe
			 * automatica per quei messaggi, ha quindi più di un SubscribeToMessageAttribute
			 * dobbiamo assicurarci che la subscribe venga fatta una ed una sola volta.
			 * per ogni tipo di messaggio.
			 */
			var messageType = genericHandler.GetGenericArguments().Single(); // attribute.MessageType;

			//logger.Verbose
			//(
			//    "\tSubscribing to message: {0}",
			//    messageType.ToString( "SN" )
			//);

			broker.Subscribe( this, messageType, invocationModel, msg =>
			{
				var handler = this.Kernel.Resolve( key, h.ComponentModel.Services.First() ) as IMessageHandler;

				//logger.Verbose
				//(
				//    "Dispatching message {0} to IMessageHandler {1}",
				//    msg.GetType().ToString( "SN" ),
				//    handler.GetType().ToString( "SN" )
				//);

				if( handler.ShouldHandle( msg ) )
				{
					handler.Handle( msg );
				}
			} );
		}

		Boolean IsInterestingHandler( IHandler h, Type t )
		{
			return h.ComponentModel.Services.Any( s => s.Is( t ) ) || h.ComponentModel.Implementation.Is( t );
		}

		/// <summary>
		/// The custom initialization for the Facility.
		/// </summary>
		/// <remarks>It must be overriden.</remarks>
		protected override void Init()
		{
			this.Kernel.ComponentRegistered += ( s, h ) =>
			{
				if( h.ComponentModel.Services.Any( svc => svc.Is<IMessageBroker>() ) )
				{
					//logger.Verbose( "Registered component is IMessageBroker. Ready to empty the buffer." );

					this.isMessageBrokerRegistered = true;

					if( this.buffer.Any() )
					{
						for( var i = buffer.Count; i > 0; i-- )
						{
							var cmp = this.buffer[ i - 1 ];

							this.Attach( cmp.Item1, cmp.Item2 );
							this.buffer.Remove( cmp );
						}

//						logger.Verbose( "All handlers in the buffer attached." );
					}
				}
				else if( this.IsInterestingHandler( h, typeof( IMessageHandler ) ) )
				{
					if( !this.isMessageBrokerRegistered )
					{
						//logger.Verbose
						//(
						//    "Registered component is IMessageHandler: {0}/{1}, but no broker yet registered. buffering...",
						//    h.Service.ToString( "SN" ),
						//    h.ComponentModel.Implementation.ToString( "SN" )
						//);

						this.buffer.Add( new Tuple<String, IHandler>( s, h ) );
					}
					else
					{
						//logger.Verbose
						//(
						//    "Registered component is IMessageHandler: {0}/{1}",
						//    h.Service.ToString( "SN" ),
						//    h.ComponentModel.Implementation.ToString( "SN" )
						//);

						this.Attach( s, h );
					}
				}
			};
		}
	}
}
