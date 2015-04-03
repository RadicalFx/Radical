namespace Test.Radical.Extensions
{
	using System;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Rhino.Mocks;
	using SharpTestsEx;
	using Topics.Radical;

	[TestClass]
	public class ArrayExtensionsTests
	{
		[TestMethod]
		public void arrayExtensions_isSameAs_normal_should_return_arrays_equality()
		{
			var actual = ArrayExtensions.IsSameAs( new[] { 1, 2, 3 }, new[] { 1, 2, 3 } );

			actual.Should().Be.True();
		}

		[TestMethod]
		public void arrayExtensions_isSameAs_using_empty_arrays_should_return_arrays_equality()
		{
			var actual = ArrayExtensions.IsSameAs( new Int32[ 0 ], new Int32[ 0 ] );

			actual.Should().Be.True();
		}

		[TestMethod]
		public void arrayExtensions_isSameAs_using_same_reference_arrays_should_return_arrays_equality()
		{
			var data = new[] { 1, 2, 3 };
			var actual = ArrayExtensions.IsSameAs( data, data );

			actual.Should().Be.True();
		}

		[TestMethod]
		public void arrayExtensions_isSameAs_using_arrays_of_different_len_should_return_arrays_disequality()
		{
			var actual = ArrayExtensions.IsSameAs( new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3 } );

			actual.Should().Be.False();
		}

		[TestMethod]
		public void arrayExtensions_isSameAs_using_arrays_of_different_content_should_return_arrays_disequality()
		{
			var actual = ArrayExtensions.IsSameAs( new[] { 4, 5, 6 }, new[] { 1, 2, 3 } );

			actual.Should().Be.False();
		}

		[TestMethod]
		public void arrayExtensions_isSameAs_using_arrays_of_different_content_and_different_len_should_return_arrays_disequality()
		{
			var actual = ArrayExtensions.IsSameAs( new[] { 4, 5, 6, 7 }, new[] { 1, 2, 3, 4, 5, 6, 7 } );

			actual.Should().Be.False();
		}

		[TestMethod]
		public void arrayExtensions_isSameAs_using_null_other_should_return_arrays_disequality()
		{
			var actual = ArrayExtensions.IsSameAs( new[] { 4, 5, 6 }, null );

			actual.Should().Be.False();
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void arrayExtensions_isSameAs_using_null_source_should_raise_ArgumentNullException()
		{
			ArrayExtensions.IsSameAs( null, new[] { 1, 2, 3 } );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void arrayExtensions_isSameAs_using_null_comparer_should_raise_ArgumentNullException()
		{
			ArrayExtensions.IsSameAs( new Object[ 0 ], null, null );
		}
	}
}
