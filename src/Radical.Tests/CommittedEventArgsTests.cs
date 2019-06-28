using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel.ChangeTracking;
using SharpTestsEx;

namespace Radical.Tests
{
    [TestClass()]
    public class CommittedEventArgsTests
    {
        [TestMethod]
        public void committedEventArgs_ctor_normal_should_set_values()
        {
            var expected = CommitReason.AcceptChanges;
            CommittedEventArgs target = new CommittedEventArgs(expected);

            target.Reason.Should().Be.EqualTo(expected);
        }
    }
}
