using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using System.Collections.Generic;
using Radical.Reflection;

namespace Radical.Tests.Helpers
{
    [TestClass()]
    public class TypeExtensionTests
    {
        [TestMethod]
        public void typeExtension_toShortString_normal_should_return_valid_type_string()
        {
            var expected = "Radical.Reflection.TypeExtensions, Radical";
            var target = typeof(Reflection.TypeExtensions);

            var actual = Reflection.TypeExtensions.ToShortString(target);
            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void typeExtension_toShortString_using_null_type_reference_should_raise_ArgumentNullException()
        {
            Reflection.TypeExtensions.ToShortString(null);
        }

        [TestMethod]
        public void typeExtension_toShortString_using_mscorelib_type_should_add_assemblyName_to_type_string()
        {
            var expected = "System.String, System.Private.CoreLib";

            var target = typeof(string);

            var actual = Reflection.TypeExtensions.ToShortString(target);
            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void typeExtension_toString_S_using_mscorelib_type_should_add_assemblyName_to_type_string()
        {
            var expected = "System.String, System.Private.CoreLib";

            var target = typeof(string);

            var actual = Reflection.TypeExtensions.ToString(target, "S");
            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void typeextensions_toShortNameString_using_non_generic_type_should_return_type_name()
        {
            var expected = "String";
            var actual = typeof(string).ToShortNameString();

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void typeextensions_toShortNameString_using_generic_type_should_return_type_name()
        {
            var expected = "List<String>";
            var actual = typeof(List<string>).ToShortNameString();

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void typeextensions_toShortNameString_using_complex_generic_type_should_return_type_name()
        {
            var expected = "IDictionary<List<Object>, List<String>>";
            var actual = typeof(IDictionary<List<Object>, List<string>>).ToShortNameString();

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void typeextensions_toString_SN_using_complex_generic_type_should_return_type_name()
        {
            var expected = "IDictionary<List<Object>, List<String>>";
            var actual = typeof(IDictionary<List<Object>, List<string>>).ToString("SN");

            actual.Should().Be.EqualTo(expected);
        }
    }
}
