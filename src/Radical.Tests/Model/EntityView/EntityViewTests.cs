namespace Radical.Tests.Model
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.ComponentModel;
    using Radical.Model;
    using SharpTestsEx;
    using System;
    using System.Linq;

    [TestClass]
    public class EntityViewTests
    {
        [TestMethod]
        public void entityView_custom_property_mapping_shuold_set_and_get_custom_values_as_expected()
        {
            var expected = "Hello World!";

            EntityItemViewValueGetter<Object, string> getter = e =>
            {
                return e.Item.GetCustomValue<string>(e.PropertyName);
            };

            EntityItemViewValueSetter<Object, string> setter = e =>
            {
                e.Item.SetCustomValue(e.PropertyName, e.Value);
            };

            var target = new EntityView<Object>(new[] { new Object() });
            target.AddCustomProperty<string>("Foo", getter, setter);

            target.First().SetCustomValue("Foo", expected);
            var actual = target.First().GetCustomValue<string>("Foo");

            actual.Should("Not yet implemented").Be.EqualTo(expected);
        }

        [TestMethod]
        public void entityView_getCustomPropertyValue_using_never_used_custom_property_should_use_initial_value_interceptor_once()
        {
            var expected = 1;
            var actual = 0;
            Func<string> interceptor = () =>
            {
                actual++;
                return "initial value";
            };

            var target = new EntityView<Object>(new[] { new Object() });
            target.AddCustomProperty<string>("Foo", e =>
           {
               return e.Item.GetCustomValue<string>(e.PropertyName);
           }, interceptor);

            var item = target.FirstOrDefault();
            item.GetCustomValue<string>("Foo");
            item.GetCustomValue<string>("Foo");
            item.GetCustomValue<string>("Foo");

            actual.Should("Not yet implemented").Be.EqualTo(expected);
        }

        [TestMethod]
        public void entityView_getCustomPropertyValue_should_return_initial_interceptor_value()
        {
            var expected = "initial value";
            var actual = "";

            Func<string> interceptor = () =>
            {
                return expected;
            };

            var target = new EntityView<Object>(new[] { new Object() });
            target.AddCustomProperty<string>("Foo", e =>
           {
               return e.Item.GetCustomValue<string>(e.PropertyName);
           }, interceptor);

            var item = target.FirstOrDefault();
            actual = item.GetCustomValue<string>("Foo");

            actual.Should("Not yet implemented").Be.EqualTo(expected);
        }

        [TestMethod]
        public void entityView_removeCustomProperty_should_remove_property_value()
        {
            var value = "Hello world!";

            EntityItemViewValueGetter<Object, string> getter = e =>
            {
                return e.Item.GetCustomValue<string>(e.PropertyName);
            };

            EntityItemViewValueSetter<Object, string> setter = e =>
            {
                e.Item.SetCustomValue(e.PropertyName, e.Value);
            };

            var target = new EntityView<Object>(new[] { new Object() });
            target.AddCustomProperty<string>("Foo", getter, setter);

            var item = target.First();
            item.SetCustomValue<string>("Foo", value);

            target.RemoveCustomProperty("Foo");
            target.AddCustomProperty<string>("Foo", getter, setter);

            var actual = target.First().GetCustomValue<string>("Foo");

            actual.Should("Not yet implemented").Not.Be.EqualTo(value);
            actual.Should("Not yet implemented").Be.Null();
        }

        [TestMethod]
        public void entityView_removeCustomProperty_should_remove_property_value_even_on_more_then_one_element()
        {
            var propertyName = "Foo";
            var expectedFirst = "Hello world, from First!";
            var expectedLast = "Hello world, from Last!";

            EntityItemViewValueGetter<Object, string> getter = e =>
            {
                return e.Item.GetCustomValue<string>(e.PropertyName);
            };

            EntityItemViewValueSetter<Object, string> setter = e =>
            {
                e.Item.SetCustomValue(e.PropertyName, e.Value);
            };

            var target = new EntityView<Object>(new[] { new Object(), new Object() });
            target.AddCustomProperty<string>(propertyName, getter, setter);

            var firstItem = target.First();
            firstItem.SetCustomValue<string>(propertyName, expectedFirst);

            var lastItem = target.Last();
            lastItem.SetCustomValue<string>(propertyName, expectedLast);

            target.RemoveCustomProperty(propertyName);
            target.AddCustomProperty<string>(propertyName, getter, setter);

            var actualFirst = target.First().GetCustomValue<string>("Foo");
            var actualLast = target.Last().GetCustomValue<string>("Foo");

            actualFirst.Should().Be.Null();
            actualLast.Should().Be.Null();
        }
    }
}
