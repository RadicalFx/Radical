//extern alias tpx;

namespace Radical.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical;
    using System;

    [TestClass()]
    public class EnumItemDescriptionAttributeTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        protected virtual EnumItemDescriptionAttribute CreateMock(string caption)
        {
            return new EnumItemDescriptionAttribute(caption);
        }

        protected virtual EnumItemDescriptionAttribute CreateMock(string caption, int index)
        {
            return new EnumItemDescriptionAttribute(caption, index);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_ctor_caption()
        {
            string caption = "fake caption";
            EnumItemDescriptionAttribute target = CreateMock(caption);

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_ctor_caption_index()
        {
            string caption = "fake caption";
            int index = 0;
            EnumItemDescriptionAttribute target = CreateMock(caption, index);

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_caption()
        {
            string caption = "fake caption";
            EnumItemDescriptionAttribute target = CreateMock(caption);

            Assert.AreEqual<string>(caption, target.Caption);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_valid_caption()
        {
            string expectedCaption = "fake caption";
            int expectedIndex = 0;
            EnumItemDescriptionAttribute target = CreateMock(expectedCaption, expectedIndex);

            Assert.AreEqual<string>(expectedCaption, target.Caption);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_valid_index()
        {
            string expectedcaption = "fake caption";
            int expectedIndex = 0;
            EnumItemDescriptionAttribute target = CreateMock(expectedcaption, expectedIndex);

            Assert.AreEqual<int>(expectedIndex, target.Index);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_Index()
        {
            EnumItemDescriptionAttribute target = CreateMock("fake caption");
            int actual = target.Index;

            Assert.AreEqual<int>(-1, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumItemDescriptionAttribute_ctor_null_caption()
        {
            EnumItemDescriptionAttribute target = CreateMock(null);
        }
    }
}
