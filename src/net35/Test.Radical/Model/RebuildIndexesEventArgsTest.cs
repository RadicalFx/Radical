using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.Model;
using SharpTestsEx;

namespace Test.Radical.Model
{
	[TestClass()]
	public class RebuildIndexesEventArgsTest
	{
		[TestMethod]
		public void rebuildIndexesEventArgs_ctor_normal_should_set_expected_values()
		{
			var expected = 10;

			var target = new RebuildIndexesEventArgs( expected );

			var actual = target.Index;

			actual.Should().Be.EqualTo( expected );
			target.Cancel.Should().Be.False();
		}
	}
}
