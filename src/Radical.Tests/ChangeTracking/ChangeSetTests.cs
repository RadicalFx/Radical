namespace Radical.Tests.ChangeTracking
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FakeItEasy;
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    using SharpTestsEx;

    [TestClass]
    public class ChangeSetTests
    {
        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeSet_ctor()
        {
            var expected = new IChange[]
            {
                A.Fake<IChange>(),
                A.Fake<IChange>(),
                A.Fake<IChange>()
            };

            var actual = new ChangeSet(expected);

            actual.Count.Should().Be.EqualTo(3);
            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory("ChangeTracking")]
        public void changeSet_ctor_null_reference()
        {
            var actual = new ChangeSet(null);
        }
    }
}