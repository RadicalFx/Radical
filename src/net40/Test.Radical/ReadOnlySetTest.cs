namespace Test.Radical
{
	using System;
	using System.Collections.Generic;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Topics.Radical.Collections;

	[TestClass()]
	public class ReadOnlySetTest
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		[TestMethod()]
		public void ReadOnlySet_iEnumerable_ctor()
		{
			List<Int32> source = new List<Int32>();
			ReadOnlyCollection<Int32> actual = new ReadOnlyCollection<int>( source );

			Assert.IsNotNull( actual );
		}

		[TestMethod()]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void ReadOnlySet_ctor_null_source()
		{
			ReadOnlyCollection<Int32> actual = new ReadOnlyCollection<int>( null );
		}

		[TestMethod()]
		public void ReadOnlySet_count()
		{
			List<Int32> source = new List<Int32>() { 0, 1, 2, 3, 4 };
			ReadOnlyCollection<Int32> actual = new ReadOnlyCollection<int>( source );

			Assert.AreEqual<Int32>( source.Count, actual.Count );
		}

		[TestMethod()]
		public void ReadOnlySet_syncRoot()
		{
			List<Int32> source = new List<Int32>();
			ReadOnlyCollection<Int32> actual = new ReadOnlyCollection<int>( source );

			Assert.IsNotNull( actual.SyncRoot );
		}

		[TestMethod()]
		public void ReadOnlySet_isSynchronized()
		{
			List<Int32> source = new List<Int32>();
			ReadOnlyCollection<Int32> actual = new ReadOnlyCollection<int>( source );

			Assert.IsFalse( actual.IsSynchronized );
		}

		[TestMethod()]
		public void ReadOnlySet_getEnumerator()
		{
			List<Int32> source = new List<Int32>();
			ReadOnlyCollection<Int32> actual = new ReadOnlyCollection<int>( source );

			Assert.IsNotNull( actual.GetEnumerator() );
		}

		[TestMethod()]
		public void ReadOnlySet_getEnumeratorOfT()
		{
			List<Int32> source = new List<Int32>();
			ReadOnlyCollection<Int32> actual = new ReadOnlyCollection<int>( source );

			Assert.IsNotNull( ( ( IEnumerable<Int32> )actual ).GetEnumerator() );
		}

		[TestMethod()]
		public void ReadOnlySet_copyTo()
		{
			List<Int32> source = new List<Int32>() { 1, 2, 1, 3 };
			ReadOnlyCollection<Int32> actual = new ReadOnlyCollection<int>( source );

			Int32[] array = new Int32[ source.Count ];
			actual.CopyTo( array, 0 );

			for( Int32 i = 0; i < source.Count; i++ )
			{
				Assert.AreEqual<Int32>( source[ i ], array[ i ] );
			}
		}
	}
}