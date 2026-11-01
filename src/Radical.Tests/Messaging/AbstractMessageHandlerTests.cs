using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel.Messaging;
using Radical.Messaging;
using SharpTestsEx;
using System;
using System.Threading.Tasks;

namespace Radical.Tests.Messaging
{
    [TestClass]
    public class AbstractMessageHandlerTests
    {
        class TestMessage
        {
        }

        class TestHandler : AbstractMessageHandler<TestMessage>
        {
            public TestMessage Handled { get; private set; }

            public override async Task HandleAsync(object sender, TestMessage message)
            {
                await Task.Yield();
                Handled = message;
            }
        }

        [TestMethod]
        public async Task AbstractMessageHandler_untyped_HandleAsync_should_await_the_typed_one()
        {
            var handler = new TestHandler();
            var message = new TestMessage();

            await ((IHandleMessage)handler).HandleAsync(this, message);

            handler.Handled.Should().Be.SameInstanceAs(message);
        }

        [TestMethod]
        public void AbstractMessageHandler_obsolete_Handle_should_throw_NotSupportedException()
        {
            var handler = new TestHandler();
            var method = typeof(AbstractMessageHandler<TestMessage>).GetMethod("Handle", [typeof(object), typeof(TestMessage)]);

            var exception = Assert.ThrowsExactly<System.Reflection.TargetInvocationException>(() => method.Invoke(handler, [this, new TestMessage()]));

            exception.InnerException.Should().Be.InstanceOf<NotSupportedException>();
        }
    }
}
