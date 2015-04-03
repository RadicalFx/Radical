//extern alias tpx;

using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Topics.Radical.Transactions;

namespace Test.Radical.Helpers
{
	[TestClass()]
	public class TransactionEnlistmentHelperEventsTest
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		TransactionEnlistmentHelper target = null;
		bool ensureTransaction = true;
		EnlistmentOptions options = EnlistmentOptions.None;

		IEnlistmentNotification enlistmentNotification = null;

		[TestInitialize]
		public void TestInitialize()
		{
			target = new TransactionEnlistmentHelper();

			ensureTransaction = true;
			options = EnlistmentOptions.None;

			enlistmentNotification = MockRepository.GenerateMock<IEnlistmentNotification>();
			enlistmentNotification.Expect( en => en.Prepare( null ) )
				.IgnoreArguments()
				.WhenCalled( a =>
				{
					PreparingEnlistment e = a.Arguments.GetValue( 0 ) as PreparingEnlistment;
					e.Prepared();
				} )
				.Repeat.Once();

			enlistmentNotification.Expect( en => en.Commit( null ) )
				.IgnoreArguments()
				.WhenCalled( a =>
				{
					Enlistment e = a.Arguments.GetValue( 0 ) as Enlistment;
					e.Done();
				} )
				.Repeat.Once();
		}

		[TestCleanup]
		public void TestCleanup()
		{
			target = null;
			ensureTransaction = true;
			options = EnlistmentOptions.None;

			enlistmentNotification = null;
		}

		[TestMethod()]
		public void EnlistInTransaction_enlist_fires_enlisted_event()
		{
			System.EventHandler handler = null;
			bool fired = false;

			handler = ( s, e ) =>
			{
				( ( TransactionEnlistmentHelper )s ).TransactionEnlisted -= handler;
				fired = true;
			};

			target.TransactionEnlisted += handler;

			using( TransactionScope ts = new TransactionScope() )
			{
				target.EnlistInTransaction( ensureTransaction, enlistmentNotification, options );
				ts.Complete();
			}

			enlistmentNotification.VerifyAllExpectations();
			Assert.IsTrue( fired );
		}

		[TestMethod()]
		public void EnlistInTransaction_enlist_fires_completed_event()
		{
			System.EventHandler handler = null;
			bool fired = false;

			handler = ( s, e ) =>
			{
				( ( TransactionEnlistmentHelper )s ).TransactionCompleted -= handler;
				fired = true;
			};

			target.TransactionCompleted += handler;

			using( TransactionScope ts = new TransactionScope() )
			{
				target.EnlistInTransaction( ensureTransaction, enlistmentNotification, options );
				ts.Complete();
			}

			enlistmentNotification.VerifyAllExpectations();
			Assert.IsTrue( fired );
		}
	}
}
