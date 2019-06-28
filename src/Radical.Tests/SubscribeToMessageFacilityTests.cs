using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel.Messaging;
using Radical.Messaging;
using Radical.Threading;
using System;

namespace Radical.Tests
{
    [TestClass]
    public class SubscribeToMessageFacilityTests
    {
        class AMessage
        {

        }

        class AMessageHandler : AbstractMessageHandler<AMessage>
        {
            public bool Invoked { get; private set; }

            public override void Handle(Object sender, AMessage message)
            {
                this.Invoked = true;
            }
        }
        
        [TestMethod]
        public void when_registering_POCO_message_handler_facility_should_correctly_subscribe_messages()
        {
            var container = new PuzzleContainer();
            container.AddFacility<SubscribeToMessageFacility>();
            container.Register(EntryBuilder.For<IMessageBroker>().UsingInstance(new MessageBroker(new NullDispatcher())));

            container.Register(EntryBuilder.For<IHandleMessage<AMessage>>().ImplementedBy<AMessageHandler>());

            var broker = container.Resolve<IMessageBroker>();
            var handler = (AMessageHandler)container.Resolve<IHandleMessage<AMessage>>();

            broker.Dispatch(this, new AMessage());

            Assert.IsTrue(handler.Invoked);
        }
    }
}
