namespace Radical.Tests.ChangeTracking
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.ChangeTracking;
    using SharpTestsEx;

    [TestClass]
    public class AdvisoryBuilderTests
    {
        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_ctor()
        {
            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_ctor_null_visitor()
        {
            var actual = new AdvisoryBuilder( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_null_service_reference()
        {
            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            actual.GenerateAdvisory( null, null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_null_changeSet_reference()
        {
            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            actual.GenerateAdvisory( new ChangeTrackingService(), null );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_for_transient_and_changed_entity()
        {
            var expectedAction = ProposedActions.Create;
            var entity = new Object();
            var entityState = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove | EntityTrackingStates.HasBackwardChanges;

            var c1 = MockRepository.GenerateStub<IChange>();
            c1.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c1.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.Update | ProposedActions.Create );
            c1.Replay();

            var c2 = MockRepository.GenerateStub<IChange>();
            c2.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c2.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.Update | ProposedActions.Create );
            c2.Replay();

            var cSet = new ChangeSet( new IChange[] { c1, c2 } );

            var svc = MockRepository.GenerateStub<IChangeTrackingService>();
            svc.Expect( obj => obj.GetEntityState( entity ) ).Return( entityState );
            svc.Expect( obj => obj.GetEntities( EntityTrackingStates.IsTransient, true ) ).Return( new Object[ 0 ] );
            svc.Replay();

            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            IAdvisory advisory = actual.GenerateAdvisory( svc, cSet );

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo( 1 );
            advisory.First().Action.Should().Be.EqualTo( expectedAction );
            advisory.First().Target.Should().Be.EqualTo( entity );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_for_non_transient_and_changed_entity()
        {
            var expectedAction = ProposedActions.Update;
            var entity = new Object();
            var entityState = EntityTrackingStates.HasBackwardChanges;

            var c1 = MockRepository.GenerateStub<IChange>();
            c1.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c1.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.Update | ProposedActions.Create );
            c1.Replay();

            var c2 = MockRepository.GenerateStub<IChange>();
            c2.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c2.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.Update | ProposedActions.Create );
            c2.Replay();

            var cSet = new ChangeSet( new IChange[] { c1, c2 } );

            var svc = MockRepository.GenerateStub<IChangeTrackingService>();
            svc.Expect( obj => obj.GetEntityState( entity ) ).Return( entityState );
            svc.Expect( obj => obj.GetEntities( EntityTrackingStates.IsTransient, true ) ).Return( new Object[ 0 ] );
            svc.Replay();

            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            IAdvisory advisory = actual.GenerateAdvisory( svc, cSet );

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo( 1 );
            advisory.First().Action.Should().Be.EqualTo( expectedAction );
            advisory.First().Target.Should().Be.EqualTo( entity );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_for_strict_isTransient_entity()
        {
            var expectedAction = ProposedActions.Create;
            var entity = new Object();
            var entityState = EntityTrackingStates.IsTransient;

            var cSet = new ChangeSet( new IChange[ 0 ] );

            var svc = MockRepository.GenerateStub<IChangeTrackingService>();
            svc.Expect( obj => obj.GetEntityState( entity ) ).Return( entityState );
            svc.Expect( obj => obj.GetEntities( EntityTrackingStates.IsTransient, true ) ).Return( new Object[] { entity } );
            svc.Replay();

            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            IAdvisory advisory = actual.GenerateAdvisory( svc, cSet );

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo( 1 );
            advisory.First().Action.Should().Be.EqualTo( expectedAction );
            advisory.First().Target.Should().Be.EqualTo( entity );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_for_isTransient_and_autoRemove_entity_with_no_changes_should_generate_an_empty_advisory()
        {
            var entity = new Object();
            var entityState = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;

            var cSet = new ChangeSet( new IChange[ 0 ] );

            var svc = MockRepository.GenerateStub<IChangeTrackingService>();
            svc.Expect( obj => obj.GetEntityState( entity ) ).Return( entityState );
            svc.Expect( obj => obj.GetEntities( EntityTrackingStates.IsTransient, true ) ).Return( new Object[ 0 ] );
            svc.Replay();

            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            IAdvisory advisory = actual.GenerateAdvisory( svc, cSet );

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo( 0 );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_for_transient_and_disposed_entity()
        {
            var expectedAction = ProposedActions.Dispose;
            var entity = new Object();
            var entityState = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove | EntityTrackingStates.HasBackwardChanges;

            var c1 = MockRepository.GenerateStub<IChange>();
            c1.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c1.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.Create | ProposedActions.Update );
            c1.Replay();

            var c2 = MockRepository.GenerateStub<IChange>();
            c2.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c2.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.Delete | ProposedActions.Dispose );
            c2.Replay();

            var cSet = new ChangeSet( new IChange[] { c1, c2 } );

            var svc = MockRepository.GenerateStub<IChangeTrackingService>();
            svc.Expect( obj => obj.GetEntityState( entity ) ).Return( entityState );
            svc.Expect( obj => obj.GetEntities( EntityTrackingStates.IsTransient, true ) ).Return( new Object[ 0 ] );
            svc.Replay();

            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            IAdvisory advisory = actual.GenerateAdvisory( svc, cSet );

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo( 1 );
            advisory.First().Action.Should().Be.EqualTo( expectedAction );
            advisory.First().Target.Should().Be.EqualTo( entity );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_for_non_transient_and_deleted_entity()
        {
            var expectedAction = ProposedActions.Delete;
            var entity = new Object();
            var entityState = EntityTrackingStates.HasBackwardChanges;

            var c1 = MockRepository.GenerateStub<IChange>();
            c1.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c1.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.Create | ProposedActions.Update );
            c1.Replay();

            var c2 = MockRepository.GenerateStub<IChange>();
            c2.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c2.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.Delete | ProposedActions.Dispose );
            c2.Replay();

            var cSet = new ChangeSet( new IChange[] { c1, c2 } );

            var svc = MockRepository.GenerateStub<IChangeTrackingService>();
            svc.Expect( obj => obj.GetEntityState( entity ) ).Return( entityState );
            svc.Expect( obj => obj.GetEntities( EntityTrackingStates.IsTransient, true ) ).Return( new Object[ 0 ] );
            svc.Replay();

            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            IAdvisory advisory = actual.GenerateAdvisory( svc, cSet );

            advisory.Should().Not.Be.Null();
            advisory.Count.Should().Be.EqualTo( 1 );
            advisory.First().Action.Should().Be.EqualTo( expectedAction );
            advisory.First().Target.Should().Be.EqualTo( entity );
        }

        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        [TestCategory( "ChangeTracking" )]
        public void advisoryBuilder_generateAdvisory_unsupported_proposedActions_value()
        {
            var entity = new Object();
            var entityState = EntityTrackingStates.HasBackwardChanges;

            var c1 = MockRepository.GenerateStub<IChange>();
            c1.Expect( obj => obj.GetChangedEntities() ).Return( new Object[] { entity } );
            c1.Expect( obj => obj.GetAdvisedAction( entity ) ).Return( ProposedActions.None );
            c1.Replay();

            var cSet = new ChangeSet( new IChange[] { c1 } );

            var svc = MockRepository.GenerateStub<IChangeTrackingService>();
            svc.Expect( obj => obj.GetEntityState( entity ) ).Return( entityState );
            svc.Expect( obj => obj.GetEntities( EntityTrackingStates.IsTransient, true ) ).Return( new Object[ 0 ] );
            svc.Replay();

            var actual = new AdvisoryBuilder( new ChangeSetDistinctVisitor() );
            IAdvisory advisory = actual.GenerateAdvisory( svc, cSet );
        }
    }
}