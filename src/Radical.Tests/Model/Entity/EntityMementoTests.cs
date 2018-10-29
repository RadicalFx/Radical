//extern alias tpx;

namespace Radical.Tests.Model.Entity
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Model;
    using SharpTestsEx;

    [TestClass()]
    public class EntityMementoTests : EntityTests
    {
        [TestMethod]
        public void entityMemento_ctor_default_set_default_values()
        {
            var target = A.Fake<MementoEntity>();

            ((IMemento)target).Memento.Should().Be.Null();
            ((IMemento)target).Memento.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_ctor_memento_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new[] { expected }));

            ((IMemento)target).Memento.Should().Be.EqualTo(expected);
            ((IMemento)target).Memento.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entityMemento_ctor_registerAsTransient_true_set_default_values()
        {
            var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { true }));

            ((IMemento)target).Memento.Should().Be.Null();
            ((IMemento)target).Memento.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_ctor_registerAsTransient_false_set_default_values()
        {
            var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { false }));

            ((IMemento)target).Memento.Should().Be.Null();
            ((IMemento)target).Memento.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_ctor_memento_registerAsTransient_false_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { expected, false }));

            ((IMemento)target).Memento.Should().Be.EqualTo(expected);
            ((IMemento)target).Memento.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entityMemento_ctor_memento_registerAsTransient_true_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { expected, true }));

            ((IMemento)target).Memento.Should().Be.EqualTo(expected);
            ((IMemento)target).Memento.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_successfully_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;
            using (ChangeTrackingService svc = new ChangeTrackingService())
            {
                var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { true }));
                ((IMemento)target).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState(target);

                actual.Should().Be.EqualTo(expected);
            }
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_to_suspended_memento_do_not_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;
            using (ChangeTrackingService svc = new ChangeTrackingService())
            {
                svc.Suspend();

                var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { true }));
                ((IMemento)target).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState(target);

                actual.Should().Be.EqualTo(expected);
            }
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_without_memento_do_not_fail()
        {
            var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { true }));
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_using_base_iMemento_successfully_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;
            using (ChangeTrackingService svc = new ChangeTrackingService())
            {
                var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { true }));
                ((IMemento)target).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState(target);

                actual.Should().Be.EqualTo(expected);
            }
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_using_base_iMemento_to_suspended_memento_do_not_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;
            using (var svc = new ChangeTrackingService())
            {
                svc.Suspend();

                var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new object[] { true }));
                ((IMemento)target).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState(target);

                actual.Should().Be.EqualTo(expected);
            }
        }

        [TestMethod]
        public void entityMemento_memento_succesfully_set_and_get_memento_reference()
        {
            var expected = new ChangeTrackingService();

            var target = A.Fake<MementoEntity>();
            ((IMemento)target).Memento = expected;
            var actual = ((IMemento)target).Memento;

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entityMemento_memento_using_base_iMemento_succesfully_set_and_get_memento_reference()
        {
            var expected = new ChangeTrackingService();

            var target = A.Fake<MementoEntity>();
            ((IMemento)target).Memento = expected;
            var actual = ((IMemento)target).Memento;

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entityMemento_memento_can_be_set_to_null()
        {
            var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new[] { new ChangeTrackingService() }));
            ((IMemento)target).Memento = null;

            ((IMemento)target).Memento.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_memento_using_base_iMemento_can_be_set_to_null()
        {
            var target = A.Fake<MementoEntity>(options => options.WithArgumentsForConstructor(new[] { new ChangeTrackingService() }));
            ((IMemento)target).Memento = null;

            ((IMemento)target).Memento.Should().Be.Null();
        }
    }
}
