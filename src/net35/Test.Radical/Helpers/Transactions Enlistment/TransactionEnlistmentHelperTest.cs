using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Topics.Radical.Transactions;

namespace Test.Radical.Helpers
{
	[TestClass()]
	public class TransactionEnlistmentHelperTest
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

		[TestMethod()]
		public void TransactionEnlistmentHelper_Dispose()
		{
			TransactionEnlistmentHelper target = new TransactionEnlistmentHelper();
			target.Dispose();
		}

		[TestMethod()]
		public void TransactionEnlistmentHelper_ctor()
		{
			TransactionEnlistmentHelper target = new TransactionEnlistmentHelper();
			Assert.IsNotNull( target );
		}

		[TestMethod()]
		[ExpectedException( typeof( System.ArgumentNullException ) )]
		public void EnlistInTransaction_with_null_iEnlistmentNotification()
		{
			TransactionEnlistmentHelper target = new TransactionEnlistmentHelper();

			bool ensureTransaction = false;
			IEnlistmentNotification enlistmentNotification = null;
			EnlistmentOptions options = EnlistmentOptions.None;

			target.EnlistInTransaction( ensureTransaction, enlistmentNotification, options );
		}


		[TestMethod()]
		[ExpectedException( typeof( System.InvalidOperationException ) )]
		public void EnlistInTransaction_without_transaction_and_ensure()
		{
			TransactionEnlistmentHelper target = new TransactionEnlistmentHelper();

			bool ensureTransaction = true;
			IEnlistmentNotification enlistmentNotification = MockRepository.GenerateMock<IEnlistmentNotification>();
			EnlistmentOptions options = EnlistmentOptions.None;

			target.EnlistInTransaction( ensureTransaction, enlistmentNotification, options );

			enlistmentNotification.VerifyAllExpectations();
		}

		[TestMethod()]
		public void EnlistInTransaction_without_transaction_and_no_ensure()
		{
			TransactionEnlistmentHelper target = new TransactionEnlistmentHelper();

			bool ensureTransaction = false;
			IEnlistmentNotification enlistmentNotification = MockRepository.GenerateMock<IEnlistmentNotification>();
			EnlistmentOptions options = EnlistmentOptions.None;

			target.EnlistInTransaction( ensureTransaction, enlistmentNotification, options );

			enlistmentNotification.VerifyAllExpectations();
		}

		[TestMethod()]
		[ExpectedException( typeof( System.NotSupportedException ) )]
		public void EnlistInTransaction_already_enlisted_in_a_different_transaction()
		{
			TransactionEnlistmentHelper target = new TransactionEnlistmentHelper();

			bool ensureTransaction = true;
			IEnlistmentNotification enlistmentNotification = MockRepository.GenerateMock<IEnlistmentNotification>();
			EnlistmentOptions options = EnlistmentOptions.None;

			using( TransactionScope ts1 = new TransactionScope() )
			{
				target.EnlistInTransaction( ensureTransaction, enlistmentNotification, options );

				using( TransactionScope ts2 = new TransactionScope( TransactionScopeOption.RequiresNew ) )
				{
					target.EnlistInTransaction( ensureTransaction, enlistmentNotification, options );
					ts2.Complete();
				}

				ts1.Complete();
			}
		}
	}
}
