//extern alias tpx;

namespace Radical.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical;

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
            EnumItemDescriptionAttribute target = this.CreateMock(caption);

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_ctor_caption_index()
        {
            string caption = "fake caption";
            int index = 0;
            EnumItemDescriptionAttribute target = this.CreateMock(caption, index);

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_caption()
        {
            string caption = "fake caption";
            EnumItemDescriptionAttribute target = this.CreateMock(caption);

            Assert.AreEqual<string>(caption, target.Caption);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_valid_caption()
        {
            string expectedCaption = "fake caption";
            int expectedIndex = 0;
            EnumItemDescriptionAttribute target = this.CreateMock(expectedCaption, expectedIndex);

            Assert.AreEqual<string>(expectedCaption, target.Caption);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_valid_index()
        {
            string expectedcaption = "fake caption";
            int expectedIndex = 0;
            EnumItemDescriptionAttribute target = this.CreateMock(expectedcaption, expectedIndex);

            Assert.AreEqual<int>(expectedIndex, target.Index);
        }

        [TestMethod()]
        public void EnumItemDescriptionAttribute_Index()
        {
            EnumItemDescriptionAttribute target = this.CreateMock("fake caption");
            int actual = target.Index;

            Assert.AreEqual<int>(-1, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumItemDescriptionAttribute_ctor_null_caption()
        {
            EnumItemDescriptionAttribute target = this.CreateMock(null);
        }
    }
}
