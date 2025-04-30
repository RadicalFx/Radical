using MessagePack;
using MessagePack.Resolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Radical.Tests.Exceptions
{
    /// <summary>
    ///This is a test class for RadicalExceptionTest and is intended
    ///to contain all RadicalExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RadicalExceptionTest
    {
        [TestMethod()]
        public void serialization()
        {
            var expected = new RadicalException();
            var target = expected.SerializeAndDeserialize();

            Assert.AreEqual(expected.GetType(), target.GetType());
            Assert.AreEqual(expected.Message, target.Message);
            Assert.AreEqual(expected.InnerException?.Message, target.InnerException?.Message);
            Assert.AreEqual(expected.StackTrace, target.StackTrace);
        }

        [TestMethod()]
        public void ctor_string()
        {
            string expectedMessage = "message";
            Exception target = new RadicalException(expectedMessage);

            Assert.AreEqual(expectedMessage, target.Message);
            Assert.IsNull(target.InnerException);
        }

        [TestMethod()]
        public void ctor_string_innerException()
        {
            string expectedMessage = "message";
            Exception expectedInnerException = new StackOverflowException();

            Exception target = new RadicalException(expectedMessage, expectedInnerException);

            Assert.AreEqual(expectedMessage, target.Message);
            Assert.AreEqual(expectedInnerException, target.InnerException);
        }
    }
}
