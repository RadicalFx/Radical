//extern alias tpx;

using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical;

namespace Test.Radical.Exceptions
{
	[TestClass()]
	public class MissingContractAttributeExceptionTest : RadicalExceptionTest
	{
		protected override Exception CreateMock()
		{
			return new MissingContractAttributeException();
		}

		protected override Exception CreateMock( String message )
		{
			return new MissingContractAttributeException( message );
		}

		protected override Exception CreateMock( String message, Exception innerException )
		{
			return new MissingContractAttributeException( message, innerException );
		}

		protected virtual MissingContractAttributeException CreateMock( Type targetType )
		{
			return new MissingContractAttributeException( targetType );
		}

		protected override void AssertAreEqual( Exception ex1, Exception ex2 )
		{
			base.AssertAreEqual( ex1, ex2 );

			MissingContractAttributeException mex1 = ex1 as MissingContractAttributeException;
			MissingContractAttributeException mex2 = ex2 as MissingContractAttributeException;

			Assert.IsNotNull( mex1 );
			Assert.IsNotNull( mex2 );

			Assert.AreEqual<Type>( mex1.TargetType, mex2.TargetType );
		}

		[TestMethod()]
		public void ctor_systemType()
		{
			Type expected = typeof( String );
			String expectedmessage = String.Format( CultureInfo.CurrentCulture, "ContractAttribute missing on type: {0}.", expected.FullName );

			MissingContractAttributeException target = this.CreateMock( expected );

			Assert.AreEqual<String>( expectedmessage, target.Message );
			Assert.AreEqual<Type>( expected, target.TargetType );
		}
	}
}
