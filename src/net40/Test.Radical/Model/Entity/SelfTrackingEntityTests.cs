//extern alias tpx;

namespace Test.Radical.Model.Entity
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using SharpTestsEx;
    using System.Linq.Expressions;
    using Topics.Radical.Model;
    using System.Collections.Generic;

    public class MockEntity : Topics.Radical.Model.Entity, IMockEntity
    {
        public MockEntity()
        {
            this.OnInitialize();
        }

        public MockEntity( String initialFirstNameValue )
        {
            this.SetInitialPropertyValue( () => this.FirstName, initialFirstNameValue );

            this.OnInitialize();
        }

        void OnInitialize()
        {
            var metadata = new PropertyMetadata<String>(this, () => this.MainProperty );
            metadata.AddCascadeChangeNotifications( () => this.SubProperty );

            this.SetPropertyMetadata( metadata );
        }

        public void SetInitialValue<T>( Expression<Func<T>> property, T value )
        {
            this.SetInitialPropertyValue( property, value );
        }

        public String FirstName
        {
            get { return this.GetPropertyValue( () => this.FirstName ); }
            set { this.SetPropertyValue( () => this.FirstName, value ); }
        }

        public String LastName
        {
            get { return this.GetPropertyValue( () => this.LastName ); }
            set { this.SetPropertyValue( () => this.LastName, value ); }
        }

        public Int32 Number
        {
            get { return this.GetPropertyValue( () => this.Number ); }
            set { this.SetPropertyValue( () => this.Number, value ); }
        }

        public String MainProperty
        {
            get { return this.GetPropertyValue( () => this.MainProperty ); }
            set { this.SetPropertyValue( () => this.MainProperty, value ); }
        }

        public String SubProperty
        {
            get { return this.GetPropertyValue( () => this.SubProperty ); }
        }
    }

    class Person : Entity
    {
        public Person( DateTime bornDate )
        {
            var metadata = new PropertyMetadata<DateTime>( this, () => this.BornDate ) { DefaultValue = bornDate };
            metadata.AddCascadeChangeNotifications( () => this.Age );
            this.SetPropertyMetadata( metadata );
        }

        public DateTime BornDate
        {
            get { return this.GetPropertyValue( () => this.BornDate ); }
            set { this.SetPropertyValue( () => this.BornDate, value ); }
        }

        public int Age
        {
            get
            {
                //Eval age base on BornDate
                return 0;
            }
        }
    }

    [TestClass]
    public class SelfTrackingEntityTests : EntityTests
    {
        protected override Topics.Radical.Model.Entity CreateMock()
        {
            return new MockEntity();
        }

        protected virtual Topics.Radical.Model.Entity CreateMock( String firstName )
        {
            return new MockEntity( firstName );
        }

        //protected override T CreateMock<T>()
        //{
        //    if( typeof( T ) == typeof( MockEntity ) )
        //    {
        //        return ( T )( new MockEntity() );
        //    }

        //    return base.CreateMock<T>();
        //}

        //protected override Topics.Radical.Model.Entity CreateMock()
        //{
        //    return this.CreateMockEntity();
        //}

        //protected virtual Topics.Radical.Model.Entity CreateMockEntity()
        //{
        //    return new MockEntity();
        //}

        //protected virtual Topics.Radical.Model.Entity CreateMockEntity( String firstName )
        //{
        //    return new MockEntity( firstName );
        //}

        [TestMethod]
        public void entity_set_property_using_internal_values_management_should_behave_as_expected()
        {
            var expected = "Mauro";

            var target = ( IMockEntity )this.CreateMock();
            target.FirstName = expected;

            var actual = target.FirstName;

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entity_normal_should_behave_have_expected_initial_default_values()
        {
            var expected = "Mauro";

            var target = ( IMockEntity )this.CreateMock( expected );

            target.FirstName.Should().Be.EqualTo( expected );
            target.LastName.Should().Be.Null();
            target.Number.Should().Be.EqualTo( 0 );
        }

        [TestMethod]
        public void entity_create_using_initial_value_should_set_expected_value()
        {
            var expected = "Mauro";

            var target = ( IMockEntity )this.CreateMock( expected );

            var actual = target.FirstName;

            actual.Should().Be.EqualTo( expected );
        }

        [Ignore]
        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void entity_create_using_initial_value_is_reset_initial_value_should_raise_ArgumentException()
        {
            /*
             * Questo in teoria dovrebbe sollevare 
             * una exception perchè stiamo cambiando il DefaultValue
             * dopo che è stato inizializzato... non credo abbia più molto senso
             */
            var target = ( IMockEntity )this.CreateMock( "Mauro" );
            target.SetInitialValue( () => target.FirstName, "Foo" );
        }

        [TestMethod]
        public void entity_set_property_normal_should_raise_propertyChanged_event()
        {
            var expected = 1;
            var actual = 0;

            var target = ( IMockEntity )this.CreateMock();
            target.PropertyChanged += ( s, e ) => actual++;

            target.FirstName = "Mauro";

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entity_set_property_twice_using_same_value_should_raise_propertyChanged_event_once()
        {
            var expected = 1;
            var actual = 0;

            var target = ( IMockEntity )this.CreateMock();
            target.PropertyChanged += ( s, e ) => actual++;

            target.FirstName = "Mauro";
            target.FirstName = "Mauro";

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entity_set_property_multiple_times_using_different_values_should_raise_propertyChanged_events_as_expected()
        {
            var expected = 3;
            var actual = 0;

            var target = ( IMockEntity )this.CreateMock();
            target.PropertyChanged += ( s, e ) => actual++;

            target.FirstName = "Mauro";
            target.FirstName = "Foo";
            target.FirstName = "Foo";
            target.FirstName = "Mauro";

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entity_with_initial_value_set_property_once_should_raise_propertyChanged_event()
        {
            var expected = 1;
            var actual = 0;

            var target = ( IMockEntity )this.CreateMock( "Mauro" );
            target.PropertyChanged += ( s, e ) => actual++;

            target.FirstName = "Foo";

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void entity_with_initial_value_set_property_multiple_times_using_different_values_should_raise_propertyChanged_events_as_expected()
        {
            var expected = 3;
            var actual = 0;

            var target = ( IMockEntity )this.CreateMock( "initial value" );
            target.PropertyChanged += ( s, e ) => actual++;

            target.FirstName = "Mauro";
            target.FirstName = "Foo";
            target.FirstName = "Foo";
            target.FirstName = "Mauro";

            actual.Should().Be.EqualTo( expected );
        }

        //[TestMethod]
        //public void entity_set_property_normal_should_set_entity_as_changed()
        //{
        //    var target = ( IMockEntity )this.CreateMock();
        //    target.FirstName = "Mauro";

        //    target.IsChanged.Should().Be.True();
        //}

        //[TestMethod]
        //public void entity_rejectChanges_normal_should_reset_isChanged()
        //{
        //    var target = ( IMockEntity )this.CreateMock();
        //    target.FirstName = "Mauro";
        //    target.RejectChanges();

        //    target.IsChanged.Should().Be.False();
        //}

        //[TestMethod]
        //[Ignore] //Non funziona più così per ora
        //public void entity_rejectChanges_normal_should_reset_property_values()
        //{
        //    var target = ( IMockEntity )this.CreateMock();
        //    target.FirstName = "Mauro";

        //    target.RejectChanges();

        //    target.FirstName.Should().Be.Null();
        //}

        //[TestMethod]
        //[Ignore] //Non funziona più così per ora
        //public void entity_with_initial_value_rejectChanges_normal_should_reset_property_values()
        //{
        //    var expected = "initial value";

        //    var target = ( IMockEntity )this.CreateMock( expected );
        //    target.FirstName = "Mauro";

        //    target.RejectChanges();

        //    target.FirstName.Should().Be.EqualTo( expected );
        //}

        //[TestMethod]
        //[Ignore] //Non funziona più così per ora
        //public void entity_rejectChanges_normal_should_raise_propertyChanged_event_on_resetted_properties()
        //{
        //    var expected = 1;
        //    var actual = 0;

        //    var target = ( IMockEntity )this.CreateMock();
        //    target.FirstName = "Mauro";

        //    target.PropertyChanged += ( s, e ) => actual++;

        //    target.RejectChanges();

        //    actual.Should().Be.EqualTo( expected );
        //}

        //[TestMethod]
        //[Ignore] //Non funziona più così per ora
        //public void entity_rejectChanges_normal_should_raise_propertyChanged_event_on_resetted_properties_with_valid_propertyName()
        //{
        //    var expected = "FirstName";
        //    var actual = "";

        //    var target = ( IMockEntity )this.CreateMock();
        //    target.FirstName = "Mauro";

        //    target.PropertyChanged += ( s, e ) => actual = e.PropertyName;

        //    target.RejectChanges();

        //    actual.Should().Be.EqualTo( expected );
        //}

        [TestMethod]
        public void entity_property_set_on_property_with_cascade_notification_should_raise_all_expected_notifications()
        {
            var expected = 2;
            var actual = 0;

            var expectedNotifications = new[] { "MainProperty", "SubProperty" };
            var actualNotifications = new List<String>();

            var target = ( IMockEntity )this.CreateMock();
            target.PropertyChanged += ( s, e ) =>
            {
                actual++;
                actualNotifications.Add( e.PropertyName );
            };

            target.MainProperty = "foo...";

            actual.Should().Be.EqualTo( expected );
            actualNotifications.Should().Have.SameSequenceAs( expectedNotifications );
        }

        [TestMethod]
        public void person_set_bornDate_should_raise_all_expected_notifications()
        {
            var expected = 2;
            var actual = 0;

            var expectedNotifications = new[] { "BornDate", "Age" };
            var actualNotifications = new List<String>();

            var target = new Person( new DateTime( 1973, 1, 10 ) );
            target.PropertyChanged += ( s, e ) =>
            {
                actual++;
                actualNotifications.Add( e.PropertyName );
            };

            target.BornDate = new DateTime( 1978, 11, 5 );

            actual.Should().Be.EqualTo( expected );
            actualNotifications.Should().Have.SameSequenceAs( expectedNotifications );
        }
    }
}
