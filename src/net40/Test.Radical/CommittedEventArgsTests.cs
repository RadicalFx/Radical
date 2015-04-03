using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.ComponentModel.ChangeTracking;
using SharpTestsEx;

namespace Test.Radical
{
	[TestClass()]
	public class CommittedEventArgsTests
	{
		[TestMethod]
		public void committedEventArgs_ctor_normal_should_set_values()
		{
			var expected = CommitReason.AcceptChanges;
			CommittedEventArgs target = new CommittedEventArgs( expected );

			target.Reason.Should().Be.EqualTo( expected );
		}
	}
}
