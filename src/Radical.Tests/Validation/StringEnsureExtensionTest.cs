using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Validation;
using SharpTestsEx;
using System;

namespace Radical.Tests.Validation
{
    [TestClass()]
    public class StringValidatorTest
    {
        [TestMethod()]
        public void stringEnsureTest_IsNotEmpty()
        {
            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() =>
            {
                Ensure.That("").IsNotEmpty();
            });
        }

        [TestMethod()]
        public void stringEnsureTest_IsNotEmpty_valid()
        {
            IEnsure<string> obj = Ensure.That("foo").IsNotEmpty();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void stringEnsureExtension_isNotNullNorEmpty_with_valid_string_should_behave_as_expected()
        {
            Ensure.That("foo").IsNotNullNorEmpty();
        }

        [TestMethod]
        public void stringEnsureExtension_isNotNullNorEmpty_with_valid_string_should_return_ensure_instance()
        {
            var expected = Ensure.That("foo");
            var actual = expected.IsNotNullNorEmpty();

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void stringEnsureExtension_isNotNullNorEmpty_null_string_should_raise_ArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                string value = null;
                Ensure.That(value).IsNotNullNorEmpty();
            });
        }

        [TestMethod]
        public void stringEnsureExtension_isNotNullNorEmpty_empty_string_should_raise_ArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                string value = string.Empty;
                Ensure.That(value).IsNotNullNorEmpty();
            });
        }

        [TestMethod]
        public void stringEnsureExtension_matches_using_valid_email_address_should_not_fail()
        {
            var pattern = Radical.Helpers.KnownRegex.MailAddress;

            Ensure.That("name@domain.tld").Matches(pattern);
        }

        [TestMethod]
        public void stringEnsureExtension_matches_using_non_valid_email_address_should_raise_FormatException()
        {
            Assert.ThrowsException<FormatException>(() =>
            {
                var pattern = Radical.Helpers.KnownRegex.MailAddress;

                Ensure.That("name_domain.tld").Matches(pattern);
            });
        }

        [TestMethod]
        public void stringEnsureExtension_isNotNullNorEmpty_using_empty_string_and_preview_should_invoke_preview_before_throw()
        {
            var actual = false;

            try
            {
                var target = Ensure.That(string.Empty)
                    .WithPreview((v, e) => actual = true);
                target.IsNotNullNorEmpty();
            }
            catch (ArgumentOutOfRangeException)
            {

            }

            actual.Should().Be.True();
        }
    }
}
