namespace Radical.Tests.Helpers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical;
    using SharpTestsEx;
    using System;

    [TestClass()]
    public class StringExtensionTests
    {
        [TestMethod]
        public void iconUriHelper_buildPackUri_normal_should_behave_as_expected()
        {
            var relativeUri = "/relativeUri";
            var expected = "pack://application:,,,/Radical.Tests;component/relativeUri";

            var actual = Radical.StringExtensions.AsPackUri(relativeUri);
            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void iconUriHelper_buildPackUri_using_null_relative_uri_should_raise_ArgumentNullException()
        {
            Radical.StringExtensions.AsPackUri(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void iconUriHelper_buildPackUri_using_empty_relative_uri_should_raise_ArgumentOutOfRangeException()
        {
            Radical.StringExtensions.AsPackUri(String.Empty);
        }

        [TestMethod]
        public void stringExtensions_likeCompare_normal_should_return_valid_compare_results()
        {
            var actual = "foo".IsLike("foo");

            actual.Should().Be.True();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_star_should_return_valid_compare_results()
        {
            var actual = "foo".IsLike("f*");

            actual.Should().Be.True();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_question_mark_should_return_valid_compare_results()
        {
            var actual = "foo".IsLike("fo?");

            actual.Should().Be.True();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_non_matching_pattern_with_question_mark_should_return_valid_compare_results()
        {
            var actual = "foo".IsLike("f?");

            actual.Should().Be.False();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_non_matching_pattern_with_star_should_return_valid_compare_results()
        {
            var actual = "Beatrice".IsLike("B*r");

            actual.Should().Be.False();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_matching_pattern_with_star_and_ignore_case_should_return_valid_compare_results()
        {
            var actual = "Foo".IsLike("f*", true);

            actual.Should().Be.True();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_matching_pattern_with_star_without_ignore_case_should_return_valid_compare_results()
        {
            var actual = "Foo".IsLike("f*", false);

            actual.Should().Be.False();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_null_reference_value_should_not_fail()
        {
            var actual = Radical.StringExtensions.IsLike(null, (String)null);

            actual.Should().Be.True();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_null_reference_value_and_valid_pattern_should_not_fail()
        {
            var actual = Radical.StringExtensions.IsLike(null, "*");

            actual.Should().Be.False();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_null_reference_pattern_should_not_fail()
        {
            var actual = "Foo".IsLike((String)null);

            actual.Should().Be.False();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_multiple_patterns_should_behave_as_expected()
        {
            Boolean actual = Radical.StringExtensions.IsLike("Foo", "*v*", "F*");

            actual.Should().Be.True();
        }

        [TestMethod]
        public void stringExtensions_likeCompare_using_single_pattern_with_leading_stars_should_behave_as_expected()
        {
            Boolean actual = Radical.StringExtensions.IsLike("Foo", "*F*");

            actual.Should().Be.True();
        }

        [TestMethod]
        public void stringExtension_AsKeywords_using_null_separators_should_use_space_as_separator()
        {
            var expected = new[] { "foo", "bar" };

            var actual = ("foo bar").AsKeywords(false);

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtension_AsKeywords_using_valid_separators_should_return_valid_keywords()
        {
            var expected = new[] { "foo", "bar" };

            var actual = ("foo,bar").AsKeywords(false, ',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtension_AsKeywords_using_valid_separators_should_return_valid_keywords_trimmed()
        {
            var expected = new[] { "foo", "bar" };

            var actual = ("foo , bar ").AsKeywords(false, ',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtension_AsKeywords_using_null_source_string_should_return_empty_keywords()
        {
            var expected = new String[0];

            var actual = ((String)null).AsKeywords(false, ',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtension_AsKeywords_using_empty_source_string_should_return_empty_keywords()
        {
            var expected = new String[0];

            var actual = ("").AsKeywords(false, ',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtensions_AsKeywords_using_source_with_duplicate_keywords_should_return_distinct_keywords()
        {
            var expected = new[] { "foo", "bar" };

            var actual = ("foo , bar, bar, foo").AsKeywords(false, ',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtensions_Askeywords_using_source_without_wild_cards_asking_for_wild_card_wrap_should_wrap_keywords()
        {
            var expected = new[] { "*foo*", "*bar*" };

            var actual = ("foo , bar, bar, foo").AsKeywords(true, ',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtensions_Askeywords_using_source_with_mixed_wild_cards_asking_for_wild_card_wrap_should_wrap_keywords()
        {
            var expected = new[] { "f?o", "*bar*", "*foo*" };

            var actual = ("f?o , bar, bar, foo").AsKeywords(true, ',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtensions_Askeywords_default_auto_parse_wildcards_using_source_without_wild_cards_asking_for_wild_card_wrap_should_wrap_keywords()
        {
            var expected = new[] { "*foo*", "*bar*" };

            var actual = ("foo , bar, bar, foo").AsKeywords(',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtensions_Askeywords_default_auto_parse_wildcards_using_source_with_mixed_wild_cards_asking_for_wild_card_wrap_should_wrap_keywords()
        {
            var expected = new[] { "f?o", "*bar*", "*foo*" };

            var actual = ("f?o , bar, bar, foo").AsKeywords(',');

            actual.Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void stringExtension_IfIsNullOrEmptyReturn_using_not_null_nor_empty_string_should_return_input_string()
        {
            var expected = "foo";

            var actual = expected.IfNullOrEmptyReturn("");

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void stringExtension_IfIsNullOrEmptyReturn_using_null_string_should_return_default_string()
        {
            var expected = "default";

            var actual = ((String)null).IfNullOrEmptyReturn(expected);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void stringExtension_IfIsNullOrEmptyReturn_using_empty_string_should_return_default_string()
        {
            var expected = "default";

            var actual = ("").IfNullOrEmptyReturn(expected);

            actual.Should().Be.EqualTo(expected);
        }
    }
}
