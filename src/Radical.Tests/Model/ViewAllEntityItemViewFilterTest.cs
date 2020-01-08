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
            var expected = ViewAllEntityItemViewFilter<GenericParameterHelper>.Instance;
            var actual = ViewAllEntityItemViewFilter<GenericParameterHelper>.Instance;

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void viewAllEntityItemViewFilter_instance_using_different_generic_types_is_not_singleton()
        {
            var expected = ViewAllEntityItemViewFilter<GenericParameterHelper>.Instance;
            var actual = ViewAllEntityItemViewFilter<Object>.Instance;

            actual.Should().Not.Be.EqualTo(expected);
        }

        [TestMethod]
        public void viewAllEntityItemViewFilter_shouldInclude_should_always_return_true()
        {
            var target = ViewAllEntityItemViewFilter<GenericParameterHelper>.Instance;
            var actual = target.ShouldInclude(new GenericParameterHelper());

            actual.Should().Be.True();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void viewAllEntityItemViewFilter_shouldInclude_using_null_reference_should_raise_ArgumentNullException()
        {
            var target = ViewAllEntityItemViewFilter<GenericParameterHelper>.Instance;
            ((ViewAllEntityItemViewFilter<GenericParameterHelper>)target).ShouldInclude(null);
        }

        [TestMethod]
        public void viewAllEntityItemViewFilter_toString_normal_should_return_expected_value()
        {
            var expected = "View all.";
            var target = ViewAllEntityItemViewFilter<GenericParameterHelper>.Instance;

            var actual = target.ToString();

            actual.Should().Be.EqualTo(expected);
        }
    }
}
