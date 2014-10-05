using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical.Messaging;
using Topics.Radical.Threading;
using Topics.Radical.ComponentModel.Messaging;

namespace Test.Radical.Windows.Messaging
{
	[TestClass]
	public class MessageBrokerTests
	{
		class TestMessage : Message
		{
			public TestMessage() { }

			public TestMessage( Object sender )
				: base( sender )
			{

			}
		}

		class MessageDerivedFromTestMessage : TestMessage
		{
			public MessageDerivedFromTestMessage( Object sender )
				: base( sender )
			{

			}
		}

		class AnotherTestMessage : Message
		{
			public AnotherTestMessage( Object sender )
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
		public void messageBroker_unsubscribe_specific_subscriber_should_remove_only_subscriptions_for_that_subscriber()
		{
			const int expected = 1;
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var target = new MessageBroker( dispatcher );

			var subscriber1 = new Object();
			var subscriber2 = new Object();

			target.Subscribe<TestMessage>( subscriber1, msg => { actual++; } );
			target.Subscribe<TestMessage>( subscriber1, msg => { actual++; } );
			target.Subscribe<TestMessage>( subscriber1, msg => { actual++; } );

			target.Subscribe<TestMessage>( subscriber2, msg => { actual++; } );

			target.Unsubscribe( subscriber1 );

			target.Dispatch( new TestMessage( this ) );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
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
		public void messageBroker_unsubscribe_specific_subscriber_and_specific_messageType_should_remove_only_subscriptions_for_that_subscriber()
		{
			const int expected = 1;
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var target = new MessageBroker( dispatcher );

			var subscriber = new Object();

			target.Subscribe<TestMessage>( subscriber, msg => { actual++; } );
			target.Subscribe<AnotherTestMessage>( subscriber, msg => { actual++; } );

			target.Unsubscribe<AnotherTestMessage>( subscriber );

			target.Dispatch( new TestMessage( this ) );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
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
		public void messageBroker_Dispatch_valid_message_should_not_fail()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Dispatch( new TestMessage( this ) );
		}

		[TestMethod]
		public void messageBroker_POCO_Dispatch_valid_message_should_not_fail()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Dispatch( this, new PocoTestMessage() );
		}

		[TestMethod]
		public void messageBroker_subscribe_using_null_action_should_raise_ArgumentNullException()
		{
			Executing.This( () =>
			{
				var dispatcher = new NullDispatcher();
				var broker = new MessageBroker( dispatcher );

				broker.Subscribe<TestMessage>( this, ( Action<TestMessage> )null );
			} )
			.Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
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
		public void messageBroker_Subscribe_based_on_message_type_should_not_fail()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<TestMessage>( this, m => { } );
		}

		[TestMethod]
		public void messageBroker_POCO_Subscribe_based_on_message_type_should_not_fail()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<PocoTestMessage>( this, ( s, m ) => { } );
		}

		[TestMethod]
		public void messageBroker_subscribe_normal_should_notify()
		{
			var expected = true;
			var actual = false;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<TestMessage>( this, msg => actual = true );
			broker.Dispatch( new TestMessage( this ) );

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
		public void messageBroker_MIXED_subscribe_POCO_normal_should_notify_IMessage()
		{
			var expected = true;
			var actual = false;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<Object>( this, ( s, msg ) => actual = true );
			broker.Dispatch( new TestMessage( this ) );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
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

				broker.Subscribe<TestMessage>( this, msg =>
				{
					s1ThreadId = Thread.CurrentThread.ManagedThreadId;
					h1.Set();
				} );

				broker.Subscribe<TestMessage>( this, msg =>
				{
					s2ThreadId = Thread.CurrentThread.ManagedThreadId;
					h2.Set();
				} );

				broker.Broadcast( new TestMessage( this ) );

				ManualResetEvent.WaitAll( new[] { h1, h2 } );

				currentThreadId.Should().Not.Be.EqualTo( s1ThreadId );
				currentThreadId.Should().Not.Be.EqualTo( s2ThreadId );
			} );
		}

		[TestMethod]
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
			broker.Dispatch( new TestMessage( this ) );

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
			broker.Dispatch( new TestMessage( this ) );
			broker.Dispatch( new AnotherTestMessage( this ) );

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
			broker.Dispatch( new AnotherTestMessage( this ) );

			actual.Should().Be.EqualTo( 2 );
		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public void MessageBroker_subscriber_using_a_base_class_should_be_dispatched_only_to_the_expected_inheritance_chain()
		{
			var actual = 0;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<TestMessage>( this, msg => actual++ );
			broker.Dispatch( new MessageDerivedFromTestMessage( this ) );
			broker.Dispatch( new AnotherTestMessage( this ) );

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
		[ExpectedException( typeof( ArgumentException ) )]
		public void messageBroker_broadcast_should_report_expected_exception()
		{
			ManualResetEvent h = new ManualResetEvent( false );

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<TestMessage>( this, msg =>
			{
				throw new ArgumentException();
			} );

			try
			{
				broker.Broadcast( new TestMessage( this ) );

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
		public void messageBroker_POCO_should_dispatch_non_POCO_message()
		{
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );
			broker.Subscribe<TestMessage>( this, msg => { } );
			broker.Dispatch( this, new TestMessage() );
		}

		[TestMethod]
		public void messageBroker_POCO_should_broadcast_non_POCO_message()
		{
			var h = new ManualResetEvent( false );
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );
			
			broker.Subscribe<TestMessage>( this, msg =>
			{
				h.Set();
			} );

			broker.Broadcast( this, new TestMessage() );

			h.WaitOne();
		}

		[TestMethod]
		public void messageBroker_POCO_should_dispatch_non_POCO_message_with_correct_Sender()
		{
			Object expected = this;
			Object actual = null;

			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<TestMessage>( this, msg =>
			{
				actual = msg.Sender;
			} );

			broker.Dispatch( this, new TestMessage() );

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		public void messageBroker_POCO_should_broadcast_non_POCO_message_with_correct_Sender()
		{
			Object expected = this;
			Object actual = null;

			var h = new ManualResetEvent( false );
			var dispatcher = new NullDispatcher();
			var broker = new MessageBroker( dispatcher );

			broker.Subscribe<TestMessage>( this, msg =>
			{
				actual = msg.Sender;
				h.Set();
			} );

			broker.Broadcast( this, new TestMessage() );

			h.WaitOne();

			Assert.AreEqual( expected, actual );
		}
	}
}