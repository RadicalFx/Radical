using Castle.Facilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Messaging;
using Topics.Radical.ComponentModel;
using Topics.Radical.Threading;

namespace Test.Radical.Extensions.Castle
{
	[TestClass()]
	public class SubscribeToMessageFacilityTest
	{
		class LegacySampleMessage : IMessage
		{
			public object Sender
			{
				get;
				set;
			}

			public void SetSenderForBackwardCompatibility( object sender )
			{
				this.Sender = sender;
			}
		}

		class LegacyAnotherSampleMessage : IMessage
		{
			public object Sender
			{
				get;
				set;
			}

			public void SetSenderForBackwardCompatibility( object sender )
			{
				this.Sender = sender;
			}
		}

		class PocoSampleMessage
		{
			
		}

		[SubscribeToMessage( typeof( LegacySampleMessage ) )]
		class LegacySampleMessageHandlerWithAttribute : MessageHandler<LegacySampleMessage>
		{
			public Boolean Invoked { get; private set; }

			public override void Handle( LegacySampleMessage message )
			{
				this.Invoked = true;
			}
		}

		[SubscribeToMessage( typeof( LegacyAnotherSampleMessage ) )]
		[SubscribeToMessage( typeof( LegacySampleMessage ) )]
		class LegacySampleMessageHandlerWithAttributeThatWantsToHandleTwoMessages :
			IMessageHandler<LegacySampleMessage>,
			IMessageHandler<LegacyAnotherSampleMessage>
		{
			public Boolean SampleMessageInvoked { get; private set; }

			public Boolean AnotherSampleMessageInvoked { get; private set; }

			public void Handle( IMessage message )
			{
				if ( message is LegacySampleMessage )
				{
					this.Handle( ( LegacySampleMessage )message );
				}
				else if ( message is LegacyAnotherSampleMessage )
				{
					this.Handle( ( LegacyAnotherSampleMessage )message );
				}
			}

			public void Handle( LegacySampleMessage message )
			{
				this.SampleMessageInvoked = true;
			}

			public void Handle( LegacyAnotherSampleMessage message )
			{
				this.AnotherSampleMessageInvoked = true;
			}

			public bool ShouldHandle( IMessage message )
			{
				return true;
			}
		}

		class LegacySampleMessageHandlerWithoutAttribute : MessageHandler<LegacySampleMessage>
		{
			public Boolean Invoked { get; private set; }

			public override void Handle( LegacySampleMessage message )
			{
				this.Invoked = true;
			}
		}

		class LegacySampleMessageHandlerWithoutAttributeThatWantsToHandleTwoMessages :
			IMessageHandler<LegacySampleMessage>,
			IMessageHandler<LegacyAnotherSampleMessage>
		{
			public Boolean SampleMessageInvoked { get; private set; }

			public Boolean AnotherSampleMessageInvoked { get; private set; }

			public void Handle( IMessage message )
			{
				if ( message is LegacySampleMessage )
				{
					this.Handle( ( LegacySampleMessage )message );
				}
				else if ( message is LegacyAnotherSampleMessage )
				{
					this.Handle( ( LegacyAnotherSampleMessage )message );
				}
			}

			public void Handle( LegacySampleMessage message )
			{
				this.SampleMessageInvoked = true;
			}

			public void Handle( LegacyAnotherSampleMessage message )
			{
				this.AnotherSampleMessageInvoked = true;
			}

			public bool ShouldHandle( IMessage message )
			{
				return true;
			}
		}

		class Poco_SampleMessageHandlerWithoutAttributeAndLegacyMessage : AbstractMessageHandler<LegacySampleMessage>
		{
			public Boolean Invoked { get; private set; }
			public Object Sender { get; private set; }

			public override void Handle( Object sender, LegacySampleMessage message )
			{
				this.Invoked = true;
				this.Sender = sender;
			}
		}

