using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.Linq;
using Topics.Radical.Messaging;
using Topics.Radical.Threading;
using Topics.Radical.ComponentModel.Messaging;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Test.Radical.Windows.Messaging
{
    [TestClass]
    public class MessageBrokerTests
    {
        class PocoTestMessage
        {

        }

        [TestMethod]
        [TestCategory( "MessageBroker" )]
        public async Task messageBroker_POCO_broadcast_async_should_not_fail()
        {
            var expected = 4;
            var actual = 0;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker( dispatcher );

            broker.Subscribe<PocoTestMessage>( this, ( s, msg ) =>
            {
                actual++;
            } );

            broker.Subscribe<PocoTestMessage>( this, ( s, msg ) =>
            {
                actual++;
            } );

            broker.Subscribe<PocoTestMessage>( this, ( s, msg ) =>
            {
                actual++;
            } );

            await broker.BroadcastAsync( this, new PocoTestMessage() );
            
            actual++;

            Assert.AreEqual( expected, actual );
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_async_subscription_POCO_broadcast_async_should_not_fail()
        {
            var expected = 4;
            var actual = 0;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<PocoTestMessage>(this, async (s, msg) =>
            {
                await Task.FromResult(false);
                actual++;
            });

            broker.Subscribe<PocoTestMessage>(this, async (s, msg) =>
            {
                await Task.FromResult(false);
                actual++;
            });

            broker.Subscribe<PocoTestMessage>(this, async (s, msg) =>
            {
                await Task.FromResult(false);
                actual++;
            });

            await broker.BroadcastAsync(this, new PocoTestMessage());

            actual++;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_mixed_async_subscription_POCO_broadcast_async_should_not_fail()
        {
            var expected = 4;
            var actual = 0;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<PocoTestMessage>(this, async (s, msg) =>
            {
                await Task.FromResult(false);
                actual++;
            });

            broker.Subscribe<PocoTestMessage>(this, async (s, msg) =>
            {
                await Task.FromResult(false);
                actual++;
            });

            broker.Subscribe<PocoTestMessage>(this, (s, msg) =>
            {
                actual++;
            });

            await broker.BroadcastAsync(this, new PocoTestMessage());

            actual++;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_async_POCO_should_respect_should_handle_predicate()
        {
            bool actual = false;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, msg) => false, async (s, msg) =>
            {
                actual = true;
                await Task.FromResult(0);
            });

            await broker.BroadcastAsync(this, new PocoTestMessage());

            Assert.IsFalse(actual);
        }
    }
}