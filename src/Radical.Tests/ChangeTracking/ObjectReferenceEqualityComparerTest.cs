using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Radical;

namespace Radical.Tests.ChangeTracking
{
    [TestClass()]
    public class ObjectReferenceEqualityComparerTest
    {
        [TestMethod()]
        [TestCategory( "ChangeTracking" )]
        public void Instance_is_not_null()
        {
            IEqualityComparer<Object> actual = ObjectReferenceEqualityComparer.Instance;

            actual.Should().Not.Be.Null();
        }

        [TestMethod()]
        [TestCategory( "ChangeTracking" )]
        public void Instance_is_singleton()
        {
            IEqualityComparer<Object> expected = ObjectReferenceEqualityComparer.Instance;
            IEqualityComparer<Object> actual = ObjectReferenceEqualityComparer.Instance;

            actual.Should().Be.EqualTo( expected );
        }
    }
}
