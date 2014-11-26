using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical.Messaging;
using Topics.Radical.Threading;
using Topics.Radical.ComponentModel.Messaging;
using System.Threading.Tasks;

namespace Test.Radical.Windows.Messaging
{
	[TestClass]
	public class MessageBrokerTests
	{
		class LegacyTestMessage : Message
		{
			public LegacyTestMessage() { }

			public LegacyTestMessage( Object sender )
				: base( sender )
			{

			}
		}

		class LegacyMessageDerivedFromLegacyTestMessage : LegacyTestMessage
		{
			public LegacyMessageDerivedFromLegacyTestMessage( Object sender )
				: base( sender )
			{

			}
		}

		class LegacyAnotherTestMessage : Message
		{
			public LegacyAnotherTestMessage( Object sender )
				: base( sender )
			{

			}
		}

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
		[TestCategory( "MessageBroker" )]
		public void messageBroker_unsubscribe_specific_subscriber_should_remove_only_subscriptions_for_that_subscriber()
		{
			const int expected = 1;
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var target = new MessageBroker( dispatcher );

			var subscriber1 = new Object();
			var subscriber2 = new Object();

			target.Subscribe<LegacyTestMessage>( subscriber1, msg => { actual++; } );
			target.Subscribe<LegacyTestMessage>( subscriber1, msg => { actual++; } );
			target.Subscribe<LegacyTestMessage>( subscriber1, msg => { actual++; } );

			target.Subscribe<LegacyTestMessage>( subscriber2, msg => { actual++; } );

			target.Unsubscribe( subscriber1 );

			target.Dispatch( new LegacyTestMessage( this ) );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_unsubscribe_specific_subscriber_should_remove_only_subscriptions_for_that_subscriber()
		{
			const int expected = 1;
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var target = new MessageBroker( dispatcher );

			var subscriber1 = new Object();
			var subscriber2 = new Object();

			target.Subscribe<PocoTestMessage>( subscriber1, ( s, msg ) => { actual++; } );
			target.Subscribe<PocoTestMessage>( subscriber1, ( s, msg ) => { actual++; } );
			target.Subscribe<PocoTestMessage>( subscriber1, ( s, msg ) => { actual++; } );

			target.Subscribe<PocoTestMessage>( subscriber2, ( s, msg ) => { actual++; } );

			target.Unsubscribe( subscriber1 );

			target.Dispatch( this, new PocoTestMessage() );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_unsubscribe_specific_subscriber_and_specific_messageType_should_remove_only_subscriptions_for_that_subscriber()
		{
			const int expected = 1;
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var target = new MessageBroker( dispatcher );

			var subscriber = new Object();

			target.Subscribe<LegacyTestMessage>( subscriber, msg => { actual++; } );
			target.Subscribe<LegacyAnotherTestMessage>( subscriber, msg => { actual++; } );

			target.Unsubscribe<LegacyAnotherTestMessage>( subscriber );

			target.Dispatch( new LegacyTestMessage( this ) );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_unsubscribe_specific_subscriber_and_specific_messageType_should_remove_only_subscriptions_for_that_subscriber()
		{
			const int expected = 1;
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var target = new MessageBroker( dispatcher );

			var subscriber = new Object();

			target.Subscribe<PocoTestMessage>( subscriber, ( s, msg ) => { actual++; } );
			target.Subscribe<AnotherPocoTestMessage>( subscriber, ( s, msg ) => { actual++; } );

			target.Unsubscribe<AnotherPocoTestMessage>( subscriber );

			target.Dispatch( this, new PocoTestMessage() );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_Dispatch_valid_message_should_not_fail()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Dispatch( new LegacyTestMessage( this ) );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_Dispatch_valid_message_should_not_fail()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Dispatch( this, new PocoTestMessage() );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_subscribe_using_null_action_should_raise_ArgumentNullException()
		{
			Executing.This( () =>
			{
				var dispatcher = new NullDispatcher();
				var broker = new MessageBroker( dispatcher );

				broker.Subscribe<LegacyTestMessage>( this, ( Action<LegacyTestMessage> )null );
			} )
			.Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_subscribe_using_null_action_should_raise_ArgumentNullException()
		{
			Executing.This( () =>
			{
				var dispatcher = new NullDispatcher();
				var broker = new MessageBroker( dispatcher );

				broker.Subscribe<PocoTestMessage>( this, ( Action<Object, PocoTestMessage> )null );
			} )
			.Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_Subscribe_based_on_message_type_should_not_fail()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, m => { } );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_Subscribe_based_on_message_type_should_not_fail()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<PocoTestMessage>( this, ( s, m ) => { } );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_subscribe_normal_should_notify()
		{
			var expected = true;
			var actual = false;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, msg => actual = true );
			broker.Dispatch( new LegacyTestMessage( this ) );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void messageBroker_POCO_subscribe_normal_should_notify()
		{
			var expected = true;
			var actual = false;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<PocoTestMessage>( this, ( s, msg ) => actual = true );
			broker.Dispatch( this, new PocoTestMessage() );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_MIXED_subscribe_POCO_normal_should_notify_IMessage()
		{
			var expected = true;
			var actual = false;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<Object>( this, ( s, msg ) => actual = true );
			broker.Dispatch( new LegacyTestMessage( this ) );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_MIXED_subscribe_IMessage_normal_should_not_notify_POCO()
		{
			var expected = false;
			var actual = false;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<IMessage>( this, ( msg ) => actual = true );
			broker.Dispatch( this, new PocoTestMessage() );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_broadcast_using_more_then_one_subscriber_should_call_on_different_thread()
		{
			TestRunner.Execute( ApartmentState.MTA, () =>
			{
				var h1 = new ManualResetEvent( false );
				var h2 = new ManualResetEvent( false );

				var currentThreadId = Thread.CurrentThread.ManagedThreadId;
				var s1ThreadId = Thread.CurrentThread.ManagedThreadId;
				var s2ThreadId = Thread.CurrentThread.ManagedThreadId;

				var dispatcher = new NullDispatcher();
				var broker = new MessageBroker( dispatcher );

				broker.Subscribe<LegacyTestMessage>( this, msg =>
				{
					s1ThreadId = Thread.CurrentThread.ManagedThreadId;
					h1.Set();
				} );

				broker.Subscribe<LegacyTestMessage>( this, msg =>
				{
					s2ThreadId = Thread.CurrentThread.ManagedThreadId;
					h2.Set();
				} );

				broker.Broadcast( new LegacyTestMessage( this ) );

				ManualResetEvent.WaitAll( new[] { h1, h2 } );

				currentThreadId.Should().Not.Be.EqualTo( s1ThreadId );
				currentThreadId.Should().Not.Be.EqualTo( s2ThreadId );
			} );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_broadcast_using_more_then_one_subscriber_should_call_on_different_thread()
		{
			TestRunner.Execute( ApartmentState.MTA, () =>
			{
				var h1 = new ManualResetEvent( false );
				var h2 = new ManualResetEvent( false );

				var currentThreadId = Thread.CurrentThread.ManagedThreadId;
				var s1ThreadId = Thread.CurrentThread.ManagedThreadId;
				var s2ThreadId = Thread.CurrentThread.ManagedThreadId;

				var dispatcher = new NullDispatcher();
				var broker = new MessageBroker( dispatcher );

				broker.Subscribe<PocoTestMessage>( this, ( s, msg ) =>
				{
					s1ThreadId = Thread.CurrentThread.ManagedThreadId;
					h1.Set();
				} );

				broker.Subscribe<PocoTestMessage>( this, ( s, msg ) =>
				{
					s2ThreadId = Thread.CurrentThread.ManagedThreadId;
					h2.Set();
				} );

				broker.Broadcast( this, new PocoTestMessage() );

				ManualResetEvent.WaitAll( new[] { h1, h2 } );

				currentThreadId.Should().Not.Be.EqualTo( s1ThreadId );
				currentThreadId.Should().Not.Be.EqualTo( s2ThreadId );
			} );
		}

		class TestRunner
		{
			readonly Action test;
			readonly ApartmentState state;
			Exception ex;

			private TestRunner( ApartmentState state, Action test )
			{
				this.state = state;
				this.test = test;
			}

			public static void Execute( ApartmentState state, Action test )
			{
				var runner = new TestRunner( state, test );
				runner.Execute();
			}

			private void Execute()
			{
				var worker = new Thread( () =>
				{
					try
					{
						this.test();
					}
					catch ( Exception e )
					{
						Console.WriteLine( e );
						this.ex = e;
					}
				} );

				worker.SetApartmentState( this.state );
				worker.Start();
				worker.Join();

				if ( this.ex != null )
				{
					throw this.ex;
				}
			}
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message()
		{
			var actual = false;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<IMessage>( this, msg => actual = true );
			broker.Dispatch( new LegacyTestMessage( this ) );

			actual.Should().Be.True();
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message()
		{
			var actual = false;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<Object>( this, ( s, msg ) => actual = true );
			broker.Dispatch( this, new PocoTestMessage() );

			actual.Should().Be.True();
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message_even_using_different_messages()
		{
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<IMessage>( this, msg => actual++ );
			broker.Dispatch( new LegacyTestMessage( this ) );
			broker.Dispatch( new LegacyAnotherTestMessage( this ) );

			actual.Should().Be.EqualTo( 2 );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message_even_using_different_messages()
		{
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<Object>( this, ( s, msg ) => actual++ );
			broker.Dispatch( this, new PocoTestMessage() );
			broker.Dispatch( this, new AnotherPocoTestMessage() );

			actual.Should().Be.EqualTo( 2 );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_MIXED_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message_even_using_different_messages()
		{
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<Object>( this, ( s, msg ) => actual++ );
			broker.Dispatch( this, new PocoTestMessage() );
			broker.Dispatch( new LegacyAnotherTestMessage( this ) );

			actual.Should().Be.EqualTo( 2 );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_subscriber_using_a_base_class_should_be_dispatched_only_to_the_expected_inheritance_chain()
		{
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, msg => actual++ );
			broker.Dispatch( new LegacyMessageDerivedFromLegacyTestMessage( this ) );
			broker.Dispatch( new LegacyAnotherTestMessage( this ) );

			actual.Should().Be.EqualTo( 1 );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_only_to_the_expected_inheritance_chain()
		{
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<PocoTestMessage>( this, ( s, m ) => actual++ );
			broker.Dispatch( this, new PocoMessageDerivedFromTestMessage() );
			broker.Dispatch( this, new AnotherPocoTestMessage() );

			actual.Should().Be.EqualTo( 1 );
		}

		//[TestMethod]
		//[TestCategory( "className" )]
		//[ExpectedException( typeof( ArgumentException ) )]
		//public void should_raise_ArgumentException()
		//{
		//    var actual = 0;
		//    var expected =1; 

		//    var dispatcher = new NullDispatcher();
		//    var broker = new MessageBroker( dispatcher );
		//    broker.Subscribe<TestMessage>( this, msg => actual++ );

		//    var partition = broker.GetSubsystem( "partition-identifier" );
		//    partition.Subscribe<TestMessage>( this, msg => actual++ );

		//    partition.Dispatch( new TestMessage( this ) );

		//    actual.Should().Be.EqualTo( expected );
		//}

		[TestMethod, Ignore]
		[TestCategory( "MessageBroker" )]
		[ExpectedException( typeof( ArgumentException ) )]
		public void messageBroker_broadcast_should_report_expected_exception()
		{
			ManualResetEvent h = new ManualResetEvent( false );

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, msg =>
			{
				throw new ArgumentException();
			} );

			try
			{
				broker.Broadcast( new LegacyTestMessage( this ) );

				h.WaitOne();
			}
			catch
			{
				h.Set();
				throw;
			}
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_dispatched_should_respect_the_given_priority()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_should_dispatch_non_POCO_message()
		{
			var received = false;
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, msg =>
			{
				received = true;
			} );

			broker.Dispatch( this, new LegacyTestMessage() );
			Assert.IsTrue( received );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_should_dispatch_non_POCO_message_and_subscribe_as_POCO()
		{
			var received = false;
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, ( s, msg ) =>
			{
				received = true;
			} );

			broker.Dispatch( this, new LegacyTestMessage() );
			Assert.IsTrue( received );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_should_broadcast_non_POCO_message()
		{
			var received = false;
			var h = new ManualResetEvent( false );
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, msg =>
			{
				received = true;
				h.Set();
			} );

			broker.Broadcast( this, new LegacyTestMessage() );

			h.WaitOne();

			Assert.IsTrue( received );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_should_broadcast_non_POCO_message_and_subscribe_as_POCO()
		{
			var received = false;
			var h = new ManualResetEvent( false );
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, ( s, msg ) =>
			{
				received = true;
				h.Set();
			} );

			broker.Broadcast( this, new LegacyTestMessage() );

			h.WaitOne();

			Assert.IsTrue( received );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_should_dispatch_non_POCO_message_with_correct_Sender()
		{
			Object expected = this;
			Object actual = null;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, msg =>
			{
				actual = msg.Sender;
			} );

			broker.Dispatch( this, new LegacyTestMessage() );

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void messageBroker_POCO_should_broadcast_non_POCO_message_with_correct_Sender()
		{
			Object expected = this;
			Object actual = null;

			var h = new ManualResetEvent( false );
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<LegacyTestMessage>( this, msg =>
			{
				actual = msg.Sender;
				h.Set();
			} );

			broker.Broadcast( this, new LegacyTestMessage() );

			h.WaitOne();

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_broadcast_from_multiple_thread_should_not_fail()
		{
			Exception failure = null;
			var wh = new ManualResetEvent( false );

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			var subscriberThread1 = new Thread( payload =>
			{
				while ( true )
				{
					try
					{
						broker.Subscribe<PocoTestMessage>( this, ( sender, msg ) =>
						{
							Thread.Sleep( 10 );
						} );
					}
					catch ( Exception e )
					{
						lock ( this )
						{
							failure = e;
							wh.Set();

							break;
						}
					}
				}
			} );
			subscriberThread1.IsBackground = true;
			subscriberThread1.Start();

			var subscriberThread2 = new Thread( payload =>
			{
				while ( true )
				{
					try
					{
						broker.Subscribe<PocoTestMessage>( this, ( sender, msg ) =>
						{
							Thread.Sleep( 10 );
						} );
					}
					catch ( Exception e )
					{
						lock ( this )
						{
							failure = e;
							wh.Set();
						}

						break;
					}
				}
			} );
			subscriberThread2.IsBackground = true;
			subscriberThread2.Start();

			var broadcastThread1 = new Thread( payload =>
			{
				while ( true )
				{
					try
					{
						broker.Broadcast( this, new PocoTestMessage() );
					}
					catch ( Exception e )
					{
						lock ( this )
						{
							failure = e;
							wh.Set();
						}

						break;
					}
				}
			} );
			broadcastThread1.IsBackground = true;
			broadcastThread1.Start();

			wh.WaitOne();

			Assert.IsNull( failure, failure != null ? failure.ToString() : "--" );

		}
	}
}