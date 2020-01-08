using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Validation;
using SharpTestsEx;
using System;

namespace Radical.Tests.Validation
{
    [TestClass()]
    public class GuidEnsureExtensionTest
    {
        [TestMethod]
        public void guidEnsureExtension_isNotEmpty_using_non_empty_guid_should_behave_as_expected()
        {
            var v = Ensure.That(Guid.NewGuid());
            v.IsNotEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void guidEnsureExtension_isNotEmpty_using_empty_guid_should_raise_ArgumentOutOfRangeException()
        {
            var v = Ensure.That(Guid.Empty);
            v.IsNotEmpty();
        }

        [TestMethod]
        public void guidEnsureExtension_isNotEmpty_using_empty_guid_and_preview_should_invoke_preview_before_throw()
        {
            var actual = false;

            try
            {
                var target = Ensure.That(Guid.Empty)
                    .WithPreview((v, e) => actual = true);
                target.IsNotEmpty();
            }
            catch (ArgumentOutOfRangeException)
            {

            }

            actual.Should().Be.True();
        }
    }
}
