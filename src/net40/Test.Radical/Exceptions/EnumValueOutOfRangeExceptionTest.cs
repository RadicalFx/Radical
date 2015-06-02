using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical;

namespace Test.Radical.Exceptions
{
	[TestClass()]
	public class EnumValueOutOfRangeExceptionTest : RadicalExceptionTest
	{
		protected override Exception CreateMock()
		{
			return new EnumValueOutOfRangeException();
		}

		protected override Exception CreateMock( String message )
		{
			return new EnumValueOutOfRangeException( message );
		}

		protected override Exception CreateMock( String message, Exception innerException )
		{
			return new EnumValueOutOfRangeException( message, innerException );
		}
	}
}
