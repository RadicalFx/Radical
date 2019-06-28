using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical;

namespace Radical.Tests.Exceptions
{
    [TestClass()]
    public class EnumValueOutOfRangeExceptionTest : RadicalExceptionTest
    {
        protected override Exception CreateMock()
        {
            return new EnumValueOutOfRangeException();
        }

        protected override Exception CreateMock( string message )
        {
            return new EnumValueOutOfRangeException( message );
        }

        protected override Exception CreateMock( string message, Exception innerException )
        {
            return new EnumValueOutOfRangeException( message, innerException );
        }
    }
}
