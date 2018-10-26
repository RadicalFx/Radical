//extern alias tpx;

namespace Radical.Tests.Extensions
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpTestsEx;
    using Radical;

    /// <summary>
    ///This is a test class for EnumExtensionTest and is intended
    ///to contain all EnumExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EnumExtensionTest
    {
        private enum TestEnum
        {
            None = 0,

            [EnumItemDescription( "" )]
            ValueWithAttribute = 1,

            ValueWithoutAttribute = 2,

            [EnumItemDescription( "caption", "this is the description description", 0 )]
            ValueWithDescription = 3,
        }

        [Flags]
        private enum FlaggedTestEnum
        {
            None = 0,

            Value1 = 1,
            Value2 = 2,
            Value4 = 4,
            Value8 = 8
        }

        [TestMethod()]
        [ExpectedException( typeof( EnumValueOutOfRangeException ) )]
        public void EnumInvalidValueValidation()
        {
            TestEnum actual = ( TestEnum )( -1 );
            EnumExtensions.EnsureIsDefined( actual );
        }

        [TestMethod()]
        public void EnumValidValueValidation()
        {
            try
            {
                TestEnum actual = ( TestEnum )( 0 );
                EnumExtensions.EnsureIsDefined( actual );
            }
            catch( EnumValueOutOfRangeException )
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void EnumExtension_IsDescriptionAttributeDefined()
        {
            TestEnum val = TestEnum.ValueWithAttribute;
            Boolean actual = EnumExtensions.IsDescriptionAttributeDefined( val );

            Assert.IsTrue( actual );
        }

        [TestMethod()]
        public void EnumExtension_not_IsDescriptionAttributeDefined()
        {
            TestEnum val = TestEnum.ValueWithoutAttribute;
            Boolean actual = EnumExtensions.IsDescriptionAttributeDefined( val );

            Assert.IsFalse( actual );
        }

        [TestMethod()]
        public void EnumExtension_GetDescriptionAttribute()
        {
            TestEnum val = TestEnum.ValueWithAttribute;
            EnumItemDescriptionAttribute actual = EnumExtensions.GetDescriptionAttribute( val );

            Assert.IsNotNull( actual );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentException ) )]
        public void EnumExtension_GetDescriptionAttribute_failure()
        {
            TestEnum val = TestEnum.ValueWithoutAttribute;
            EnumItemDescriptionAttribute actual = EnumExtensions.GetDescriptionAttribute( val );
        }

        [TestMethod()]
        public void EnumExtension_GetCaption()
        {
            TestEnum val = TestEnum.ValueWithDescription;
            String actual = EnumExtensions.GetCaption( val );
            String expected = "caption";

            Assert.AreEqual<String>( expected, actual );
        }

        [TestMethod]
        public void enumExtension_getDescription_normal_should_work_as_expected()
        {
            TestEnum val = TestEnum.ValueWithDescription;
            String actual = EnumExtensions.GetDescription( val );
            String expected = "this is the description description";

            Assert.AreEqual<String>( expected, actual );
        }

        [TestMethod]
        public void enumExtensions_isDefined_using_defined_value_should_return_true()
        {
            var target = TestEnum.ValueWithDescription;
            var actual = target.IsDefined();

            actual.Should().Be.True();
        }

        [TestMethod]
        public void enumExtensions_isDefined_using_non_defined_value_should_return_false()
        {
            var target = ( TestEnum )100;
            var actual = target.IsDefined();

            actual.Should().Be.False();
        }
    }
}
