//extern alias tpx;

using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel.ChangeTracking;
using SharpTestsEx;
using System;

namespace Radical.Tests
{
    [TestClass()]
    public class ChangeCommittedArgsTests : ChangeArgsTests
    {
        [TestMethod]
        public void changeCommittedArgs_generic_ctor_normal_should_correctly_set_values()
        {
            var entity = new Object();
            var cachedValue = new GenericParameterHelper();
            var iChange = A.Dummy<IChange>();
            var reason = CommitReason.AcceptChanges;

            var target = new ChangeCommittedEventArgs<GenericParameterHelper>(entity, cachedValue, iChange, reason);

            target.Entity.Should().Be.EqualTo(entity);
            target.CachedValue.Should().Be.EqualTo(cachedValue);
            target.Source.Should().Be.EqualTo(iChange);
            target.Reason.Should().Be.EqualTo(reason);
        }
    }
}
