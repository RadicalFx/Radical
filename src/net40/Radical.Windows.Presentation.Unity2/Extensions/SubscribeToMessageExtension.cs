using System;
using System.Linq;
using System.Reflection;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Reflection;
using System.Collections.Generic;
using Topics.Radical;
using Topics.Radical.Diagnostics;
using System.Diagnostics;
using Topics.Radical.Linq;
using Microsoft.Practices.Unity;

namespace Topics.Radical.Windows.Presentation.Extensions
{
#pragma warning disable 0618

	public sealed class SubscribeToMessageExtension : UnityContainerExtension
	{
		static readonly TraceSource logger = new TraceSource( typeof( SubscribeToMessageExtension ).FullName );

		IList<Tuple<String, RegisterEventArgs>> buffer = new List<Tuple<String, RegisterEventArgs>>();
		Boolean isMessageBrokerRegistered = false;

		void Attach( String key, RegisterEventArgs h )
		{
			var broker = this.Container.Resolve<IMessageBroker>();

			var invocationModel = h.TypeTo.Is<INeedSafeSubscription>() ? InvocationModel.Safe : InvocationModel.Default;
			h.TypeTo.GetAttributes<SubscribeToMessageAttribute>()
				.Return( attributes =>
				{
					return attributes.Select( a => typeof( IMessageHandler<> ).MakeGenericType( a.MessageType ) )
					.Where( gh => this.IsInterestingHandler( h, gh ) );
				}, () =>
				{
					return h.TypeTo.GetInterfaces()
						.Where( i => ( i.Is<IMessageHandler>() || i.Is<IHandleMessage>() ) && i.IsGenericType );
				}, attributes =>
				{
					return !attributes.Any();
				} )
				.ForEach( t => this.Subscribe( broker, key, h, t, invocationModel ) );
		}

		void Subscribe( IMessageBroker broker, String key, RegisterEventArgs h, Type genericHandler, InvocationModel invocationModel )
		{
			var messageType = genericHandler.GetGenericArguments().Single();

			logger.Debug
			(
				"\tSubscribing to message: {0}",
				messageType.ToString( "SN" )
			);

			if ( genericHandler.Is<IMessageHandler>() )
			{
				broker.Subscribe( this, messageType, invocationModel, msg =>
				{
					if ( this.Container != null )
					{
						var handler = this.Container.Resolve( h.TypeFrom, key ) as IMessageHandler;
						if ( handler != null )
						{
							logger.Debug
							(
								"Dispatching message {0} to IMessageHandler {1}",
									msg.GetType().ToString( "SN" ),
									handler.GetType().ToString( "SN" )
							);

							if ( handler.ShouldHandle( msg ) )
							{
								handler.Handle( msg );
							}
						}
						else
						{
							logger.Debug
							(
								"IMessageHandler for {0} is null.",
								msg.GetType().ToString( "SN" )
							);
						}
					}
					else
					{
						logger.Debug( "Kernel is null." );
						logger.Debug( "Kernel is null." );
					}
				} );
			}
			else if ( genericHandler.Is<IHandleMessage>() )
			{
				broker.Subscribe( this, messageType, invocationModel, ( s, msg ) =>
				{
					if ( this.Container != null )
					{
						var handler = this.Container.Resolve( h.TypeFrom, key ) as IHandleMessage;
						if ( handler != null )
						{
							logger.Debug
							(
								"Dispatching message {0} to IHandleMessage {1}",
									msg.GetType().ToString( "SN" ),
									handler.GetType().ToString( "SN" )
							);

							if ( handler.ShouldHandle( s, msg ) )
							{
								handler.Handle( s, msg );
							}
						}
						else
						{
							logger.Debug
							(
								"IHandleMessage for {0} is null.",
								msg.GetType().ToString( "SN" )
							);
						}
					}
					else
					{
						logger.Debug( "Kernel is null." );
					}
				} );
			}
		}

		Boolean IsInterestingHandler( RegisterEventArgs e, Type t )
		{
			return e.TypeFrom.Is( t ) || e.TypeTo.Is( t );
		}

		protected override void Initialize()
		{
			this.Context.Registering += ( s, e ) =>
			{
				if ( e.TypeFrom.Is<IMessageBroker>() )
				{
					logger.Debug( "Registered component is IMessageBroker. Ready to empty the buffer." );

					this.isMessageBrokerRegistered = true;

					if ( this.buffer.Any() )
					{
						for ( var i = buffer.Count; i > 0; i-- )
						{
							var cmp = this.buffer[ i - 1 ];
							this.Attach( cmp.Item1, cmp.Item2 );
							this.buffer.Remove( cmp );
						}

						logger.Debug( "All handlers in the buffer attached." );
					}
				}
				else if ( this.IsInterestingHandler( e, typeof( IMessageHandler ) ) || this.IsInterestingHandler( e, typeof( IHandleMessage ) ) )
				{
					if ( !this.isMessageBrokerRegistered )
					{
						logger.Debug
						(
							"Registered component is IMessageHandler/IHandleMessage: {0}/{1}, but no broker yet registered. buffering...",
							e.TypeFrom == null ? "<null>" : e.TypeFrom.ToString( "SN" ),
							e.TypeTo.ToString( "SN" )
						);

						this.buffer.Add( new Tuple<String, RegisterEventArgs>( e.Name, e ) );

					}
					else
					{
						logger.Debug
						(
							"Registered component is IMessageHandler/IHandleMessage: {0}/{1}",
							e.TypeFrom == null ? "<null>" : e.TypeFrom.ToString( "SN" ),
							e.TypeTo.ToString( "SN" )
						);

						this.Attach( e.Name, e );
					}
				}
			};
		}
	}

#pragma warning restore 0618

}
