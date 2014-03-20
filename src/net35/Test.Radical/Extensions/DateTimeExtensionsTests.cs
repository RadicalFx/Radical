namespace Test.Radical.Extensions
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using SharpTestsEx;
	using Topics.Radical;

	[TestClass]
	public class DateTimeExtensionsTests
	{
		[TestMethod]
		public void dateTimeExtensions_toEndOfMonth_using_valid_date_should_return_expected_date()
		{
			var target = new DateTime( 2010, 1, 12 );
			var actual = DateTimeExtensions.ToEndOfMonth( target );

			actual.Day.Should().Be.EqualTo( 31 );
			actual.Month.Should().Be.EqualTo( 1 );
			actual.Year.Should().Be.EqualTo( 2010 );
		}

		[TestMethod]
		public void dateTimeExtensions_toEndOfMonth_using_forst_day_of_month_should_return_expected_date()
		{
			var target = new DateTime( 2010, 1, 1 );
			var actual = DateTimeExtensions.ToEndOfMonth( target );

			actual.Day.Should().Be.EqualTo( 31 );
			actual.Month.Should().Be.EqualTo( 1 );
			actual.Year.Should().Be.EqualTo( 2010 );
		}

		[TestMethod]
		public void dateTimeExtensions_toEndOfMonth_using_last_day_should_return_expected_date()
		{
			var target = new DateTime( 2010, 1, 31 );
			var actual = DateTimeExtensions.ToEndOfMonth( target );

			actual.Day.Should().Be.EqualTo( 31 );
			actual.Month.Should().Be.EqualTo( 1 );
			actual.Year.Should().Be.EqualTo( 2010 );
		}

		[TestMethod]
		public void dateTimeExtensions_toEndOfMonth_using_last_day_of_february_should_return_expected_date()
		{
			var target = new DateTime( 2009, 2, 28 );
			var actual = DateTimeExtensions.ToEndOfMonth( target );

			actual.Day.Should().Be.EqualTo( 28 );
			actual.Month.Should().Be.EqualTo( 2 );
			actual.Year.Should().Be.EqualTo( 2009 );
		}

		[TestMethod]
		public void dateTimeExtensions_toEndOfMonth_using_one_day_of_february_should_return_expected_date()
		{
			var target = new DateTime( 2009, 2, 15 );
			var actual = DateTimeExtensions.ToEndOfMonth( target );

			actual.Day.Should().Be.EqualTo( 28 );
			actual.Month.Should().Be.EqualTo( 2 );
			actual.Year.Should().Be.EqualTo( 2009 );
		}

		[TestMethod]
		public void dateTimeExtensions_toEndOfMonth_using_one_day_of_february_with29_should_return_expected_date()
		{
			var target = new DateTime( 2012, 2, 11 );
			var actual = DateTimeExtensions.ToEndOfMonth( target );

			actual.Day.Should().Be.EqualTo( 29 );
			actual.Month.Should().Be.EqualTo( 2 );
			actual.Year.Should().Be.EqualTo( 2012 );
		}

		[TestMethod]
		public void dateTimeExtensions_toEndOfMonth_using_last_day_of_february_with29_should_return_expected_date()
		{
			var target = new DateTime( 2012, 2, 29 );
			var actual = DateTimeExtensions.ToEndOfMonth( target );

			actual.Day.Should().Be.EqualTo( 29 );
			actual.Month.Should().Be.EqualTo( 2 );
			actual.Year.Should().Be.EqualTo( 2012 );
		}
	}
}
