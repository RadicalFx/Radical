using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Radical.Linq;
using SharpTestsEx;

namespace Radical.Tests.Extensions
{
    [TestClass]
    public class SelectorExtensionsTests
    {
        [TestMethod]
        [TestCategory( "SelectorExtensions" )]
        public void SelectorExtensions_SingleOr_using_valid_source_list_with_no_values_should_return_the_func_value()
        {
            var expected = 12;
            var actual = 0;

            var list = new List<Int32>();
            actual = list.SingleOr( () => expected );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "SelectorExtensions" )]
        public void SelectorExtensions_FirstOr_using_valid_source_list_with_no_values_should_return_the_func_value()
        {
            var expected = 12;
            var actual = 0;

            var list = new List<Int32>();
            actual = list.FirstOr( () => expected );

            actual.Should().Be.EqualTo( expected );
        }
    }
}
