namespace Radical.Tests.ChangeTracking
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical;
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    using SharpTestsEx;
    using System;

    [TestClass]
    public class AdvisedActionTests
    {
        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisedAction_ctor()
        {
            GenericParameterHelper target = new GenericParameterHelper();

            AdvisedAction actual = new AdvisedAction(target, ProposedActions.Delete);

            actual.Action.Should().Be.EqualTo(ProposedActions.Delete);
            actual.Target.Should().Be.EqualTo(target);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisedAction_ctor_null_reference_target()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                AdvisedAction actual = new AdvisedAction(null, ProposedActions.Delete);
            });
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisedAction_ctor_not_supported_proposed_action()
        {
            Assert.ThrowsExactly<NotSupportedException>(() =>
            {
                AdvisedAction actual = new AdvisedAction(new object(), ProposedActions.None);
            });
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisedAction_ctor_invalid_proposed_action()
        {
            Assert.ThrowsExactly<EnumValueOutOfRangeException>(() =>
            {
                AdvisedAction actual = new AdvisedAction(new object(), (ProposedActions)1000);
            });
        }
    }
}