//extern alias tpx;

namespace Radical.Tests.ChangeTracking
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpTestsEx;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.ChangeTracking;
    using Radical.Linq;

    [TestClass]
    public class CollectionChangeTrackingServiceTests
    {
        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_Add_canUndo_is_true()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( svc, false ) );

            svc.CanUndo.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_Undo_collection_is_rolledback()
        {
            Int32 expected = 0;

            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( svc, false ) );

            svc.Undo();

            p.Count.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_Add_getChangeSet_contains_change()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( svc, false ) );
            p.Add( new Person( svc, false ) );

            IChangeSet cSet = svc.GetChangeSet();

            cSet.Count.Should().Be.EqualTo( 2 );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_add_GetEntityState_is_Changed()
        {
            EntityTrackingStates expected = EntityTrackingStates.HasBackwardChanges;

            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( svc, false ) );
            p.Add( new Person( svc, false ) );

            EntityTrackingStates actual = svc.GetEntityState( p );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_mutual_exclusive_actions_service_CanUndo()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( svc, false ) );
            p.RemoveAt( 0 );

            Assert.IsTrue( svc.CanUndo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_mutual_exclusive_actions_entity_state_is_HasBackwardChanges()
        {
            EntityTrackingStates expected = EntityTrackingStates.HasBackwardChanges;

            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( svc, false ) );
            p.RemoveAt( 0 );

            EntityTrackingStates actual = svc.GetEntityState( p );
            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedEntity_service_canRedo_is_false()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedEntity_service_canUndo_is_false()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );

            Assert.IsFalse( svc.CanUndo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedEntity_service_isChanged_is_false()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedEntity_service_getEntityState_is_None()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();
            PersonCollection p = new PersonCollection( svc );

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void afterUndo_service_canRedo()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );
            svc.Undo();

            Assert.IsTrue( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_change_canRedo_is_false()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_multiple_change_canRedo_is_false()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );
            svc.Undo();
            p.Add( new Person( null, false ) );

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void afterUndo_service_Redo_restore_previous_value()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            Int32 expected = 1;

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.Undo();
            svc.Redo();

            Assert.AreEqual<Int32>( expected, p.Count );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_acceptChanges_entity_is_no_more_changed()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.AcceptChanges();

            EntityTrackingStates actual = svc.GetEntityState( p );
            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_acceptChanges_service_isChanged_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.AcceptChanges();

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_acceptChanges_service_canUndo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.AcceptChanges();

            Assert.IsFalse( svc.CanUndo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_acceptChanges_service_canRedo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.AcceptChanges();

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_rejectChanges_entity_is_no_more_changed()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.RejectChanges();

            EntityTrackingStates actual = svc.GetEntityState( p );
            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_rejectChanges_service_isChanged_is_false()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.RejectChanges();

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_rejectChanges_service_canUndo_is_false()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.RejectChanges();

            Assert.IsFalse( svc.CanUndo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_rejectChanges_service_canRedo_is_false()
        {
            IChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            svc.RejectChanges();

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_single_add_generate_valid_advisory()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );

            IEnumerable<IAdvisedAction> advisory = svc.GetAdvisory();

            Assert.IsNotNull( advisory );
            Assert.AreEqual<Int32>( 1, advisory.Count() );
            Assert.AreEqual<ProposedActions>( ProposedActions.Update, advisory.First().Action );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_single_add_generate_valid_advisory_even_with_transient_persistable_entity()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, ChangeTrackingRegistration.AsTransient, TransientRegistration.AsPersistable ) );

            IEnumerable<IAdvisedAction> advisory = svc.GetAdvisory();

            Assert.IsNotNull( advisory );
            Assert.AreEqual<Int32>( 1, advisory.Count() );
            Assert.AreEqual<ProposedActions>( ProposedActions.Create, advisory.First().Action );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_clear_generate_valid_advisory()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );
            p.Add( new Person( null, false ) );
            p.Add( new Person( null, false ) );
            p.Add( new Person( null, false ) );
            p.Add( new Person( null, false ) );

            svc.AcceptChanges();

            p.Clear();

            IEnumerable<IAdvisedAction> advisory = svc.GetAdvisory();

            Assert.IsNotNull( advisory );
            Assert.AreEqual<Int32>( 5, advisory.Count() );
            foreach( var aa in advisory )
            {
                Assert.AreEqual<ProposedActions>( ProposedActions.Delete, aa.Action );
            }
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getChangeSet_returns_valid_cSet()
        {
            Int32 expected = 3;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection p = new PersonCollection( svc );
            p.Add( new Person( null, false ) );
            p.Add( new Person( null, false ) );
            p.Move( 0, 1 );

            IChangeSet cSet = svc.GetChangeSet();
            actual = cSet.Count;

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_clear_undo_entities_are_restored()
        {
            var source = new Person[] 
            {
                new Person( null, false ),
                new Person( null, false ),
                new Person( null, false ),
                new Person( null, false ),
                new Person( null, false )
            };

            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection list = new PersonCollection( svc );
            list.BeginInit();
            list.AddRange( source );
            list.EndInit();

            list.Clear();
            svc.Undo();

            Assert.AreEqual<Int32>( source.Length, list.Count() );
            source.ForEach( p =>
            {
                Int32 expected = Array.IndexOf( source, p );
                Int32 actual = list.IndexOf( p );

                Assert.AreEqual<Int32>( expected, actual );
            } );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_undoes_are_in_the_correct_order()
        {
            var p1 = new Person( null, false );
            var p2 = new Person( null, false );
            var p3 = new Person( null, false );

            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection list = new PersonCollection( svc );
            list.Add( p1 );
            list.Add( p2 );
            list.Add( p3 );
            list.Move( p2, 0 );
            list.Remove( p1 );

            svc.Undo();
            Assert.AreEqual<Int32>( 3, list.Count );
            Assert.IsTrue( list.Contains( p1 ) );

            svc.Undo();
            Assert.AreEqual<Int32>( 1, list.IndexOf( p2 ) );

            svc.Undo();
            Assert.AreEqual<Int32>( 2, list.Count );
            Assert.IsFalse( list.Contains( p3 ) );

            svc.Undo();
            Assert.AreEqual<Int32>( 1, list.Count );
            Assert.IsFalse( list.Contains( p2 ) );

            svc.Undo();
            Assert.AreEqual<Int32>( 0, list.Count );
            Assert.IsFalse( list.Contains( p1 ) );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_redoes_are_in_the_correct_order()
        {
            var p1 = new Person( null, false );
            var p2 = new Person( null, false );
            var p3 = new Person( null, false );

            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection list = new PersonCollection( svc );
            list.Add( p1 );
            list.Add( p2 );
            list.Add( p3 );
            list.Move( p2, 0 );
            list.Remove( p1 );

            while( svc.CanUndo )
            {
                svc.Undo();
            }

            svc.Redo();
            Assert.AreEqual<Int32>( 1, list.Count );
            Assert.IsTrue( list.Contains( p1 ) );

            svc.Redo();
            Assert.AreEqual<Int32>( 2, list.Count );
            Assert.IsTrue( list.Contains( p2 ) );

            svc.Redo();
            Assert.AreEqual<Int32>( 3, list.Count );
            Assert.IsTrue( list.Contains( p3 ) );

            svc.Redo();
            Assert.AreEqual<Int32>( 0, list.IndexOf( p2 ) );

            svc.Redo();
            Assert.AreEqual<Int32>( 2, list.Count );
            Assert.IsFalse( list.Contains( p1 ) );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getAdvisory_generate_valid_advisory_with_more_changes_applied_to_the_same_entity()
        {
            ProposedActions expected = ProposedActions.Delete;
            ChangeTrackingService svc = new ChangeTrackingService();

            PersonCollection list = new PersonCollection( svc );
            list.Add( new Person( null, false ) );    //First IChange
            list.RemoveAt( 0 );                        //Second IChange

            IAdvisory advisory = svc.GetAdvisory();
            IAdvisedAction action = advisory.First();
            ProposedActions actual = action.Action;

            Assert.AreEqual<ProposedActions>( expected, actual );
        }
    }
}