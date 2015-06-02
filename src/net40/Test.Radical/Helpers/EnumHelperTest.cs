//extern alias tpx;

namespace Test.Radical.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using SharpTestsEx;
	using Topics.Radical;
	using Topics.Radical.Helpers;

	[TestClass()]
	public class EnumHelperTest
	{
		private enum TestEnum
		{
			None = 0,

			[EnumItemDescription( "Value 1", 0 )]
			Value1 = 1,

			[EnumItemDescription( "Value 2", 2 )]
			Value2 = 2,

			[EnumItemDescription( "Value 3", 1 )]
			Value3 = 3,
		}

		[TestMethod()]
		public void ExtractDescriptionsTest()
		{
			String[] expected = new[] { "Value 1", "Value 3", "Value 2" };
			IEnumerable<String> actual = EnumHelper.ExtractDescriptions<TestEnum>();

			actual.Should().Have.SameSequenceAs( expected );
		}

		[TestMethod()]
		public void ExtractDescriptionsTest_ensure_order()
		{
			String[] expected = new[] { "Value 1", "Value 3", "Value 2" };
			IEnumerable<String> actual = EnumHelper.ExtractDescriptions<TestEnum>();

			/*
			 * ExtractDescriptions dovrebbe rispettare l'ordine imposto
			 * dalla proprietà Index dell'attributo
			 */
			Assert.AreEqual<Int32>( expected.Length, actual.Count() );
			for( Int32 i = 0; i < expected.Length; i++ )
			{
				Assert.AreEqual<String>( expected[ i ], actual.ElementAt( i ) );
			}
		}

		[TestMethod()]
		public void ExtractBindingDataTest()
		{
			var actual = EnumHelper.ExtractBindingData<TestEnum>();

			Int32 expectedCount = 3;
			Assert.AreEqual<Int32>( expectedCount, actual.Count() );

			Assert.AreEqual<TestEnum>( TestEnum.Value1, actual.ElementAt( 0 ).Value );
			Assert.AreEqual<TestEnum>( TestEnum.Value3, actual.ElementAt( 1 ).Value );
			Assert.AreEqual<TestEnum>( TestEnum.Value2, actual.ElementAt( 2 ).Value );
		}
	}
}
