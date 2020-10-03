using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Radical.Helpers
{
    /// <summary>
    /// An utility class to generate random strings
    /// </summary>
    public class RandomStrings
    {
        static readonly RandomStrings instance = new RandomStrings();

        /// <summary>
        /// Generates a random string using default settings.
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandom()
        {
            return instance.Next();
        }

        const int DEFAULT_MINIMUM = 6;
        const int DEFAULT_MAXIMUM = 10;
        const int U_BOUND_DIGIT = 61;

        private readonly char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".ToCharArray();

        /// <summary>
        /// Given 2 bound returns a random number between them
        /// </summary>
        /// <param name="lBound">The lower bound</param>
        /// <param name="uBound">The upper bound</param>
        /// <returns>The random result</returns>
        protected int GetCryptographicRandomNumber(int lBound, int uBound)
        {
            // Assumes lBound >= 0 && lBound < uBound
            // returns an int >= lBound and < uBound
            uint urndnum;
            byte[] rndnum = new byte[4];

            if (lBound == uBound - 1 || lBound == uBound)
            {
                // test for degenerate case where only lBound can be returned
                return lBound;
            }

            uint xcludeRndBase = (uint.MaxValue - (uint.MaxValue % (uint)(uBound - lBound)));

            using var rng = new RNGCryptoServiceProvider();
            do
            {
                rng.GetBytes(rndnum);
                urndnum = BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);

            return (int)(urndnum % (uBound - lBound)) + lBound;
        }

        /// <summary>
        /// Return a random char
        /// </summary>
        /// <returns>The chosen char</returns>
        protected char GetRandomCharacter()
        {
            int upperBound = pwdCharArray.GetUpperBound(0);

            if (!AllowSymbols)
            {
                upperBound = U_BOUND_DIGIT;
            }

            int randomCharPosition = GetCryptographicRandomNumber(pwdCharArray.GetLowerBound(0), upperBound);
            return pwdCharArray[randomCharPosition];
        }

        /// <summary>
        /// Generates one string based on the defined rules.
        /// </summary>
        /// <returns>The generated string.</returns>
        public string Next()
        {
            // Pick random length between minimum and maximum
            var pwdLength = GetCryptographicRandomNumber(MinLenght, MaxLenght);

            var pwdBuffer = new StringBuilder()
            {
                Capacity = pwdLength
            };

            // Generate random characters
            char nextCharacter;
            // Initial dummy character flag
            var lastCharacter = nextCharacter = '\n';

            for (var i = 0; i < pwdLength; i++)
            {
                nextCharacter = GetRandomCharacter();
                nextCharacter = EvaluateAllowConsecutiveCharacters(lastCharacter, nextCharacter);
                nextCharacter = EvaluateAllowRepeatCharacters(pwdBuffer, nextCharacter);
                nextCharacter = EvaluateExclusions(nextCharacter);

                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }

            return pwdBuffer.ToString();
        }

        private char EvaluateAllowConsecutiveCharacters(char lastCharacter, char nextCharacter)
        {
            if (AllowConsecutiveCharacters)
            {
                return nextCharacter;
            }

            while (lastCharacter == nextCharacter)
            {
                nextCharacter = GetRandomCharacter();
            }

            return nextCharacter;
        }

        char EvaluateAllowRepeatCharacters(StringBuilder pwdBuffer, char nextCharacter)
        {
            if (AllowRepeatCharacters)
            {
                return nextCharacter;
            }

            var temp = pwdBuffer.ToString();
            var duplicateIndex = temp.IndexOf(nextCharacter);
            while (-1 != duplicateIndex)
            {
                nextCharacter = GetRandomCharacter();
                duplicateIndex = temp.IndexOf(nextCharacter);
            }

            return nextCharacter;
        }

        char EvaluateExclusions(char nextCharacter)
        {
            if (Exclusions == null)
            {
                return nextCharacter;
            }

            while (Exclusions.Contains(nextCharacter))
            {
                nextCharacter = GetRandomCharacter();
            }

            return nextCharacter;
        }

        /// <summary>
        /// A list of char that must be excluded from the
        /// generated password
        /// </summary>
        /// <value>The exclusions.</value>
        public List<char> Exclusions { get; } = new List<char>();

        private int _minLength = DEFAULT_MINIMUM;

        /// <summary>
        /// Minimum char number of the generated password
        /// </summary>
        /// <value>The min length.</value>
        public int MinLength
        {
            get { return _minLength; }
            set
            {
                _minLength = value;
                if (DEFAULT_MINIMUM > _minLength)
                {
                    _minLength = DEFAULT_MINIMUM;
                }
            }
        }

        /// <summary>
        /// Minimum char number of the generated password
        /// </summary>
        /// <value>The min length.</value>
        [Obsolete("Use the MinLength property, this will be removed in v3.0.0")]
        public int MinLenght
        {
            get { return MinLength; }
            set { MinLength = value; }
        }

        private int _maxLength = DEFAULT_MAXIMUM;

        /// <summary>
        /// Maximum char number of the generated password
        /// </summary>
        /// <value>The max length.</value>
        public int MaxLength
        {
            get { return _maxLength; }
            set
            {
                _maxLength = value;
                if (_minLength >= _maxLength)
                {
                    _maxLength = DEFAULT_MAXIMUM;
                }
            }
        }

        /// <summary>
        /// Maximum char number of the generated password
        /// </summary>
        /// <value>The max length.</value>
        [Obsolete("Use the MaxLength property, this will be removed in v3.0.0")]
        public int MaxLenght
        {
            get { return MaxLength; }
            set { MaxLength = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether symbols are allowed.
        /// </summary>
        /// <value><c>true</c> if symbols are allowed; otherwise, <c>false</c>.</value>
        public bool AllowSymbols{ get; set; }

        /// <summary>
        /// If true the resulting string can contain
        /// equals characters.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the resulting string can contain equals characters; otherwise, <c>false</c>.
        /// </value>
        public bool AllowRepeatCharacters { get; set; }


        /// <summary>
        /// If true the resulting string can contain
        /// consecutive equals chars.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the resulting string can contain consecutive equals characters; otherwise, <c>false</c>.
        /// </value>
        public bool AllowConsecutiveCharacters { get; set; }
    }
}
