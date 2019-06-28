using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel;
using Radical.Model;
using SharpTestsEx;
using System;

namespace Radical.Tests.Model
{
    [TestClass()]
    public class EntityItemViewFilterBaseTests
    {
        [TestMethod]
        public void entityItemViewFilterBase_interface_shouldInclude_using_valid_entity_should_call_abstract_implementation()
        {
            var expected = new GenericParameterHelper();

            var filter = A.Fake<EntityItemViewFilterBase<GenericParameterHelper>>();
            A.CallTo(() => filter.ShouldInclude(expected)).Returns(true).Once();
            
            var target = (IEntityItemViewFilter)filter;
            target.ShouldInclude(expected);

            A.CallTo(() => filter.ShouldInclude(expected)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void entityItemViewFilterBase_interface_shouldInclude_using_invalid_type_should_raise_ArgumentException()
        {
            var filter = A.Fake<EntityItemViewFilterBase<GenericParameterHelper>>();

            var target = (IEntityItemViewFilter)filter;
            target.ShouldInclude(new Object());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void entityItemViewFilterBase_interface_shouldInclude_using_null_reference_should_raise_ArgumentNullException()
        {
            var target = (IEntityItemViewFilter)A.Fake<EntityItemViewFilterBase<GenericParameterHelper>>();
            target.ShouldInclude(null);
        }

        [TestMethod]
        public void entityItemViewFilterBase_toString_normal_should_return_expected_value()
        {
            var expected = "Default name.";

            var target = A.Fake<EntityItemViewFilterBase<GenericParameterHelper>>();
            var actual = target.ToString();

            actual.Should().Be.EqualTo(expected);
        }
    }
}
