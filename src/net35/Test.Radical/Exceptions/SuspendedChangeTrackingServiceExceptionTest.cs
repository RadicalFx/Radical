using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical;

namespace Test.Radical.Exceptions
{
	[TestClass()]
	public class SuspendedChangeTrackingServiceExceptionTest : RadicalExceptionTest
	{
		protected override Exception CreateMock()
		{
			return new SuspendedChangeTrackingServiceException();
		}

		protected override Exception CreateMock( String message )
		{
			return new SuspendedChangeTrackingServiceException( message );
		}

		protected override Exception CreateMock( String message, Exception innerException )
		{
			return new SuspendedChangeTrackingServiceException( message, innerException );
		}
	}
}
