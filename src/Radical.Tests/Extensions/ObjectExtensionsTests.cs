using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;

namespace Radical.Tests.Extensions
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_Return_using_valid_parameter_should_behave_as_expected()
        {
            var expected = "a string";

            var actual = Radical.ObjectExtensions.Return(expected, v => v, "failed", s => string.IsNullOrEmpty(s));

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_Return_using_invalid_parameter_should_behave_as_expected()
        {
            var expected = "failed";

            var actual = Radical.ObjectExtensions.Return("", v => v, expected, s => string.IsNullOrEmpty(s));

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_Return_using_null_parameter_should_behave_as_expected()
        {
            var expected = "failed";

            var actual = Radical.ObjectExtensions.Return((string)null, v => v, expected, s => string.IsNullOrEmpty(s));

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_Return_using_valid_parameter_should_never_try_to_call_failureValue_provider()
        {
            var actual = false;

            Radical.ObjectExtensions.Return("a value", v => v, () => { actual = true; return "failed"; }, s => string.IsNullOrEmpty(s));

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_Return_using_valid_parameter_and_default_failure_evaluator_should_never_try_to_call_failureValue_provider()
        {
            var actual = false;

            Radical.ObjectExtensions.Return("a value", v => v, () => { actual = true; return "failed"; });

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_With_using_valid_parameter_should_return_expected_value()
        {
            var expected = "foo";
            var actual = Radical.ObjectExtensions.With(expected, s => s);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_With_using_null_parameter_should_return_failure_value()
        {
            var expected = (string)null;
            var actual = expected.With(s => s);

            actual.Should().Be.Null();
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_With_using_null_parameter_should_return_alternative_value()
        {
            var expected = "foo";
            var actual = ((string)null).With(s => s, () => expected);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_With_using_empty_parameter_and_failure_evaluator_should_return_alternative_value()
        {
            var expected = "foo";
            var actual = ("").With(s => s, s => string.IsNullOrEmpty(s), () => expected);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_fluent_evaluation_using_invalid_graph_should_behave_as_expected()
        {
            var executed = false;

            var person = new Person()
            {
                Address = new Address()
            };

            person.With(p => p.Address)
                .Return(a => a.Street)
                .Do(s => executed = true);

            executed.Should().Be.False();
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_fluent_evaluation_using_valid_graph_should_behave_as_expected()
        {
            var executed = false;

            var person = new Person()
            {
                Address = new Address()
                {
                    Street = "street"
                }
            };

            person.With(p => p.Address)
                .Return(a => a.Street)
                .Do(s => executed = true);

            executed.Should().Be.True();
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_fluent_evaluation_using_valid_graph_should_pass_expected_value()
        {
            var expected = "street";
            var actual = "";

            var person = new Person()
            {
                Address = new Address()
                {
                    Street = expected
                }
            };

            person.With(p => p.Address)
                .Return(a => a.Street)
                .Do(s => actual = expected);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        [TestCategory("ObjectExtensions")]
        public void ObjectExtensions_fluent_evaluation_using_valid_graph_should_return_expected_value()
        {
            var expected = "street";
            var actual = "";

            var person = new Person()
            {
                Address = new Address()
                {
                    Street = expected
                }
            };

            actual = person.With(p => p.Address)
                .Return(a => a.Street)
                .Do(s => { });

            actual.Should().Be.EqualTo(expected);
        }
    }
}
