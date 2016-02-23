using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical;
using Topics.Radical.ComponentModel;

namespace Test.Radical
{
    [TestClass]
    public class GenericKeyTest
    {
        [TestMethod]
        public void genericKey_ctor_default_to_null_with_reference()
        {
            String expected = null;

            Key<String> key = new Key<String>();

            Assert.AreEqual<String>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_ctor_default_to_value_with_valueType()
        {
            Int32 expected = 0;

            Key<Int32> key = new Key<Int32>();

            Assert.AreEqual<Int32>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_value_ctor_set_value()
        {
            Int32 expected = 10;

            Key<Int32> key = new Key<Int32>( expected );

            Assert.AreEqual<Int32>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_value_ctor_null_arg_set_value()
        {
            String expected = null;

            Key<String> key = new Key<String>( expected );

            Assert.AreEqual<String>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_toString_with_null_value()
        {
            String expected = "";

            Key<String> key = new Key<String>( null );
            String actual = key.ToString();

            Assert.AreEqual<String>( expected, actual );
        }

        [TestMethod]
        public void genericKey_toString_with_non_null_value()
        {
            Int32 value = 12;
            String expected = value.ToString();

            Key<Int32> key = new Key<Int32>( value );
            String actual = key.ToString();

            Assert.AreEqual<String>( expected, actual );
        }

        [TestMethod]
        public void genericKey_implicit_operator_from_value_to_key()
        {
            Int32 expected = 12;
            Key<Int32> key = expected;

            Assert.IsNotNull( key );
            Assert.AreEqual<Int32>( expected, key.Value );
        }

        [TestMethod]
        public void genericKey_implicit_operator_from_key_to_value()
        {
            Int32 expected = 12;
            Key<Int32> key = new Key<int>( expected );
            Int32 value = key;

            Assert.AreEqual<Int32>( expected, value );
        }

        [TestMethod]
        public void genericKey_implicit_operator_from_key_to_value_using_null_key()
        {
            Int32 expected = 0;

            Key<Int32> key = null;
            Int32 value = key;

            Assert.AreEqual<Int32>( expected, value );
        }

        [TestMethod]
        public void BUG_genericKey_equality_operator_key_to_key()
        {
            //BUG.2858
            Boolean expected = false;

            Key<Int32> key1 = null;
            Key<Int32> key2 = 10;

            Boolean actual = key1 == key2;

            Assert.AreEqual<Boolean>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_null_to_null()
        {
            Boolean expected = true;

            Key<Int32> key1 = null;
            Key<Int32> key2 = null;

            Boolean actual = key1 == key2;

            Assert.AreEqual<Boolean>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_nullValue_to_nullValue()
        {
            Boolean expected = true;

            Key<String> key1 = new Key<string>( null );
            Key<String> key2 = new Key<string>( null );

            Boolean actual = key1 == key2;

            Assert.AreEqual<Boolean>( expected, actual );
        }

        [TestMethod]
        public void genericKey_multiple_calls_getHashCode_same_value()
        {
            Key<String> key1 = new Key<string>( "Foo" );

            Int32 expected = key1.GetHashCode();

            for( Int32 i = 0; i < 10; i++ )
            {
                Int32 actual = key1.GetHashCode();
                Assert.AreEqual<Int32>( expected, actual );
            }
        }

        [TestMethod]
        public void genericKey_with_null_value_multiple_calls_getHashCode_same_value()
        {
            Key<String> key1 = new Key<string>( null );

            Int32 expected = key1.GetHashCode();

            for( Int32 i = 0; i < 10; i++ )
            {
                Int32 actual = key1.GetHashCode();
                Assert.AreEqual<Int32>( expected, actual );
            }
        }

        [TestMethod]
        public void BUG_genericKey_equality_operator_key_to_key_not_equal()
        {
            //BUG.2858
            Boolean expected = true;

            Key<Int32> key1 = null;
            Key<Int32> key2 = 10;

            Boolean actual = key1 != key2;

            Assert.AreEqual<Boolean>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_null_to_null_not_equal()
        {
            Boolean expected = false;

            Key<Int32> key1 = null;
            Key<Int32> key2 = null;

            Boolean actual = key1 != key2;

            Assert.AreEqual<Boolean>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_nullValue_to_nullValue_not_equal()
        {
            Boolean expected = false;

            Key<String> key1 = new Key<string>( null );
            Key<String> key2 = new Key<string>( null );

            Boolean actual = key1 != key2;

            Assert.AreEqual<Boolean>( expected, actual );
        }

        [TestMethod]
        public void genericKey_static_equals_same_reference_should_be_true()
        {
            Key<String> target = new Key<string>( "Foo" );
            Boolean actual = Key<String>.Equals( target, target );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_equality_operator_same_reference_should_be_true()
        {
            Key<String> target = new Key<string>( "Foo" );
            Boolean actual = target == target;

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_equals_same_reference_should_be_true()
        {
            Key<String> target = new Key<string>( "Foo" );
            Boolean actual = target.Equals( target );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_disequality_operator_same_reference_should_be_false()
        {
            Key<String> target = new Key<string>( "Foo" );
            Boolean actual = target != target;

            actual.Should().Be.False();
        }

        [TestMethod]
        public void genericKey_equals_other_genericKey_reference_should_be_true()
        {
            Key<String> target = new Key<String>( "Foo" );
            Key<String> key = new Key<String>( "Foo" );

            Boolean actual = target.Equals( key );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_equals_other_iKey_reference_should_be_true()
        {
            Key<String> target = new Key<String>( "Foo" );
            IKey key = new Key<String>( "Foo" );

            Boolean actual = target.Equals( key );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_equals_other_object_reference_should_be_true()
        {
            Key<String> target = new Key<String>( "Foo" );
            object key = new Key<String>( "Foo" );

            Boolean actual = target.Equals( key );

            actual.Should().Be.True();
        }

        [TestMethod]
        public void genericKey_compareTo_other_object_reference_should_be_zero()
        {
            Key<String> target = new Key<String>( "Foo" );
            object key = new Key<String>( "Foo" );

            Int32 actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( 0 );
        }

        [TestMethod]
        public void genericKey_compareTo_other_genericKey_reference_should_be_zero()
        {
            var target = new Key<String>( "Foo" );
            var key = new Key<String>( "Foo" );

            Int32 actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( 0 );
        }

        [TestMethod]
        public void genericKey_compareTo_null_reference_should_be_one()
        {
            var target = new Key<String>( "Foo" );

            Int32 actual = target.CompareTo( null );

            actual.Should().Be.EqualTo( 1 );
        }

        [TestMethod]
        public void genericKey_compareTo_other_genericKey_reference_with_null_value_should_be_more_then_zero()
        {
            var target = new Key<String>( "Foo" );
            var key = new Key<String>( null );

            Int32 actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( 1 );
        }

        [TestMethod]
        public void genericKey_with_null_value_compareTo_other_genericKey_reference_should_be_less_then_zero()
        {
            var target = new Key<String>( null );
            var key = new Key<String>( "Foo" );

            Int32 actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( -1 );
        }

        [TestMethod]
        public void genericKey_with_null_value_compareTo_other_genericKey_reference_with_null_value_should_be_zero()
        {
            var target = new Key<String>( null );
            var key = new Key<String>( null );

            Int32 actual = target.CompareTo( key );

            actual.Should().Be.EqualTo( 0 );
        }
    }
}
