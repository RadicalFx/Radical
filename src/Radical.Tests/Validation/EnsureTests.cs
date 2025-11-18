using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Validation;
using SharpTestsEx;
using System;

namespace Radical.Tests.Validation
{
    [TestClass()]
    public class EnsureTests
    {
        [TestMethod]
        public void ensure_getFullErrorMessage_should_contain_ClassName()
        {
            var bak = Ensure.SourceInfoLoadStrategy;
            try
            {
                Ensure.SourceInfoLoadStrategy = SourceInfoLoadStrategy.Load;
                var obj = Ensure.That("");
                string actual = obj.GetFullErrorMessage("validator specific message");

                var containsClassName = actual.Contains(typeof(EnsureTests).Name);

                containsClassName.Should().Be.True();
            }
            finally
            {
                Ensure.SourceInfoLoadStrategy = bak;
            }
        }

        [TestMethod]
        public void ensure_getFullErrorMessage_should_contain_MethodName()
        {
            var bak = Ensure.SourceInfoLoadStrategy;
            try
            {
                Ensure.SourceInfoLoadStrategy = SourceInfoLoadStrategy.Load;
                var obj = Ensure.That("");
                string actual = obj.GetFullErrorMessage("validator specific message");

                var containsMethodName = actual.Contains("ensure_getFullErrorMessage_should_contain_MethodName");

                containsMethodName.Should().Be.True();
            }
            finally
            {
                Ensure.SourceInfoLoadStrategy = bak;
            }
        }

        [TestMethod]
        public void ensure_getFullErrorMessage_should_contain_ClassName_even_w_lazy_load()
        {
            var bak = Ensure.SourceInfoLoadStrategy;
            try
            {
                Ensure.SourceInfoLoadStrategy = SourceInfoLoadStrategy.LazyLoad;
                var obj = Ensure.That("");
                string actual = obj.GetFullErrorMessage("validator specific message");

                var containsClassName = actual.Contains(typeof(EnsureTests).Name);
                var containsMethodName = actual.Contains(typeof(EnsureTests).Name);

                containsClassName.Should().Be.True();
                containsMethodName.Should().Be.True();
            }
            finally
            {
                Ensure.SourceInfoLoadStrategy = bak;
            }
        }

        [TestMethod]
        public void ensure_getFullErrorMessage_should_contain_MethodName_even_w_lazy_load()
        {
            var bak = Ensure.SourceInfoLoadStrategy;
            try
            {
                Ensure.SourceInfoLoadStrategy = SourceInfoLoadStrategy.LazyLoad;
                var obj = Ensure.That("");
                string actual = obj.GetFullErrorMessage("validator specific message");

                var containsMethodName = actual.Contains("ensure_getFullErrorMessage_should_contain_MethodName_even_w_lazy_load");

                containsMethodName.Should().Be.True();
            }
            finally
            {
                Ensure.SourceInfoLoadStrategy = bak;
            }
        }

        [TestMethod]
        public void ensure_getFullErrorMessage_should_contain_custom_message()
        {
            var expected = "custom message";

            var obj = Ensure.That("").WithMessage(expected);

            string actual = obj.GetFullErrorMessage("validator specific message");

            actual.Contains(expected).Should().Be.True();
        }

        [TestMethod]
        public void ensure_getFullErrorMessage_using_more_then_one_custom_message_should_contain_custom_message()
        {
            var expected1 = "custom message 1";
            var expected2 = "custom message 2";

            var obj = Ensure.That("")
                .WithMessage(expected1);

            var actual1 = obj.GetFullErrorMessage("validator specific message");

            obj.WithMessage(expected2);

            var actual2 = obj.GetFullErrorMessage("validator specific message");

            actual1.Contains(expected1).Should().Be.True();
            actual2.Contains(expected2).Should().Be.True();
        }

        [TestMethod]
        public void ensure_getFullErrorMessage_should_not_be_null()
        {
            var obj = Ensure.That("");
            string actual = obj.GetFullErrorMessage("validator specific message");

            actual.Should().Not.Be.Null();
        }

        [TestMethod]
        public void ensure_getFullErrorMessage_should_contain_validator_specific_message()
        {
            var expected = "validator specific message";

            var obj = Ensure.That("");
            var actual = obj.GetFullErrorMessage(expected);

            actual.Contains(expected).Should().Be.True();
        }

        [TestMethod()]
        public void validator_inspect_isNotNull()
        {
            IEnsure<string> obj = Ensure.That("");
            Assert.IsNotNull(obj);
        }

