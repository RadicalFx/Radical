using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using System.Collections.Generic;

namespace Radical.Tests.ChangeTracking
{
    [TestClass()]
    public class ObjectReferenceEqualityComparerTest
    {
        [TestMethod()]
        [TestCategory("ChangeTracking")]
        public void Instance_is_not_null()
        {
            IEqualityComparer<object> actual = ObjectReferenceEqualityComparer.Instance;

            actual.Should().Not.Be.Null();
        }

        [TestMethod()]
        [TestCategory("ChangeTracking")]
        public void Instance_is_singleton()
        {
            IEqualityComparer<object> expected = ObjectReferenceEqualityComparer.Instance;
            IEqualityComparer<object> actual = ObjectReferenceEqualityComparer.Instance;

            actual.Should().Be.EqualTo(expected);
        }
    }
}
