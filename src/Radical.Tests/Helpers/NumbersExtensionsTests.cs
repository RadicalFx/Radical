namespace Radical.Tests.Helpers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpTestsEx;
    using Radical;

    [TestClass]
    public class NumbersExtensionsTests
    {
        [TestMethod]
        public void numbersExtensions_isEven_even_number_should_be_true()
        {
            int target = 0;
            bool actual = NumbersExtensions.IsEven( target );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void numbersExtensions_isEvent_non_even_number_should_be_false()
        {
            int target = 1;
            bool actual = NumbersExtensions.IsEven( target );

            actual.Should().Be.False();
        }
    }
}
