using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel;
using Radical.ComponentModel.Messaging;
using Radical.Messaging;
using Radical.Threading;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Radical.Tests.Windows.Messaging
{
    [TestClass]
    public class MessageBrokerTests
    {
        class PocoTestMessage
        {
        }

        class PocoMessageDerivedFromTestMessage : PocoTestMessage
        {
        }

        class AnotherPocoTestMessage
        {
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task
            messageBroker_POCO_unsubscribe_specific_subscriber_should_remove_only_subscriptions_for_that_subscriber()
        {
            const int expected = 1;
            var actual = 0;

            var dispatcher = new NullDispatcher();
            var target = new MessageBroker(dispatcher);

            var subscriber1 = new object();
            var subscriber2 = new object();

            target.Subscribe<PocoTestMessage>(subscriber1, (s, msg) => { actual++; return Task.CompletedTask; });
            target.Subscribe<PocoTestMessage>(subscriber1, (s, msg) => { actual++; return Task.CompletedTask; });
            target.Subscribe<PocoTestMessage>(subscriber1, (s, msg) => { actual++; return Task.CompletedTask; });

            target.Subscribe<PocoTestMessage>(subscriber2, (s, msg) => { actual++; return Task.CompletedTask; });

            target.Unsubscribe(subscriber1);

            await target.DispatchAsync(this, new PocoTestMessage());

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task
            messageBroker_POCO_unsubscribe_specific_subscriber_and_specific_messageType_should_remove_only_subscriptions_for_that_subscriber()
        {
            const int expected = 1;
            var actual = 0;

            var dispatcher = new NullDispatcher();
            var target = new MessageBroker(dispatcher);

            var subscriber = new object();

            target.Subscribe<PocoTestMessage>(subscriber, (s, msg) => { actual++; return Task.CompletedTask; });
            target.Subscribe<AnotherPocoTestMessage>(subscriber, (s, msg) => { actual++; return Task.CompletedTask; });

            target.Unsubscribe<AnotherPocoTestMessage>(subscriber);

            await target.DispatchAsync(this, new PocoTestMessage());

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_POCO_Dispatch_valid_message_should_not_fail()
        {
            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            await broker.DispatchAsync(this, new PocoTestMessage());
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public void messageBroker_POCO_subscribe_using_null_action_should_raise_ArgumentNullException()
        {
            Executing.This(() =>
                {
                    var dispatcher = new NullDispatcher();
                    var broker = new MessageBroker(dispatcher);

                    broker.Subscribe<PocoTestMessage>(this, (Func<object, PocoTestMessage, Task>)null);
                })
                .Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public void messageBroker_POCO_Subscribe_based_on_message_type_should_not_fail()
        {
            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, m) => Task.CompletedTask);
        }

        [TestMethod]
        public async Task messageBroker_POCO_subscribe_normal_should_notify()
        {
            var expected = true;
            var actual = false;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, msg) => { actual = true; return Task.CompletedTask; });
            await broker.DispatchAsync(this, new PocoTestMessage());

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public void messageBroker_POCO_broadcast_using_more_then_one_subscriber_should_call_on_different_thread()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                Assert.Inconclusive(
                    "Test that depends on setting the ApartmentState to MTA can only be run on Windows");
                return;
            }

            TestRunner.Execute(ApartmentState.MTA, () =>
            {
                var h1 = new ManualResetEvent(false);
                var h2 = new ManualResetEvent(false);

                var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                var s1ThreadId = Thread.CurrentThread.ManagedThreadId;
                var s2ThreadId = Thread.CurrentThread.ManagedThreadId;

                var dispatcher = new NullDispatcher();
                var broker = new MessageBroker(dispatcher);

                broker.Subscribe<PocoTestMessage>(this, (s, msg) =>
                {
                    s1ThreadId = Thread.CurrentThread.ManagedThreadId;
                    h1.Set();
                    return Task.CompletedTask;
                });

                broker.Subscribe<PocoTestMessage>(this, (s, msg) =>
                {
                    s2ThreadId = Thread.CurrentThread.ManagedThreadId;
                    h2.Set();
                    return Task.CompletedTask;
                });

                _ = broker.BroadcastAsync(this, new PocoTestMessage());

                WaitHandle.WaitAll(new[] {h1, h2});

                currentThreadId.Should().Not.Be.EqualTo(s1ThreadId);
                currentThreadId.Should().Not.Be.EqualTo(s2ThreadId);
            });
        }

        class TestRunner
        {
            readonly Action test;
            readonly ApartmentState state;
            Exception ex;

            private TestRunner(ApartmentState state, Action test)
            {
                this.state = state;
                this.test = test;
            }

            public static void Execute(ApartmentState state, Action test)
            {
                var runner = new TestRunner(state, test);
                runner.Execute();
            }

            private void Execute()
            {
                var worker = new Thread(() =>
                {
                    try
                    {
                        test();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        ex = e;
                    }
                });

                worker.SetApartmentState(state);
                worker.Start();
                worker.Join();

                if (ex != null)
                {
                    throw ex;
                }
            }
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task
            MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message()
        {
            var actual = false;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<object>(this, (s, msg) => { actual = true; return Task.CompletedTask; });
            await broker.DispatchAsync(this, new PocoTestMessage());

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task
            MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message_even_using_different_messages()
        {
            var actual = 0;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<object>(this, (s, msg) => { actual++; return Task.CompletedTask; });
            await broker.DispatchAsync(this, new PocoTestMessage());
            await broker.DispatchAsync(this, new AnotherPocoTestMessage());

            actual.Should().Be.EqualTo(2);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task
            MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_only_to_the_expected_inheritance_chain()
        {
            var actual = 0;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, m) => { actual++; return Task.CompletedTask; });
            await broker.DispatchAsync(this, new PocoMessageDerivedFromTestMessage());
            await broker.DispatchAsync(this, new AnotherPocoTestMessage());

            actual.Should().Be.EqualTo(1);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public void MessageBroker_dispatched_should_respect_the_given_priority()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public void MessageBroker_broadcast_from_multiple_thread_should_not_fail()
        {
            Exception failure = null;
            var wh = new ManualResetEvent(false);
            var run = true;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            // do some metrics to find a good number of subscription to let the pub do a long running
            const int metricsCount = 100;
            for (int i = 0; i < metricsCount; i++)
            {
                broker.Subscribe<PocoTestMessage>(this, (sender, msg) => Task.CompletedTask);
            }

            var sw = Stopwatch.StartNew();
            _ = broker.BroadcastAsync(this, new PocoTestMessage());
            sw.Stop();

            const int longRunningTiming = 100;
            var elapsedMilliseconds = sw.ElapsedMilliseconds == 0
                ? longRunningTiming
                : sw.ElapsedMilliseconds;

            var subCountForLongRunning = (longRunningTiming / elapsedMilliseconds) * metricsCount;
            Trace.WriteLine(string.Format("Need {0} subscriptions to run for at least {1} ms. {2} took {3} ms.",
                subCountForLongRunning, longRunningTiming, metricsCount, sw.ElapsedMilliseconds));

            for (int i = 0; i < subCountForLongRunning - metricsCount; i++)
            {
                broker.Subscribe<PocoTestMessage>(this, (sender, msg) => Task.CompletedTask);
            }

            // this will take approximately 100 ms to do 1 broadcast
            var broadcastThread1 = new Thread(payload =>
            {
                while (run)
                {
                    try
                    {
                        _ = broker.BroadcastAsync(this, new PocoTestMessage());
                    }
                    catch (Exception e)
                    {
                        lock (this)
                        {
                            failure = e;
                            wh.Set();
                        }

                        break;
                    }
                }
            });
            broadcastThread1.IsBackground = true;
            broadcastThread1.Start();

            // this should istantly throw an error because the broadcasting is enumerating the subscriptions and should take 100 ms to enumerate them all
            var subscriberThread1 = new Thread(payload =>
            {
                while (run)
                {
                    try
                    {
                        broker.Subscribe<PocoTestMessage>(this, (sender, msg) =>
                        {
                            Thread.Sleep(10);
                            return Task.CompletedTask;
                        });
                    }
                    catch (Exception e)
                    {
                        lock (this)
                        {
                            failure = e;
                            wh.Set();

                            break;
                        }
                    }
                }
            });
            subscriberThread1.IsBackground = true;
            subscriberThread1.Start();

            var timeout = 1;
            var signaled = wh.WaitOne(TimeSpan.FromSeconds(timeout));
            if (!signaled)
            {
                Trace.WriteLine(string.Format("Run without any issue for {0} seconds.", timeout));
            }

            run = false;

            subscriberThread1.Join();

            Assert.IsNull(failure, failure != null ? failure.ToString() : "--");
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_POCO_should_respect_should_handle_predicate()
        {
            bool actual = false;

            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, msg) => false, (s, msg) =>
            {
                actual = true;
                return Task.CompletedTask;
            });

            await broker.DispatchAsync(this, new PocoTestMessage());

            Assert.IsFalse(actual);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        //BUG: https://github.com/RadicalFx/Radical/issues/241
        public void messageBroker_should_allow_POCO_subscriptions_and_not_IMessage_ones()
        {
            var dispatcher = new NullDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe(this, this, typeof(PocoTestMessage), InvocationModel.Default, (s, msg) => false,
                (s, msg) =>
                {
                    /* NOP */
                    return Task.CompletedTask;
                });
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_DispatchAsync_should_await_each_subscriber_before_invoking_the_next_one()
        {
            var invocations = new List<string>();

            var broker = new MessageBroker(new NullDispatcher());

            broker.Subscribe<PocoTestMessage>(this, async (s, msg) =>
            {
                invocations.Add("first started");
                await Task.Delay(10);
                invocations.Add("first completed");
            });
            broker.Subscribe<PocoTestMessage>(this, (s, msg) =>
            {
                invocations.Add("second");
                return Task.CompletedTask;
            });

            await broker.DispatchAsync(this, new PocoTestMessage());

            invocations.Should().Have.SameSequenceAs("first started", "first completed", "second");
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_DispatchAsync_should_propagate_subscriber_exceptions()
        {
            var broker = new MessageBroker(new NullDispatcher());

            broker.Subscribe<PocoTestMessage>(this, async (s, msg) =>
            {
                await Task.Yield();
                throw new InvalidOperationException("subscriber failure");
            });

            var exception = await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => broker.DispatchAsync(this, new PocoTestMessage()));

            exception.Message.Should().Be.EqualTo("subscriber failure");
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_BroadcastAsync_should_complete_when_all_subscribers_completed()
        {
            var completed = 0;

            var broker = new MessageBroker(new NullDispatcher());

            for (var i = 0; i < 3; i++)
            {
                broker.Subscribe<PocoTestMessage>(this, async (s, msg) =>
                {
                    await Task.Delay(10);
                    Interlocked.Increment(ref completed);
                });
            }

            await broker.BroadcastAsync(this, new PocoTestMessage());

            completed.Should().Be.EqualTo(3);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_BroadcastAsync_should_propagate_subscriber_exceptions()
        {
            var broker = new MessageBroker(new NullDispatcher());

            Func<object, PocoTestMessage, Task> failingCallback = (s, msg) => throw new InvalidOperationException("subscriber failure");
            broker.Subscribe(this, failingCallback);

            await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => broker.BroadcastAsync(this, new PocoTestMessage()));
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_BroadcastAsync_should_marshal_safe_subscribers_through_the_dispatcher()
        {
            var dispatcher = new RecordingDispatcher();
            var broker = new MessageBroker(dispatcher);

            broker.Subscribe<PocoTestMessage>(this, InvocationModel.Safe, (s, msg) => Task.CompletedTask);
            broker.Subscribe<PocoTestMessage>(this, InvocationModel.Default, (s, msg) => Task.CompletedTask);

            await broker.BroadcastAsync(this, new PocoTestMessage());

            dispatcher.DispatchCount.Should().Be.EqualTo(1);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_unsubscribe_specific_callback_should_remove_only_that_subscription()
        {
            var actual = 0;

            var broker = new MessageBroker(new NullDispatcher());

            Func<object, PocoTestMessage, Task> callback = (s, msg) => { actual += 10; return Task.CompletedTask; };
            broker.Subscribe(this, callback);
            broker.Subscribe<PocoTestMessage>(this, (s, msg) => { actual++; return Task.CompletedTask; });

            broker.Unsubscribe<PocoTestMessage>(this, callback);

            await broker.DispatchAsync(this, new PocoTestMessage());

            actual.Should().Be.EqualTo(1);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_subscribe_by_message_type_should_notify()
        {
            object received = null;

            var broker = new MessageBroker(new NullDispatcher());

            broker.Subscribe(this, typeof(PocoTestMessage), (s, msg) => { received = msg; return Task.CompletedTask; });

            var message = new PocoTestMessage();
            await broker.DispatchAsync(this, message);

            received.Should().Be.SameInstanceAs(message);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        [DataRow("Dispatch", new[] { typeof(object), typeof(object) })]
        [DataRow("Broadcast", new[] { typeof(object), typeof(object) })]
        public void messageBroker_obsolete_synchronous_publishing_should_throw_NotSupportedException(string methodName, Type[] parameterTypes)
        {
            var broker = new MessageBroker(new NullDispatcher());
            var method = typeof(MessageBroker).GetMethod(methodName, parameterTypes);

            var exception = Assert.ThrowsExactly<TargetInvocationException>(() => method.Invoke(broker, [this, new PocoTestMessage()]));

            exception.InnerException.Should().Be.InstanceOf<NotSupportedException>();
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public void messageBroker_obsolete_synchronous_subscribe_should_throw_NotSupportedException()
        {
            var broker = new MessageBroker(new NullDispatcher());
            var method = typeof(MessageBroker)
                .GetMethods()
                .Single(m => m.Name == "Subscribe" && m.IsGenericMethod && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType.IsGenericType && m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Action<,>))
                .MakeGenericMethod(typeof(PocoTestMessage));

            var exception = Assert.ThrowsExactly<TargetInvocationException>(() => method.Invoke(broker, [this, new Action<object, PocoTestMessage>((s, msg) => { })]));

            exception.InnerException.Should().Be.InstanceOf<NotSupportedException>();
        }

        class RecordingDispatcher : IDispatcher
        {
            public int DispatchCount { get; private set; }

            public bool IsSafe => false;

            public TResult Dispatch<TResult>(Func<TResult> func)
            {
                DispatchCount++;
                return func();
            }

            public void Invoke(Delegate d, params object[] args) => throw new NotSupportedException();

            public void Dispatch(Action action) => throw new NotSupportedException();

            public void Dispatch<T>(T arg, Action<T> action) => throw new NotSupportedException();

            public void Dispatch<T1, T2>(T1 arg1, T2 arg2, Action<T1, T2> action) => throw new NotSupportedException();
        }
    }
}
