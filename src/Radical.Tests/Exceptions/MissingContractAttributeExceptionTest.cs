//extern alias tpx;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Radical.Tests.Exceptions
{
    [TestClass()]
    public class MissingContractAttributeExceptionTest
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
            Exception expected = new MissingContractAttributeException(typeof(MissingContractAttributeExceptionTest));
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
            Exception target = new MissingContractAttributeException(typeof(MissingContractAttributeExceptionTest), expectedMessage);

            Assert.AreEqual(expectedMessage, target.Message);
            Assert.IsNull(target.InnerException);
        }

        [TestMethod()]
        public void ctor_string_innerException()
        {
            string expectedMessage = "message";
            Exception expectedInnerException = new StackOverflowException();

            Exception target = new MissingContractAttributeException(expectedMessage, expectedInnerException);

            Assert.AreEqual(expectedMessage, target.Message);
            Assert.AreEqual(expectedInnerException, target.InnerException);
        }

        [TestMethod()]
        public void ctor_systemType()
        {
            Type expected = typeof(string);
            string expectedmessage = string.Format(CultureInfo.CurrentCulture, "ContractAttribute missing on type: {0}.", expected.FullName);

            MissingContractAttributeException target = new MissingContractAttributeException(expected);

            Assert.AreEqual<string>(expectedmessage, target.Message);
            Assert.AreEqual<Type>(expected, target.TargetType);
        }
    }
}
