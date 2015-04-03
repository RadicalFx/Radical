using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical.Model;

namespace Test.Radical.Model
{
	[TestClass()]
	public class PredicateEntityItemViewFilterTest
	{
		[TestMethod]
		public void predicateEntityItemViewFilter_ctor_predicate_should_set_expected_values()
		{
			var expectedName = "Default Predicate name.";
			var expected = 1;
			var actual = 0;

			var target = new PredicateEntityItemViewFilter<GenericParameterHelper>( v =>
			{
				actual++;
				return true;
			} );

			target.ShouldInclude( new GenericParameterHelper() );

			actual.Should().Be.EqualTo( expected );
			target.ToString().Should().Be.EqualTo( expectedName );
		}

		[TestMethod]
		public void predicateEntityItemViewFilter_ctor_predicate_string_should_set_expected_values()
		{
			var expectedName = "filter name";
			var expected = 1;
			var actual = 0;

			var target = new PredicateEntityItemViewFilter<GenericParameterHelper>( v =>
			{
				actual++;
				return true;
			}, expectedName );

			target.ShouldInclude( new GenericParameterHelper() );

			actual.Should().Be.EqualTo( expected );
			target.ToString().Should().Be.EqualTo( expectedName );
		}

		[TestMethod]
		public void predicateEntityItemViewFilter_ctor_predicate_null_string_should_set_expected_values()
		{
			var expectedName = "Default Predicate name.";
			var expected = 1;
			var actual = 0;

			var target = new PredicateEntityItemViewFilter<GenericParameterHelper>( v =>
			{
				actual++;
				return true;
			}, null );

			target.ShouldInclude( new GenericParameterHelper() );

			actual.Should().Be.EqualTo( expected );
			target.ToString().Should().Be.EqualTo( expectedName );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void predicateEntityItemViewFilter_ctor_null_predicate_should_raise_ArgumentNullException()
		{
			var target = new PredicateEntityItemViewFilter<GenericParameterHelper>( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void predicateEntityItemViewFilter_ctor_null_predicate_null_string_should_raise_ArgumentNullException()
		{
			var target = new PredicateEntityItemViewFilter<GenericParameterHelper>( null, null );
		}
	}
}
