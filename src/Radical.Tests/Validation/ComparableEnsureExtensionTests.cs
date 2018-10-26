namespace Radical.Tests.Validation
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.Validation;

    [TestClass]
    public class ComparableEnsureExtensionTests
    {
        [TestMethod]
        public void comparableEnsureExtension_isGreaterThen_using_valid_value_should_behave_as_expected()
        {
            Ensure.That( 10 ).IsGreaterThen( 5 );
        }

        [TestMethod]
        public void comparableEnsureExtension_isGreaterThen_Or_Equal_using_valid_value_should_behave_as_expected()
        {
            Ensure.That( 10 ).IsGreaterThen( 6, Or.Equal );
        }

        [TestMethod]
        public void comparableEnsureExtension_isGreaterThen_Or_Equal_using_boundary_value_should_behave_as_expected()
        {
            Ensure.That( 10 ).IsGreaterThen( 10, Or.Equal );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        public void comparableEnsureExtension_isGreaterThen_using_non_valid_value_should_raise_ArgumentOutOfRangeException()
        {
            Ensure.That( 10 ).IsGreaterThen( 15 );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        public void comparableEnsureExtension_isGreaterThen_using_non_valid_boundary_value_should_raise_ArgumentOutOfRangeException()
        {
            Ensure.That( 10 ).IsGreaterThen( 10 );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        public void comparableEnsureExtension_isGreaterThen_Or_Equal_using_non_valid_value_should_raise_ArgumentOutOfRangeException()
        {
            Ensure.That( 10 ).IsGreaterThen( 11, Or.Equal );
        }

        [TestMethod]
        public void comparableEnsureExtension_isWithin_using_valid_range_should_behave_as_expected()
        {
            Ensure.That( 10 ).IsWithin( 8, 11 );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        public void comparableEnsureExtension_isWithin_using_value_out_of_range_should_raise_ArgumentOutOfRangeException()
        {
            Ensure.That( 100 ).IsWithin( 8, 11 );
        }

        [TestMethod]
        public void comparableEnsureExtension_isWithin_using_value_equals_to_lower_range_inclusive_should_not_fail()
        {
            Ensure.That( 8 ).IsWithin( 8, 11, Boundary.IncludeLower );
        }

        [TestMethod]
        public void comparableEnsureExtension_isWithin_using_value_equals_to_higher_range_inclusive_should_not_fail()
        {
            Ensure.That( 11 ).IsWithin( 8, 11, Boundary.IncludeHigher );
        }
    }
}
