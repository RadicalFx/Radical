using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical;

namespace Radical.Tests.Exceptions
{
    [TestClass()]
    public class SuspendedChangeTrackingServiceExceptionTest : RadicalExceptionTest
    {
        protected override Exception CreateMock()
        {
            return new SuspendedChangeTrackingServiceException();
        }

        protected override Exception CreateMock( string message )
        {
            return new SuspendedChangeTrackingServiceException( message );
        }

        protected override Exception CreateMock( string message, Exception innerException )
        {
            return new SuspendedChangeTrackingServiceException( message, innerException );
        }
    }
}
