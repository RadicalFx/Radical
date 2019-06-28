using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Radical.Reflection;
using System.Reflection;

namespace Radical.Tests.Extensions.Reflection
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

            var getter = person.CreateFastPropertyGetter<string>( "Name" );
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
            var actual = ( string )getter.DynamicInvoke();

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
            var actual = ( string )getter();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "ObjectExtensions" )]
        [TestCategory( "FastPropertyGetter" )]
        public void ObjectExtensions_CreateFastPropertyGetter_typed_using_private_property_should_create_a_valid_getter()
        {
            var expected = "Mauro";
            var person = new Person( expected );

            var getter = person.CreateFastPropertyGetter<string>( typeof( Person ).GetProperty( "privateValue", BindingFlags.NonPublic | BindingFlags.Instance ) );
            var actual = getter();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod, Ignore]
        [TestCategory( "ObjectExtensions" )]
        [TestCategory( "FastPropertyGetter" )]
        public void ObjectExtensions_CreateFastPropertyGetter_untyped_using_valuetype_property_should_create_a_valid_getter()
        {
            //Do not sincerly know if there is a way to make it work as expected.
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