		class Poco_SampleMessageHandlerWithoutAttributeThatWantsToHandleTwoLegacyMessages :
			IHandleMessage<LegacySampleMessage>,
			IHandleMessage<LegacyAnotherSampleMessage>
		{
			public Boolean SampleMessageInvoked { get; private set; }

			public Boolean AnotherSampleMessageInvoked { get; private set; }

			public Object SampleMessageSender { get; private set; }

			public Object AnotherSampleMessageSender { get; private set; }

			public void Handle( Object sender, Object message )
			{
				if ( message is LegacySampleMessage )
				{
					this.Handle( sender, ( LegacySampleMessage )message );
				}
				else if ( message is LegacyAnotherSampleMessage )
				{
					this.Handle( sender, ( LegacyAnotherSampleMessage )message );
				}
			}

			public void Handle( Object sender, LegacySampleMessage message )
			{
				this.SampleMessageSender = sender;
				this.SampleMessageInvoked = true;
			}

			public void Handle( Object sender, LegacyAnotherSampleMessage message )
			{
				this.AnotherSampleMessageSender = sender;
				this.AnotherSampleMessageInvoked = true;
			}

			public bool ShouldHandle( Object sender, Object message )
			{
				return true;
			}
		}

		class Poco_SampleMessageHandlerWithoutAttributeThatWantsToHandleALegacyMessageAndAPocoMessage :
			IHandleMessage<LegacySampleMessage>,
			IHandleMessage<PocoSampleMessage>
		{
			public Boolean SampleMessageInvoked { get; private set; }

			public Boolean PocoSampleMessageInvoked { get; private set; }

			public Object SampleMessageSender { get; private set; }

			public Object PocoSampleMessageSender { get; private set; }

			public void Handle( Object sender, Object message )
			{
				if ( message is LegacySampleMessage )
				{
					this.Handle( sender, ( LegacySampleMessage )message );
				}
				else if ( message is PocoSampleMessage )
				{
					this.Handle( sender, ( PocoSampleMessage )message );
				}
			}

			public void Handle( Object sender, LegacySampleMessage message )
			{
				this.SampleMessageSender = sender;
				this.SampleMessageInvoked = true;
			}

			public void Handle( Object sender, PocoSampleMessage message )
			{
				this.PocoSampleMessageSender = sender;
				this.PocoSampleMessageInvoked = true;
			}

			public bool ShouldHandle( Object sender, Object message )
			{
				return true;
			}
		}

		[TestMethod]
		[TestCategory( "SubscribeToMessageFacility" )]
		[TestCategory( "MessageBroker" )]
		public void SubscribeToMessageFacility_should_subscribe_handlers_using_SubscribeToMessageAttribute()
		{
			var handler = new LegacySampleMessageHandlerWithAttribute();

			var container = new WindsorContainer();
			container.AddFacility<SubscribeToMessageFacility>();

			container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
			container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
			container.Register( Component.For<LegacySampleMessageHandlerWithAttribute>().Instance( handler ) );

			var broker = container.Resolve<IMessageBroker>();
			broker.Dispatch( new LegacySampleMessage() { Sender = this } );

			Assert.IsTrue( handler.Invoked );
		}

		[TestMethod]
		[TestCategory( "SubscribeToMessageFacility" )]
		[TestCategory( "MessageBroker" )]
		public void SubscribeToMessageFacility_should_subscribe_handlers_using_SubscribeToMessageAttributealso_with_multiple_messages()
		{
			var handler = new LegacySampleMessageHandlerWithAttributeThatWantsToHandleTwoMessages();

			var container = new WindsorContainer();
			container.AddFacility<SubscribeToMessageFacility>();

			container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
			container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
			container.Register( Component.For<LegacySampleMessageHandlerWithAttributeThatWantsToHandleTwoMessages>().Instance( handler ) );

			var broker = container.Resolve<IMessageBroker>();
			broker.Dispatch( new LegacySampleMessage() { Sender = this } );
			broker.Dispatch( new LegacyAnotherSampleMessage() { Sender = this } );

			Assert.IsTrue( handler.SampleMessageInvoked );
			Assert.IsTrue( handler.AnotherSampleMessageInvoked );
		}

		[TestMethod]
		[TestCategory( "SubscribeToMessageFacility" )]
		[TestCategory( "MessageBroker" )]
		public void SubscribeToMessageFacility_should_subscribe_handlers_even_without_SubscribeToMessageAttribute()
		{
			var handler = new LegacySampleMessageHandlerWithoutAttribute();

			var container = new WindsorContainer();
			container.AddFacility<SubscribeToMessageFacility>();

			container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
			container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
			container.Register( Component.For<LegacySampleMessageHandlerWithoutAttribute>().Instance( handler ) );

			var broker = container.Resolve<IMessageBroker>();
			broker.Dispatch( new LegacySampleMessage() { Sender = this } );

			Assert.IsTrue( handler.Invoked );
		}

