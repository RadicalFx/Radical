using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.ChangeTracking.Specialized;
using SharpTestsEx;

namespace Test.Radical.Model
{
    [TestClass()]
    public class ItemChangedDescriptorTest
    {
        [TestMethod]
        public void itemChangedDescriptor_ctor_normal_should_set_expected_values()
        {
            var item = new GenericParameterHelper();
            var index = 10;

            var target = new ItemChangedDescriptor<GenericParameterHelper>( item, index );

            target.Index.Should().Be.EqualTo( index );
            target.Item.Should().Be.EqualTo( item );
        }
    }
}
