using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;

namespace Test.Radical.Extensions
{
    /// <summary>
    /// Summary description for EnumExtensionFx35Test
    /// </summary>
    [TestClass]
    public class EnumExtensionFx35Test
    {
        [Flags]
        private enum FlaggedTestEnum
        {
            None = 0,

            Value1 = 1,
            Value2 = 2,
            Value4 = 4,
            Value8 = 8
        }

        [TestMethod]
        public void enumExtensions_HasFlag_checking_set_flag_return_true()
        {
            var testCase = FlaggedTestEnum.Value1 | FlaggedTestEnum.Value2;
            var actual = testCase.HasFlag(FlaggedTestEnum.Value1);

            actual.Should().Be.True();
        }

        [TestMethod]
        public void enumExtensions_HasFlag_checking_not_set_flag_return_false()
        {
            var testCase = FlaggedTestEnum.Value1 | FlaggedTestEnum.Value2;
            var actual = testCase.HasFlag(FlaggedTestEnum.Value8);

            actual.Should().Be.False();
        }
    }
}
