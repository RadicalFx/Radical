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
    using System;
    using System.Linq;
    using Topics.Radical;

    [TestClass()]
    public class EntityMementoTrackingFeaturesTests : EntityMementoTests
    {
        #region Mocks

        public sealed class TestableMementoEntity : MementoEntity
        {
            public TestableMementoEntity()
                : base()
            {

            }

            public TestableMementoEntity( IChangeTrackingService memento )
                : base( memento, true )
            {

            }

            public TestableMementoEntity( Boolean registerAsTransient )
                : base( null, registerAsTransient )
            {

            }

            public TestableMementoEntity( IChangeTrackingService memento, Boolean registerAsTransient )
                : base( memento, registerAsTransient )
            {

            }

            internal Boolean GetIsTracking()
            {
                return base.IsTracking;
            }

            internal IChange InvokeCacheChange<T>( T value, RejectCallback<T> rc )
            {
                return base.CacheChange( "property-name", value, rc );
            }

            internal IChange InvokeCacheChange<T>( T value, RejectCallback<T> rc, CommitCallback<T> cc )
            {
                return base.CacheChange( "property-name", value, rc, cc );
            }

            internal IChange InvokeCacheChange<T>( T value, RejectCallback<T> rc, CommitCallback<T> cc, AddChangeBehavior behavior )
            {
                return base.CacheChange( "property-name", value, rc, cc, behavior );
            }

            internal IChange InvokeCacheChangeOnRejectCallback<T>( T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, ChangeRejectedEventArgs<T> args )
            {
                return base.CacheChangeOnRejectCallback( "property-name", value, rejectCallback, commitCallback, args );
            }

            internal event EventHandler<MementoChangedEventArgs> MementoChanged;

            protected override void OnMementoChanged( IChangeTrackingService newMemento, IChangeTrackingService oldMemento )
            {
                base.OnMementoChanged( newMemento, oldMemento );

                if( this.MementoChanged != null )
                {
                    this.MementoChanged( this, new MementoChangedEventArgs( newMemento, oldMemento ) );
                }
            }
        }

        public class ChangeTrackingServiceStub : ChangeTrackingService
        {

        }

        public class MementoChangedEventArgs : EventArgs
        {
            internal MementoChangedEventArgs( IChangeTrackingService newMemento, IChangeTrackingService oldMemento )
            {
                this.NewMemento = newMemento;
                this.OldMemento = oldMemento;
            }

            public readonly IChangeTrackingService NewMemento;
            public readonly IChangeTrackingService OldMemento;
        }

        #endregion

        #region Mock creation

        protected override Entity CreateMock()
        {
            return this.CreateTestableEntityMock();
        }

        protected virtual TestableMementoEntity CreateTestableEntityMock()
        {
            return new TestableMementoEntity();
        }

        protected override MementoEntity CreateMock( bool registerAsrTransient )
        {
            return this.CreateTestableEntityMock( registerAsrTransient );
        }

        protected virtual TestableMementoEntity CreateTestableEntityMock( bool registerAsrTransient )
        {
            return new TestableMementoEntity( registerAsrTransient );
        }

        protected override MementoEntity CreateMock( IChangeTrackingService memento )
        {
            return this.CreateTestableEntityMock( memento );
        }

        protected virtual TestableMementoEntity CreateTestableEntityMock( IChangeTrackingService memento )
        {
            return new TestableMementoEntity( memento );
        }

        protected override MementoEntity CreateMock( IChangeTrackingService memento, bool registerAsrTransient )
        {
            return this.CreateTestableEntityMock( memento, registerAsrTransient );
        }

        protected virtual TestableMementoEntity CreateTestableEntityMock( IChangeTrackingService memento, bool registerAsrTransient )
        {
            return new TestableMementoEntity( memento, registerAsrTransient );
        }

        #endregion

        [TestMethod]
        public void entityMemento_isTracking_without_service_should_be_false()
        {
            var target = this.CreateTestableEntityMock();
            target.GetIsTracking().Should().Be.False();
        }

        [TestMethod]
        public void entityMemento_isTracking_without_service_and_with_registerAsTransient_request_true_should_be_false()
        {
            var target = this.CreateTestableEntityMock( true );
            target.GetIsTracking().Should().Be.False();
        }

        [TestMethod]
        public void entityMemento_isTracking_without_service_and_with_registerAsTransient_request_false_should_be_false()
        {
            var target = this.CreateTestableEntityMock( false );
            target.GetIsTracking().Should().Be.False();
        }

        [TestMethod]
        public void entityMemento_isTracking_with_service_should_be_true()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );
            target.GetIsTracking().Should().Be.True();
        }

        [TestMethod]
        public void entityMemento_isTracking_with_service_and_registerAsTransient_true_should_be_true()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento, true );
            target.GetIsTracking().Should().Be.True();
        }

        [TestMethod]
        public void entityMemento_isTracking_with_service_and_registerAsTransient_false_should_be_true()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento, false );
            target.GetIsTracking().Should().Be.True();
        }

        [TestMethod]
        public void entityMemento_isTracking_with_suspended_service_should_be_false()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock( memento );
            target.GetIsTracking().Should().Be.False();
        }

        [TestMethod]
        public void entityMemento_isTracking_with_suspended_serviceand_registerAsTransient_false_should_be_false()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock( memento, false );
            target.GetIsTracking().Should().Be.False();
        }

        [TestMethod]
        public void entityMemento_isTracking_with_suspended_serviceand_registerAsTransient_true_should_be_false()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock( memento, true );
            target.GetIsTracking().Should().Be.False();
        }

        [TestMethod]
        public void entityMemento_memento_normal_onMementoChanged_is_invoked_with_expected_values()
        {
            IChangeTrackingService expectedNew = new ChangeTrackingService();
            IChangeTrackingService expectedOld = null;
            IChangeTrackingService actualNew = null;
            IChangeTrackingService actualOld = null;

            var target = this.CreateTestableEntityMock();

            target.MementoChanged += ( s, e ) =>
            {
                actualNew = e.NewMemento;
                actualOld = e.OldMemento;
            };
            ( ( IMemento )target ).Memento = expectedNew;

            actualNew.Should().Be.EqualTo( expectedNew );
            actualOld.Should().Be.EqualTo( expectedOld );
        }

        [TestMethod]
        public void entityMemento_memento_changing_current_onMementoChanged_is_invoked_with_expected_values()
        {
            IChangeTrackingService expectedNew = new ChangeTrackingService();
            IChangeTrackingService expectedOld = new ChangeTrackingService();
            IChangeTrackingService actualNew = null;
            IChangeTrackingService actualOld = null;

            var target = this.CreateTestableEntityMock( expectedOld );

            target.MementoChanged += ( s, e ) =>
            {
                actualNew = e.NewMemento;
                actualOld = e.OldMemento;
            };
            ( ( IMemento )target ).Memento = expectedNew;

            actualNew.Should().Be.EqualTo( expectedNew );
            actualOld.Should().Be.EqualTo( expectedOld );
        }

        [TestMethod]
        public void entityMemento_memento_normal_onMementoChanged_is_not_invoked_setting_same_reference()
        {
            var expected = 0;
            var actual = 0;

            var memento = new ChangeTrackingService();

            var target = this.CreateTestableEntityMock( memento );

            target.MementoChanged += ( s, e ) => { actual++; };
            ( ( IMemento )target ).Memento = memento;

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [ExpectedException( typeof( ObjectDisposedException ) )]
        public void entityMemento_memento_changing_on_disposed_entity_should_raise_ObjectDisposedException()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );
            target.Dispose();
            ( ( IMemento )target ).Memento = null;
        }

        [TestMethod]
        [ExpectedException( typeof( ObjectDisposedException ) )]
        public void entityMemento_memento_changing_on_disposed_entity_using_base_iMemento_should_raise_ObjectDisposedException()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );
            target.Dispose();
            ( ( IMemento )target ).Memento = null;
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_should_be_called()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { } );

            change.Should().Not.Be.Null();
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_should_be_called_with_expected_values()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { } );

            change.IsCommitSupported.Should().Be.False();
            change.Owner.Should().Be.EqualTo( target );
            change.Description.Should().Be.EqualTo( String.Empty );
        }

        [TestMethod]
        [ExpectedException( typeof( ObjectDisposedException ) )]
        public void entityMemento_chacheChange_value_rejectCallback_on_disposed_object_should_raise_ObjectDisposedException()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );
            target.Dispose();

            target.InvokeCacheChange( "Foo", cv => { } );
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_without_service_should_not_be_called()
        {
            var target = this.CreateTestableEntityMock();

            var change = target.InvokeCacheChange( "Foo", cv => { } );

            change.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_with_suspended_service_should_not_be_called()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { } );

            change.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_should_be_called()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { }, cv => { } );

            change.Should().Not.Be.Null();
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_should_be_called_with_expected_values()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { }, cv => { } );

            change.IsCommitSupported.Should().Be.True();
            change.Owner.Should().Be.EqualTo( target );
            change.Description.Should().Be.EqualTo( String.Empty );
        }

        [TestMethod]
        [ExpectedException( typeof( ObjectDisposedException ) )]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_on_disposed_object_should_raise_ObjectDisposedException()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );
            target.Dispose();

            target.InvokeCacheChange( "Foo", cv => { }, cv => { } );
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_without_service_should_not_be_called()
        {
            var target = this.CreateTestableEntityMock();

            var change = target.InvokeCacheChange( "Foo", cv => { }, cv => { } );

            change.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_with_suspended_service_should_not_be_called()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { }, cv => { } );

            change.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_should_be_called()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { }, cv => { }, AddChangeBehavior.Default );

            change.Should().Not.Be.Null();
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_should_be_called_with_expected_values()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { }, cv => { }, AddChangeBehavior.Default );

            change.IsCommitSupported.Should().Be.True();
            change.Owner.Should().Be.EqualTo( target );
            change.Description.Should().Be.EqualTo( String.Empty );
        }

        [TestMethod]
        [ExpectedException( typeof( ObjectDisposedException ) )]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_on_disposed_object_should_raise_ObjectDisposedException()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock( memento );
            target.Dispose();

            target.InvokeCacheChange( "Foo", cv => { }, cv => { }, AddChangeBehavior.Default );
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_without_service_should_not_be_called()
        {
            var target = this.CreateTestableEntityMock();

            var change = target.InvokeCacheChange( "Foo", cv => { }, cv => { }, AddChangeBehavior.Default );

            change.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_with_suspended_service_should_not_be_called()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock( memento );

            var change = target.InvokeCacheChange( "Foo", cv => { }, cv => { }, AddChangeBehavior.Default );

            change.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_Undo_should_return_valid_iChange()
        {
            var iChange = MockRepository.GenerateStub<IChange>();
            var memento = MockRepository.GenerateStub<IChangeTrackingService>();
            memento.Stub( obj => obj.IsSuspended ).Return( false );

            var target = this.CreateTestableEntityMock( memento );

            var value = "foo";
            var rejArgs = new ChangeRejectedEventArgs<String>( target, value, iChange, RejectReason.Undo );

            var actual = target.InvokeCacheChangeOnRejectCallback( value, cv => { }, cv => { }, rejArgs );

            actual.Should().Not.Be.Null();
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_Redo_should_return_valid_iChange()
        {
            var iChange = MockRepository.GenerateStub<IChange>();
            var memento = MockRepository.GenerateStub<IChangeTrackingService>();
            memento.Stub( obj => obj.IsSuspended ).Return( false );

            var target = this.CreateTestableEntityMock( memento );

            var value = "foo";
            var rejArgs = new ChangeRejectedEventArgs<String>( target, value, iChange, RejectReason.Redo );

            var actual = target.InvokeCacheChangeOnRejectCallback( value, cv => { }, cv => { }, rejArgs );

            actual.Should().Not.Be.Null();
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_Revert_should_return_null_iChange()
        {
            var iChange = MockRepository.GenerateStub<IChange>();
            var memento = MockRepository.GenerateStub<IChangeTrackingService>();
            memento.Stub( obj => obj.IsSuspended ).Return( false );

            var target = this.CreateTestableEntityMock( memento );

            var value = "foo";
            var rejArgs = new ChangeRejectedEventArgs<String>( target, value, iChange, RejectReason.Revert );

            var actual = target.InvokeCacheChangeOnRejectCallback( value, cv => { }, cv => { }, rejArgs );

            actual.Should().Be.Null();
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_RejectChanges_should_return_null_iChange()
        {
            var iChange = MockRepository.GenerateStub<IChange>();
            var memento = MockRepository.GenerateStub<IChangeTrackingService>();
            memento.Stub( obj => obj.IsSuspended ).Return( false );

            var target = this.CreateTestableEntityMock( memento );

            var value = "foo";
            var rejArgs = new ChangeRejectedEventArgs<String>( target, value, iChange, RejectReason.RejectChanges );

            var actual = target.InvokeCacheChangeOnRejectCallback( value, cv => { }, cv => { }, rejArgs );

            actual.Should().Be.Null();
        }

        [TestMethod]
        [ExpectedException( typeof( ObjectDisposedException ) )]
        public void entityMemento_chacheChangeOnRejectCallback_on_disposed_entity_should_raise_ObjectDisposedException()
        {
            var iChange = MockRepository.GenerateStub<IChange>();
            var memento = MockRepository.GenerateStub<IChangeTrackingService>();
            memento.Stub( obj => obj.IsSuspended ).Return( false );

            var target = this.CreateTestableEntityMock( memento );
            target.Dispose();

            var value = "foo";
            var rejArgs = new ChangeRejectedEventArgs<String>( target, value, iChange, RejectReason.Revert );

            target.InvokeCacheChangeOnRejectCallback( value, cv => { }, cv => { }, rejArgs );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_none_should_raise_ArgumentOutOfRangeException()
        {
            var iChange = MockRepository.GenerateStub<IChange>();
            var memento = MockRepository.GenerateStub<IChangeTrackingService>();
            memento.Stub( obj => obj.IsSuspended ).Return( false );

            var target = this.CreateTestableEntityMock( memento );

            var value = "foo";
            var rejArgs = new ChangeRejectedEventArgs<String>( target, value, iChange, RejectReason.None );

            target.InvokeCacheChangeOnRejectCallback( value, cv => { }, cv => { }, rejArgs );
        }

        [TestMethod]
        [ExpectedException( typeof( EnumValueOutOfRangeException ) )]
        public void entityMemento_cacheChangeOnRejectCallback_invalid_rejectReason_should_raise_EnumValueOutOfRangeException()
        {
            var iChange = MockRepository.GenerateStub<IChange>();
            var memento = MockRepository.GenerateStub<IChangeTrackingService>();
            memento.Stub( obj => obj.IsSuspended ).Return( false );

            var target = this.CreateTestableEntityMock( memento );

            var value = "foo";
            var rejArgs = new ChangeRejectedEventArgs<String>( target, value, iChange, ( RejectReason )1000 );

            target.InvokeCacheChangeOnRejectCallback( value, cv => { }, cv => { }, rejArgs );
        }
    }
}
