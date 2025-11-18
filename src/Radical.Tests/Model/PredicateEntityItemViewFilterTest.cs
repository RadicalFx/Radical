using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Model;
using SharpTestsEx;
using System;

namespace Radical.Tests.Model
{
    [TestClass()]
    public class PredicateEntityItemViewFilterTest
    {
        [TestMethod]
        public void predicateEntityItemViewFilter_ctor_predicate_should_set_expected_values()
        {
            var expectedName = "Default Predicate name.";
            var expected = 1;
            var actual = 0;

            var target = new PredicateEntityItemViewFilter<TestTypeHelper>(v =>
           {
               actual++;
               return true;
           });

            target.ShouldInclude(new TestTypeHelper());

            actual.Should().Be.EqualTo(expected);
            target.ToString().Should().Be.EqualTo(expectedName);
        }

        [TestMethod]
        public void predicateEntityItemViewFilter_ctor_predicate_string_should_set_expected_values()
        {
            var expectedName = "filter name";
            var expected = 1;
            var actual = 0;

            var target = new PredicateEntityItemViewFilter<TestTypeHelper>(v =>
           {
               actual++;
               return true;
           }, expectedName);

            target.ShouldInclude(new TestTypeHelper());

            actual.Should().Be.EqualTo(expected);
            target.ToString().Should().Be.EqualTo(expectedName);
        }

        [TestMethod]
        public void predicateEntityItemViewFilter_ctor_predicate_null_string_should_set_expected_values()
        {
            var expectedName = "Default Predicate name.";
            var expected = 1;
            var actual = 0;

            var target = new PredicateEntityItemViewFilter<TestTypeHelper>(v =>
           {
               actual++;
               return true;
           }, null);

            target.ShouldInclude(new TestTypeHelper());

            actual.Should().Be.EqualTo(expected);
            target.ToString().Should().Be.EqualTo(expectedName);
        }

        [TestMethod]
        public void predicateEntityItemViewFilter_ctor_null_predicate_should_raise_ArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var target = new PredicateEntityItemViewFilter<TestTypeHelper>(null);
            });
        }

        [TestMethod]
        public void predicateEntityItemViewFilter_ctor_null_predicate_null_string_should_raise_ArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var target = new PredicateEntityItemViewFilter<TestTypeHelper>(null, null);
            });
        }
    }
}
