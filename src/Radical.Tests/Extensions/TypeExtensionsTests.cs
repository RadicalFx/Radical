namespace Radical.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.Reflection;
    using SharpTestsEx;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        [TestCategory("TypeExtensions")]
        public void TypeExtensions_is_non_generic_using_same_types_should_return_true()
        {
            var actual = typeof(Object).Is(typeof(Object));
            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("TypeExtensions")]
        public void TypeExtensions_is_non_generic_using_valid_types_should_return_true()
        {
            var actual = typeof(Int32).Is(typeof(Object));
            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("TypeExtensions")]
        public void TypeExtensions_is_non_generic_using_non_inherited_types_should_return_false()
        {
            var actual = typeof(Int32).Is(typeof(String));
            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory("TypeExtensions")]
        public void TypeExtensions_getInheritanceChain_using_valid_type_should_return_expected_inheritance_data()
        {
            IEnumerable<Type> chain = Radical.Reflection.TypeExtensions.GetInheritanceChain(typeof(String));
            chain.Should().Have.SameSequenceAs(new Type[] { typeof(String), typeof(Object) });
        }

        [TestMethod]
        [TestCategory("TypeExtensions")]
        public void TypeExtensions_getInheritanceChain_using_valid_type_and_valid_stopper_should_return_expected_inheritance_data()
        {
            IEnumerable<Type> chain = Radical.Reflection.TypeExtensions.GetInheritanceChain(typeof(String), t => t != typeof(String));
            chain.Should().Have.SameSequenceAs(new Type[] { typeof(String) });
        }

        class Root { }

        class DescendantA : Root { }

        class DescendantB : Root { }

        [TestMethod]
        [TestCategory("TypeExtensions")]
        public void TypeExtensions_getDescendants_using_valid_type_should_return_expected_descendants()
        {
            IEnumerable<Type> descendants = Radical.Reflection.TypeExtensions.GetDescendants(typeof(Root));
            descendants.Should().Have.SameSequenceAs(new Type[] { typeof(Root), typeof(DescendantA), typeof(DescendantB) });
        }
    }
}