        [TestMethod()]
        public void validator_inspect_isNotNull_after_if()
        {
            IEnsure<string> obj = Ensure.That("");
            IEnsure<string> actual = obj.If(s => true);

            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void validator_inspect_isNotNull_after_then_with_false_state()
        {
            var obj = Ensure.That("");
            var actual = obj.If(s => false).Then((val) =>
         {
                //NOP 
            });

            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void validator_inspect_isNotNull_after_then_with_true_state()
        {
            var obj = Ensure.That("");
            var actual = obj.If(s => true).Then((val) =>
         {
                //NOP 
            });

            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void validator_inspect_isNotNull_after_then_value_name_with_false_state()
        {
            var obj = Ensure.That("");
            var actual = obj.If(s => false).Then((v, n) =>
         {
                //NOP 
            });

            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void validator_inspect_isNotNull_after_then_value_name_with_true_state()
        {
            var obj = Ensure.That("");
            var actual = obj.If(s => true).Then((v, n) =>
         {
                //NOP 
            });

            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void validator_instpect_on_nullString_Throw()
        {
            Assert.ThrowsException<System.ArgumentNullException>(() =>
            {
                string expected = "null";
                Ensure.That((string)null).If(s => s == null)
                    .Then((val) =>
                   {
                       throw new System.ArgumentNullException(expected);
                   });
            });
        }

        [TestMethod()]
        public void validator_instpect_on_nullString_exception()
        {
            Assert.ThrowsException<System.ArgumentNullException>(() =>
            {
                Ensure.That((string)null).IsNotNull();
            });
        }

        [TestMethod()]
        public void validator_instpect_on_notNullString_valid()
        {
            IEnsure<string> obj = Ensure.That("Foo").IsNotNull();
            Assert.IsNotNull(obj);
        }

        [TestMethod()]
        public void validator_name_using_Named_fluent_interface()
        {
            string expected = "paramName";

            IEnsure<int> obj = Ensure.That(0)
                .Named(expected);

            Assert.AreEqual<string>(expected, obj.Name);
        }

        [TestMethod]
        public void validator_getFullErrorMessage_using_valid_name_should_contain_name()
        {
            var expected = "foo.name";
            var actual = Ensure.That("").Named("foo.name").GetFullErrorMessage("validator.specific.message");

            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void ensureGeneric_value_normal_should_be_as_expected()
        {
            string expected = "Foo";
            var target = Ensure.That(expected);

            target.Value.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void ensureGeneric_Is_using_invalid_value_should_invoke_preview_before_failure()
        {
            var actual = false;

            try
            {
                Ensure.That(string.Empty)
                    .WithPreview((v, e) => actual = true)
                    .Is("not-empty");
            }
            catch
            {

            }

            actual.Should().Be.True();
        }

        [TestMethod]
        public void ensureGeneric_IsNot_using_invalid_value_should_invoke_preview_before_failure()
        {
            var actual = false;

            try
            {
                Ensure.That(string.Empty)
                    .WithPreview((v, e) => actual = true)
                    .IsNot(string.Empty);
            }
            catch
            {

            }

            actual.Should().Be.True();
        }

        [TestMethod]
        public void ensureGeneric_IsTrue_using_invalid_value_should_invoke_preview_before_failure()
        {
            var actual = false;

            try
            {
                Ensure.That(string.Empty)
                    .WithPreview((v, e) => actual = true)
                    .IsTrue(s => s == "not-empty");
            }
            catch
            {

            }

            actual.Should().Be.True();
        }

        [TestMethod]
        public void ensureGeneric_IsFalse_using_invalid_value_should_invoke_preview_before_failure()
        {
            var actual = false;

            try
            {
                Ensure.That(string.Empty)
                    .WithPreview((v, e) => actual = true)
                    .IsFalse(s => s == string.Empty);
            }
            catch
            {

            }

            actual.Should().Be.True();
        }

        [TestMethod]
        public void ensureGeneric_ThenThrow_should_invoke_preview_before_failure()
        {
            var actual = false;

            try
            {
                Ensure.That(string.Empty)
                    .WithPreview((v, e) => actual = true)
                    .If(s => s == string.Empty)
                    .ThenThrow(v => new Exception());
            }
            catch
            {

            }

            actual.Should().Be.True();
        }

        [TestMethod]
        public void ensureGeneric_Throw_should_invoke_preview_before_failure()
        {
            var actual = false;

            try
            {
                Ensure.That(string.Empty)
                    .WithPreview((v, e) => actual = true)
                    .Throw(new Exception());
            }
            catch
            {

            }

            actual.Should().Be.True();
        }
    }
}
