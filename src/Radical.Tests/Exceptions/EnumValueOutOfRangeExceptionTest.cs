using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Radical.Tests.Exceptions
{
    [TestClass()]
    public class EnumValueOutOfRangeExceptionTest
    {
        Exception Process(Exception source)
        {
            using MemoryStream ms = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, source);
            ms.Position = 0;

            return formatter.Deserialize(ms) as Exception;
        }

        [TestMethod()]
        public void serialization()
        {
            Exception expected = new EnumValueOutOfRangeException();
            Exception target = Process(expected);

            Assert.AreEqual(expected.GetType(), target.GetType());
            Assert.AreEqual(expected.Message, target.Message);
            Assert.AreEqual(expected.InnerException?.Message, target.InnerException?.Message);
            Assert.AreEqual(expected.StackTrace, target.StackTrace);
        }

        [TestMethod()]
        public void ctor_string()
        {
            string expectedMessage = "message";
            Exception target = new EnumValueOutOfRangeException(expectedMessage);

            Assert.AreEqual(expectedMessage, target.Message);
            Assert.IsNull(target.InnerException);
        }

        [TestMethod()]
        public void ctor_string_innerException()
        {
            string expectedMessage = "message";
            Exception expectedInnerException = new StackOverflowException();

            Exception target = new EnumValueOutOfRangeException(expectedMessage, expectedInnerException);

            Assert.AreEqual(expectedMessage, target.Message);
            Assert.AreEqual(expectedInnerException, target.InnerException);
        }
    }
}
