namespace Radical.Tests.Model
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.Model;

    [TestClass()]
    public class SetValueAtEventArgsTest
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
        public void SetValueAtEventArgs_ctor_default_values()
        {
            string expectedOldValue = "Old Foo";
            string expectedNewValue = "New Foo";
            int expectedIndex = 10;

            SetValueAtEventArgs<string> obj = new SetValueAtEventArgs<string>( expectedIndex, expectedNewValue, expectedOldValue );

            Assert.IsFalse( obj.Cancel );
            Assert.AreEqual<int>( expectedIndex, obj.Index );
            Assert.AreEqual<string>( expectedNewValue, obj.NewValue );
            Assert.AreEqual<string>( expectedOldValue, obj.OldValue );
        }
    }
}
