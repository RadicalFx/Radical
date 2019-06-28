//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel;

namespace Radical.Tests
{
    [TestClass()]
    public class ByteArrayTimeStampTest : TimeStampTest
    {
        Byte[] VALUE = new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };

        protected override void AssertGetHashCode(Timestamp ts)
        {
            unchecked
            {
                ByteArrayTimestamp target = (ByteArrayTimestamp)ts;

                int expected = (target.Value.GetHashCode() * 35) ^ 73;
                int actual = target.GetHashCode();

                Assert.AreEqual<int>(expected, actual);
            }
        }

        void AssertValueAreEqual(Byte[] expected, ByteArrayTimestamp baActual)
        {
            Byte[] actual = baActual.Value;

            Assert.AreEqual<int>(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual<Byte>(expected[i], actual[i]);
            }
        }

        [TestMethod()]
        public void valueTest()
        {
            Timestamp ts = new ByteArrayTimestamp(VALUE);
            ByteArrayTimestamp target = (ByteArrayTimestamp)ts;

            AssertValueAreEqual(VALUE, target);
        }

        [TestMethod()]
        public void empty_is_singleton()
        {
            ByteArrayTimestamp expected = ByteArrayTimestamp.Empty;
            ByteArrayTimestamp actual = ByteArrayTimestamp.Empty;

            Assert.AreEqual<ByteArrayTimestamp>(expected, actual);
        }

        [TestMethod()]
        public void implicit_op_from_byteArray_to_timeStamp()
        {
            ByteArrayTimestamp target = VALUE;

            AssertValueAreEqual(VALUE, target);
        }

        [TestMethod()]
        public void implicit_op_from_timeStamp_to_byteArray()
        {
            ByteArrayTimestamp target = new ByteArrayTimestamp(VALUE);
            Byte[] actual = target;

            AssertValueAreEqual(actual, target);
        }

        [TestMethod()]
        public void byteArrayTimeStamp_empty_toString()
        {
            string expected = "";

            ByteArrayTimestamp target = new ByteArrayTimestamp(new Byte[0]);
            string actual = target.ToString();

            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod()]
        public void byteArrayTimeStamp_toString()
        {
            string expected = "00:0C:37:FF";

            ByteArrayTimestamp target = new ByteArrayTimestamp(new Byte[] { 0, 12, 55, 255 });
            string actual = target.ToString();

            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod()]
        public void byteArrayTimeStamp_Equals_with_different_value_timeStamp()
        {
            ByteArrayTimestamp v1 = new ByteArrayTimestamp(new Byte[] { 0, 12, 55, 255 });
            ByteArrayTimestamp v2 = new ByteArrayTimestamp(new Byte[] { 0, 1, 55, 255 });

            bool expected = false;
            bool actual = v1.Equals(v2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void byteArrayTimeStamp_Equals_with_different_len_timeStamp()
        {
            ByteArrayTimestamp v1 = new ByteArrayTimestamp(new Byte[] { 0, 12, 55, 255 });
            ByteArrayTimestamp v2 = new ByteArrayTimestamp(new Byte[] { 0, 1, 55 });

            bool expected = false;
            bool actual = v1.Equals(v2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void byteArrayTimeStamp_Equals_with_null()
        {
            ByteArrayTimestamp v1 = new ByteArrayTimestamp(new Byte[] { 0, 12, 55, 255 });
            ByteArrayTimestamp v2 = null;

            bool expected = false;
            bool actual = v1.Equals(v2);

            Assert.AreEqual(expected, actual);
        }
    }
}
