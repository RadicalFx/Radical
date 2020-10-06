namespace Radical.Tests.Helpers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical;
    using Radical.DataBinding;
    using System;

    [TestClass()]
    public class EnumBinderTest
    {
        private enum TestEnum
        {
            Value1,
            Value2,
            Value3
        }

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

        [TestMethod()]
        public void EnumBinder_enumItemDescriptionAttribute_object_ctor()
        {
            EnumItemDescriptionAttribute attribute = new EnumItemDescriptionAttribute("");
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(attribute, value);

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumBinder_enumItemDescriptionAttribute_object_ctor_null_attribute()
        {
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>((EnumItemDescriptionAttribute)null, value);
        }

        [TestMethod()]
        public void EnumBinder_enumItemDescriptionAttribute_object_ctor_description()
        {
            EnumItemDescriptionAttribute attribute = new EnumItemDescriptionAttribute("item description");
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(attribute, value);

            Assert.AreEqual<string>(attribute.Caption, target.Caption);
        }

        [TestMethod()]
        public void EnumBinder_enumItemDescriptionAttribute_object_ctor_value()
        {
            EnumItemDescriptionAttribute attribute = new EnumItemDescriptionAttribute("item description");
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(attribute, value);

            Assert.AreEqual<object>(value, target.Value);
        }

        [TestMethod()]
        public void EnumBinder_enumItemDescriptionAttribute_object_ctor_index()
        {
            var attribute = new EnumItemDescriptionAttribute("item description", 10);
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(attribute, value);

            Assert.AreEqual<int>(attribute.Index, target.Index);
        }

        [TestMethod()]
        public void EnumBinder_string_object_ctor()
        {
            var description = "description";
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(description, value);

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void EnumBinder_string_object_int32_ctor()
        {
            var description = "description";
            var value = TestEnum.Value2;
            var index = 0;
            var target = new EnumBinder<TestEnum>(description, value, index);

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumBinder_string_object_null_description()
        {
            var description = (string)null;
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(description, value);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumBinder_string_object_int32_null_description()
        {
            var description = (string)null;
            var value = TestEnum.Value2;
            var index = 0;
            var target = new EnumBinder<TestEnum>(description, value, index);
        }

        [TestMethod()]
        public void EnumBinder_string_object_ctor_description()
        {
            var description = "description";
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(description, value);

            Assert.AreEqual<string>(description, target.Caption);
        }

        [TestMethod()]
        public void EnumBinder_string_object_ctor_value()
        {
            var description = "description";
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(description, value);

            Assert.AreEqual<object>(value, target.Value);
        }

        [TestMethod()]
        public void EnumBinder_string_object_ctor_index()
        {
            var description = "description";
            var value = TestEnum.Value2;
            var target = new EnumBinder<TestEnum>(description, value);

            Assert.AreEqual<object>(-1, target.Index);
        }

        [TestMethod()]
        public void EnumBinder_string_object_int32_ctor_description()
        {
            var description = "description";
            var value = TestEnum.Value2;
            var index = 0;
            var target = new EnumBinder<TestEnum>(description, value, index);

            Assert.AreEqual<string>(description, target.Caption);
        }

        [TestMethod()]
        public void EnumBinder_string_object_int32_ctor_value()
        {
            var description = "description";
            var value = TestEnum.Value2;
            var index = 0;
            var target = new EnumBinder<TestEnum>(description, value, index);

            Assert.AreEqual<object>(value, target.Value);
        }

        [TestMethod()]
        public void EnumBinder_string_object_int32_ctor_index()
        {
            var description = "description";
            var value = TestEnum.Value2;
            var index = 0;
            var target = new EnumBinder<TestEnum>(description, value, index);

            Assert.AreEqual<object>(index, target.Index);
        }
    }
}
