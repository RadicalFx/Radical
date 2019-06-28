//extern alias tpx;

using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel;
using System;

namespace Radical.Tests
{
    [TestClass()]
    public class TimeStampTest
    {
        Timestamp CreateMock()
        {
            var ts = A.Fake<Timestamp>();

            A.CallTo(() => ts.Equals(null))
                .Returns(true)
                .NumberOfTimes(1);

            A.CallTo(() => ts.GetHashCode())
                .Returns(0)
                .NumberOfTimes(1);

            return ts;
        }

        [TestMethod()]
        public void op_InequalityTest()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = this.CreateMock();

            bool expected = false;
            bool actual = (v1 != v2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void op_EqualityTest()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = this.CreateMock();

            bool expected = true;
            bool actual = (v1 == v2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void op_Equality_non_null_to_null()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = null;

            bool expected = false;
            bool actual = (v1 == v2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void op_Equality_null_to_non_null()
        {
            Timestamp v1 = null;
            Timestamp v2 = this.CreateMock();

            bool expected = false;
            bool actual = (v1 == v2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void timeStampEqualsTest()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = this.CreateMock();

            bool expected = true;
            bool actual = v1.Equals(v2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void objectEquals_using_non_timestamp_reference()
        {
            Timestamp v1 = this.CreateMock();
            Object v2 = new Object();

            bool expected = false;
            bool actual = v1.Equals(v2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void objectEquals_using_timestamp_reference()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = this.CreateMock();

            bool expected = true;
            bool actual = v1.Equals((Object)v2);

            Assert.AreEqual(expected, actual);
        }

        protected virtual void AssertGetHashCode(Timestamp ts)
        {
            int expected = 0;
            int actual = ts.GetHashCode();

            Assert.AreEqual<int>(expected, actual);
        }

        [TestMethod()]
        public void getHashCodeTest()
        {
            Timestamp v1 = this.CreateMock();
            this.AssertGetHashCode(v1);
        }
    }
}
