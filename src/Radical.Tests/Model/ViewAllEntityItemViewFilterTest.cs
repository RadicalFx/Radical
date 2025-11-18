using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Model;
using SharpTestsEx;
using System;

namespace Radical.Tests.Model
{
    [TestClass()]
    public class ViewAllEntityItemViewFilterTest
    {
        [TestMethod]
        public void viewAllEntityItemViewFilter_instance_normal_is_singleton()
        {
            var expected = ViewAllEntityItemViewFilter<TestTypeHelper>.Instance;
            var actual = ViewAllEntityItemViewFilter<TestTypeHelper>.Instance;

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void viewAllEntityItemViewFilter_instance_using_different_generic_types_is_not_singleton()
        {
            var expected = ViewAllEntityItemViewFilter<TestTypeHelper>.Instance;
            var actual = ViewAllEntityItemViewFilter<object>.Instance;

            actual.Should().Not.Be.EqualTo(expected);
        }

        [TestMethod]
        public void viewAllEntityItemViewFilter_shouldInclude_should_always_return_true()
        {
            var target = ViewAllEntityItemViewFilter<TestTypeHelper>.Instance;
            var actual = target.ShouldInclude(new TestTypeHelper());

            actual.Should().Be.True();
        }

        [TestMethod]
        public void viewAllEntityItemViewFilter_shouldInclude_using_null_reference_should_raise_ArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var target = ViewAllEntityItemViewFilter<TestTypeHelper>.Instance;
                ((ViewAllEntityItemViewFilter<TestTypeHelper>)target).ShouldInclude(null);
            });
        }

        [TestMethod]
        public void viewAllEntityItemViewFilter_toString_normal_should_return_expected_value()
        {
            var expected = "View all.";
            var target = ViewAllEntityItemViewFilter<TestTypeHelper>.Instance;

            var actual = target.ToString();

            actual.Should().Be.EqualTo(expected);
        }
    }
}
