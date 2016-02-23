using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical;

namespace Test.Radical.ChangeTracking
{
    [TestClass()]
    public class ReferenceEqualityComparerTest
    {
        [TestMethod()]
        [TestCategory( "ChangeTracking" )]
        public void referenceEqualityComparer_ctor()
        {
            ReferenceEqualityComparer<GenericParameterHelper> instance = new ReferenceEqualityComparer<GenericParameterHelper>();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void generic_iEqualityComparer_equals_with_equal_instances()
        {
            var instance = new ReferenceEqualityComparer<GenericParameterHelper>();

            var x = new GenericParameterHelper();
            var y = x;

            bool actual = instance.Equals( x, y );

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void generic_iEqualityComparer_equals_with_instance_and_null()
        {
            var instance = new ReferenceEqualityComparer<GenericParameterHelper>();

            var x = new GenericParameterHelper();
            bool actual = instance.Equals( x, null );

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void generic_iEqualityComparer_equals_with_null_and_instance()
        {
            var instance = new ReferenceEqualityComparer<GenericParameterHelper>();

            var y = new GenericParameterHelper();
            bool actual = instance.Equals( null, y );

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void generic_iEqualityComparer_equals_with_null_and_null()
        {
            var instance = new ReferenceEqualityComparer<GenericParameterHelper>();
            bool actual = instance.Equals( null, null );

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void generic_iEqualityComparer_getHashCode()
        {
            IEqualityComparer<GenericParameterHelper> instance = new ReferenceEqualityComparer<GenericParameterHelper>();

            var obj = new GenericParameterHelper();
            Int32 expected = obj.GetHashCode();

            Int32 actual = instance.GetHashCode( obj );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void generic_iEqualityComparer_getHashCode_null_reference()
        {
            IEqualityComparer<GenericParameterHelper> instance = new ReferenceEqualityComparer<GenericParameterHelper>();
            Int32 actual = instance.GetHashCode( null );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void iEqualityComparer_equals_with_equal_instances()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<GenericParameterHelper>();

            var x = new GenericParameterHelper();
            var y = x;

            bool actual = instance.Equals( x, y );

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void iEqualityComparer_equals_with_instance_and_null()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<GenericParameterHelper>();

            var x = new GenericParameterHelper();
            bool actual = instance.Equals( x, null );

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void iEqualityComparer_equals_with_null_and_instance()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<GenericParameterHelper>();

            var y = new GenericParameterHelper();
            bool actual = instance.Equals( null, y );

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void iEqualityComparer_equals_with_null_and_null()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<GenericParameterHelper>();
            bool actual = instance.Equals( null, null );

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void iEqualityComparer_getHashCode()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<GenericParameterHelper>();
            var obj = new GenericParameterHelper();

            Int32 expected = obj.GetHashCode();
            Int32 actual = instance.GetHashCode( obj );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void iEqualityComparer_getHashCode_null_reference()
        {
            IEqualityComparer instance = new ReferenceEqualityComparer<GenericParameterHelper>();
            Int32 actual = instance.GetHashCode( null );
        }
    }
}
