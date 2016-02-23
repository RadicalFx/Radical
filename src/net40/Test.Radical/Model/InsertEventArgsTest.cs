namespace Test.Radical.Model
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Topics.Radical.Model;

    [TestClass()]
    public class InsertEventArgsTest
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
        public void InsertEventArgs_ctor_default_values()
        {
            string expectedValue = "Foo";
            int expectedIndex = 10;

            InsertEventArgs<string> obj = new InsertEventArgs<string>( expectedIndex, expectedValue );

            Assert.IsFalse( obj.Cancel );
            Assert.AreEqual<int>( expectedIndex, obj.Index );
            Assert.AreEqual<string>( expectedValue, obj.NewValue );
        }
    }
}
