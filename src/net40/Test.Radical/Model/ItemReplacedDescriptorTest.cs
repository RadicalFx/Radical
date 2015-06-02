using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.ChangeTracking.Specialized;
using SharpTestsEx;

namespace Test.Radical.Model
{
	[TestClass()]
	public class ItemReplacedDescriptorTest
	{
		[TestMethod]
		public void itemReplacedDescriptor_ctor_normal_should_set_expected_values()
		{
			var newItem = new GenericParameterHelper();
			var replacedItem = new GenericParameterHelper();
			var index = 102;

			var target = new ItemReplacedDescriptor<GenericParameterHelper>( newItem, replacedItem, index );

			target.Index.Should().Be.EqualTo( index );
			target.Item.Should().Be.EqualTo( newItem );
			target.ReplacedItem.Should().Be.EqualTo( replacedItem );
			target.NewItem.Should().Be.EqualTo( newItem );
		}
	}
}
