//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Radical.ComponentModel;

namespace Radical.Tests
{
    [TestClass()]
    public class GenericTimeStampTest : TimeStampTest
    {
        const int VALUE = 1000;

        public Timestamp<T> CreateMock<T>(T initialValue)
        {
            Timestamp<T> ts = new Timestamp<T>(initialValue);

            return ts;
        }

        protected override void AssertGetHashCode(Timestamp ts)
        {
            unchecked
            {
                Timestamp<int> target = (Timestamp<int>)ts;

                int expected = (target.Value * 35) ^ 73;
                int actual = target.GetHashCode();

                Assert.AreEqual<int>(expected, actual);
            }
        }

        [TestMethod()]
        public void valueTest()
        {
            var target = new Timestamp<int>(VALUE);

            int expected = VALUE;
            int actual = target.Value;

            Assert.AreEqual<int>(expected, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void value_ctor_Test()
        {
            Timestamp<Object> target = this.CreateMock<Object>(null);
        }

        [TestMethod()]
        public void implicit_op_from_T_to_timeStamp()
        {
            Timestamp<int> target = VALUE;

            Assert.IsNotNull(target);
            Assert.AreEqual<int>(VALUE, target.Value);
        }

        [TestMethod()]
        public void implicit_op_from_timeStamp_to_T()
        {
            Timestamp<int> target = new Timestamp<int>(VALUE);
            int actual = target;

            Assert.AreEqual<int>(VALUE, actual);
        }

        [TestMethod()]
        public void implicit_op_from_null_timeStamp_to_T()
        {
            Timestamp<int> target = null;
            int actual = target;

            Assert.AreEqual<int>(0, actual);
        }

        [TestMethod]
        public void genericTimeStamp_equals_to_null_timestamp_should_return_false()
        {
            Timestamp<int> target = new Timestamp<int>(VALUE);
            bool actual = target.Equals((Timestamp)null);

            actual.Should().Be.False();
        }

        [TestMethod]
        public void genericTimeStamp_equals_using_non_iComparbale_values_should_return_true_using_same_reference()
        {
            Object val = new Object();

            Timestamp<Object> v1 = new Timestamp<Object>(val);
            Timestamp<Object> v2 = new Timestamp<Object>(val);
            bool actual = v1.Equals(v2);

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericTimeStamp_equals_using_non_iComparbale_values_should_return_false_using_different_reference()
        {
            Object val1 = new Object();
            Object val2 = new Object();

            Timestamp<Object> v1 = new Timestamp<Object>(val1);
            Timestamp<Object> v2 = new Timestamp<Object>(val2);
            bool actual = v1.Equals(v2);

            actual.Should().Be.False();
        }
    }
}
