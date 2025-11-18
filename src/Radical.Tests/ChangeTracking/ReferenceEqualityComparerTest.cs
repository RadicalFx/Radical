using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Radical.Tests.ChangeTracking
{
    [TestClass()]
    public class ReferenceEqualityComparerTest
    {
        [TestMethod()]
        [TestCategory("ChangeTracking")]
        public void referenceEqualityComparer_ctor()
        {
            ReferenceEqualityComparer<TestTypeHelper> instance = new ReferenceEqualityComparer<TestTypeHelper>();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iEqualityComparer_equals_with_equal_instances()
        {
            var instance = new ReferenceEqualityComparer<TestTypeHelper>();

            var x = new TestTypeHelper();
            var y = x;

            bool actual = instance.Equals(x, y);

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iEqualityComparer_equals_with_instance_and_null()
        {
            var instance = new ReferenceEqualityComparer<TestTypeHelper>();

            var x = new TestTypeHelper();
            bool actual = instance.Equals(x, null);

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iEqualityComparer_equals_with_null_and_instance()
        {
            var instance = new ReferenceEqualityComparer<TestTypeHelper>();

            var y = new TestTypeHelper();
            bool actual = instance.Equals(null, y);

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iEqualityComparer_equals_with_null_and_null()
        {
            var instance = new ReferenceEqualityComparer<TestTypeHelper>();
            bool actual = instance.Equals(null, null);

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iEqualityComparer_getHashCode()
        {
            IEqualityComparer<TestTypeHelper> instance = new ReferenceEqualityComparer<TestTypeHelper>();

            var obj = new TestTypeHelper();
            int expected = obj.GetHashCode();

            int actual = instance.GetHashCode(obj);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void generic_iEqualityComparer_getHashCode_null_reference()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                IEqualityComparer<TestTypeHelper> instance = new ReferenceEqualityComparer<TestTypeHelper>();
                int actual = instance.GetHashCode(null);
            });
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void iEqualityComparer_equals_with_equal_instances()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<TestTypeHelper>();

            var x = new TestTypeHelper();
            var y = x;

            bool actual = instance.Equals(x, y);

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void iEqualityComparer_equals_with_instance_and_null()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<TestTypeHelper>();

            var x = new TestTypeHelper();
            bool actual = instance.Equals(x, null);

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void iEqualityComparer_equals_with_null_and_instance()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<TestTypeHelper>();

            var y = new TestTypeHelper();
            bool actual = instance.Equals(null, y);

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void iEqualityComparer_equals_with_null_and_null()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<TestTypeHelper>();
            bool actual = instance.Equals(null, null);

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void iEqualityComparer_getHashCode()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<TestTypeHelper>();
            var obj = new TestTypeHelper();

            int expected = obj.GetHashCode();
            int actual = instance.GetHashCode(obj);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void iEqualityComparer_getHashCode_null_reference()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                IEqualityComparer instance = new ReferenceEqualityComparer<TestTypeHelper>();
                int actual = instance.GetHashCode(null);
            });
        }
    }
}
