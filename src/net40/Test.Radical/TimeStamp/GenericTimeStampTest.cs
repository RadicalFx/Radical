//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical.ComponentModel;

namespace Test.Radical
{
    [TestClass()]
    public class GenericTimeStampTest : TimeStampTest
    {
        const Int32 VALUE = 1000;

        public override Timestamp CreateMock()
        {
            Timestamp<Int32> ts = new Timestamp<Int32>( VALUE );

            return this.CreateMock<Int32>( VALUE );
        }

        public Timestamp<T> CreateMock<T>( T initialValue )
        {
            Timestamp<T> ts = new Timestamp<T>( initialValue );

            return ts;
        }

        protected override void AssertGetHashCode( Timestamp ts )
        {
            unchecked
            {
                Timestamp<Int32> target = ( Timestamp<Int32> )ts;

                Int32 expected = ( target.Value * 35 ) ^ 73;
                Int32 actual = target.GetHashCode();

                Assert.AreEqual<Int32>( expected, actual );
            }
        }

        [TestMethod()]
        public void valueTest()
        {
            Timestamp ts = this.CreateMock();
            Timestamp<Int32> target = ( Timestamp<Int32> )ts;

            Int32 expected = VALUE;
            Int32 actual = target.Value;

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void value_ctor_Test()
        {
            Timestamp<Object> target = this.CreateMock<Object>( null );
        }

        [TestMethod()]
        public void implicit_op_from_T_to_timeStamp()
        {
            Timestamp<Int32> target = VALUE;

            Assert.IsNotNull( target );
            Assert.AreEqual<Int32>( VALUE, target.Value );
        }

        [TestMethod()]
        public void implicit_op_from_timeStamp_to_T()
        {
            Timestamp<Int32> target = new Timestamp<int>( VALUE );
            Int32 actual = target;

            Assert.AreEqual<Int32>( VALUE, actual );
        }

        [TestMethod()]
        public void implicit_op_from_null_timeStamp_to_T()
        {
            Timestamp<Int32> target = null;
            Int32 actual = target;

            Assert.AreEqual<Int32>( 0, actual );
        }

        [TestMethod]
        public void genericTimeStamp_equals_to_null_timestamp_should_return_false()
        {
            Timestamp<Int32> target = new Timestamp<int>( VALUE );
            Boolean actual = target.Equals( ( Timestamp )null );

            actual.Should().Be.False();
        }

        [TestMethod]
        public void genericTimeStamp_equals_using_non_iComparbale_values_should_return_true_using_same_reference()
        {
            Object val = new Object();

            Timestamp<Object> v1 = new Timestamp<Object>( val );
            Timestamp<Object> v2 = new Timestamp<Object>( val );
            Boolean actual = v1.Equals( v2 );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericTimeStamp_equals_using_non_iComparbale_values_should_return_false_using_different_reference()
        {
            Object val1 = new Object();
            Object val2 = new Object();

            Timestamp<Object> v1 = new Timestamp<Object>( val1 );
            Timestamp<Object> v2 = new Timestamp<Object>( val2 );
            Boolean actual = v1.Equals( v2 );

            actual.Should().Be.False();
        }
    }
}
