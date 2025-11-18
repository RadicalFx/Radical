//extern alias tpx;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ChangeTracking.Specialized;
using Radical.ComponentModel.ChangeTracking;
using SharpTestsEx;
using System;

namespace Radical.Tests.Model
{
    [TestClass()]
    public class PropertyValueChangeTest : ChangeTracking.ChangeTest
    {
        protected override IChange<T> Mock<T>(object owner, T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, string description)
        {
            return MockPropertyValue<T>(owner, value, rejectCallback, commitCallback, description);
        }

        protected virtual PropertyValueChange<T> MockPropertyValue<T>(object owner, T value, RejectCallback<T> rejectCallback)
        {
            return new PropertyValueChange<T>(owner, "property-name", value, rejectCallback);
        }

        protected virtual PropertyValueChange<T> MockPropertyValue<T>(object owner, T value, RejectCallback<T> rejectCallback, string description)
        {
            return new PropertyValueChange<T>(owner, "property-name", value, rejectCallback, description);
        }

        protected virtual PropertyValueChange<T> MockPropertyValue<T>(object owner, T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, string description)
        {
            return new PropertyValueChange<T>(owner, "property-name", value, rejectCallback, commitCallback, description);
        }

        [TestMethod]
        public void propertyValueChange_ctor_owner_value_rejectCallback_normal_should_set_expected_values()
        {
            var expected = new object();

            var target = MockPropertyValue(expected, "Foo", cv => { });
            var actual = target.Owner;

            actual.Should().Be.EqualTo(expected);
            target.Description.Should().Be.EqualTo(string.Empty);
        }

        [TestMethod]
        public void propertyValueChange_ctor_owner_value_rejectCallback_on_reject_should_return_expected_values()
        {
            var expected = "Foo";
            ChangeRejectedEventArgs<string> actual = null;
            var owner = new object();

            var target = MockPropertyValue(owner, expected, cv => { actual = cv; });
            target.Reject(RejectReason.RejectChanges);

            actual.Should().Not.Be.Null();
            actual.CachedValue.Should().Be.EqualTo(expected);
            actual.Reason.Should().Be.EqualTo(RejectReason.RejectChanges);
            actual.Source.Should().Be.EqualTo(target);
            actual.Entity.Should().Be.EqualTo(owner);
        }

        [TestMethod]
        public void propertyValueChange_getAdvisedAction_should_return_expected_values()
        {
            var expected = ProposedActions.Create | ProposedActions.Update;
            var owner = new object();

            var target = MockPropertyValue(owner, "Foo", cv => { });
            var actual = target.GetAdvisedAction(owner);

            actual.Should().Be.EqualTo(expected);
        }


        [TestMethod]
        public void propertyVaueChange_getAdvisedAction_using_invalid_owner_should_raise_ArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var target = MockPropertyValue(new object(), "Foo", cv => { });
                target.GetAdvisedAction(new object());
            });
        }

        [TestMethod]
        public void propertyVaueChange_getAdvisedAction_using_invalid_owner_should_raise_ArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var target = MockPropertyValue(new object(), "Foo", cv => { });
                target.GetAdvisedAction(null);
            });
        }

        [TestMethod]
        public void propertyValueChange_clone_normal_should_return_cloned_value()
        {
            var target = MockPropertyValue(new object(), "Foo", cv => { });
            var actual = target.Clone();

            actual.Should().Not.Be.EqualTo(target);
            actual.Owner.Should().Be.EqualTo(target.Owner);
            actual.Description.Should().Be.EqualTo(target.Description);
            actual.GetChangedEntities().Should().Have.SameSequenceAs(target.GetChangedEntities());
        }

        [TestMethod]
        public void propertyValueChange_ctor_owner_value_rejectCallback_description_normal_should_set_expected_values()
        {
            var expected = new object();

            var target = MockPropertyValue(expected, "Foo", cv => { }, "description");
            var actual = target.Owner;

            actual.Should().Be.EqualTo(expected);
            target.Description.Should().Be.EqualTo("description");
        }
    }
}
