using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Helpers;

namespace Radical.Tests.Helpers
{
    [TestClass]
    public class RandomStringsTests
    {
        private readonly char[] allCharsArray =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?"
                .ToCharArray();

        private readonly char[] symbolsArray = "`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".ToCharArray();

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

        [TestMethod]
        public void Generated_string_should_respect_set_min_max_len()
        {
            const int expectedMinLength = 10;
            const int expectedMaxLength = 20;

            var sut = new RandomStrings {MinLength = expectedMinLength, MaxLength = expectedMaxLength};
            var rndStr = sut.Next();

            Assert.IsTrue(rndStr.Length >= expectedMinLength);
            Assert.IsTrue(rndStr.Length <= expectedMaxLength);
        }

        [TestMethod]
        public void Generated_string_by_default_should_not_allow_symbols()
        {
            var sut = new RandomStrings();
            var rndStr = sut.Next();

            Assert.IsFalse(rndStr.All(c => symbolsArray.Contains(c)), $"{rndStr} contains symbols.");
        }

        [TestMethod]
        public void Generated_string_allow_symbols_should_contain_symbols()
        {
            var sut = new RandomStrings {MaxLength = 100, AllowSymbols = true};
            var rndStr = sut.Next();

            Assert.IsTrue(rndStr.Any(c => symbolsArray.Contains(c)), $"{rndStr} does not contain symbols.");
        }

        [TestMethod]
        public void Generated_string_should_allow_min_len_equal_to_max_len()
        {
            var sut = new RandomStrings {MaxLength = 30, MinLength = 30};
            var rndStr = sut.Next();

            Assert.AreEqual(30, rndStr.Length);
        }

        [TestMethod]
        public void Generated_string_by_default_should_not_allow_consecutive_equal_chars()
        {
            var sut = new RandomStrings {MaxLength = 30, MinLength = 30};
            var rndStr = sut.Next();

            char lastChar = default;
            foreach (char c in rndStr)
            {
                if (c == lastChar)
                {
                    Assert.Fail($"{rndStr} contains consecutive equals characters.");
                }

                lastChar = c;
            }
        }

        [TestMethod]
        public void Generated_string_by_default_should_allow_consecutive_equal_chars()
        {
            var sut = new RandomStrings {MaxLength = 400, MinLength = 400, AllowConsecutiveCharacters = true, AllowRepeatCharacters = true};
            var rndStr = sut.Next();

            var foundConsecutiveChars = false;
            char lastChar = default;
            foreach (char c in rndStr)
            {
                if (c == lastChar)
                {
                    foundConsecutiveChars = true;
                }

                lastChar = c;
            }

            Assert.IsTrue(foundConsecutiveChars, rndStr);
        }
        
        [TestMethod]
        public void Generated_string_by_default_allow_consecutive_and_no_allow_repeated_should_throw()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var sut = new RandomStrings { AllowConsecutiveCharacters = true, AllowRepeatCharacters = false};
                sut.Next(); 
            });
        }
    }
}