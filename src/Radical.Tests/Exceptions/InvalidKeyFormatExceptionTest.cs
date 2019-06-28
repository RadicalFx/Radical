using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical;

namespace Radical.Tests.Exceptions
{
    [TestClass()]
    public class InvalidKeyFormatExceptionExceptionTest : RadicalExceptionTest
    {
        protected override Exception CreateMock()
        {
            return new InvalidKeyFormatException();
        }

        protected override Exception CreateMock(string message)
        {
            return new InvalidKeyFormatException(message);
        }

        protected override Exception CreateMock(string message, Exception innerException)
        {
            return new InvalidKeyFormatException(message, innerException);
        }
    }
}
