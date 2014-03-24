using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Topics.Radical.Transactions;

namespace Test.Radical.Helpers
{
	[TestClass()]
	public class TransactionEnlistmentHelperEnlistTest
	{
		[TestMethod()]
		public void EnlistInTransaction_enlist()
		{
			var target = new TransactionEnlistmentHelper();

			bool ensureTransaction = true;
			EnlistmentOptions options = EnlistmentOptions.None;

			var enlistmentNotification = MockRepository.GenerateMock<IEnlistmentNotification>();
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

			using( var ts = new TransactionScope() )
			{
				target.EnlistInTransaction( ensureTransaction, enlistmentNotification, options );
				ts.Complete();
			}

			enlistmentNotification.VerifyAllExpectations();
		}
	}
}
