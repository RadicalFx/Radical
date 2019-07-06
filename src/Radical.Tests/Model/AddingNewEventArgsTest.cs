using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Model;
using SharpTestsEx;

namespace Radical.Tests.Model
{
    [TestClass()]
    public class AddingNewEventArgsTest
    {
        [TestMethod]
        public void addingNewEventArgs_ctor_normal_should_set_expected_values()
        {
            var expected = new GenericParameterHelper();
            var target = new AddingNewEventArgs<GenericParameterHelper>();
            target.NewItem = expected;

            var actual = target.NewItem;

            actual.Should().Be.EqualTo(expected);
            target.Cancel.Should().Be.False();
        }
    }
}
