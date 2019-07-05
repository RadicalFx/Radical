using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel.ChangeTracking;
using SharpTestsEx;

namespace Radical.Tests
{
    [TestClass()]
    public class RejectedEventArgsTests
    {
        [TestMethod]
        public void rejectedEventArgs_ctor_normal_should_set_values()
        {
            var expected = RejectReason.RejectChanges;
            RejectedEventArgs target = new RejectedEventArgs(expected);

            target.Reason.Should().Be.EqualTo(expected);
        }
    }
}