		[TestMethod]
		[TestCategory( "SubscribeToMessageFacility" )]
		[TestCategory( "MessageBroker" )]
		public void SubscribeToMessageFacility_should_subscribe_handlers_without_SubscribeToMessageAttribute_also_with_multiple_messages()
		{
			var handler = new LegacySampleMessageHandlerWithoutAttributeThatWantsToHandleTwoMessages();

			var container = new WindsorContainer();
			container.AddFacility<SubscribeToMessageFacility>();

			container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
			container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
			container.Register( Component.For<LegacySampleMessageHandlerWithoutAttributeThatWantsToHandleTwoMessages>().Instance( handler ) );

			var broker = container.Resolve<IMessageBroker>();
			broker.Dispatch( new LegacySampleMessage() { Sender = this } );
			broker.Dispatch( new LegacyAnotherSampleMessage() { Sender = this } );

			Assert.IsTrue( handler.SampleMessageInvoked );
			Assert.IsTrue( handler.AnotherSampleMessageInvoked );
		}

		[TestMethod]
		[TestCategory( "SubscribeToMessageFacility" )]
		[TestCategory( "MessageBroker" )]
		public void SubscribeToMessageFacility_should_subscribe_POCO_handlers_using_without_attribute_and_LEGACY_message()
		{
			var handler = new Poco_SampleMessageHandlerWithoutAttributeAndLegacyMessage();

			var container = new WindsorContainer();
			container.AddFacility<SubscribeToMessageFacility>();

			container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
			container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
			container.Register( Component.For<Poco_SampleMessageHandlerWithoutAttributeAndLegacyMessage>().Instance( handler ) );

			var broker = container.Resolve<IMessageBroker>();
			broker.Dispatch( this, new LegacySampleMessage() );

			Assert.IsTrue( handler.Invoked );
			Assert.AreEqual( handler.Sender, this );
		}

		[TestMethod]
		[TestCategory( "SubscribeToMessageFacility" )]
		[TestCategory( "MessageBroker" )]
		public void SubscribeToMessageFacility_should_subscribe_POCO_handlers_without_attribute_with_multiple_LEGACY_messages()
		{
			var handler = new Poco_SampleMessageHandlerWithoutAttributeThatWantsToHandleTwoLegacyMessages();

			var container = new WindsorContainer();
			container.AddFacility<SubscribeToMessageFacility>();

			container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
			container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
			container.Register( Component.For<Poco_SampleMessageHandlerWithoutAttributeThatWantsToHandleTwoLegacyMessages>().Instance( handler ) );

			var broker = container.Resolve<IMessageBroker>();
			broker.Dispatch(this, new LegacySampleMessage() );
			broker.Dispatch( this, new LegacyAnotherSampleMessage() );

			Assert.IsTrue( handler.SampleMessageInvoked );
			Assert.AreEqual( handler.SampleMessageSender, this );
			Assert.IsTrue( handler.AnotherSampleMessageInvoked );
			Assert.AreEqual( handler.AnotherSampleMessageSender, this );
		}


		[TestMethod]
		[TestCategory( "SubscribeToMessageFacility" )]
		[TestCategory( "MessageBroker" )]
		public void SubscribeToMessageFacility_should_subscribe_POCO_handlers_without_attribute_with_mixed_LEGACY_and_POCO_messages()
		{
			var handler = new Poco_SampleMessageHandlerWithoutAttributeThatWantsToHandleALegacyMessageAndAPocoMessage();

			var container = new WindsorContainer();
			container.AddFacility<SubscribeToMessageFacility>();

			container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
			container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
			container.Register( Component.For<Poco_SampleMessageHandlerWithoutAttributeThatWantsToHandleALegacyMessageAndAPocoMessage>().Instance( handler ) );

			var broker = container.Resolve<IMessageBroker>();
			broker.Dispatch( this, new LegacySampleMessage() );
			broker.Dispatch( this, new PocoSampleMessage() );

			Assert.IsTrue( handler.SampleMessageInvoked );
			Assert.AreEqual( handler.SampleMessageSender, this );
			Assert.IsTrue( handler.PocoSampleMessageInvoked );
			Assert.AreEqual( handler.PocoSampleMessageSender, this );
		}
	}
}
