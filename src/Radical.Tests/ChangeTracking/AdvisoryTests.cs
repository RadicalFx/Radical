namespace Radical.Tests.ChangeTracking
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    using SharpTestsEx;
    using System;

    [TestClass]
    public class AdvisoryTests
    {
        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisory_ctor()
        {
            var expected = new IAdvisedAction[]
            {
                A.Fake<IAdvisedAction>(),
                A.Fake<IAdvisedAction>(),
                A.Fake<IAdvisedAction>()
            };

            var actual = new Advisory(expected);

            actual.Count.Should().Be.EqualTo(3);
            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory("ChangeTracking")]
        public void advisory_ctor_null_reference()
        {
            var actual = new Advisory(null);
        }
    }
}