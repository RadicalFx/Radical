namespace Radical.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.ComponentModel;
    using System;

    [TestClass()]
    public class ContractAttributeTest
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

        [TestMethod()]
        public void ContractAttribute_default_ctor()
        {
            var target = new ContractAttribute();

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void ContractAttribute_ctor()
        {
            ContractAttribute target = new ContractAttribute(typeof(object));

            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void ContractAttribute_ctor_argumentNullException_on_null_contract()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                ContractAttribute target = new ContractAttribute(null);
            });
        }

        [TestMethod()]
        public void ContractAttribute_contractInterfaceProperty_via_ctor()
        {
            Type expected = typeof(object);
            ContractAttribute target = new ContractAttribute(expected);

            Assert.AreEqual<Type>(expected, target.ContractInterface);
        }

        [TestMethod()]
        public void ContractAttribute_contractInterfaceProperty_via_default_ctor()
        {
            var target = new ContractAttribute();
            Assert.IsNull(target.ContractInterface);
        }
    }
}
