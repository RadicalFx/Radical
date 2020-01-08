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

        private readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
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
                Capacity = MaxLenght
            };

            // Generate random characters
            char nextCharacter;
            // Initial dummy character flag
            var lastCharacter = nextCharacter = '\n';

            for (var i = 0; i < pwdLength; i++)
            {
                nextCharacter = GetRandomCharacter();

                if (!AllowConsecutiveCharacters)
                {
                    while (lastCharacter == nextCharacter)
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                if (!AllowRepeatCharacters)
                {
                    var temp = pwdBuffer.ToString();
                    var duplicateIndex = temp.IndexOf(nextCharacter);
                    while (-1 != duplicateIndex)
                    {
                        nextCharacter = GetRandomCharacter();
                        duplicateIndex = temp.IndexOf(nextCharacter);
                    }
                }

                if (Exclusions != null)
                {
                    while (Exclusions.Contains(nextCharacter))
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }

            return pwdBuffer.ToString();
        }

        private readonly List<char> _exclusions = new List<char>();

        /// <summary>
        /// A list of char that must be excluded from the
        /// generated password
        /// </summary>
        /// <value>The exclusions.</value>
        public List<char> Exclusions
        {
            get { return _exclusions; }
        }

        private int _minLenght = DEFAULT_MINIMUM;

        /// <summary>
        /// Minimum char number of the generated password
        /// </summary>
        /// <value>The min length.</value>
        public int MinLenght
        {
            get { return _minLenght; }
            set
            {
                _minLenght = value;
                if (DEFAULT_MINIMUM > _minLenght)
                {
                    _minLenght = DEFAULT_MINIMUM;
                }
            }
        }

        private int _maxLenght = DEFAULT_MAXIMUM;

        /// <summary>
        /// Maximum char number of the generated password
        /// </summary>
        /// <value>The max length.</value>
        public int MaxLenght
        {
            get { return _maxLenght; }
            set
            {
                _maxLenght = value;
                if (_minLenght >= _maxLenght)
                {
                    _maxLenght = DEFAULT_MAXIMUM;
                }
            }
        }


        private bool _allowSymbols = true;

        /// <summary>
        /// Gets or sets a value indicating whether symbols are allowed.
        /// </summary>
        /// <value><c>true</c> if symbols are allowed; otherwise, <c>false</c>.</value>
        public bool AllowSymbols
        {
            get { return _allowSymbols; }
            set { _allowSymbols = value; }
        }

        private bool _allowRepeatCharacters = true;

        /// <summary>
        /// If true the resulting string can contains
        /// equals chars.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [allow repeat characters]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowRepeatCharacters
        {
            get { return _allowRepeatCharacters; }
            set { _allowRepeatCharacters = value; }
        }


        /// <summary>
        /// If true the resulting string can contains
        /// consecutive equals chars.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [allow consecutive characters]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowConsecutiveCharacters
        {
            get;
            set;
        }
    }
}
