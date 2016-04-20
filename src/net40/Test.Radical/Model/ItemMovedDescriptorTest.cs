
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.ChangeTracking.Specialized;
using SharpTestsEx;

namespace Test.Radical.Model
{
    [TestClass()]
    public class ItemMovedDescriptorTest
    {
        [TestMethod]
        public void itemMovedDescriptor_ctor_normal_should_set_expected_values()
        {
            var item = new GenericParameterHelper();
            var newIndex = 50;
            var oldIndex = 2;

            var target = new ItemMovedDescriptor<GenericParameterHelper>( item, newIndex, oldIndex );

            target.Index.Should().Be.EqualTo( newIndex );
            target.Item.Should().Be.EqualTo( item );
            target.NewIndex.Should().Be.EqualTo( newIndex );
            target.OldIndex.Should().Be.EqualTo( oldIndex );
        }
    }
}
