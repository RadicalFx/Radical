namespace Radical.Tests.Helpers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical;
    using SharpTestsEx;

    [TestClass]
    public class NumbersExtensionsTests
    {
        [TestMethod]
        public void numbersExtensions_isEven_even_number_should_be_true()
        {
            int target = 0;
            bool actual = NumbersExtensions.IsEven(target);

            actual.Should().Be.True();
        }

        [TestMethod]
        public void numbersExtensions_isEvent_non_even_number_should_be_false()
        {
            int target = 1;
            bool actual = NumbersExtensions.IsEven(target);

            actual.Should().Be.False();
        }
    }
}
