using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical;
using SharpTestsEx;

namespace Test.Radical
{
	[TestClass()]
	public class ValuesTest
	{
		[TestMethod]
		public void values_ctor_normal_should_set_expected_values()
		{
			var v1 = "foo";
			var v2 = 10;

			var actual = new Values<String, Int32>( v1, v2 );

			actual.Value1.Should().Be.EqualTo( v1 );
			actual.Value2.Should().Be.EqualTo( v2 );
		}
	}
}
