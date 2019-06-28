//extern alias tpx;

namespace Radical.Tests.Model.Entity
{
    using Radical.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.ComponentModel;
    using SharpTestsEx;
    using FakeItEasy;
    using System.ComponentModel;
    using System;

    [TestClass()]
    public class EntityPropertyChangedEventsTests : EntityTests
    {
        public sealed class TestableEntity : Entity
        {
            internal void RaisePropertyChanged( PropertyChangedEventArgs e )
            {
                this.OnPropertyChanged( e );
            }

            internal void RaisePropertyChanged( string propertyName )
            {
                this.OnPropertyChanged( propertyName );
            }
        }

        [TestMethod]
        public void entity_propertyChanged_event_using_propertyChangedEventArgs_raised_with_expected_values()
        {
            var expected = "Foo";
            var actual = string.Empty;

            var target = new TestableEntity();
            target.PropertyChanged += ( s, e ) => { actual = e.PropertyName; };
            target.RaisePropertyChanged( new PropertyChangedEventArgs( expected ) );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entity_propertyChanged_event_using_propertyName_raised_with_expected_values()
        {
            var expected = "Foo";
            var actual = string.Empty;

            var target = new TestableEntity();
            target.PropertyChanged += ( s, e ) => { actual = e.PropertyName; };
            target.RaisePropertyChanged( expected );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [ExpectedException( typeof( ObjectDisposedException ) )]
        public void entity_propertyChanged_event_on_disposed_entity_using_propertyChangedEventArgs_should_raise_ObjectDisposedException()
        {
            var expected = "Foo";

            var target = new TestableEntity();
            target.Dispose();
            target.RaisePropertyChanged( new PropertyChangedEventArgs( expected ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ObjectDisposedException ) )]
        public void entity_propertyChanged_event_on_disposed_entity_using_propertyName_should_raise_ObjectDisposedException()
        {
            var expected = "Foo";

            var target = new TestableEntity();
            target.Dispose();
            target.RaisePropertyChanged( expected );
        }
    }
}
