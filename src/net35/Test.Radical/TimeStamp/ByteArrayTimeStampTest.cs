//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.ComponentModel;

namespace Test.Radical
{
	[TestClass()]
	public class ByteArrayTimeStampTest : TimeStampTest
	{
		Byte[] VALUE = new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };

		public override Timestamp CreateMock()
		{
			ByteArrayTimestamp ts = new ByteArrayTimestamp( VALUE );
			return ts;
		}

		protected override void AssertGetHashCode( Timestamp ts )
		{
			unchecked
			{
				ByteArrayTimestamp target = ( ByteArrayTimestamp )ts;

				Int32 expected = ( target.Value.GetHashCode() * 35 ) ^ 73;
				Int32 actual = target.GetHashCode();

				Assert.AreEqual<Int32>( expected, actual );
			}
		}

		void AssertValueAreEqual( Byte[] expected, ByteArrayTimestamp baActual )
		{
			Byte[] actual = baActual.Value;

			Assert.AreEqual<Int32>( expected.Length, actual.Length );

			for( Int32 i = 0; i < expected.Length; i++ )
			{
				Assert.AreEqual<Byte>( expected[ i ], actual[ i ] );
			}
		}

		[TestMethod()]
		public void valueTest()
		{
			Timestamp ts = this.CreateMock();
			ByteArrayTimestamp target = ( ByteArrayTimestamp )ts;

			AssertValueAreEqual( VALUE, target );
		}

		[TestMethod()]
		public void empty_is_singleton()
		{
			ByteArrayTimestamp expected = ByteArrayTimestamp.Empty;
			ByteArrayTimestamp actual = ByteArrayTimestamp.Empty;

			Assert.AreEqual<ByteArrayTimestamp>( expected, actual );
		}

		[TestMethod()]
		public void implicit_op_from_byteArray_to_timeStamp()
		{
			ByteArrayTimestamp target = VALUE;

			AssertValueAreEqual( VALUE, target );
		}

		[TestMethod()]
		public void implicit_op_from_timeStamp_to_byteArray()
		{
			ByteArrayTimestamp target = new ByteArrayTimestamp( VALUE );
			Byte[] actual = target;

			AssertValueAreEqual( actual, target );
		}

		[TestMethod()]
		public void byteArrayTimeStamp_empty_toString()
		{
			String expected = "";

			ByteArrayTimestamp target = new ByteArrayTimestamp( new Byte[ 0 ] );
			String actual = target.ToString();

			Assert.AreEqual<String>( expected, actual );
		}

		[TestMethod()]
		public void byteArrayTimeStamp_toString()
		{
			String expected = "00:0C:37:FF";

			ByteArrayTimestamp target = new ByteArrayTimestamp( new Byte[]{ 0, 12, 55, 255 } );
			String actual = target.ToString();

			Assert.AreEqual<String>( expected, actual );
		}

		[TestMethod()]
		public void byteArrayTimeStamp_Equals_with_different_value_timeStamp()
		{
			ByteArrayTimestamp v1 = new ByteArrayTimestamp( new Byte[] { 0, 12, 55, 255 } );
			ByteArrayTimestamp v2 = new ByteArrayTimestamp( new Byte[] { 0, 1, 55, 255 } );

			Boolean expected = false;
			Boolean actual = v1.Equals( v2 );

			Assert.AreEqual( expected, actual );
		}

		[TestMethod()]
		public void byteArrayTimeStamp_Equals_with_different_len_timeStamp()
		{
			ByteArrayTimestamp v1 = new ByteArrayTimestamp( new Byte[] { 0, 12, 55, 255 } );
			ByteArrayTimestamp v2 = new ByteArrayTimestamp( new Byte[] { 0, 1, 55 } );

			Boolean expected = false;
			Boolean actual = v1.Equals( v2 );

			Assert.AreEqual( expected, actual );
		}

		[TestMethod()]
		public void byteArrayTimeStamp_Equals_with_null()
		{
			ByteArrayTimestamp v1 = new ByteArrayTimestamp( new Byte[] { 0, 12, 55, 255 } );
			ByteArrayTimestamp v2 = null;

			Boolean expected = false;
			Boolean actual = v1.Equals( v2 );

			Assert.AreEqual( expected, actual );
		}
	}
}
