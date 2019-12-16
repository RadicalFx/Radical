using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Radical.Tests.Exceptions
{
    /// <summary>
    ///This is a test class for RadicalExceptionTest and is intended
    ///to contain all RadicalExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RadicalExceptionTest
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        protected virtual Exception CreateMock()
        {
            return new RadicalException();
        }

        protected virtual Exception CreateMock(string message)
        {
            return new RadicalException(message);
        }

        protected virtual Exception CreateMock(string message, Exception innerException)
        {
            return new RadicalException(message, innerException);
        }

        protected Exception Process(Exception source)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, source);

                ms.Position = 0;

                Object resVal = formatter.Deserialize(ms);
                return resVal as Exception;
            }
        }

        protected virtual void AssertAreEqual(Exception ex1, Exception ex2)
        {

        }

        [TestMethod()]
        public void ctor_default()
        {
            Exception target = this.CreateMock();
            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void serialization()
        {
            Exception expected = this.CreateMock();
            Exception target = this.Process(expected);

            AssertAreEqual(expected, target);
        }

        [TestMethod()]
        public void ctor_string()
        {
            string expectedMessage = "message";
            Exception target = this.CreateMock(expectedMessage);

            Assert.AreEqual<string>(expectedMessage, target.Message);
            Assert.IsNull(target.InnerException);
        }

        [TestMethod()]
        public void ctor_string_innerException()
        {
            string expectedMessage = "message";
            Exception expectedInnerException = new StackOverflowException();

            Exception target = this.CreateMock(expectedMessage, expectedInnerException);

            Assert.AreEqual<string>(expectedMessage, target.Message);
            Assert.AreEqual<Exception>(expectedInnerException, target.InnerException);
        }
    }
}
