using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Topics.Radical.Helpers
{
	/// <summary>
	/// An utility class to generate random strings
	/// </summary>
	public class RandomStrings
	{
		static RandomStrings instance = new RandomStrings();

		/// <summary>
		/// Generates a random string using default settings.
		/// </summary>
		/// <returns></returns>
		public static String GenerateRandom()
		{
			return instance.Next();
		}

		const Int32 DEFAULT_MINIMUM = 6;
		const Int32 DEFAULT_MAXIMUM = 10;
		const Int32 U_BOUND_DIGIT = 61;

		private RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
		private char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".ToCharArray();

		/// <summary>
		/// Given 2 bound returns a random number between them
		/// </summary>
		/// <param name="lBound">The lower bound</param>
		/// <param name="uBound">The upper bound</param>
		/// <returns>The random result</returns>
		protected Int32 GetCryptographicRandomNumber( Int32 lBound, Int32 uBound )
		{
			// Assumes lBound >= 0 && lBound < uBound
			// returns an Int32 >= lBound and < uBound
			UInt32 urndnum;
			Byte[] rndnum = new Byte[ 4 ];

			if( lBound == uBound - 1 || lBound == uBound )
			{
				// test for degenerate case where only lBound can be returned
				return lBound;
			}

			UInt32 xcludeRndBase = ( UInt32.MaxValue - ( UInt32.MaxValue % ( UInt32 )( uBound - lBound ) ) );

			do
			{
				rng.GetBytes( rndnum );
				urndnum = BitConverter.ToUInt32( rndnum, 0 );
			} while( urndnum >= xcludeRndBase );

			return ( Int32 )( urndnum % ( uBound - lBound ) ) + lBound;
		}

		/// <summary>
		/// Return a random char
		/// </summary>
		/// <returns>The choosen char</returns>
		protected char GetRandomCharacter()
		{
			Int32 upperBound = pwdCharArray.GetUpperBound( 0 );

			if( !this.AllowSymbols )
			{
				upperBound = U_BOUND_DIGIT;
			}

			Int32 randomCharPosition = GetCryptographicRandomNumber( pwdCharArray.GetLowerBound( 0 ), upperBound );
			return pwdCharArray[ randomCharPosition ];
		}

		/// <summary>
		/// Generates one string based on the defined rules.
		/// </summary>
		/// <returns>The generated string.</returns>
		public String Next()
		{
			// Pick random length between minimum and maximum   
			var pwdLength = GetCryptographicRandomNumber( this.MinLenght, this.MaxLenght );

			var pwdBuffer = new StringBuilder ()
			{
				Capacity = this.MaxLenght
			};

			// Generate random characters
			char nextCharacter;
			// Initial dummy character flag
			var lastCharacter = nextCharacter = '\n';

			for( var i = 0; i < pwdLength; i++ )
			{
				nextCharacter = GetRandomCharacter();

				if( !this.AllowConsecutiveCharacters )
				{
					while( lastCharacter == nextCharacter )
					{
						nextCharacter = GetRandomCharacter();
					}
				}

				if( !this.AllowRepeatCharacters )
				{
					var temp = pwdBuffer.ToString();
					var duplicateIndex = temp.IndexOf( nextCharacter );
					while( -1 != duplicateIndex )
					{
						nextCharacter = GetRandomCharacter();
						duplicateIndex = temp.IndexOf( nextCharacter );
					}
				}

				if( this.Exclusions != null )
				{
					while( this.Exclusions.Contains( nextCharacter ) )
					{
						nextCharacter = GetRandomCharacter();
					}
				}

				pwdBuffer.Append( nextCharacter );
				lastCharacter = nextCharacter;
			}

			return pwdBuffer.ToString();
		}

		private readonly List<Char> _exclusions = new List<char>();

		/// <summary>
		/// A list of char that must be excluded from the
		/// generated password
		/// </summary>
		/// <value>The exclusions.</value>
		public List<Char> Exclusions
		{
			get { return this._exclusions; }
		}

		private Int32 _minLenght = DEFAULT_MINIMUM;

		/// <summary>
		/// Minimum char number of the generated password
		/// </summary>
		/// <value>The min lenght.</value>
		public Int32 MinLenght
		{
			get { return this._minLenght; }
			set
			{
				this._minLenght = value;
				if( RandomStrings.DEFAULT_MINIMUM > this._minLenght )
				{
					this._minLenght = RandomStrings.DEFAULT_MINIMUM;
				}
			}
		}

		private Int32 _maxLenght = DEFAULT_MAXIMUM;

		/// <summary>
		/// Maximum char number of the generated password
		/// </summary>
		/// <value>The max lenght.</value>
		public Int32 MaxLenght
		{
			get { return this._maxLenght; }
			set
			{
				this._maxLenght = value;
				if( this._minLenght >= this._maxLenght )
				{
					this._maxLenght = RandomStrings.DEFAULT_MAXIMUM;
				}
			}
		}


		private Boolean _allowSymbols = true;

		/// <summary>
		/// Gets or sets a value indicating whether symbols are allowed.
		/// </summary>
		/// <value><c>true</c> if symbols are allowed; otherwise, <c>false</c>.</value>
		public Boolean AllowSymbols
		{
			get { return this._allowSymbols; }
			set { this._allowSymbols = value; }
		}

		private Boolean _allowRepeatCharacters = true;

		/// <summary>
		/// If true the resulting string can contains
		/// equals chars.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [allow repeat characters]; otherwise, <c>false</c>.
		/// </value>
		public Boolean AllowRepeatCharacters
		{
			get { return this._allowRepeatCharacters; }
			set { this._allowRepeatCharacters = value; }
		}

		
		/// <summary>
		/// If true the resulting string can contains
		/// consecutive equals chars.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [allow consecutive characters]; otherwise, <c>false</c>.
		/// </value>
		public Boolean AllowConsecutiveCharacters
		{
			get;
			set;
		}
	}
}
