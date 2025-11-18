namespace Radical.Tests.ChangeTracking
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    using SharpTestsEx;
    using System;
    using System.Linq;

    [TestClass]
    public class AdvisoryBuilderTests
    {
        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_ctor()
        {
            var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_ctor_null_visitor()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var actual = new AdvisoryBuilder(null);
            });
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_null_service_reference()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
                actual.GenerateAdvisory(null, null);
            });
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_null_changeSet_reference()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
                actual.GenerateAdvisory(new ChangeTrackingService(), null);
            });
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_for_transient_and_changed_entity()
        {
            var expectedAction = ProposedActions.Create;
            var entity = new object();
            var entityState = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove | EntityTrackingStates.HasBackwardChanges;

            var c1 = A.Fake<IChange>();
            A.CallTo(() => c1.GetChangedEntities()).Returns(new object[] { entity });
            A.CallTo(() => c1.GetAdvisedAction(entity)).Returns(ProposedActions.Update | ProposedActions.Create);
            //c1.Replay();

            var c2 = A.Fake<IChange>();
            A.CallTo(() => c2.GetChangedEntities()).Returns(new object[] { entity });
            A.CallTo(() => c2.GetAdvisedAction(entity)).Returns(ProposedActions.Update | ProposedActions.Create);

            var cSet = new ChangeSet(new IChange[] { c1, c2 });

            var svc = A.Fake<IChangeTrackingService>();
            A.CallTo(() => svc.GetEntityState(entity)).Returns(entityState);
            A.CallTo(() => svc.GetEntities(EntityTrackingStates.IsTransient, true)).Returns(new object[0]);

            var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
            IAdvisory advisory = actual.GenerateAdvisory(svc, cSet);

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo(1);
            advisory.First().Action.Should().Be.EqualTo(expectedAction);
            advisory.First().Target.Should().Be.EqualTo(entity);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_for_non_transient_and_changed_entity()
        {
            var expectedAction = ProposedActions.Update;
            var entity = new object();
            var entityState = EntityTrackingStates.HasBackwardChanges;

            var c1 = A.Fake<IChange>();
            A.CallTo(() => c1.GetChangedEntities()).Returns(new object[] { entity });
            A.CallTo(() => c1.GetAdvisedAction(entity)).Returns(ProposedActions.Update | ProposedActions.Create);

            var c2 = A.Fake<IChange>();
            A.CallTo(() => c2.GetChangedEntities()).Returns(new object[] { entity });
            A.CallTo(() => c2.GetAdvisedAction(entity)).Returns(ProposedActions.Update | ProposedActions.Create);

            var cSet = new ChangeSet(new IChange[] { c1, c2 });

            var svc = A.Fake<IChangeTrackingService>();
            A.CallTo(() => svc.GetEntityState(entity)).Returns(entityState);
            A.CallTo(() => svc.GetEntities(EntityTrackingStates.IsTransient, true)).Returns(new object[0]);

            var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
            IAdvisory advisory = actual.GenerateAdvisory(svc, cSet);

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo(1);
            advisory.First().Action.Should().Be.EqualTo(expectedAction);
            advisory.First().Target.Should().Be.EqualTo(entity);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_for_strict_isTransient_entity()
        {
            var expectedAction = ProposedActions.Create;
            var entity = new object();
            var entityState = EntityTrackingStates.IsTransient;

            var cSet = new ChangeSet(new IChange[0]);

            var svc = A.Fake<IChangeTrackingService>();
            A.CallTo(() => svc.GetEntityState(entity)).Returns(entityState);
            A.CallTo(() => svc.GetEntities(EntityTrackingStates.IsTransient, true)).Returns(new object[] { entity });

            var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
            IAdvisory advisory = actual.GenerateAdvisory(svc, cSet);

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo(1);
            advisory.First().Action.Should().Be.EqualTo(expectedAction);
            advisory.First().Target.Should().Be.EqualTo(entity);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_for_isTransient_and_autoRemove_entity_with_no_changes_should_generate_an_empty_advisory()
        {
            var entity = new object();
            var entityState = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;

            var cSet = new ChangeSet(new IChange[0]);

            var svc = A.Fake<IChangeTrackingService>();
            A.CallTo(() => svc.GetEntityState(entity)).Returns(entityState);
            A.CallTo(() => svc.GetEntities(EntityTrackingStates.IsTransient, true)).Returns(new object[0]);

            var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
            IAdvisory advisory = actual.GenerateAdvisory(svc, cSet);

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo(0);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_for_transient_and_disposed_entity()
        {
            var expectedAction = ProposedActions.Dispose;
            var entity = new object();
            var entityState = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove | EntityTrackingStates.HasBackwardChanges;

            var c1 = A.Fake<IChange>();
            A.CallTo(() => c1.GetChangedEntities()).Returns(new object[] { entity });
            A.CallTo(() => c1.GetAdvisedAction(entity)).Returns(ProposedActions.Create | ProposedActions.Update);

            var c2 = A.Fake<IChange>();
            A.CallTo(() => c2.GetChangedEntities()).Returns(new object[] { entity });
            A.CallTo(() => c2.GetAdvisedAction(entity)).Returns(ProposedActions.Delete | ProposedActions.Dispose);

            var cSet = new ChangeSet(new IChange[] { c1, c2 });

            var svc = A.Fake<IChangeTrackingService>();
            A.CallTo(() => svc.GetEntityState(entity)).Returns(entityState);
            A.CallTo(() => svc.GetEntities(EntityTrackingStates.IsTransient, true)).Returns(new object[0]);

            var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
            IAdvisory advisory = actual.GenerateAdvisory(svc, cSet);

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo(1);
            advisory.First().Action.Should().Be.EqualTo(expectedAction);
            advisory.First().Target.Should().Be.EqualTo(entity);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_for_non_transient_and_deleted_entity()
        {
            var expectedAction = ProposedActions.Delete;
            var entity = new object();
            var entityState = EntityTrackingStates.HasBackwardChanges;

            var c1 = A.Fake<IChange>();
            A.CallTo(() => c1.GetChangedEntities()).Returns(new object[] { entity });
            A.CallTo(() => c1.GetAdvisedAction(entity)).Returns(ProposedActions.Create | ProposedActions.Update);

            var c2 = A.Fake<IChange>();
            A.CallTo(() => c2.GetChangedEntities()).Returns(new object[] { entity });
            A.CallTo(() => c2.GetAdvisedAction(entity)).Returns(ProposedActions.Delete | ProposedActions.Dispose);

            var cSet = new ChangeSet(new IChange[] { c1, c2 });

            var svc = A.Fake<IChangeTrackingService>();
            A.CallTo(() => svc.GetEntityState(entity)).Returns(entityState);
            A.CallTo(() => svc.GetEntities(EntityTrackingStates.IsTransient, true)).Returns(new object[0]);

            var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
            IAdvisory advisory = actual.GenerateAdvisory(svc, cSet);

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo(1);
            advisory.First().Action.Should().Be.EqualTo(expectedAction);
            advisory.First().Target.Should().Be.EqualTo(entity);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void advisoryBuilder_generateAdvisory_unsupported_proposedActions_value()
        {
            Assert.ThrowsException<NotSupportedException>(() =>
            {
                var entity = new object();
                var entityState = EntityTrackingStates.HasBackwardChanges();

                var c1 = A.Fake<IChange>();
                A.CallTo(() => c1.GetChangedEntities()).Returns(new object[] { entity });
                A.CallTo(() => c1.GetAdvisedAction(entity)).Returns(ProposedActions.None);

                var cSet = new ChangeSet(new IChange[] { c1 });

                var svc = A.Fake<IChangeTrackingService>();
                A.CallTo(() => svc.GetEntityState(entity)).Returns(entityState);
                A.CallTo(() => svc.GetEntities(EntityTrackingStates.IsTransient, true)).Returns(new object[0]);

                var actual = new AdvisoryBuilder(new ChangeSetDistinctVisitor());
                IAdvisory advisory = actual.GenerateAdvisory(svc, cSet);
            });
        }
    }
}