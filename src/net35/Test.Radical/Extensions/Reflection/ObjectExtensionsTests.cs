using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical.Reflection;
using System.Reflection;

namespace Test.Radical.Extensions.Reflection
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        [TestCategory( "ObjectExtensions" )]
        [TestCategory( "FastPropertyGetter" )]
        public void ObjectExtensions_CreateFastPropertyGetter_typed_using_valid_property_should_create_a_valid_getter()
        {
            var expected = "Mauro";
            var person = new Person() { Name = expected };

            var getter = person.CreateFastPropertyGetter<String>( "Name" );
            var actual = getter();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ObjectExtensions" )]
        [TestCategory( "FastPropertyGetter" )]
        public void ObjectExtensions_CreateFastPropertyGetter_untyped_using_valid_property_should_create_a_valid_getter()
        {
            var expected = "Mauro";
            var person = new Person() { Name = expected };

            var getter = person.CreateFastPropertyGetter( typeof( Person ).GetProperty( "Name" ) );
            var actual = ( String )getter.DynamicInvoke();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ObjectExtensions" )]
        [TestCategory( "FastPropertyGetter" )]
        public void ObjectExtensions_CreateFastPropertyGetter_untyped_using_private_property_should_create_a_valid_getter()
        {
            var expected = "Mauro";
            var person = new Person( expected );

            var getter = person.CreateFastPropertyGetter( typeof( Person ).GetProperty( "privateValue", BindingFlags.NonPublic | BindingFlags.Instance ) );
            var actual = ( String )getter();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ObjectExtensions" )]
        [TestCategory( "FastPropertyGetter" )]
        public void ObjectExtensions_CreateFastPropertyGetter_typed_using_private_property_should_create_a_valid_getter()
        {
            var expected = "Mauro";
            var person = new Person( expected );

            var getter = person.CreateFastPropertyGetter<String>( typeof( Person ).GetProperty( "privateValue", BindingFlags.NonPublic | BindingFlags.Instance ) );
            var actual = getter();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ObjectExtensions" )]
        [TestCategory( "FastPropertyGetter" )]
        public void ObjectExtensions_CreateFastPropertyGetter_untyped_using_valuetype_property_should_create_a_valid_getter()
        {
            var expected = 10;
            var person = new Person();
            person.Age = expected;

            //AFAIK there is no way to make type inference work.
            var getter = person.CreateFastPropertyGetter( typeof( Person ).GetProperty( "Age" ) );
            var actual = getter();

            actual.Should().Be.EqualTo( expected );
        }
    }
}
