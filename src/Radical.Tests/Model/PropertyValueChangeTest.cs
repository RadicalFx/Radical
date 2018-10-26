//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel.ChangeTracking;
using Radical.ChangeTracking.Specialized;
using SharpTestsEx;

namespace Radical.Tests.Model
{
    [TestClass()]
    public class PropertyValueChangeTest : ChangeTracking.ChangeTest
    {
        protected override IChange<T> Mock<T>( object owner, T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, string description )
        {
            return this.MockPropertyValue<T>( owner, value, rejectCallback, commitCallback, description );
        }

        protected virtual PropertyValueChange<T> MockPropertyValue<T>( object owner, T value, RejectCallback<T> rejectCallback )
        {
            return new PropertyValueChange<T>( owner, "property-name", value, rejectCallback );
        }

        protected virtual PropertyValueChange<T> MockPropertyValue<T>( object owner, T value, RejectCallback<T> rejectCallback, string description )
        {
            return new PropertyValueChange<T>( owner, "property-name", value, rejectCallback, description );
        }

        protected virtual PropertyValueChange<T> MockPropertyValue<T>( object owner, T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, string description )
        {
            return new PropertyValueChange<T>( owner, "property-name", value, rejectCallback, commitCallback, description );
        }

        [TestMethod]
        public void propertyValueChange_ctor_owner_value_rejectCallback_normal_should_set_expected_values()
        {
            var expected = new Object();

            var target = this.MockPropertyValue( expected, "Foo", cv => { } );
            var actual = target.Owner;

            actual.Should().Be.EqualTo( expected );
            target.Description.Should().Be.EqualTo( String.Empty );
        }

        [TestMethod]
        public void propertyValueChange_ctor_owner_value_rejectCallback_on_reject_should_return_expected_values()
        {
            var expected = "Foo";
            ChangeRejectedEventArgs<String> actual = null;
            var owner = new Object();

            var target = this.MockPropertyValue( owner, expected, cv => { actual = cv; } );
            target.Reject( RejectReason.RejectChanges );

            actual.Should().Not.Be.Null();
            actual.CachedValue.Should().Be.EqualTo( expected );
            actual.Reason.Should().Be.EqualTo( RejectReason.RejectChanges );
            actual.Source.Should().Be.EqualTo( target );
            actual.Entity.Should().Be.EqualTo( owner );
        }

        [TestMethod]
        public void propertyValueChange_getAdvisedAction_should_return_expected_values()
        {
            var expected = ProposedActions.Create | ProposedActions.Update;
            var owner = new Object();

            var target = this.MockPropertyValue( owner, "Foo", cv => { } );
            var actual = target.GetAdvisedAction( owner );

            actual.Should().Be.EqualTo( expected );
        }


        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        public void propertyVaueChange_getAdvisedAction_using_invalid_owner_should_raise_ArgumentOutOfRangeException()
        {
            var target = this.MockPropertyValue( new Object(), "Foo", cv => { } );
            target.GetAdvisedAction( new Object() );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void propertyVaueChange_getAdvisedAction_using_invalid_owner_should_raise_ArgumentNullException()
        {
            var target = this.MockPropertyValue( new Object(), "Foo", cv => { } );
            target.GetAdvisedAction( null );
        }

        [TestMethod]
        public void propertyValueChange_clone_normal_should_return_cloned_value()
        {
            var target = this.MockPropertyValue( new Object(), "Foo", cv => { } );
            var actual = target.Clone();

            actual.Should().Not.Be.EqualTo( target );
            actual.Owner.Should().Be.EqualTo( target.Owner );
            actual.Description.Should().Be.EqualTo( target.Description );
            actual.GetChangedEntities().Should().Have.SameSequenceAs( target.GetChangedEntities() );
        }

        [TestMethod]
        public void propertyValueChange_ctor_owner_value_rejectCallback_description_normal_should_set_expected_values()
        {
            var expected = new Object();

            var target = this.MockPropertyValue( expected, "Foo", cv => { }, "description" );
            var actual = target.Owner;

            actual.Should().Be.EqualTo( expected );
            target.Description.Should().Be.EqualTo( "description" );
        }
    }
}
