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
        class SampleMessage : IMessage
        {
            public object Sender
            {
                get;
                set;
            }
        }

        class AnotherSampleMessage : IMessage
        {
            public object Sender
            {
                get;
                set;
            }
        }

        [SubscribeToMessage( typeof( SampleMessage ) )]
        class SampleMessageHandlerWithAttribute : MessageHandler<SampleMessage>
        {
            public Boolean Invoked { get; private set; }

            public override void Handle( SampleMessage message )
            {
                this.Invoked = true;
            }
        }

        [SubscribeToMessage( typeof( AnotherSampleMessage ) )]
        [SubscribeToMessage( typeof( SampleMessage ) )]
        class SampleMessageHandlerWithAttributeThatWantsToHandleTwoMessages :
            IMessageHandler<SampleMessage>,
            IMessageHandler<AnotherSampleMessage>
        {
            public Boolean SampleMessageInvoked { get; private set; }

            public Boolean AnotherSampleMessageInvoked { get; private set; }

            public void Handle( IMessage message )
            {
                if ( message is SampleMessage )
                {
                    this.Handle( ( SampleMessage )message );
                }
                else if ( message is AnotherSampleMessage )
                {
                    this.Handle( ( AnotherSampleMessage )message );
                }
            }

            public void Handle( SampleMessage message )
            {
                this.SampleMessageInvoked = true;
            }

            public void Handle( AnotherSampleMessage message )
            {
                this.AnotherSampleMessageInvoked = true;
            }

            public bool ShouldHandle( IMessage message )
            {
                return true;
            }
        }

        class SampleMessageHandlerWithoutAttribute : MessageHandler<SampleMessage>
        {
            public Boolean Invoked { get; private set; }

            public override void Handle( SampleMessage message )
            {
                this.Invoked = true;
            }
        }

        class SampleMessageHandlerWithoutAttributeThatWantsToHandleTwoMessages :
            IMessageHandler<SampleMessage>,
            IMessageHandler<AnotherSampleMessage>
        {
            public Boolean SampleMessageInvoked { get; private set; }

            public Boolean AnotherSampleMessageInvoked { get; private set; }

            public void Handle( IMessage message )
            {
                if ( message is SampleMessage )
                {
                    this.Handle( ( SampleMessage )message );
                }
                else if ( message is AnotherSampleMessage )
                {
                    this.Handle( ( AnotherSampleMessage )message );
                }
            }

            public void Handle( SampleMessage message )
            {
                this.SampleMessageInvoked = true;
            }

            public void Handle( AnotherSampleMessage message )
            {
                this.AnotherSampleMessageInvoked = true;
            }

            public bool ShouldHandle( IMessage message )
            {
                return true;
            }
        }


        [TestMethod]
        [TestCategory( "SubscribeToMessageFacility" )]
        [TestCategory( "MessageBroker" )]
        public void SubscribeToMessageFacility_should_subscribe_handlers_using_SubscribeToMessageAttribute()
        {
            var handler = new SampleMessageHandlerWithAttribute();

            var container = new WindsorContainer();
            container.AddFacility<SubscribeToMessageFacility>();

            container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
            container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
            container.Register( Component.For<SampleMessageHandlerWithAttribute>().Instance( handler ) );

            var broker = container.Resolve<IMessageBroker>();
            broker.Dispatch( new SampleMessage() { Sender = this } );

            Assert.IsTrue( handler.Invoked );
        }

        [TestMethod]
        [TestCategory( "SubscribeToMessageFacility" )]
        [TestCategory( "MessageBroker" )]
        public void SubscribeToMessageFacility_should_subscribe_handlers_using_SubscribeToMessageAttributealso_with_multiple_messages()
        {
            var handler = new SampleMessageHandlerWithAttributeThatWantsToHandleTwoMessages();

            var container = new WindsorContainer();
            container.AddFacility<SubscribeToMessageFacility>();

            container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
            container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
            container.Register( Component.For<SampleMessageHandlerWithAttributeThatWantsToHandleTwoMessages>().Instance( handler ) );

            var broker = container.Resolve<IMessageBroker>();
            broker.Dispatch( new SampleMessage() { Sender = this } );
            broker.Dispatch( new AnotherSampleMessage() { Sender = this } );

            Assert.IsTrue( handler.SampleMessageInvoked );
            Assert.IsTrue( handler.AnotherSampleMessageInvoked );
        }

        [TestMethod]
        [TestCategory( "SubscribeToMessageFacility" )]
        [TestCategory( "MessageBroker" )]
        public void SubscribeToMessageFacility_should_subscribe_handlers_even_without_SubscribeToMessageAttribute()
        {
            var handler = new SampleMessageHandlerWithoutAttribute();

            var container = new WindsorContainer();
            container.AddFacility<SubscribeToMessageFacility>();

            container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
            container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
            container.Register( Component.For<SampleMessageHandlerWithoutAttribute>().Instance( handler ) );

            var broker = container.Resolve<IMessageBroker>();
            broker.Dispatch( new SampleMessage() { Sender = this } );

            Assert.IsTrue( handler.Invoked );
        }

        [TestMethod]
        [TestCategory( "SubscribeToMessageFacility" )]
        [TestCategory( "MessageBroker" )]
        public void SubscribeToMessageFacility_should_subscribe_handlers_without_SubscribeToMessageAttribute_also_with_multiple_messages()
        {
            var handler = new SampleMessageHandlerWithoutAttributeThatWantsToHandleTwoMessages();

            var container = new WindsorContainer();
            container.AddFacility<SubscribeToMessageFacility>();

            container.Register( Component.For<IDispatcher>().ImplementedBy<NullDispatcher>() );
            container.Register( Component.For<IMessageBroker>().ImplementedBy<MessageBroker>() );
            container.Register( Component.For<SampleMessageHandlerWithoutAttributeThatWantsToHandleTwoMessages>().Instance( handler ) );

            var broker = container.Resolve<IMessageBroker>();
            broker.Dispatch( new SampleMessage() { Sender = this } );
            broker.Dispatch( new AnotherSampleMessage() { Sender = this } );

            Assert.IsTrue( handler.SampleMessageInvoked );
            Assert.IsTrue( handler.AnotherSampleMessageInvoked );
        }
    }
}
