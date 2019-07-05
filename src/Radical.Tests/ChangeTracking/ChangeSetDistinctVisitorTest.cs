using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using Radical.ChangeTracking;
using Radical.ComponentModel.ChangeTracking;
using SharpTestsEx;

namespace Radical.Tests.ChangeTracking
{
    [TestClass]
    public class ChangeSetDistinctVisitorTests
    {
        [TestMethod()]
        [TestCategory("ChangeTracking")]
        public void changeSetDistinctVisitor_visit()
        {
            ChangeSetDistinctVisitor target = new ChangeSetDistinctVisitor();

            var entities = new Object[] { new Object() };
            var c1 = A.Fake<IChange>();
            A.CallTo(() => c1.GetChangedEntities()).Returns(entities);

            var c2 = A.Fake<IChange>();
            A.CallTo(() => c2.GetChangedEntities()).Returns(entities);

            IChangeSet cSet = new ChangeSet(new IChange[] { c1, c2 });

            var result = target.Visit(cSet);

            result.Count.Should().Be.EqualTo(1);
            result[entities[0]].Should().Be.EqualTo(c2);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory("ChangeTracking")]
        public void changeSetDistinctVisitor_visit_null_changeSet_reference()
        {
            ChangeSetDistinctVisitor target = new ChangeSetDistinctVisitor();
            target.Visit(null);
        }
    }
}
