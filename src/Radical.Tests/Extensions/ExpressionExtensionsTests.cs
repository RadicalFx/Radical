namespace Radical.Tests.Extensions
{
    using System;
    using System.Reflection;
    using Radical.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpTestsEx;

    [TestClass]
    public class ExpressionExtensionsTests
    {
        class TestPerson
        {
            public DateTime BornDate { get; set; }
        }

        [TestMethod]
        public void expressionExtensions_getMemberName_using_dataTime_as_dataType_should_not_fail()
        {
            var expected = "BornDate";

            Expression<Func<TestPerson, Object>> target = p => p.BornDate;

            var actual = target.GetMemberName();

            actual.Should().Be.EqualTo(expected);
        }
    }
}
