//extern alias tpx;

namespace Radical.Tests.Model.Entity
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.Model;
    using SharpTestsEx;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class MockEntity : Entity, IMockEntity
    {
        public MockEntity()
        {
            OnInitialize();
        }

        public MockEntity(string initialFirstNameValue)
        {
            SetInitialPropertyValue(() => FirstName, initialFirstNameValue);

            OnInitialize();
        }

        void OnInitialize()
        {
            var metadata = new PropertyMetadata<string>(this, () => MainProperty);
            metadata.AddCascadeChangeNotifications(() => SubProperty);

            SetPropertyMetadata(metadata);
        }

        public void SetInitialValue<T>(Expression<Func<T>> property, T value)
        {
            SetInitialPropertyValue(property, value);
        }

        public string FirstName
        {
            get { return GetPropertyValue(() => FirstName); }
            set { SetPropertyValue(() => FirstName, value); }
        }

        public string LastName
        {
            get { return GetPropertyValue(() => LastName); }
            set { SetPropertyValue(() => LastName, value); }
        }

        public int Number
        {
            get { return GetPropertyValue(() => Number); }
            set { SetPropertyValue(() => Number, value); }
        }

        public string MainProperty
        {
            get { return GetPropertyValue(() => MainProperty); }
            set { SetPropertyValue(() => MainProperty, value); }
        }

        public string SubProperty
        {
            get { return GetPropertyValue(() => SubProperty); }
        }
    }

    class Person : Entity
    {
        public Person(DateTime bornDate)
        {
            var metadata = new PropertyMetadata<DateTime>(this, () => BornDate) { DefaultValue = bornDate };
            metadata.AddCascadeChangeNotifications(() => Age);
            SetPropertyMetadata(metadata);
        }

        public DateTime BornDate
        {
            get { return GetPropertyValue(() => BornDate); }
            set { SetPropertyValue(() => BornDate, value); }
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
        [TestMethod]
        public void entity_set_property_using_internal_values_management_should_behave_as_expected()
        {
            var expected = "Mauro";

            var target = new MockEntity();
            target.FirstName = expected;

            var actual = target.FirstName;

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entity_normal_should_behave_have_expected_initial_default_values()
        {
            var expected = "Mauro";

            var target = new MockEntity(expected);

            target.FirstName.Should().Be.EqualTo(expected);
            target.LastName.Should().Be.Null();
            target.Number.Should().Be.EqualTo(0);
        }

        [TestMethod]
        public void entity_create_using_initial_value_should_set_expected_value()
        {
            var expected = "Mauro";

            var target = new MockEntity(expected);

            var actual = target.FirstName;

            actual.Should().Be.EqualTo(expected);
        }

        [Ignore]
        [TestMethod]
        public void entity_create_using_initial_value_is_reset_initial_value_should_raise_ArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                /*
                 * Questo in teoria dovrebbe sollevare 
                 * una exception perchè stiamo cambiando il DefaultValue
                 * dopo che è stato inizializzato... non credo abbia più molto senso
                 */
                var target = new MockEntity("Mauro");
                target.SetInitialValue(() => target.FirstName, "Foo");
            });
        }

        [TestMethod]
        public void entity_set_property_normal_should_raise_propertyChanged_event()
        {
            var expected = 1;
            var actual = 0;

            var target = new MockEntity();
            target.PropertyChanged += (s, e) => actual++;

            target.FirstName = "Mauro";

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entity_set_property_twice_using_same_value_should_raise_propertyChanged_event_once()
        {
            var expected = 1;
            var actual = 0;

            var target = new MockEntity();
            target.PropertyChanged += (s, e) => actual++;

            target.FirstName = "Mauro";
            target.FirstName = "Mauro";

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entity_set_property_multiple_times_using_different_values_should_raise_propertyChanged_events_as_expected()
        {
            var expected = 3;
            var actual = 0;

            var target = new MockEntity();
            target.PropertyChanged += (s, e) => actual++;

            target.FirstName = "Mauro";
            target.FirstName = "Foo";
            target.FirstName = "Foo";
            target.FirstName = "Mauro";

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entity_with_initial_value_set_property_once_should_raise_propertyChanged_event()
        {
            var expected = 1;
            var actual = 0;

            var target = new MockEntity("Mauro");
            target.PropertyChanged += (s, e) => actual++;

            target.FirstName = "Foo";

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void entity_with_initial_value_set_property_multiple_times_using_different_values_should_raise_propertyChanged_events_as_expected()
        {
            var expected = 3;
            var actual = 0;

            var target = new MockEntity("initial value");
            target.PropertyChanged += (s, e) => actual++;

            target.FirstName = "Mauro";
            target.FirstName = "Foo";
            target.FirstName = "Foo";
            target.FirstName = "Mauro";

            actual.Should().Be.EqualTo(expected);
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
            var actualNotifications = new List<string>();

            var target = new MockEntity();
            target.PropertyChanged += (s, e) =>
            {
                actual++;
                actualNotifications.Add(e.PropertyName);
            };

            target.MainProperty = "foo...";

            actual.Should().Be.EqualTo(expected);
            actualNotifications.Should().Have.SameSequenceAs(expectedNotifications);
        }

        [TestMethod]
        public void person_set_bornDate_should_raise_all_expected_notifications()
        {
            var expected = 2;
            var actual = 0;

            var expectedNotifications = new[] { "BornDate", "Age" };
            var actualNotifications = new List<string>();

            var target = new Person(new DateTime(1973, 1, 10));
            target.PropertyChanged += (s, e) =>
            {
                actual++;
                actualNotifications.Add(e.PropertyName);
            };

            target.BornDate = new DateTime(1978, 11, 5);

            actual.Should().Be.EqualTo(expected);
            actualNotifications.Should().Have.SameSequenceAs(expectedNotifications);
        }
    }
}
