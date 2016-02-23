//extern alias tpx;

namespace Test.Radical.Model.Entity
{
    using Topics.Radical.Model;
    using SharpTestsEx;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Topics.Radical.ComponentModel;
    using Rhino.Mocks;
    using Topics.Radical.ComponentModel.ChangeTracking;
    using System.ComponentModel;
    using Topics.Radical.ChangeTracking;

    [TestClass()]
    public class EntityMementoTests : EntityTests
    {
        protected override Entity CreateMock()
        {
            MockRepository mocks = new MockRepository();
            var entity = mocks.PartialMock<MementoEntity>();
            entity.Replay();

            return entity;
        }

        protected virtual MementoEntity CreateMock( bool registerAsrTransient ) 
        {
            MockRepository mocks = new MockRepository();
            var entity = mocks.PartialMock<MementoEntity>( registerAsrTransient );
            entity.Replay();

            return entity;
        }

        protected virtual MementoEntity CreateMock( IChangeTrackingService memento )
        {
            MockRepository mocks = new MockRepository();
            var entity = mocks.PartialMock<MementoEntity>( memento );
            entity.Replay();

            return entity;
        }

        protected virtual MementoEntity CreateMock( IChangeTrackingService memento, bool registerAsrTransient ) 
        {
            MockRepository mocks = new MockRepository();
            var entity = mocks.PartialMock<MementoEntity>( memento, registerAsrTransient );
            entity.Replay();

            return entity;
        }

        [TestMethod]
        public void entityMemento_ctor_default_set_default_values()
        {
            var target = ( MementoEntity )this.CreateMock();

            ( ( IMemento )target ).Memento.Should().Be.Null();
            ( ( IMemento )target ).Memento.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_ctor_memento_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = this.CreateMock( expected );

            ( ( IMemento )target ).Memento.Should().Be.EqualTo( expected );
            ( ( IMemento )target ).Memento.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entityMemento_ctor_registerAsTransient_true_set_default_values()
        {
            var target = this.CreateMock( true );

            ( ( IMemento )target ).Memento.Should().Be.Null();
            ( ( IMemento )target ).Memento.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_ctor_registerAsTransient_false_set_default_values()
        {
            var target = this.CreateMock( false );

            ( ( IMemento )target ).Memento.Should().Be.Null();
            ( ( IMemento )target ).Memento.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_ctor_memento_registerAsTransient_false_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = this.CreateMock( expected, false );

            ( ( IMemento )target ).Memento.Should().Be.EqualTo( expected );
            ( ( IMemento )target ).Memento.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entityMemento_ctor_memento_registerAsTransient_true_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = this.CreateMock( expected, true );

            ( ( IMemento )target ).Memento.Should().Be.EqualTo( expected );
            ( ( IMemento )target ).Memento.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_successfully_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;
            using( ChangeTrackingService svc = new ChangeTrackingService() )
            {
                var target = this.CreateMock( true );
                ( ( IMemento )target ).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState( target );

                actual.Should().Be.EqualTo( expected );
            }
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_to_suspended_memento_do_not_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;
            using( ChangeTrackingService svc = new ChangeTrackingService() )
            {
                svc.Suspend();

                var target = this.CreateMock( true );
                ( ( IMemento )target ).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState( target );

                actual.Should().Be.EqualTo( expected );
            }
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_without_memento_do_not_fail()
        {
            var target = this.CreateMock( true );
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_using_base_iMemento_successfully_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;
            using( ChangeTrackingService svc = new ChangeTrackingService() )
            {
                var target = this.CreateMock( true );
                ( ( IMemento )target ).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState( target );

                actual.Should().Be.EqualTo( expected );
            }
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_using_base_iMemento_to_suspended_memento_do_not_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;
            using( var svc = new ChangeTrackingService() )
            {
                svc.Suspend();

                var target = this.CreateMock( true );
                ( ( IMemento )target ).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState( target );

                actual.Should().Be.EqualTo( expected );
            }
        }

        [TestMethod]
        public void entityMemento_memento_succesfully_set_and_get_memento_reference()
        {
            var expected = new ChangeTrackingService();

            var target = (MementoEntity)this.CreateMock();
            ( ( IMemento )target ).Memento = expected;
            var actual = ( ( IMemento )target ).Memento;

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entityMemento_memento_using_base_iMemento_succesfully_set_and_get_memento_reference()
        {
            var expected = new ChangeTrackingService();

            var target = this.CreateMock();
            ( ( IMemento )target ).Memento = expected;
            var actual = ( ( IMemento )target ).Memento;

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entityMemento_memento_can_be_set_to_null()
        {
            var target = this.CreateMock( new ChangeTrackingService() );
            ( ( IMemento )target ).Memento = null;

            ( ( IMemento )target ).Memento.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_memento_using_base_iMemento_can_be_set_to_null()
        {
            var target = this.CreateMock( new ChangeTrackingService() );
            ( ( IMemento )target ).Memento = null;

            ( ( IMemento )target ).Memento.Should().Be.Null();
        }
    }
}
