//extern alias tpx;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using SharpTestsEx;
using Radical.ComponentModel.ChangeTracking;
using Radical.ChangeTracking;
using Radical;

namespace Radical.Tests.ChangeTracking
{
    [TestClass()]
    public class ChangeTest
    {
        class ChangeMock<T> : Change<T>
        {
            public ChangeMock(object owner, T valueToCache, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, string description)
                : base(owner, valueToCache, rejectCallback, commitCallback, description)
            {

            }

            public override IChange Clone()
            {
                return new ChangeMock<T>(Owner, CachedValue, RejectCallback, CommitCallback, Description);
            }

            public override ProposedActions GetAdvisedAction(object changedItem)
            {
                throw new NotImplementedException();
            }
        }

        protected virtual IChange<T> Mock<T>(Object owner, T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, string description)
        {
            return new ChangeMock<T>(owner, value, rejectCallback, commitCallback, description);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_properties_tests()
        {
            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = e => { };
            var description = string.Empty;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);

            target.Should().Not.Be.Null();
            target.Owner.Should().Be.EqualTo(owner);
            target.CachedValue.Should().Be.EqualTo(value);
            target.Description.Should().Be.EqualTo(description);
            target.IsCommitSupported.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_null_commitCallback()
        {
            var target = new ChangeMock<string>(new Object(), "Foo", e => { }, null, string.Empty);

            target.IsCommitSupported.Should().Be.False();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_ctor_null_owner_argumentNullException()
        {
            var target = new ChangeMock<string>(null, "Foo", null, null, string.Empty);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_rejectCallback_is_invoked_on_reject()
        {
            var invoked = false;

            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { invoked = true; };
            CommitCallback<string> cc = null;
            var description = string.Empty;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Reject(RejectReason.Undo);

            invoked.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_rejectCallback_is_invoked_with_expected_values()
        {
            ChangeRejectedEventArgs<string> expected = null;

            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { expected = e; };
            CommitCallback<string> cc = null;
            var description = string.Empty;
            var reason = RejectReason.Undo;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Reject(reason);

            expected.Should().Not.Be.Null();
            expected.Source.Should().Be.EqualTo(target);
            expected.Reason.Should().Be.EqualTo(reason);
            expected.Entity.Should().Be.EqualTo(owner);
            expected.CachedValue.Should().Be.EqualTo(value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_reject_invoked_with_invalid_rejectReason()
        {
            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = null;
            var description = string.Empty;
            var reason = RejectReason.None;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Reject(reason);
        }

        [TestMethod]
        [ExpectedException(typeof(EnumValueOutOfRangeException))]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_reject_invoked_with_outOfRange_rejectReason()
        {
            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = null;
            var description = string.Empty;
            var reason = (RejectReason)1000;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Reject(reason);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_ctor_null_rejectCallback_argumentNullException()
        {
            var target = new ChangeMock<string>(new Object(), "Foo", null, null, string.Empty);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_rejected_fired()
        {
            int expected = 1;
            int actual = 0;

            var owner = new object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = null;
            var description = string.Empty;
            var reason = RejectReason.Undo;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Rejected += (s, e) => { actual++; };
            target.Reject(reason);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_committed_fired()
        {
            int expected = 1;
            int actual = 0;

            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = null;
            var description = string.Empty;
            var reason = CommitReason.AcceptChanges;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Committed += (s, e) => { actual++; };
            target.Commit(reason);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_commit_invoked_with_invalid_rejectReason()
        {
            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = e => { };
            var description = string.Empty;
            var reason = CommitReason.None;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Commit(reason);
        }

        [TestMethod]
        [ExpectedException(typeof(EnumValueOutOfRangeException))]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_commit_invoked_with_outOfRange_rejectReason()
        {
            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = e => { };
            var description = string.Empty;
            var reason = (CommitReason)1000;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Commit(reason);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_commitCallback_is_invoked_with_expected_values()
        {
            ChangeCommittedEventArgs<string> expected = null;

            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = e => { expected = e; };
            var description = string.Empty;
            var reason = CommitReason.AcceptChanges;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            target.Commit(reason);

            expected.Should().Not.Be.Null();
            expected.Source.Should().Be.EqualTo(target);
            expected.Reason.Should().Be.EqualTo(reason);
            expected.Entity.Should().Be.EqualTo(owner);
            expected.CachedValue.Should().Be.EqualTo(value);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iChange_getChangedEntities()
        {
            var owner = new Object();
            var value = "Foo";
            RejectCallback<string> rc = e => { };
            CommitCallback<string> cc = e => { };
            var description = string.Empty;

            var target = new ChangeMock<string>(owner, value, rc, cc, description);
            IEnumerable<Object> ce = target.GetChangedEntities();

            ce.Should().Not.Be.Null();
            ce.Count().Should().Be.EqualTo(1);
            ce.First().Should().Be.EqualTo(owner);
        }
    }
}
