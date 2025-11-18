using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ChangeTracking.Specialized;
using SharpTestsEx;

namespace Radical.Tests.Model
{
    [TestClass()]
    public class CollectionClearedDescriptorTest
    {
        [TestMethod]
        public void collectionClearedDescriptor_ctor_normal_should_set_expected_values()
        {
            var items = new[]
            {
                new TestTypeHelper(),
                new TestTypeHelper(),
                new TestTypeHelper()
            };

            var target = new CollectionRangeDescriptor<TestTypeHelper>(items);

            target.Items.Should().Have.SameSequenceAs(items);
        }
    }
}
