using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Topics.Radical.Validation;

namespace Radical.Tests.Validation
{
	[TestClass()]
	public class ArrayEnsureExtensionTest
	{
		[TestMethod()]
		public void arrayEnsureExtension_containsIndex_using_an_out_of_range_index_lower()
		{
            Assert.ThrowsException<ArgumentOutOfRangeException>( () =>
            {
                Int32[] data = new Int32[ 1 ] { 0 };
                Ensure.That( data ).ContainsIndex( -1 );
            } );
		}

		[TestMethod()]
		public void arrayEnsureExtension_containsIndex_using_an_out_of_range_index_lower_using_named_ensure()
		{
            Assert.ThrowsException<ArgumentOutOfRangeException>( () =>
            {
                Int32[] data = new Int32[ 1 ] { 0 };
                Ensure.That( data ).Named( "foo" ).ContainsIndex( -1 );
            } );
		}

		[TestMethod()]
		public void arrayEnsureExtension_containsIndex_using_an_out_of_range_index_upper()
		{
            Assert.ThrowsException<ArgumentOutOfRangeException>( () =>
            {
                Int32[] data = new Int32[ 1 ] { 0 };
                Ensure.That( data ).ContainsIndex( 2 );
            } );
		}

		[TestMethod()]
		public void arrayEnsureExtension_containsIndex_using_an_out_of_range_index_upper_using_named_ensure()
		{
            Assert.ThrowsException<ArgumentOutOfRangeException>( () =>
            {
                Int32[] data = new Int32[ 1 ] { 0 };
                Ensure.That( data ).Named( "foo" ).ContainsIndex( 2 );
            } );
		}

		[TestMethod()]
		public void arrayEnsureExtension_containsIndex_using_a_valid_index()
		{
			Int32[] data = new Int32[ 3 ];
			Ensure.That( data ).ContainsIndex( 2 );
		}

		[TestMethod]
		public void arrayEnsureExtension_containsIndex_using_using_an_out_of_range_index_and_preview_should_invoke_preview_before_throw()
		{
			var actual = false;

			try
			{
				var target = Ensure.That( new Int32[ 1 ] { 0 } )
					.WithPreview( ( v, e ) => actual = true );
				target.ContainsIndex( 2 );
			}
			catch( ArgumentOutOfRangeException )
			{

			}

            Assert.IsTrue(actual);
		}
	}
}
