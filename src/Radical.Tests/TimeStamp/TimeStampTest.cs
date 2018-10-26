//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Radical.ComponentModel;

namespace Radical.Tests
{
    [TestClass()]
    public class TimeStampTest
    {
        public virtual Timestamp CreateMock()
        {
            MockRepository m = new MockRepository();
            Timestamp ts = m.PartialMock<Timestamp>();

            ts.Stub( obj => obj.Equals( null ) )
                .IgnoreArguments()
                .Repeat.Once()
                .Return( true );

            ts.Stub( obj => obj.GetHashCode() )
                .Repeat.Once()
                .Return( 0 );

            ts.Replay();

            return ts;
        }

        [TestMethod()]
        public void op_InequalityTest()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = this.CreateMock();

            Boolean expected = false;
            Boolean actual = ( v1 != v2 );

            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void op_EqualityTest()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = this.CreateMock();

            Boolean expected = true;
            Boolean actual = ( v1 == v2 );

            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void op_Equality_non_null_to_null()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = null;

            Boolean expected = false;
            Boolean actual = ( v1 == v2 );

            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void op_Equality_null_to_non_null()
        {
            Timestamp v1 = null;
            Timestamp v2 = this.CreateMock();

            Boolean expected = false;
            Boolean actual = ( v1 == v2 );

            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void timeStampEqualsTest()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = this.CreateMock();

            Boolean expected = true;
            Boolean actual = v1.Equals( v2 );

            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void objectEquals_using_non_timestamp_reference()
        {
            Timestamp v1 = this.CreateMock();
            Object v2 = new Object();

            Boolean expected = false;
            Boolean actual = v1.Equals( v2 );

            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void objectEquals_using_timestamp_reference()
        {
            Timestamp v1 = this.CreateMock();
            Timestamp v2 = this.CreateMock();

            Boolean expected = true;
            Boolean actual = v1.Equals( ( Object )v2 );

            Assert.AreEqual( expected, actual );
        }

        protected virtual void AssertGetHashCode( Timestamp ts )
        {
            Int32 expected = 0;
            Int32 actual = ts.GetHashCode();

            Assert.AreEqual<Int32>( expected, actual );
        }

        [TestMethod()]
        public void getHashCodeTest()
        {
            Timestamp v1 = this.CreateMock();
            this.AssertGetHashCode( v1 );
        }
    }
}
