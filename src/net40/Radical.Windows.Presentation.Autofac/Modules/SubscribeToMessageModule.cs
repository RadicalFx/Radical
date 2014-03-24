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
using Autofac.Core;
using Autofac;
using Topics.Radical.Windows.Presentation.Boot;

namespace Topics.Radical.Windows.Presentation.Modules
{
	public sealed class SubscribeToMessageModule : IModule
	{
		class Bag 
		{ 
			public Type Implementation { get; set; }
			public IEnumerable<Type> Services { get; set; }
		}

		static readonly TraceSource logger = new TraceSource( typeof( SubscribeToMessageModule ).FullName );

		readonly IList<Tuple<Guid, Bag>> buffer = new List<Tuple<Guid, Bag>>();
		readonly BootstrapConventions conventions;

		public SubscribeToMessageModule( Boot.BootstrapConventions conventions )
		{
			// TODO: Complete member initialization
			this.conventions = conventions;
		}

		public void Configure( IComponentRegistry componentRegistry )
		{
			componentRegistry.Registered += OnRegistered;
		}

		void OnRegistered( object sender, ComponentRegisteredEventArgs e )
		{
			var services = e.ComponentRegistration.Services
				.OfType<TypedService>()
				.Select( t => t.ServiceType )
				.ToArray();
			
			var implementation = e.ComponentRegistration.Activator.LimitType;

			var bag = new Bag() 
			{
				Services = services,
				Implementation = implementation
			};

			if ( this.IsInterestingHandler( bag, typeof( IMessageHandler ) ) || this.IsInterestingHandler( bag, typeof( IHandleMessage ) ) )
			{
				logger.Debug
				(
					"Registered component is IMessageHandler/IHandleMessage: {0}/{1}, but no broker yet registered. buffering...",
					services.FirstOrDefault() == null ? "<null>" : services.FirstOrDefault().ToString( "SN" ),
					implementation.ToString( "SN" )
				);

				this.buffer.Add( new Tuple<Guid, Bag>( e.ComponentRegistration.Id, bag ) );
			}
		}

		void Attach( IContainer container, IMessageBroker broker, Guid key, Bag bag )
		{
			var invocationModel = bag.Implementation.Is<INeedSafeSubscription>() 
				? InvocationModel.Safe 
				: InvocationModel.Default;
			
			bag.Implementation.GetAttributes<SubscribeToMessageAttribute>()
				.Return( attributes =>
				{
					return attributes.Select( a => typeof( IMessageHandler<> ).MakeGenericType( a.MessageType ) )
						.Where( gh => this.IsInterestingHandler( bag, gh ) );
				}, () =>
				{
					return bag.Implementation
						.GetInterfaces()
						.Where( i => ( i.Is<IMessageHandler>() || i.Is<IHandleMessage>() ) && i.IsGenericType );
				}, attributes =>
				{
					return !attributes.Any();
				} )
				.ForEach( t => this.Subscribe( container, broker, key, bag, t, invocationModel ) );
		}

		void Subscribe( IContainer container, IMessageBroker broker, Guid key, Bag bag, Type genericHandler, InvocationModel invocationModel )
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
					var handler = container.Resolve( bag.Services.FirstOrDefault() ) as IMessageHandler;
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
				} );
			}
			else if ( genericHandler.Is<IHandleMessage>() )
			{
				broker.Subscribe( this, messageType, invocationModel, ( s, msg ) =>
				{
					var handler = container.Resolve( bag.Services.FirstOrDefault() ) as IHandleMessage;
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
				} );
			}
		}

		Boolean IsInterestingHandler( Bag bag, Type compare )
		{
			return bag.Implementation.Is( compare ) || bag.Services.Any( t => t.Is( compare ) );
		}

		internal void Commit( IContainer container )
		{
			var broker = container.Resolve<IMessageBroker>();
			foreach ( var item in this.buffer )
			{
				this.Attach( container, broker, item.Item1, item.Item2 );
			}
		}
	}
}
