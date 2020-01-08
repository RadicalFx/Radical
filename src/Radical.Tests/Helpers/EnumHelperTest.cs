//extern alias tpx;

namespace Radical.Tests.Helpers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical;
    using Radical.Helpers;
    using SharpTestsEx;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass()]
    public class EnumHelperTest
    {
        private enum TestEnum
        {
            None = 0,

            [EnumItemDescription("Value 1", 0)]
            Value1 = 1,

            [EnumItemDescription("Value 2", 2)]
            Value2 = 2,

            [EnumItemDescription("Value 3", 1)]
            Value3 = 3,
        }

        [TestMethod()]
        public void ExtractDescriptionsTest()
        {
            string[] expected = new[] { "Value 1", "Value 3", "Value 2" };
            IEnumerable<string> actual = EnumHelper.ExtractDescriptions<TestEnum>();

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod()]
        public void ExtractDescriptionsTest_ensure_order()
        {
            string[] expected = new[] { "Value 1", "Value 3", "Value 2" };
            IEnumerable<string> actual = EnumHelper.ExtractDescriptions<TestEnum>();

            /*
             * ExtractDescriptions dovrebbe rispettare l'ordine imposto
             * dalla proprietà Index dell'attributo
             */
            Assert.AreEqual<int>(expected.Length, actual.Count());
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual<string>(expected[i], actual.ElementAt(i));
            }
        }

        [TestMethod()]
        public void ExtractBindingDataTest()
        {
            var actual = EnumHelper.ExtractBindingData<TestEnum>();

            int expectedCount = 3;
            Assert.AreEqual<int>(expectedCount, actual.Count());

            Assert.AreEqual<TestEnum>(TestEnum.Value1, actual.ElementAt(0).Value);
            Assert.AreEqual<TestEnum>(TestEnum.Value3, actual.ElementAt(1).Value);
            Assert.AreEqual<TestEnum>(TestEnum.Value2, actual.ElementAt(2).Value);
        }
    }
}
