using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Helpers;

namespace Radical.Tests.Helpers
{
    [TestClass]
    public class RandomStringsTests
    {
        [TestMethod]
        public void Generated_string_should_respect_default_min_max_len()
        {
            var sut = new RandomStrings();
            var defaultMinLen = sut.MinLength;
            var defaultMaxLen = sut.MaxLength;
            var rndStr = sut.Next();
            
            Assert.IsTrue(rndStr.Length >= defaultMinLen);
            Assert.IsTrue(rndStr.Length <= defaultMaxLen);
        }
    }
}