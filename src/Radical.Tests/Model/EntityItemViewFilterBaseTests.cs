using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Radical.Model;
using Radical.ComponentModel;
using SharpTestsEx;

namespace Radical.Tests.Model
{
    [TestClass()]
    public class EntityItemViewFilterBaseTests
    {
        [TestMethod]
        public void entityItemViewFilterBase_interface_shouldInclude_using_valid_entity_should_call_abstract_implementation()
        {
            var expected = new GenericParameterHelper();

            MockRepository mocks = new MockRepository();

            var filter = mocks.PartialMock<EntityItemViewFilterBase<GenericParameterHelper>>();
            filter.Expect( obj => obj.ShouldInclude( expected ) )
                .Return( true )
                .Repeat.Once();
            filter.Replay();

            var target = ( IEntityItemViewFilter )filter;
            target.ShouldInclude( expected );

            filter.VerifyAllExpectations();
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void entityItemViewFilterBase_interface_shouldInclude_using_invalid_type_should_raise_ArgumentException()
        {
            MockRepository mocks = new MockRepository();

            var filter = mocks.PartialMock<EntityItemViewFilterBase<GenericParameterHelper>>();
            filter.Replay();

            var target = ( IEntityItemViewFilter )filter;
            target.ShouldInclude( new Object() );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void entityItemViewFilterBase_interface_shouldInclude_using_null_reference_should_raise_ArgumentNullException()
        {
            MockRepository mocks = new MockRepository();

            var filter = mocks.PartialMock<EntityItemViewFilterBase<GenericParameterHelper>>();
            filter.Replay();

            var target = ( IEntityItemViewFilter )filter;
            target.ShouldInclude( null );
        }

        [TestMethod]
        public void entityItemViewFilterBase_toString_normal_should_return_expected_value()
        {
            var expected = "Default name.";

            MockRepository mocks = new MockRepository();

            var target = mocks.PartialMock<EntityItemViewFilterBase<GenericParameterHelper>>();
            target.Replay();

            var actual = target.ToString();

            actual.Should().Be.EqualTo( expected );
        }
    }
}
