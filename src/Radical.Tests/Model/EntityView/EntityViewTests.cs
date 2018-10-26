namespace Radical.Tests.Model
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpTestsEx;
    using Radical.ComponentModel;
    using Radical.Model;

    [TestClass]
    public class EntityViewTests
    {
        [TestMethod]
        public void entityView_custom_property_mapping_shuold_set_and_get_custom_values_as_expected()
        {
            var expected = "Hello World!";

            EntityItemViewValueGetter<Object, String> getter = e =>
            {
                return e.Item.GetCustomValue<String>( e.PropertyName );
            };

            EntityItemViewValueSetter<Object, String> setter = e =>
            {
                e.Item.SetCustomValue( e.PropertyName, e.Value );
            };

            var target = new EntityView<Object>( new[] { new Object() } );
            target.AddPropertyMapping<String>( "Foo", getter, setter );

            target.First().SetCustomValue( "Foo", expected );
            var actual = target.First().GetCustomValue<String>( "Foo" );

            actual.Should( "Not yet implemented" ).Be.EqualTo( expected );
        }

        [TestMethod]
        public void entityView_getCustomPropertyValue_using_never_used_custom_property_should_use_initial_value_interceptor_once()
        {
            var expected = 1;
            var actual = 0;
            Func<String> interceptor = () =>
            {
                actual++;
                return "initial value";
            };

            var target = new EntityView<Object>( new[] { new Object() } );
            target.AddPropertyMapping<String>( "Foo", e =>
            {
                return e.Item.GetCustomValue<String>( e.PropertyName );
            }, interceptor );

            var item = target.FirstOrDefault();
            item.GetCustomValue<String>( "Foo" );
            item.GetCustomValue<String>( "Foo" );
            item.GetCustomValue<String>( "Foo" );

            actual.Should( "Not yet implemented" ).Be.EqualTo( expected );
        }

        [TestMethod]
        public void entityView_getCustomPropertyValue_should_return_initial_interceptor_value()
        {
            var expected = "initial value";
            var actual = "";

            Func<String> interceptor = () =>
            {
                return expected;
            };

            var target = new EntityView<Object>( new[] { new Object() } );
            target.AddPropertyMapping<String>( "Foo", e =>
            {
                return e.Item.GetCustomValue<String>( e.PropertyName );
            }, interceptor );

            var item = target.FirstOrDefault();
            actual = item.GetCustomValue<String>( "Foo" );

            actual.Should( "Not yet implemented" ).Be.EqualTo( expected );
        }

        [TestMethod]
        public void entityView_removePropertyMapping_should_remove_property_value()
        {
            var value = "Hello world!";

            EntityItemViewValueGetter<Object, String> getter = e =>
            {
                return e.Item.GetCustomValue<String>( e.PropertyName );
            };

            EntityItemViewValueSetter<Object, String> setter = e =>
            {
                e.Item.SetCustomValue( e.PropertyName, e.Value );
            };

            var target = new EntityView<Object>( new[] { new Object() } );
            target.AddPropertyMapping<String>( "Foo", getter, setter );

            var item = target.First();
            item.SetCustomValue<String>( "Foo", value );

            target.RemovePropertyMapping( "Foo" );
            target.AddPropertyMapping<String>( "Foo", getter, setter );

            var actual = target.First().GetCustomValue<String>( "Foo" );

            actual.Should( "Not yet implemented" ).Not.Be.EqualTo( value );
            actual.Should( "Not yet implemented" ).Be.Null();
        }

        [TestMethod]
        public void entityView_removePropertyMapping_should_remove_property_value_even_on_more_then_one_element()
        {
            var propertyName = "Foo";
            var expectedFirst = "Hello world, from First!";
            var expectedLast = "Hello world, from Last!";

            EntityItemViewValueGetter<Object, String> getter = e =>
            {
                return e.Item.GetCustomValue<String>( e.PropertyName );
            };

            EntityItemViewValueSetter<Object, String> setter = e =>
            {
                e.Item.SetCustomValue( e.PropertyName, e.Value );
            };

            var target = new EntityView<Object>( new[] { new Object(), new Object() } );
            target.AddPropertyMapping<String>( propertyName, getter, setter );

            var firstItem = target.First();
            firstItem.SetCustomValue<String>( propertyName, expectedFirst );

            var lastItem = target.Last();
            lastItem.SetCustomValue<String>( propertyName, expectedLast );

            target.RemovePropertyMapping( propertyName );
            target.AddPropertyMapping<String>( propertyName, getter, setter );

            var actualFirst = target.First().GetCustomValue<String>( "Foo" );
            var actualLast = target.Last().GetCustomValue<String>( "Foo" );

            actualFirst.Should().Be.Null();
            actualLast.Should().Be.Null();
        }
    }
}
