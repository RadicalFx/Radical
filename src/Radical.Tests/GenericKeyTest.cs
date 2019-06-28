using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Radical;
using Radical.ComponentModel;

namespace Radical.Tests
{
    [TestClass]
    public class GenericKeyTest
    {
        [TestMethod]
        public void genericKey_ctor_default_to_null_with_reference()
        {
            string expected = null;

            Key<string> key = new Key<string>();

            Assert.AreEqual<string>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_ctor_default_to_value_with_valueType()
        {
            int expected = 0;

            Key<int> key = new Key<int>();

            Assert.AreEqual<int>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_value_ctor_set_value()
        {
            int expected = 10;

            Key<int> key = new Key<int>( expected );

            Assert.AreEqual<int>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_value_ctor_null_arg_set_value()
        {
            string expected = null;

            Key<string> key = new Key<string>( expected );

            Assert.AreEqual<string>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_toString_with_null_value()
        {
            string expected = "";

            Key<string> key = new Key<string>( null );
            string actual = key.ToString();

            Assert.AreEqual<string>( expected, actual );
        }

        [TestMethod]
        public void genericKey_toString_with_non_null_value()
        {
            int value = 12;
            string expected = value.ToString();

            Key<int> key = new Key<int>( value );
            string actual = key.ToString();

            Assert.AreEqual<string>( expected, actual );
        }

        [TestMethod]
        public void genericKey_implicit_operator_from_value_to_key()
        {
            int expected = 12;
            Key<int> key = expected;

            Assert.IsNotNull( key );
            Assert.AreEqual<int>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_implicit_operator_from_key_to_value()
        {
            int expected = 12;
            Key<int> key = new Key<int>( expected );
            int value = key;

            Assert.AreEqual<int>( expected, value );
        }

        [TestMethod]
        public void genericKey_implicit_operator_from_key_to_value_using_null_key()
        {
            int expected = 0;

            Key<int> key = null;
            int value = key;

            Assert.AreEqual<int>( expected, value );
        }

        [TestMethod]
        public void BUG_genericKey_equality_operator_key_to_key()
        {
            //BUG.2858
            bool expected = false;

            Key<int> key1 = null;
            Key<int> key2 = 10;

            bool actual = key1 == key2;

            Assert.AreEqual<bool>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_null_to_null()
        {
            bool expected = true;

            Key<int> key1 = null;
            Key<int> key2 = null;

            bool actual = key1 == key2;

            Assert.AreEqual<bool>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_nullValue_to_nullValue()
        {
            bool expected = true;

            Key<string> key1 = new Key<string>( null );
            Key<string> key2 = new Key<string>( null );

            bool actual = key1 == key2;

            Assert.AreEqual<bool>( expected, actual );
        }

        [TestMethod]
        public void genericKey_multiple_calls_getHashCode_same_value()
        {
            Key<string> key1 = new Key<string>( "Foo" );

            int expected = key1.GetHashCode();

            for( int i = 0; i < 10; i++ )
            {
                int actual = key1.GetHashCode();
                Assert.AreEqual<int>( expected, actual );
            }
        }

        [TestMethod]
        public void genericKey_with_null_value_multiple_calls_getHashCode_same_value()
        {
            Key<string> key1 = new Key<string>( null );

            int expected = key1.GetHashCode();

            for( int i = 0; i < 10; i++ )
            {
                int actual = key1.GetHashCode();
                Assert.AreEqual<int>( expected, actual );
            }
        }

        [TestMethod]
        public void BUG_genericKey_equality_operator_key_to_key_not_equal()
        {
            //BUG.2858
            bool expected = true;

            Key<int> key1 = null;
            Key<int> key2 = 10;

            bool actual = key1 != key2;

            Assert.AreEqual<bool>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_null_to_null_not_equal()
        {
            bool expected = false;

            Key<int> key1 = null;
            Key<int> key2 = null;

            bool actual = key1 != key2;

            Assert.AreEqual<bool>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_nullValue_to_nullValue_not_equal()
        {
            bool expected = false;

            Key<string> key1 = new Key<string>( null );
            Key<string> key2 = new Key<string>( null );

            bool actual = key1 != key2;

            Assert.AreEqual<bool>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_same_reference_should_be_true()
        {
            var target = new Key<string>( "Foo" );
            bool actual = Key<string>.Equals( target, target );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_equality_operator_same_reference_should_be_true()
        {
            var target = new Key<string>( "Foo" );
            bool actual = target == target;

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_equals_same_reference_should_be_true()
        {
            Key<string> target = new Key<string>( "Foo" );
            bool actual = target.Equals( target );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_disequality_operator_same_reference_should_be_false()
        {
            Key<string> target = new Key<string>( "Foo" );
            bool actual = target != target;

            actual.Should().Be.False();
        }

        [TestMethod]
        public void genericKey_equals_other_genericKey_reference_should_be_true()
        {
            Key<string> target = new Key<string>( "Foo" );
            Key<string> key = new Key<string>( "Foo" );

            bool actual = target.Equals( key );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_equals_other_iKey_reference_should_be_true()
        {
            Key<string> target = new Key<string>( "Foo" );
            IKey key = new Key<string>( "Foo" );

            bool actual = target.Equals( key );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_equals_other_object_reference_should_be_true()
        {
            Key<string> target = new Key<string>( "Foo" );
            object key = new Key<string>( "Foo" );

            bool actual = target.Equals( key );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_compareTo_other_object_reference_should_be_zero()
        {
            Key<string> target = new Key<string>( "Foo" );
            object key = new Key<string>( "Foo" );

            int actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( 0 );
        }

        [TestMethod]
        public void genericKey_compareTo_other_genericKey_reference_should_be_zero()
        {
            var target = new Key<string>( "Foo" );
            var key = new Key<string>( "Foo" );

            int actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( 0 );
        }

        [TestMethod]
        public void genericKey_compareTo_null_reference_should_be_one()
        {
            var target = new Key<string>( "Foo" );

            int actual = target.CompareTo( null );

            actual.Should().Be.EqualTo( 1 );
        }

        [TestMethod]
        public void genericKey_compareTo_other_genericKey_reference_with_null_value_should_be_more_then_zero()
        {
            var target = new Key<string>( "Foo" );
            var key = new Key<string>( null );

            int actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( 1 );
        }

        [TestMethod]
        public void genericKey_with_null_value_compareTo_other_genericKey_reference_should_be_less_then_zero()
        {
            var target = new Key<string>( null );
            var key = new Key<string>( "Foo" );

            int actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( -1 );
        }

        [TestMethod]
        public void genericKey_with_null_value_compareTo_other_genericKey_reference_with_null_value_should_be_zero()
        {
            var target = new Key<string>( null );
            var key = new Key<string>( null );

            int actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( 0 );
        }
    }
}
