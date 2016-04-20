﻿namespace Test.Radical.ChangeTracking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using SharpTestsEx;
    using Topics.Radical.ComponentModel.ChangeTracking;
    using Topics.Radical.ChangeTracking;
    using Topics.Radical;
    using Topics.Radical.ChangeTracking.Specialized;

    [TestClass]
    public class EntityChangeTrackingServiceTests
    {
        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void afterPropertyChange_canUndo_is_true()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";

            Assert.IsTrue( svc.CanUndo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_ctor_normal_hasTransientEntities_should_be_false()
        {
            var svc = new ChangeTrackingService();
            svc.HasTransientEntities.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_hasTransientEntities_should_be_true_with_transient_entities()
        {
            var svc = new ChangeTrackingService();

            var p = new Person( svc, true );

            svc.HasTransientEntities.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_hasTransientEntities_should_be_false_after_rejectChanges()
        {
            var svc = new ChangeTrackingService();
            var p = new Person( svc, true );
            svc.RejectChanges();

            svc.HasTransientEntities.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_hasTransientEntities_should_be_false_after_acceptChanges()
        {
            var svc = new ChangeTrackingService();
            var p = new Person( svc, true );
            svc.AcceptChanges();

            svc.HasTransientEntities.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_hasTransientEntities_should_be_true_after_undo()
        {
            var svc = new ChangeTrackingService();
            var p = new Person( svc, true );
            svc.Undo();

            svc.HasTransientEntities.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void afterUndo_propertyValue_is_rolledback()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            String expected = p.Name;
            p.Name = "Mauro";

            svc.Undo();

            Assert.AreEqual<String>( expected, p.Name );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void afterPropertyChanges_getChangeSet_contains_change()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";
            p.Name = "Andrea";

            IChangeSet cSet = svc.GetChangeSet();

            Assert.AreEqual<Int32>( 2, cSet.Count );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void afterPropertyChanges_GetEntityState_is_Changed()
        {
            EntityTrackingStates expected = EntityTrackingStates.HasBackwardChanges;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void tansientEntity_afterPropertyChanges_GetEntityState_IsTransient_AutoRemove_HasBackwardChanges()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove | EntityTrackingStates.HasBackwardChanges;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void tansientEntity_GetEntityState_is_Transient_and_AutoRemove()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_getEntityState_using_entity_not_managed_by_any_service_should_return_None()
        {
            var expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( null );
            var actual = svc.GetEntityState( p );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_getEntityState_using_entity_managed_by_different_service_should_return_None()
        {
            var expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( new ChangeTrackingService(), true );
            var actual = svc.GetEntityState( p );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedEntity_service_canNotUndo()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );

            Assert.IsFalse( svc.CanUndo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedEntity_service_canRedo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedEntity_service_isChanged_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedTransientEntity_service_isChanged_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedTransientEntity_service_getTransientEntities_contains_entity()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );

            var list = svc.GetEntities( EntityTrackingStates.IsTransient, false );
            var persons = list.OfType<Person>();

            Assert.IsTrue( persons.Contains( p ) );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void unchangedTransientEntity_service_RejectChanges_entity_is_not_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );

            svc.RejectChanges();

            var list = svc.GetEntities( EntityTrackingStates.IsTransient, false );
            var persons = list.OfType<Person>();

            Assert.IsFalse( persons.Contains( p ) );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void afterUndo_service_canRedo()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";

            svc.Undo();

            Assert.IsTrue( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_change_canRedo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_multiple_change_canRedo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";
            svc.Undo();
            p.Name = "Foo";

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void afterUndo_service_Redo_restore_previous_value()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            String expected = "Mauro";

            Person p = new Person( svc, false );
            p.Name = expected;

            svc.Undo();
            svc.Redo();

            Assert.AreEqual<String>( expected, p.Name );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_explicit_registerAsTransient_entityTrackingStates_is_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;

            Person p = new Person( svc, false );
            svc.RegisterTransient( p );
            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_explicit_registerAsTransient_with_autoRemove_true_entityTrackingStates_is_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;

            Person p = new Person( svc, false );
            svc.RegisterTransient( p, true );
            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_explicit_registerAsTransient_with_autoRemove_false_entityTrackingStates_is_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.IsTransient;

            Person p = new Person( svc, false );
            svc.RegisterTransient( p, false );
            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        [TestCategory( "ChangeTracking" )]
        public void explicit_registerAsTransient_throws_exception_if_already_registered()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            svc.RegisterTransient( p );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void entity_registeredAsTransient_with_autoRemove_on_Undo_is_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;

            Person p = new Person( svc );
            svc.Undo();

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void entity_registeredAsTransient_with_autoRemove_on_RejectChanges_is_no_more_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.None;

            Person p = new Person( svc );
            svc.RejectChanges();

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        [ExpectedException( typeof( ArgumentException ) )]
        public void explicit_registerAsTransient_without_autoRemove_throws_exception_if_already_registered()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            svc.RegisterTransient( p, false );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void entity_registeredAsTransient_with_autoRemove_on_AcceptChanges_is_no_more_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.None;

            Person p = new Person( svc );
            svc.AcceptChanges();

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void entity_registeredAsTransient_without_autoRemove_on_Undo_is_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.IsTransient;

            Person p = new Person( svc, false );
            svc.RegisterTransient( p, false );
            svc.Undo();

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void entity_registeredAsTransient_without_autoRemove_on_RejectChanges_is_no_more_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.None;

            Person p = new Person( svc, false );
            svc.RegisterTransient( p, false );
            svc.RejectChanges();

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void entity_registeredAsTransient_without_autoRemove_on_AcceptChanges_is_no_more_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.None;

            Person p = new Person( svc, false );
            svc.RegisterTransient( p, false );
            svc.AcceptChanges();

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void if_entity_is_iComponent_on_Disposed_entity_is_removed_from_transient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.None;

            Person p = null;
            using( p = new Person( svc, false ) )
            {
                svc.RegisterTransient( p, false );
            }

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void if_entity_is_iComponent_on_Disposed_entity_changes_are_removed_changes_stack()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            Boolean expected = false;

            Person p = null;
            using( p = new Person( svc, false ) )
            {
                p.Name = "Foo";
                p.Name = "Mauro";
            }

            Boolean actual = svc.IsChanged;

            Assert.AreEqual<Boolean>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_acceptChanges_entity_is_no_more_changed()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";

            svc.AcceptChanges();

            EntityTrackingStates actual = svc.GetEntityState( p );
            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_acceptChanges_service_isChanged_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            svc.AcceptChanges();

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_acceptChanges_service_canUndo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            svc.AcceptChanges();

            Assert.IsFalse( svc.CanUndo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_acceptChanges_service_canRedo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            svc.AcceptChanges();

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_rejectChanges_entity_is_no_more_changed()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.RejectChanges();

            EntityTrackingStates actual = svc.GetEntityState( p );
            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_rejectChanges_service_isChanged_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            svc.RejectChanges();

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_rejectChanges_service_canUndo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            svc.RejectChanges();

            Assert.IsFalse( svc.CanUndo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_rejectChanges_service_canRedo_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            svc.RejectChanges();

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_no_more_changes_can_be_added()
        {
            RejectCallback<String> cb = cv => { };
            Object fakeOwner = new Object();
            String change = String.Empty;
            PropertyValueChange<String> stub = new PropertyValueChange<string>( fakeOwner, "property-name", change, cb );

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Suspend();

            svc.Add( stub, AddChangeBehavior.RedoRequest );
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_cannot_call_RegisterTransient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            svc.Suspend();
            svc.RegisterTransient( p );
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_cannot_call_RegisterTransient_with_explicit_autoRemove_true()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            svc.Suspend();
            svc.RegisterTransient( p, true );
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_cannot_call_RegisterTransient_with_explicit_autoRemove_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            svc.Suspend();
            svc.RegisterTransient( p, false );
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_cannot_call_Undo()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.Suspend();
            svc.Undo();
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_cannot_call_Redo()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            svc.Undo();

            svc.Suspend();
            svc.Redo();
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_cannot_call_AcceptChanges()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.Suspend();
            svc.AcceptChanges();
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_cannot_call_RejectChanges()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.Suspend();
            svc.RejectChanges();
        }

        [TestMethod]
        [ExpectedException( typeof( SuspendedChangeTrackingServiceException ) )]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_cannot_call_UnregisterTransient()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );

            svc.Suspend();
            svc.UnregisterTransient( p );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_resume_service_restart_tracking_changes()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );

            svc.Suspend();
            svc.Resume();

            p.Name = "Mauro";

            Assert.IsTrue( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_suspend_isSuspended_is_true()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Suspend();

            Assert.IsTrue( svc.IsSuspended );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void after_resume_isSuspended_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Suspend();
            svc.Resume();

            Assert.IsFalse( svc.IsSuspended );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_initial_state_isSuspended_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            Assert.IsFalse( svc.IsSuspended );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_initial_state_has_no_transient_entities()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            IEnumerable entities = svc.GetEntities( EntityTrackingStates.IsTransient, false );

            Assert.AreEqual<Int32>( 0, entities.OfType<Object>().Count() );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_initial_state_isChanged_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_on_null_iChange_add_argumentNullException()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( null, AddChangeBehavior.RedoRequest );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_after_add_isChanged_is_true()
        {
            RejectCallback<String> cb = cv => { };
            Object fakeOwner = new Object();
            String change = String.Empty;
            PropertyValueChange<String> stub = new PropertyValueChange<String>( fakeOwner, "property-name", change, cb );

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( stub, AddChangeBehavior.RedoRequest );

            Assert.IsTrue( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_after_add_getChangeSet_return_valid_cSet()
        {
            RejectCallback<String> cb = cv => { };
            Object fakeOwner = new Object();
            String change = String.Empty;
            IChange expected = new PropertyValueChange<String>( fakeOwner, "property-name", change, cb );

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( expected, AddChangeBehavior.RedoRequest );

            IChangeSet cSet = svc.GetChangeSet();

            Assert.AreEqual<IChange>( expected, cSet.First() );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_add_notSupported_AddChangeBehavior()
        {
            IChange stub = MockRepository.GenerateStub<IChange>();

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( stub, AddChangeBehavior.None );
        }

        [TestMethod]
        [ExpectedException( typeof( EnumValueOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_add_invalid_AddChangeBehavior()
        {
            IChange stub = MockRepository.GenerateStub<IChange>();

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( stub, ( AddChangeBehavior )1000 );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_create_non_null_bookmark()
        {
            RejectCallback<String> cb = cv => { };
            Object fakeOwner = new Object();
            String change = String.Empty;
            IChange expected = new PropertyValueChange<String>( fakeOwner, "property-name", change, cb );

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( expected, AddChangeBehavior.RedoRequest );

            var bmk = svc.CreateBookmark();

            Assert.IsNotNull( bmk );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_with_no_changes_create_non_null_bookmark()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            var bmk = svc.CreateBookmark();

            Assert.IsNotNull( bmk );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_cannot_validate_a_null_bookmark()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Validate( null );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_validate_a_valid_bookmark_is_true()
        {
            RejectCallback<String> cb = cv => { };
            Object fakeOwner = new Object();
            String change = String.Empty;
            IChange expected = new PropertyValueChange<String>( fakeOwner, "property-name", change, cb );

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( expected, AddChangeBehavior.RedoRequest );

            var bmk = svc.CreateBookmark();

            Assert.IsTrue( svc.Validate( bmk ) );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_validate_a_bookmark_owned_by_another_service_is_false()
        {
            ChangeTrackingService svc1 = new ChangeTrackingService();
            var bmk = svc1.CreateBookmark();

            ChangeTrackingService svc2 = new ChangeTrackingService();

            Assert.IsFalse( svc2.Validate( bmk ) );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_validate_a_bookmark_invalid_is_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";

            var bmk = svc.CreateBookmark();
            svc.Undo();

            Assert.IsFalse( svc.Validate( bmk ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_revert_with_a_null_bookmark()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Revert( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_revert_to_a_bookmark_created_by_another_service()
        {
            ChangeTrackingService svc1 = new ChangeTrackingService();
            IBookmark bmk = svc1.CreateBookmark();

            ChangeTrackingService svc2 = new ChangeTrackingService();
            svc2.Revert( bmk );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_successfully_revert_to_a_valid_bookmark()
        {
            String expected = "Mauro";

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = expected;
            IBookmark bmk = svc.CreateBookmark();

            p.Name = "foo..";
            p.Name = "foo once again...";

            svc.Revert( bmk );

            Assert.AreEqual<String>( expected, p.Name );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_successfully_revert_to_a_valid_bookmark_created_at_initial_state()
        {
            String expected = String.Empty;

            ChangeTrackingService svc = new ChangeTrackingService();
            IBookmark bmk = svc.CreateBookmark();

            Person p = new Person( svc );
            p.Name = "foo..";
            p.Name = "foo once again...";

            svc.Revert( bmk );

            Assert.AreEqual<String>( expected, p.Name );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_successfully_revert_to_a_valid_bookmark_created_at_initial_state_and_entity_is_no_more_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();
            IBookmark bmk = svc.CreateBookmark();

            Person p = new Person( svc );
            p.Name = "foo..";
            p.Name = "foo once again...";

            svc.Revert( bmk );

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_cannot_revert_to_an_invalid_bookmark()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";

            var bmk = svc.CreateBookmark();
            svc.Undo();
            svc.Revert( bmk );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_generate_valid_advisory()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";

            IEnumerable<IAdvisedAction> advisory = svc.GetAdvisory();

            Assert.IsNotNull( advisory );
            Assert.AreEqual<Int32>( 1, advisory.Count() );
            Assert.AreEqual<ProposedActions>( ProposedActions.Create, advisory.First().Action );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_should_generate_advisory_with_update_action()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";

            IEnumerable<IAdvisedAction> advisory = svc.GetAdvisory();

            Assert.AreEqual<ProposedActions>( ProposedActions.Update, advisory.First().Action );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_with_no_changes_generate_empty_advisory()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            IEnumerable<IAdvisedAction> advisory = svc.GetAdvisory();

            Assert.IsNotNull( advisory );
            Assert.AreEqual<Int32>( 0, advisory.Count() );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_generate_valid_adivsory_with_valid_iAdvisoryBuilder()
        {
            IAdvisoryBuilder mock = MockRepository.GenerateMock<IAdvisoryBuilder>();
            mock.Expect( b => b.GenerateAdvisory( null, null ) )
                .IgnoreArguments()
                .Repeat.Once()
                .Return( new Advisory( new IAdvisedAction[ 0 ] ) );

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";

            IEnumerable<IAdvisedAction> advisory = svc.GetAdvisory( mock );

            mock.VerifyAllExpectations();
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_getAdvisory_with_null_iAdvisoryBuilder_argumentNull_Exception()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.GetAdvisory( null );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getChangeSet_when_is_not_changed_empty_cSet()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            IChangeSet cSet = svc.GetChangeSet();

            Assert.IsNotNull( cSet );
            Assert.AreEqual<Int32>( 0, cSet.Count );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_getChangeSet_with_null_iChangeSetFilter_argumentNullExeption()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.GetChangeSet( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_getChangeSet_with_null_iChangeSetFilter()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.GetChangeSet( null );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getChangeSet_with_valid_iChangeSetFilter()
        {
            Int32 expected = 2;
            Int32 actual = 0;

            IChangeSetFilter mock = MockRepository.GenerateMock<IChangeSetFilter>();
            mock.Expect( f => f.ShouldInclude( null ) )
                .IgnoreArguments()
                .Repeat.Times( 2 )
                .Return( true );

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, true );
            p.Name = "Mauro";
            p.Name = "Mauro";
            p.Name = "Foo";

            IChangeSet cSet = svc.GetChangeSet( mock );
            actual = cSet.Count;

            mock.VerifyAllExpectations();
            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getChangeSet_returns_valid_cSet()
        {
            Int32 expected = 2;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, true );
            p.Name = "Mauro";
            p.Name = "Mauro";
            p.Name = "Foo";

            IChangeSet cSet = svc.GetChangeSet();
            actual = cSet.Count;

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getEntities_state_None_exactMatch_false_gets_all_tracked_entities()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, true );
            p.Name = "Mauro";
            p.Name = "Mauro";
            p.Name = "Foo";

            IEnumerable<Object> entities = svc.GetEntities( EntityTrackingStates.None, false );

            Assert.AreEqual<Int32>( 1, entities.Count() );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getEntities_gets_all_tracked_entities()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p1 = new Person( svc, true );
            p1.Name = "Mauro";
            p1.Name = "Mauro";
            p1.Name = "Foo";

            Person p2 = new Person( svc, true );

            IEnumerable<Object> entities = svc.GetEntities();

            Assert.AreEqual<Int32>( 2, entities.Count() );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getEntities_state_Changed_exactMatch_true_entity_in_Changed_state()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";
            p.Name = "Mauro";
            p.Name = "Foo";

            IEnumerable<Object> entities = svc.GetEntities( EntityTrackingStates.HasBackwardChanges, true );

            Assert.AreEqual<Int32>( 1, entities.Count() );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getEntities_state_Changed_exactMatch_false_entity_in_Changed_state()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc, false );
            p.Name = "Mauro";
            p.Name = "Mauro";
            p.Name = "Foo";

            IEnumerable<Object> entities = svc.GetEntities( EntityTrackingStates.HasBackwardChanges, false );

            Assert.AreEqual<Int32>( 1, entities.Count() );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getEntities_state_Changed_exactMatch_true_entity_in_Changed_and_Transient_state()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            p.Name = "Mauro";
            p.Name = "Foo";

            IEnumerable<Object> entities = svc.GetEntities( EntityTrackingStates.HasBackwardChanges, true );

            Assert.AreEqual<Int32>( 0, entities.Count() );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getEntities_state_Changed_exactMatch_false_entity_in_Changed_and_Transient_state()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            p.Name = "Mauro";
            p.Name = "Foo";

            IEnumerable<Object> entities = svc.GetEntities( EntityTrackingStates.HasBackwardChanges, false );

            Assert.AreEqual<Int32>( 1, entities.Count() );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_getEntities_state_Transient_exactMatch_false_entity_in_Changed_and_Transient_state()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Mauro";
            p.Name = "Mauro";
            p.Name = "Foo";

            IEnumerable<Object> entities = svc.GetEntities( EntityTrackingStates.IsTransient, false );

            Assert.AreEqual<Int32>( 1, entities.Count() );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_registerTransient_null_reference_argumentNullException()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.RegisterTransient( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_registerTransient_null_reference_explicit_autoRemove_true_argumentNullException()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.RegisterTransient( null, true );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_registerTransient_null_reference_explicit_autoRemove_false_argumentNullException()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.RegisterTransient( null, false );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_double_registerTransient_with_same_reference_invalidOperationException()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            svc.RegisterTransient( p );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_double_registerTransient_with_same_reference_and_explicit_autoRemove_true_invalidOperationException()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            svc.RegisterTransient( p, true );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_double_registerTransient_with_same_reference_and_explicit_autoRemove_false_invalidOperationException()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            svc.RegisterTransient( p, false );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_registerTransient_valid_reference_and_explicit_autoRemove_true()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc, false );
            svc.RegisterTransient( p, true );

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_registerTransient_valid_reference_and_explicit_autoRemove_false()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc, false );
            svc.RegisterTransient( p, false );

            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_unregisterTransient_null_reference_argumentNullException()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            svc.UnregisterTransient( null );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_unregisterTransient_valid_reference()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            EntityTrackingStates expected = EntityTrackingStates.None;

            Person p = new Person( svc, true );
            svc.UnregisterTransient( p );
            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_unregisterTransient_reference_not_registered_as_transient()
        {
            Person p = new Person( null );

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.UnregisterTransient( p );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_add_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 3;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };

            Person p = new Person( svc );    //raise
            p.Name = "Mau";                    //raise
            p.Name = "Mau";                    //no changes, no raise
            p.Name = "Mauro";                //raise

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_registerTransient_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };

            Person p = new Person( svc );

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_registerTransient_with_explicit_autoRemove_false_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };

            Person p = new Person( svc, false );
            svc.RegisterTransient( p, false );

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_registerTransient_with_explicit_autoRemove_true_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };

            Person p = new Person( svc, false );
            svc.RegisterTransient( p, true );

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_unregisterTransient_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.UnregisterTransient( p );

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_with_only_transient_entities_on_acceptChanges_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.AcceptChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_with_only_changes_on_acceptChanges_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc, false );
            p.Name = "Mauro";

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.AcceptChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_with_changes_and_transientEntities_on_acceptChanges_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.AcceptChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_acceptChanges_with_nothing_to_do_not_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 0;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc, false );

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.AcceptChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_with_only_transient_entities_on_rejectChanges_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.RejectChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_with_only_changes_on_rejectChanges_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc, false );
            p.Name = "Mauro";

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.RejectChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_with_changes_and_transientEntities_on_rejectChanges_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.RejectChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_rejectChanges_raise_trackingServiceStateChanged_event_once_even_if_there_are_more_changes()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            p.Name = "M";
            p.Name = "Ma";
            p.Name = "Mau";
            p.Name = "Maur";
            p.Name = "Mauro";

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.RejectChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_rejectChanges_with_nothing_to_do_not_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 0;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc, false );

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.RejectChanges();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_undo_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 2; //1: UndoRequest change, 2: Undo
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.Undo();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_undo_of_last_change_entity_if_not_tansient_has_only_forward_changes()
        {
            EntityTrackingStates expected = EntityTrackingStates.HasForwardChanges;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc, false );
            p.Name = "Mauro";

            svc.Undo();
            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_undo_of_last_change_entity_if_tansient_has_forward_changes_and_is_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove | EntityTrackingStates.HasForwardChanges;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.Undo();
            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_undo_of_last_change_isChanged_false()
        {
            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            p.Name = "Mauro";

            svc.Undo();

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_undo_with_nothing_to_do_not_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 0;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.Undo();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_redo_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 2; //1: Store actual value, 2: Redo
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );
            p.Name = "Mauro";
            svc.Undo();

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.Redo();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_redo_with_nothing_to_do_not_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 0;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();
            Person p = new Person( svc );

            svc.TrackingServiceStateChanged += ( sender, args ) => { actual++; };
            svc.Redo();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_revert_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            var bmk = svc.CreateBookmark();
            p.Name = "Mauro";

            svc.TrackingServiceStateChanged += ( s, e ) => { actual++; };
            svc.Revert( bmk );

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_revert_with_nothing_to_do_not_raise_trackingServiceStateChanged_event()
        {
            Int32 expected = 0;
            Int32 actual = 0;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            var bmk = svc.CreateBookmark();

            svc.TrackingServiceStateChanged += ( s, e ) => { actual++; };
            svc.Revert( bmk );

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_isChanged_must_be_false_if_there_are_only_transientEntities_with_no_changes()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            new Person( svc );
            new Person( svc );
            new Person( svc );
            new Person( svc );

            Assert.IsFalse( svc.IsChanged );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_correcly_handle_multiple_undos_redos()
        {
            String expected = "Mauro";

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = expected;

            svc.Undo();
            svc.Redo();
            svc.Undo();
            svc.Redo();

            Assert.AreEqual<String>( expected, p.Name );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_after_redo_CanRedo_is_false()
        {
            String expected = "Mauro";

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = expected;

            svc.Undo();
            svc.Redo();

            Assert.IsFalse( svc.CanRedo );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_stopTracking_removes_every_reference_to_entity()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;

            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Foo";

            svc.Detach( p );
            EntityTrackingStates actual = svc.GetEntityState( p );

            Assert.AreEqual<EntityTrackingStates>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_stopTracking_if_enetity_is_iMemento_removes_self()
        {
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Foo";

            svc.Detach( p );

            var actual = ( ( IMemento )p ).Memento;
            Assert.IsNull( actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_stopTracking_raise_trackingServiceStateChanged()
        {
            Int32 expected = 1;
            Int32 actual = 0;
            ChangeTrackingService svc = new ChangeTrackingService();

            Person p = new Person( svc );
            p.Name = "Foo";

            svc.TrackingServiceStateChanged += ( s, e ) => { actual++; };
            svc.Detach( p );

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_explicit_call_to_Dispose()
        {
            using( ChangeTrackingService svc = new ChangeTrackingService() )
            {

            }
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_explicit_call_to_Dispose_with_registered_iComponent()
        {
            using( ChangeTrackingService svc = new ChangeTrackingService() )
            {
                new Person( svc, true );
            }
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_after_trackingServiceStateChanged_unregister_event_is_not_raised()
        {
            Int32 expected = 0;
            Int32 actual = 0;

            EventHandler h = ( s, e ) => { actual++; };
            using( ChangeTrackingService svc = new ChangeTrackingService() )
            {
                svc.TrackingServiceStateChanged += h;
                svc.TrackingServiceStateChanged -= h;

                new Person( svc, true );
            }

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_set_and_get_iSite()
        {
            var expected = MockRepository.GenerateStub<ISite>();

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Site = expected;

            svc.Site.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_dispose_remove_itself_from_iSite_container()
        {
            var svc = new ChangeTrackingService();

            var iContainer = MockRepository.GenerateMock<IContainer>();
            iContainer.Expect( obj => obj.Remove( svc ) ).Repeat.Once();

            var iSite = MockRepository.GenerateStub<ISite>();
            iSite.Expect( obj => obj.Container ).Return( iContainer ).Repeat.Any();

            svc.Site = iSite;

            svc.Dispose();

            iContainer.VerifyAllExpectations();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_acceptChanges_successfully_commit_a_commitSupported_change()
        {
            var iChange = MockRepository.GenerateStub<IChange>();
            iChange.Expect( obj => obj.IsCommitSupported ).Return( true );
            iChange.Expect( obj => obj.Commit( CommitReason.AcceptChanges ) ).Repeat.Once();

            var svc = new ChangeTrackingService();
            svc.Add( iChange, AddChangeBehavior.Default );

            svc.AcceptChanges();

            iChange.VerifyAllExpectations();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_change_commit_remove_change_from_changesStack()
        {
            RejectCallback<String> rc = e => { };
            CommitCallback<String> cc = e => { };
            Object fakeOwner = new Object();
            String change = "Foo";

            var iChange = new PropertyValueChange<String>( fakeOwner, "property-name", change, rc, cc, "" );

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( iChange, AddChangeBehavior.Default );

            iChange.Commit( CommitReason.AcceptChanges );

            svc.IsChanged.Should().Be.False();
        }

        [TestMethod]
        [ExpectedException( typeof( EnumValueOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_on_change_commit_EnumValueOutOfRangeException_on_invalid_commit_reason_with_hacked_property_change()
        {
            RejectCallback<String> rc = e => { };
            CommitCallback<String> cc = e => { };
            Object fakeOwner = new Object();
            String change = "Foo";

            var iChange = new HackedPropertyValueChange( fakeOwner, change, rc, cc );
            iChange.HackedCommitReason = ( CommitReason )1000;

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( iChange, AddChangeBehavior.Default );

            iChange.Commit( CommitReason.AcceptChanges );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_on_change_commit_ArgumentOutOfRangeException_on_none_commit_reason_with_hacked_property_change()
        {
            RejectCallback<String> rc = e => { };
            CommitCallback<String> cc = e => { };
            Object fakeOwner = new Object();
            String change = "Foo";

            var iChange = new HackedPropertyValueChange( fakeOwner, change, rc, cc );
            iChange.HackedCommitReason = CommitReason.None;

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( iChange, AddChangeBehavior.Default );

            iChange.Commit( CommitReason.AcceptChanges );
        }

        [TestMethod]
        [ExpectedException( typeof( EnumValueOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_on_change_reject_EnumValueOutOfRangeException_on_invalid_commit_reason_with_hacked_property_change()
        {
            RejectCallback<String> rc = e => { };
            CommitCallback<String> cc = e => { };
            Object fakeOwner = new Object();
            String change = "Foo";

            var iChange = new HackedPropertyValueChange( fakeOwner, change, rc, cc );
            iChange.HackedRejectReason = ( RejectReason )1000;

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( iChange, AddChangeBehavior.Default );

            iChange.Reject( RejectReason.RejectChanges );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        [TestCategory( "ChangeTracking" )]
        public void service_on_change_reject_ArgumentOutOfRangeException_on_none_commit_reason_with_hacked_property_change()
        {
            RejectCallback<String> rc = e => { };
            CommitCallback<String> cc = e => { };
            Object fakeOwner = new Object();
            String change = "Foo";

            var iChange = new HackedPropertyValueChange( fakeOwner, change, rc, cc );
            iChange.HackedRejectReason = RejectReason.None;

            ChangeTrackingService svc = new ChangeTrackingService();
            svc.Add( iChange, AddChangeBehavior.Default );

            iChange.Reject( RejectReason.RejectChanges );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_dispose_with_changes()
        {
            RejectCallback<String> rc = e => { };
            CommitCallback<String> cc = e => { };
            Object fakeOwner = new Object();
            String change = "Foo";

            var iChange = new PropertyValueChange<String>( fakeOwner, "property-name", change, rc, cc, "" );

            using( ChangeTrackingService svc = new ChangeTrackingService() )
            {
                svc.Add( iChange, AddChangeBehavior.Default );
            }
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void service_on_dispose_raise_disposed_event()
        {
            Int32 expected = 1;
            Int32 actual = 0;

            EventHandler h = null;
            h = ( s, e ) =>
            {
                actual++;
                ( ( IComponent )s ).Disposed -= h;
            };

            using( ChangeTrackingService svc = new ChangeTrackingService() )
            {
                svc.Disposed += h;
            }

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_createBookmark_normal_should_create_a_bookmark_with_entities_transient_before_bookmark_creation()
        {
            var memento = new ChangeTrackingService();
            var person = new Person( memento, true );
            var target = memento.CreateBookmark();
            var actual = target.TransientEntities.Contains( person );

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_revert_normal_entity_registered_transient_with_changes_after_bookmark_creation_is_no_more_transient()
        {
            var expected = EntityTrackingStates.None;

            var memento = new ChangeTrackingService();
            var bookmark = memento.CreateBookmark();

            var person = new Person( memento, true );
            person.Name = "Mauro";
            memento.Revert( bookmark );

            var actual = memento.GetEntityState( person );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_revert_normal_entity_registered_transient_with_no_changes_after_bookmark_creation_is_no_more_transient()
        {
            var expected = EntityTrackingStates.None;

            var memento = new ChangeTrackingService();
            var bookmark = memento.CreateBookmark();

            var person = new Person( memento, true );
            memento.Revert( bookmark );

            var actual = memento.GetEntityState( person );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_revert_normal_entity_registered_transient_with_no_autoRemove_and_no_changes_after_bookmark_creation_is_still_transient()
        {
            var expected = EntityTrackingStates.IsTransient;

            var memento = new ChangeTrackingService();
            var bookmark = memento.CreateBookmark();

            var person = new Person( memento, false );

            memento.RegisterTransient( person, false );
            memento.Revert( bookmark );

            var actual = memento.GetEntityState( person );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_revert_normal_entity_registered_transient_with_no_autoRemove_and_changes_after_bookmark_creation_is_still_transient()
        {
            var expected = EntityTrackingStates.IsTransient;

            var memento = new ChangeTrackingService();
            var bookmark = memento.CreateBookmark();

            var person = new Person( memento, false );
            person.Name = "Mauro";

            memento.RegisterTransient( person, false );
            memento.Revert( bookmark );

            var actual = memento.GetEntityState( person );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_revert_normal_entity_registered_transient_with_changes_before_bookmark_creation_should_remain_transient()
        {
            var expected = EntityTrackingStates.AutoRemove | EntityTrackingStates.IsTransient | EntityTrackingStates.HasBackwardChanges;

            var memento = new ChangeTrackingService();
            var person = new Person( memento, true );
            person.Name = "Mauro";

            var bookmark = memento.CreateBookmark();

            new Person( memento, true );
            memento.Revert( bookmark );

            var actual = memento.GetEntityState( person );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_revert_normal_entity_registered_transient_with_no_changes_before_bookmark_creation_should_remain_transient()
        {
            var expected = EntityTrackingStates.AutoRemove | EntityTrackingStates.IsTransient;

            var memento = new ChangeTrackingService();
            var person = new Person( memento, true );

            var bookmark = memento.CreateBookmark();

            new Person( memento, true );
            memento.Revert( bookmark );

            var actual = memento.GetEntityState( person );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_revert_normal_entity_registered_transient_with_no_autoRemove_and_no_changes_before_bookmark_creation_should_remain_transient()
        {
            var expected = EntityTrackingStates.IsTransient;

            var memento = new ChangeTrackingService();
            var person = new Person( memento, false );
            memento.RegisterTransient( person, false );

            var bookmark = memento.CreateBookmark();

            new Person( memento, true );
            memento.Revert( bookmark );

            var actual = memento.GetEntityState( person );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeTrackingService_revert_normal_entity_registered_transient_with_no_autoRemove_and_changes_before_bookmark_creation_should_remain_transient()
        {
            var expected = EntityTrackingStates.IsTransient | EntityTrackingStates.HasBackwardChanges;

            var memento = new ChangeTrackingService();
            var person = new Person( memento, false );
            memento.RegisterTransient( person, false );
            person.Name = "Mauro";

            var bookmark = memento.CreateBookmark();

            new Person( memento, true );
            memento.Revert( bookmark );

            var actual = memento.GetEntityState( person );

            actual.Should().Be.EqualTo( expected );
        }
    }
}